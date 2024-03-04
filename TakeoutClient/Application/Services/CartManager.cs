using Application.Models;
using Blazored.LocalStorage;

namespace Application.Services;

public sealed class CartManager( ILocalStorageService localStorage, ILogger<IdentityManager> logger, HttpService http )
{
    const string KEY = "Cart";
    readonly ILocalStorageService _localStorage = localStorage;
    readonly ILogger<IdentityManager> _logger = logger;
    readonly HttpService _http = http;
    
    public async Task<Cart> GetCart( string? token = null )
    {
        Cart localCart = await GetLocalCart();

        if ( string.IsNullOrWhiteSpace( token ) )
            return localCart;

        ApiReply<Cart?> serverReply = await _http.TryGetRequest<Cart>( "api/cart/update", null, token );

        if ( serverReply.Data is null )
            return localCart;

        await _localStorage.SetItemAsync( KEY, serverReply.Data );

        return serverReply.Data;
    }
    public async Task<Cart> UpdateCart( Cart newCart, string? token = null )
    {
        if ( !string.IsNullOrWhiteSpace( token ) )
        {
            ApiReply<Cart?> serverReply = await _http.TryPostRequest<Cart>( "api/cart/update", newCart, token );

            if ( serverReply.Data is not null )
                newCart = serverReply.Data;
        }
        
        await _localStorage.SetItemAsync( KEY, newCart );

        return newCart;
    }
    
    async Task<Cart> GetLocalCart()
    {
        try
        {
            return await _localStorage.GetItemAsync<Cart>( KEY ) ?? new Cart();
        }
        catch ( Exception e )
        {
            _logger.LogError( e.Message, e );
            return new Cart();
        }
    }
    async Task SetLocalCart( Cart cart )
    {
        try
        {
            await _localStorage.SetItemAsync( KEY, cart );
        }
        catch ( Exception e )
        {
            _logger.LogError( e.Message, e );
        }
    }
}