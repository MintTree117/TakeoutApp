using System.Net;
using System.Text.Json;

namespace Api.Errors;

public class ExceptionMiddleware
{
    readonly RequestDelegate _next;
    readonly ILogger<ExceptionMiddleware> _logger;
    readonly IHostEnvironment _environment;

    public ExceptionMiddleware( RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment )
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync( HttpContext httpContext )
    {
        try
        {
            await _next( httpContext );
        }
        catch( Exception ex )
        {
            _logger.LogError( ex, ex.Message );
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = ( int ) HttpStatusCode.InternalServerError;

            ApiError response = _environment.IsDevelopment()
                ? new ApiException( ApiErrorType.ServerError, ex.Message, ex.StackTrace )
                : new ApiException( ApiErrorType.ServerError, "An unexpected error occured. Please contact support." );

            JsonSerializerOptions options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            string json = JsonSerializer.Serialize( response, options );
            await httpContext.Response.WriteAsync( json );
        }
    }
}