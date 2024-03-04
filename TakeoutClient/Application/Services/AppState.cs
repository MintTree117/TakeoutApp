using Application.Dtos;

namespace Application.Services;

public sealed class AppState // Singleton
{
    public event Action<UserDto?>? OnIdentityChanged;
    public event Action<CartSummary>? OnCartChanged;

    public void ChangeIdentity( UserDto? user )
    {
        OnIdentityChanged?.Invoke( user );
    }
    public void ChangeCart( CartSummary summary )
    {
        OnCartChanged?.Invoke( summary );
    } 
}