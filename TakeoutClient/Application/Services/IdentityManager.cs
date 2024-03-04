using Application.Dtos;
using Blazored.LocalStorage;

namespace Application.Services;

// Scoped
public sealed class IdentityManager : IIdentityManager
{
    const string KEY = "Identity";
    
    readonly ILocalStorageService _localStorage;
    readonly ILogger<IdentityManager> _logger;
    readonly IHttpService _http;
    readonly AppState _state;
    
    public IdentityManager( ILocalStorageService localStorage, ILogger<IdentityManager> logger, IHttpService http, AppState state )
    {
        _localStorage = localStorage;
        _logger = logger;
        _http = http;
        _state = state;
    }
    
    public async Task<UserDto?> GetIdentity()
    {
        try
        {
            return await _localStorage.GetItemAsync<UserDto>( KEY );
        }
        catch ( Exception e )
        {
            _logger.LogError( e, e.Message );
            return null;
        }
    }
    public async Task<UserDto?> Login( LoginDto login )
    {
        try
        {
            ServiceReply<UserDto?> loginReply = await _http.TryPostRequest<UserDto>( "api/users/login", login );

            if ( loginReply.Data is null )
                return null;
            
            await _localStorage.SetItemAsync( KEY, loginReply.Data );

            _state.ChangeIdentity( loginReply.Data );

            return loginReply.Data;
        }
        catch ( Exception e )
        {
            _logger.LogError( e, e.Message );
            return null;
        }
    }
    public async Task Logout()
    {
        try
        {
            await _localStorage.RemoveItemAsync( KEY );
            await _http.TryPutRequest<UserDto>( "api/users/logout" );
        }
        catch ( Exception e )
        {
            _logger.LogError( e, e.Message );
        }

        _state.ChangeIdentity( null );
    }
}