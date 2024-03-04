using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Components;

namespace Application.Pages;

public sealed partial class LoginRegister : PageBase
{
    [Inject] NavigationManager Navigation { get; init; } = default!;
    [Inject] IdentityManager IdentityManager { get; init; } = default!;
    [Inject] CartManager CartManager { get; init; } = default!;

    readonly Login _login = new();
    readonly Register _register = new();

    async Task Login()
    {
        ApiReply<User?> reply = await IdentityManager.Login( _login );
        
        if ( reply.Data is null )
        {
            Alert( AlertType.Danger, reply.Details() );
            return;
        }

        await CartManager.GetCart( reply.Data.Token );
        
        Navigation.NavigateTo( "/" );
    }
    async Task Register()
    {
        ApiReply<bool> reply = await IdentityManager.Register( _register );
        
        if ( !reply.Data )
        {
            Alert( AlertType.Danger, reply.Details() );
            return;
        }

        Navigation.NavigateTo( "/registered" );
    }
}