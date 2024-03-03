using Application.Dtos;
using Blazored.LocalStorage;

namespace Application.Services;

public sealed class LocalIdentityCache : ILocalIdentityCache
{
    const string KEY = "Identity";
    readonly ILocalStorageService _localStorage;
    
    public LocalIdentityCache( ILocalStorageService localStorage )
    {
        _localStorage = localStorage;
    }

    public async Task<UserDto?> GetIdentity()
    {
        return await _localStorage.GetItemAsync<UserDto>( KEY );
    }
    public async Task SetIdentity( UserDto? user )
    {
        await _localStorage.SetItemAsync( KEY, user );
    }
}