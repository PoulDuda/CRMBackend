using Newtonsoft.Json;

namespace CRMAuth.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _requestDelegate;

    public ExceptionHandlingMiddleware(RequestDelegate requestDelegate)
    {
        _requestDelegate = requestDelegate;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _requestDelegate(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "Server internal error. Please, try later!",
            Detailed = ex.Message // Для отладки можно включить
        };
        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    } 
}