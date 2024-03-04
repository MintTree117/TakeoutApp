using System.Collections.Specialized;
using System.Net.Http.Json;
using System.Web;

namespace Application.Services;

public sealed class HttpService( HttpClient _http, ILogger<HttpService> _logger )
{
    public async Task<ApiReply<T?>> TryGetRequest<T>( string apiPath, Dictionary<string, object>? parameters = null, string? authToken = null )
    {
        try
        {
            SetAuthHttpHeader( authToken );
            string path = GetQueryParameters( apiPath, parameters );
            HttpResponseMessage httpResponse = await _http.GetAsync( path );
            return await HandleHttpResponse<T?>( httpResponse, "Get" );
        }
        catch ( Exception e )
        {
            return HandleHttpException<T?>( e, "Get" );
        }
    }
    public async Task<ApiReply<T?>> TryPostRequest<T>( string apiPath, object? body = null, string? authToken = null )
    {
        try
        {
            SetAuthHttpHeader( authToken );
            HttpResponseMessage httpResponse = await _http.PostAsJsonAsync( apiPath, body );
            return await HandleHttpResponse<T?>( httpResponse, "Post" );
        }
        catch ( Exception e )
        {
            return HandleHttpException<T?>( e, "Post" );
        }
    }
    public async Task<ApiReply<T?>> TryPutRequest<T>( string apiPath, object? body = null, string? authToken = null )
    {
        try
        {
            SetAuthHttpHeader( authToken );
            HttpResponseMessage httpResponse = await _http.PutAsJsonAsync( apiPath, body );
            return await HandleHttpResponse<T?>( httpResponse, "Put" );
        }
        catch ( Exception e )
        {
            return HandleHttpException<T?>( e, "Put" );
        }
    }
    public async Task<ApiReply<T?>> TryDeleteRequest<T>( string apiPath, Dictionary<string, object>? parameters = null, string? authToken = null )
    {
        try
        {
            SetAuthHttpHeader( authToken );
            string path = GetQueryParameters( apiPath, parameters );
            HttpResponseMessage httpResponse = await _http.DeleteAsync( path );
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
    async Task<ApiReply<T?>> HandleHttpResponse<T>( HttpResponseMessage httpResponse, string requestTypeName )
    {
        if ( typeof( T ) == typeof( string ) )
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return new ApiReply<T?>( ( T ) ( object ) responseString );
        }
        else if ( httpResponse.IsSuccessStatusCode )
        {
            var getReply = await httpResponse.Content.ReadFromJsonAsync<T>();

            return getReply is not null
                ? new ApiReply<T?>( getReply )
                : new ApiReply<T?>( ServiceErrorType.NotFound, "No data returned from request" );
        }
        
        string errorContent = await httpResponse.Content.ReadAsStringAsync();
        
        switch ( httpResponse.StatusCode )
        {
            case System.Net.HttpStatusCode.BadRequest:
                _logger.LogError( $"{requestTypeName}: Bad request: {errorContent}" );
                return new ApiReply<T?>( ServiceErrorType.ValidationError, errorContent );

            case System.Net.HttpStatusCode.NotFound:
                _logger.LogError( $"{requestTypeName}: Not found: {errorContent}" );
                return new ApiReply<T?>( ServiceErrorType.NotFound, errorContent );

            case System.Net.HttpStatusCode.Unauthorized:
                _logger.LogError( $"{requestTypeName}: Unauthorized: {errorContent}" );
                return new ApiReply<T?>( ServiceErrorType.Unauthorized, errorContent );

            case System.Net.HttpStatusCode.Conflict:
                _logger.LogError( $"{requestTypeName}: Conflict: {errorContent}" );
                return new ApiReply<T?>( ServiceErrorType.Conflict, errorContent );

            case System.Net.HttpStatusCode.InternalServerError:
                _logger.LogError( $"{requestTypeName}: Server error: {errorContent}" );
                return new ApiReply<T?>( ServiceErrorType.ServerError, errorContent );

            default:
                _logger.LogError( $"{requestTypeName}: Other error: {httpResponse.StatusCode}, Content: {errorContent}" );
                return new ApiReply<T?>( ServiceErrorType.ServerError, $"Error: {httpResponse.StatusCode}" );
        }
    }
    
    ApiReply<T?> HandleHttpException<T>( Exception e, string requestType )
    {
        _logger.LogError( e, $"{requestType}: Exception occurred while sending API request." );
        return new ApiReply<T?>( ServiceErrorType.ServerError, e.Message );
    }
    void SetAuthHttpHeader( string? token )
    {
        _http.DefaultRequestHeaders.Authorization = !string.IsNullOrWhiteSpace( token )
            ? new System.Net.Http.Headers.AuthenticationHeaderValue( "Bearer", token )
            : null;
    }
}