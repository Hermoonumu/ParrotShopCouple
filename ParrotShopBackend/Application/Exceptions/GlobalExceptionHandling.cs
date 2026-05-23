namespace ParrotShopBackend.Application.Exceptions;



public class GlobalExceptionHandling : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await ExceptionHandler(context, ex);
        }
    }

    private Task ExceptionHandler(HttpContext ctx, Exception ex)
    {
        switch (ex)
        {
            case UserDoesntExistException: ctx.Response.StatusCode = StatusCodes.Status404NotFound; break;
            case UserAlreadyExistsException: ctx.Response.StatusCode = StatusCodes.Status409Conflict; break;
            case PasswordCheckFailedException: ctx.Response.StatusCode = StatusCodes.Status401Unauthorized; break;
            case RefreshFailedException: ctx.Response.StatusCode = StatusCodes.Status401Unauthorized; break;
            case PasswordTooShortOrLongException: ctx.Response.StatusCode = StatusCodes.Status400BadRequest; break;
            case UsernameTooShortOrLongException: ctx.Response.StatusCode = StatusCodes.Status400BadRequest; break;
            case InvalidFormException: ctx.Response.StatusCode = StatusCodes.Status400BadRequest; break;
            default: ctx.Response.StatusCode = StatusCodes.Status500InternalServerError; break;
        }
        ctx.Response.ContentType = "application/json";
        var response = new
        {
            Message = ex.Message+ex.StackTrace,
        };
        return ctx.Response.WriteAsJsonAsync(response);
    }
}