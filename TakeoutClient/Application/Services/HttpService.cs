using System.Collections.Specialized;
using System.Net.Http.Json;
using System.Web;

namespace Application.Services;

public class HttpService( HttpClient Http, ILogger<HttpService> logger ) : IHttpService
{
    public async Task<ServiceReply<T?>> TryGetRequest<T>( string apiPath, Dictionary<string, object>? parameters = null )
    {
        try
        {
            string path = GetQueryParameters( apiPath, parameters );
            HttpResponseMessage httpResponse = await Http.GetAsync( path );
            return await HandleHttpResponse<T?>( httpResponse, "Get" );
        }
        catch ( Exception e )
        {
            return HandleHttpException<T?>( e, "Get" );
        }
    }
    public async Task<ServiceReply<T?>> TryPostRequest<T>( string apiPath, object? body = null )
    {
        try
        {
            HttpResponseMessage httpResponse = await Http.PostAsJsonAsync( apiPath, body );
            return await HandleHttpResponse<T?>( httpResponse, "Post" );
        }
        catch ( Exception e )
        {
            return HandleHttpException<T?>( e, "Post" );
        }
    }
    public async Task<ServiceReply<T?>> TryPutRequest<T>( string apiPath, object? body = null )
    {
        try
        {
            HttpResponseMessage httpResponse = await Http.PutAsJsonAsync( apiPath, body );
            return await HandleHttpResponse<T?>( httpResponse, "Put" );
        }
        catch ( Exception e )
        {
            return HandleHttpException<T?>( e, "Put" );
        }
    }
    public async Task<ServiceReply<T?>> TryDeleteRequest<T>( string apiPath, Dictionary<string, object>? parameters = null )
    {
        try
        {
            string path = GetQueryParameters( apiPath, parameters );
            HttpResponseMessage httpResponse = await Http.DeleteAsync( path );
            return await HandleHttpResponse<T?>( httpResponse, "Delete" );
        }
        catch ( Exception e )
        {
            return HandleHttpException<T?>( e, "Delete" );
        }
    }

    static string GetQueryParameters( string apiPath, Dictionary<string, object>? parameters )
    {
        if ( parameters is null )
            return apiPath;

        NameValueCollection query = HttpUtility.ParseQueryString( string.Empty );

        foreach ( KeyValuePair<string, object> param in parameters )
        {
            query[ param.Key ] = param.Value.ToString();
        }

        return $"{apiPath}?{query}";
    }
    async Task<ServiceReply<T?>> HandleHttpResponse<T>( HttpResponseMessage httpResponse, string requestTypeName )
    {
        if ( typeof( T ) == typeof( string ) )
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return new ServiceReply<T?>( ( T ) ( object ) responseString );
        }
        else if ( httpResponse.IsSuccessStatusCode )
        {
            var getReply = await httpResponse.Content.ReadFromJsonAsync<T>();

            return getReply is not null
                ? new ServiceReply<T?>( getReply )
                : new ServiceReply<T?>( ServiceErrorType.NotFound, "No data returned from request" );
        }
        
        string errorContent = await httpResponse.Content.ReadAsStringAsync();
        
        switch ( httpResponse.StatusCode )
        {
            case System.Net.HttpStatusCode.BadRequest:
                logger.LogError( $"{requestTypeName}: Bad request: {errorContent}" );
                return new ServiceReply<T?>( ServiceErrorType.ValidationError, errorContent );

            case System.Net.HttpStatusCode.NotFound:
                logger.LogError( $"{requestTypeName}: Not found: {errorContent}" );
                return new ServiceReply<T?>( ServiceErrorType.NotFound, errorContent );

            case System.Net.HttpStatusCode.Unauthorized:
                logger.LogError( $"{requestTypeName}: Unauthorized: {errorContent}" );
                return new ServiceReply<T?>( ServiceErrorType.Unauthorized, errorContent );

            case System.Net.HttpStatusCode.Conflict:
                logger.LogError( $"{requestTypeName}: Conflict: {errorContent}" );
                return new ServiceReply<T?>( ServiceErrorType.Conflict, errorContent );

            case System.Net.HttpStatusCode.InternalServerError:
                logger.LogError( $"{requestTypeName}: Server error: {errorContent}" );
                return new ServiceReply<T?>( ServiceErrorType.ServerError, errorContent );

            default:
                logger.LogError( $"{requestTypeName}: Other error: {httpResponse.StatusCode}, Content: {errorContent}" );
                return new ServiceReply<T?>( ServiceErrorType.ServerError, $"Error: {httpResponse.StatusCode}" );
        }
    }
    
    ServiceReply<T?> HandleHttpException<T>( Exception e, string requestType )
    {
        logger.LogError( e, $"{requestType}: Exception occurred while sending API request." );
        return new ServiceReply<T?>( ServiceErrorType.ServerError, e.Message );
    }
}