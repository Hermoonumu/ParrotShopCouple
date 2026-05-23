using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace ParrotShopBackend.Application.Extensions;


public static class DistributedCacheExtension
{
    public static async Task SetRecordAsync<T>( this IDistributedCache c,
                                                string key, 
                                                T value,
                                                TimeSpan? ttl=null,
                                                TimeSpan? UnusedExprTime=null)
    {
        var opt = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = ttl??TimeSpan.FromMinutes(1),
            SlidingExpiration = UnusedExprTime
        };

        var JsonData = JsonSerializer.Serialize(value);
        await c.SetStringAsync(key, JsonData, opt);
    }

    public static async Task<T?> GetRecordAsync<T>( this IDistributedCache c, string recordId)
    {
        string? jsonData = await c.GetStringAsync(recordId); 
        if (jsonData is null) return default;
        return JsonSerializer.Deserialize<T>(jsonData);
    }
}