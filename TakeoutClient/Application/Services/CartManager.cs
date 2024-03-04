using Application.Dtos;
using Blazored.LocalStorage;

namespace Application.Services;

public sealed class CartManager : ICartManager
{
    const string KEY = "Cart";
    readonly ILocalStorageService _localStorage;
    readonly ILogger<IdentityManager> _logger;
    
    public CartManager( ILocalStorageService localStorage, ILogger<IdentityManager> logger )
    {
        _localStorage = localStorage;
        _logger = logger;
    }
    
    public Task<CartSummary> GetCartSummary()
    {
        throw new NotImplementedException();
    }
    public Task UpdateServerCart( UserDto? user )
    {
        throw new NotImplementedException();
    }
}