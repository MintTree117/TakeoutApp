using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Components;

namespace Application.Pages;

public sealed partial class LoginRegister( ILogger<LoginRegister> logger ) : PageBase( logger )
{
    [Inject] NavigationManager Navigation { get; init; } = default!;
    [Inject] IHttpService HttpService { get; init; } = default!;
    [Inject] ILocalIdentityCache IdentityCache { get; init; } = default!;

    readonly LoginDto _loginDto = new();
    readonly RegisterDto _registerDto = new();

    async Task Login()
    {
        ServiceReply<UserDto?> loginReply = await HttpService.TryPostRequest<UserDto>( "api/users/login", _loginDto );

        if ( loginReply.Data is null )
        {
            Alert( AlertType.Danger, loginReply.Message );
            return;
        }

        await IdentityCache.SetIdentity( loginReply.Data );
    }
    async Task Logout()
    {
        Task local = IdentityCache.SetIdentity( null );
        Task<ServiceReply<UserDto?>> server = HttpService.TryPutRequest<UserDto>( "api/users/logout" );

        await Task.WhenAll( local, server );

        Navigation.NavigateTo( "/" );
    }
    async Task Register()
    {
        ServiceReply<UserDto?> registerReply = await HttpService.TryPostRequest<UserDto>( "api/users/register", _registerDto );

        if ( registerReply.Data is null )
        {
            Alert( AlertType.Danger, registerReply.Message );
            return;
        }

        Navigation.NavigateTo( "/registered" );
    }
}