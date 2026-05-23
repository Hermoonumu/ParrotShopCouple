using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ParrotShopBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ParrotShopBackend.Application.Services;
using ParrotShopBackend.Infrastructure.Repos;
using FluentValidation;
using FluentValidation.AspNetCore;
using ParrotShopBackend.Application.Exceptions;
using Hangfire;
using Hangfire.PostgreSql;
using ParrotShopBackend.Application.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using ParrotShopBackend.Domain;
using StackExchange.Redis;
using Stripe;
using Newtonsoft.Json.Serialization;


// stripe listen --forward-to http://localhost:5084/api/payments/webhook


//Console.Write("Enter the webhook signing key: ");
//var StripeWebhookSecret = Console.ReadLine();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


//Allow angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(["http://localhost:4200", "http://192.168.1.118:4200", "http://0.0.0.0:4200"]) // Your Angular URL
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Crucial if you are using cookies/tokens
    });
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddDbContext<ShopContext>(option => { option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")); });
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IParrotService, ParrotService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IParrotRepository, ParrotRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICheckoutRepository, CheckoutRepository>();
builder.Services.AddScoped<ICheckoutService, ParrotShopBackend.Application.Services.CheckoutService>();

builder.Services.AddSingleton<RedisCacheExtension>();
builder.Services.AddTransient<GlobalExceptionHandling>();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    })
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    });

builder.Services.AddHangfire(conf =>
{
    conf.UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection")!);

});
try
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")! + ",allowAdmin=true"));
}
catch (RedisConnectionException _)
{
    Console.WriteLine("!!!!!!!!! WARNING !!!!!!!!!\nThe service will not run without functional Redis connection. The app will be halted.");
    return;
}


/*builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration=builder.Configuration.GetConnectionString("Redis");
    opt.InstanceName="ParrotCache_";
});*/

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        conf =>
        {
            conf.RequireHttpsMetadata = false;
            conf.Audience = builder.Configuration["API:Audience"];
            conf.SaveToken = true;
            conf.TokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                                    builder.Configuration["SecSettings:SecretKey"]!)),
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["API:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["API:Audience"],
                ClockSkew = TimeSpan.FromMinutes(2)
            };
            conf.Events = new JwtBearerEvents
            {
                OnMessageReceived = async context =>
                {
                    context.Token = context.Request.Cookies["AccessToken"];
                },
                OnAuthenticationFailed = async context =>
                {
                    Console.WriteLine($"Auth Failed: {context.Exception.Message}");
                },
                OnTokenValidated = async context =>
                {
                    var TokenToCheck = context
                                            .HttpContext
                                            .Request
                                            .Cookies["AccessToken"];
                    var _redis = context
                                    .HttpContext
                                    .RequestServices
                                    .GetRequiredService<IConnectionMultiplexer>()
                                    .GetDatabase();
                    var isRevoked = await _redis.StringGetAsync($"Revoked_{TokenToCheck}");
                    if (!string.IsNullOrEmpty(isRevoked))
                    {
                        context.Fail("Token has been revoked.");
                    }
                }
            };
        }
    );

builder.Services.AddAuthorization(options =>
        options.AddPolicy("Admin", policy =>
        policy.RequireRole("Admin"))
    );

builder.Services.AddHangfireServer();
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

var app = builder.Build();

app.UseStaticFiles();

