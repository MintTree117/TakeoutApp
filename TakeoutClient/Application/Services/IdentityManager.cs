using Application.Models;
using Blazored.LocalStorage;

namespace Application.Services;

// Scoped
public sealed class IdentityManager
{
    const string KEY = "Identity";
    
    readonly ILocalStorageService _localStorage;
    readonly ILogger<IdentityManager> _logger;
    readonly HttpService _http;
    readonly AppStateService _stateService;
    
    public IdentityManager( ILocalStorageService localStorage, ILogger<IdentityManager> logger, HttpService http, AppStateService stateService )
    {
        _localStorage = localStorage;
        _logger = logger;
        _http = http;
        _stateService = stateService;
    }
    
    public async Task<User?> GetIdentity()
    {
        return await TryRetrieveIdentity();
    }
    public async Task<ApiReply<bool>> Register( Register register )
    {
        ApiReply<bool> reply = await _http.TryPostRequest<bool>( "api/identiy/register", register );
        return reply;
    }
    public async Task<ApiReply<User?>> Login( Login login )
    {
        ApiReply<User?> loginReply = await _http.TryPostRequest<User>( "api/identity/login", login );

        if ( loginReply.Data is null )
            return loginReply;

        await TryStoreIdentity( loginReply.Data );

        _stateService.ChangeIdentity( loginReply.Data );

        return loginReply;
    }
    public async Task Logout()
    {
        Task local =TryClearIdentity();
        Task<ApiReply<User?>> server = _http.TryPutRequest<User>( "api/identity/logout" );

        await Task.WhenAll( local, server );
        
        _stateService.ChangeIdentity( null );
    }

    async Task TryClearIdentity()
    {
        try
        {
            await _localStorage.RemoveItemAsync( KEY );
        }
        catch ( Exception e )
        {
            _logger.LogError( e, e.Message );
        }
    }
    async Task TryStoreIdentity( User user )
    {
        try
        {
            await _localStorage.SetItemAsync( KEY, user );
        }
        catch ( Exception e )
        {
            _logger.LogError( e, e.Message );
        }
    }
    async Task<User?> TryRetrieveIdentity()
    {
        try
        {
            return await _localStorage.GetItemAsync<User>( KEY );
        }
        catch ( Exception e )
        {
            _logger.LogError( e, e.Message );
            return null;
        }
    }
}