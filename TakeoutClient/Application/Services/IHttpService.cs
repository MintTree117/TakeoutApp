namespace Application.Services;

public interface IHttpService
{
    Task<ServiceReply<T?>> TryGetRequest<T>( string apiPath, Dictionary<string, object>? parameters = null, string? authToken = null );
    Task<ServiceReply<T?>> TryPostRequest<T>( string apiPath, object? body = null, string? authToken = null );
    Task<ServiceReply<T?>> TryPutRequest<T>( string apiPath, object? body = null, string? authToken = null );
    Task<ServiceReply<T?>> TryDeleteRequest<T>( string apiPath, Dictionary<string, object>? parameters = null, string? authToken = null );
}