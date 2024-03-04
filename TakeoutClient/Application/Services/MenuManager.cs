using Application.Models;

namespace Application.Services;

public sealed class MenuManager
{
    readonly ILogger<IdentityManager> _logger;
    readonly HttpService _http;
    readonly AppStateService _stateService;

    public MenuManager( ILogger<IdentityManager> logger, HttpService http, AppStateService stateService )
    {
        _logger = logger;
        _http = http;
        _stateService = stateService;
    }
    public async Task<ApiReply<List<MenuItem>?>> GetItems()
    {
        ApiReply<List<MenuItem>?> reply = await _http.TryGetRequest<List<MenuItem>>( "api/menu/get/items" );
        return reply;
    }
    public async Task<ApiReply<List<MenuCategory>?>> GetCategories()
    {
        ApiReply<List<MenuCategory>?> reply = await _http.TryGetRequest<List<MenuCategory>>( "api/menu/get/categories" );
        return reply;
    }
    public async Task<ApiReply<MenuOptions?>> GetOptions()
    {
        ApiReply<MenuOptions?> reply = await _http.TryGetRequest<MenuOptions>( "api/menu/get/options" );
        return reply;
    }
}