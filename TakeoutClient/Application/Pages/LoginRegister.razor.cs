using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Components;

namespace Application.Pages;

public sealed partial class LoginRegister : PageBase
{
    [Inject] NavigationManager Navigation { get; init; } = default!;
    [Inject] IHttpService HttpService { get; init; } = default!;
    [Inject] IIdentityManager IdentityManager { get; init; } = default!;
    [Inject] ICartManager CartManager { get; init; } = default!;

    readonly LoginDto _loginDto = new();
    readonly RegisterDto _registerDto = new();

    async Task Login()
    {
        UserDto? user = await IdentityManager.Login( _loginDto );
        
        if ( user is null )
        {
            Alert( AlertType.Danger, "An error occured!" );
            return;
        }
        
        await CartManager.UpdateServerCart( user );

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