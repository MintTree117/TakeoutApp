using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Application.Pages;

public sealed partial class Home : PageBase
{
    [Inject] IJSRuntime JsRuntime { get; init; } = default!;
    [Inject] MenuManager MenuManager { get; init; } = default!;
    
    MenuCategory[] _categories = [ ];
    MenuItem[] _items = [ ];
    MenuOptions _options = new();
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        List<Task<bool>> tasks =
        [
            LoadMenuCategories(),
            LoadMenuItems(),
            LoadMenuOptions()
        ];

        await Task.WhenAll( tasks );

        StateHasChanged();
        
        if ( tasks.Any( t => !t.Result ) )
            Alert( AlertType.Danger, "Failed to fetch some parts of the menu!" );
    }

    async Task<bool> LoadMenuCategories()
    {
        ApiReply<List<MenuCategory>?> reply = await MenuManager.GetCategories();
        _categories = reply.Data?.ToArray() ?? Array.Empty<MenuCategory>();
        
        if ( reply.Data is not null ) 
            return true;
        
        Logger.LogError( reply.Details() );
        return false;
    }
    async Task<bool> LoadMenuItems()
    {
        ApiReply<List<MenuItem>?> reply = await MenuManager.GetItems();
        _items = reply.Data?.ToArray() ?? Array.Empty<MenuItem>();

        if ( reply.Data is not null )
            return true;

        Logger.LogError( reply.Details() );
        return false;
    }
    async Task<bool> LoadMenuOptions()
    {
        ApiReply<MenuOptions?> reply = await MenuManager.GetOptions();
        _options = reply.Data ?? new MenuOptions();

        if ( reply.Data is not null )
            return true;

        Logger.LogError( reply.Details() );
        return false;
    }

    async Task ScrollToCategory( int categoryId )
    {
        await JsRuntime.InvokeVoidAsync( "scrollToCategory", categoryId );
    }
}