using Microsoft.AspNetCore.Components;

namespace Application.Pages;

public abstract class PageBase : ComponentBase
{
    public event Action<AlertType, string>? OnAlert;
    readonly ILogger<PageBase> Logger;
    
    protected PageBase( ILogger<PageBase> logger )
    {
        Logger = logger;
    }

    protected void Alert( AlertType type, string message )
    {
        if ( type is AlertType.Danger )
            Logger.LogError( message );
        
        OnAlert?.Invoke( type, message );
    }
}