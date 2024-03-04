using Application.Models;

namespace Application.Services;

public sealed class AppStateService // Singleton
{
    public event Action<User?>? IdentityChanged;
    public event Action<Cart>? CartChanged;

    public void ChangeIdentity( User? user )
    {
        IdentityChanged?.Invoke( user );
    }
    public void ChangeCart( Cart cart )
    {
        CartChanged?.Invoke( cart );
    } 
}