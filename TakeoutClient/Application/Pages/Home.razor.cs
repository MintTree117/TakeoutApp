using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Components;

namespace Application.Pages;

public partial class Home
{
    [Inject] IHttpService _httpService { get; init; } = default!;
    
    event Action? OptionsLoaded;
    
    MenuCategory[] _categories = [ ];
    MenuItem[] _items = [ ];
    MenuOptions _options = new();
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        List<Task> tasks =
        [
            LoadMenuCategories(),
            LoadMenuItems()
        ];

        await Task.WhenAll( tasks );

        _ = LoadMenuOptions();
    }

    async Task LoadMenuCategories()
    {
        ServiceReply<List<MenuCategory>?> reply = 
            await _httpService.TryGetRequest<List<MenuCategory>>( "api/menu/get/categories" );

        if ( reply.Data is null )
        {
            return;
        }

        _categories = reply.Data.ToArray();
    }
    async Task LoadMenuItems()
    {
        ServiceReply<List<MenuItem>?> reply =
            await _httpService.TryGetRequest<List<MenuItem>>( "api/menu/get/items" );

        if ( reply.Data is null )
        {
            return;
        }

        _items = reply.Data.ToArray();
    }
    async Task LoadMenuOptions()
    {
        ServiceReply<MenuOptions?> reply =
            await _httpService.TryGetRequest<MenuOptions>( "api/menu/get/options" );

        if ( reply.Data is null )
        {
            return;
        }

        _options = reply.Data;

        OptionsLoaded?.Invoke();
    }
}