app.UseMiddleware<GlobalExceptionHandling>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowAngular"); //Letting the angular go through

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (ctx, next) =>
{
    app.Logger.LogInformation($"The request was initiated on {DateTime.Now.ToString()}");
    app.Logger.LogInformation($"The request had this auth header: {ctx.Request.Headers.Authorization}");
    await next.Invoke(ctx);
});

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var _recurringJob = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    _recurringJob.AddOrUpdate<IAuthService>(
        "jwt-clean",
        service => service.ClearExpiredTokensAsync(),
        Cron.Daily
    );

    var _authSvr = scope.ServiceProvider.GetRequiredService<IAuthService>();
    string[] adminCreds = await _authSvr.GetAdministratorAsync();
    if (adminCreds[1] == null)
    {
        app.Logger.LogInformation("ADMIN ALREADY INSTANTIATED. IF CREDENTIALS ARE NOT AVAILABLE -- COSIDER DELETING AN ADMIN FROM DB");
    }
    else
    {
        app.Logger.LogInformation($"ADMIN INSTANTIATED\n\nCREDENTIALS:\nUSERNAME: {adminCreds[0]}\nPASSWORD: {adminCreds[1]}\n\n");
    }
    /*var c = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
    await DistributedCacheExtension.SetRecordAsync<List<RevokedJWT>>(   c,
                                                                        "RevokedJWTs", 
                                                                        new List<RevokedJWT>(), 
                                                                        TimeSpan.FromDays(365));*/
    var c = scope.ServiceProvider.GetRequiredService<RedisCacheExtension>();
    var db = scope.ServiceProvider.GetRequiredService<ShopContext>();
    long LastId = default;
    while (true)
    {
        var redisBatch = c._redis.CreateBatch();
        var redisTasks = new List<Task>();
        List<Parrot> allParrots = await db.Parrots
                         .IgnoreQueryFilters()
                         .Include(p => p.Traits)
                         .Where(p => p.Id > LastId)
                         .Take(1000)
                         .ToListAsync();
        if (allParrots.Count <= 0) break;
        LastId = allParrots.Last().Id;
        foreach (Parrot p in allParrots)
        {
            redisTasks.Add(redisBatch.SetAddAsync($"Set_AllParrots", p.Id));
            if (p.Traits is not null)
            {
                if (p.Traits.KidSafety.HasValue)
                {
                    redisTasks.Add(redisBatch.SetAddAsync($"Set_KidSafety_{p.Traits.KidSafety.Value}", p.Id));
                }
                if (p.Traits.CareComplexity.HasValue)
                {
                    redisTasks.Add(redisBatch.SetAddAsync($"Set_CareComplexity_{p.Traits.CareComplexity.Value}", p.Id));
                }
                if (p.Traits.ChewingRisk.HasValue)
                {
                    redisTasks.Add(redisBatch.SetAddAsync($"Set_ChewingRisk_{p.Traits.ChewingRisk.Value}", p.Id));
                }
                if (p.Traits.NoiseLevel.HasValue)
                {
                    redisTasks.Add(redisBatch.SetAddAsync($"Set_NoiseLevel_{p.Traits.NoiseLevel.Value}", p.Id));
                }
                if (p.Traits.Size.HasValue)
                {
                    redisTasks.Add(redisBatch.SetAddAsync($"Set_Size_{p.Traits.Size.Value}", p.Id));
                }
                if (p.Traits.Sociability.HasValue)
                {
                    redisTasks.Add(redisBatch.SetAddAsync($"Set_Sociability_{p.Traits.Sociability.Value}", p.Id));
                }
                if (p.Traits.Talkativeness.HasValue)
                {
                    redisTasks.Add(redisBatch.SetAddAsync($"Set_Talkativeness_{p.Traits.Talkativeness.Value}", p.Id));
                }
                if (p.Traits.Trainability.HasValue)
                {
                    redisTasks.Add(redisBatch.SetAddAsync($"Set_Trainability_{p.Traits.Trainability.Value}", p.Id));
                }
            }
            foreach (Color col in Enum.GetValues<Color>())
            {
                if (p.ColorType.HasFlag(col))
                {
                    redisTasks.Add(redisBatch.SetAddAsync($"Set_Color_{col}", p.Id));
                }
            }
            redisTasks.Add(redisBatch.SetAddAsync($"Set_Gender_{p.GenderType}", p.Id));
            redisTasks.Add(redisBatch.SetAddAsync($"Set_Species_{p.SpeciesType}", p.Id));

            redisTasks.Add(redisBatch.SortedSetAddAsync("Set_Price", p.Id, (double)p.Price!));
        }
        redisBatch.Execute();
        await Task.WhenAll(redisTasks);
    }
}



/*List<string> propNames = (new Parrot())
                        .GetType()
                        .GetProperties()
                        .Select(p => p.Name)
                        .ToList();*/



app.Run();
