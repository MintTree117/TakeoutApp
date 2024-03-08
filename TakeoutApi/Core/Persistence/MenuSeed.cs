using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence;

public sealed class MenuSeed
{
    public static async Task SeedMenuAsync( EfContext context, int numOptionGroups, int maxOptionsPerGroup, int itemsPerCategory, int maxOptionsPerItem )
    {
        if ( !context.MenuCategories.Any() )
            await SeedMenuCategories( context );

        await context.MenuOptions.ExecuteDeleteAsync();
        await context.MenuOptionGroups.ExecuteDeleteAsync();
        await context.MenuItems.ExecuteDeleteAsync();
        
        if ( !context.MenuOptionGroups.Any() || !context.MenuOptions.Any() )
            await SeedMenuOptions( context, numOptionGroups, maxOptionsPerGroup );

        if ( !context.MenuItems.Any() )
            await SeedMenuItems( context, itemsPerCategory, maxOptionsPerItem );
    }

    static async Task SeedMenuCategories( EfContext context )
    {
        List<MenuCategory> categories =
        [
            new MenuCategory { Id = 1, Name = "Entrees" },
            new MenuCategory { Id = 2, Name = "Sides" },
            new MenuCategory { Id = 3, Name = "Drinks" },
            new MenuCategory { Id = 4, Name = "Desserts" },
            new MenuCategory { Id = 5, Name = "SingleItems" }
        ];

        await context.MenuCategories.AddRangeAsync( categories );
    }
    static async Task SeedMenuOptions( EfContext context, int numOptionGroups, int maxOptionsPerGroup )
    {
        List<MenuOptionGroup> groups = [ ];
        //List<MenuOption> options = [ ];
        
        for ( int i = 0; i < numOptionGroups; i++ )
        {
            MenuOptionGroup group = new()
            {
                Name = $"MenuOptionGroup: {i + 1}"
            };

            for ( int j = 0; j < maxOptionsPerGroup; j++ )
            {
                MenuOption option = new()
                {
                    Name = $"MenuOption: {i + 1},{j + 1}",
                    Price = 1.99m,
                    SalePrice = 0.99m,
                    MenuOptionGroup = group,
                    MenuOptionGroupId = group.Id
                };

                group.MenuOptions.Add( option );

                //options.Add( option );
            }
            
            groups.Add( group );
        }

        await context.AddRangeAsync( groups );
        await context.SaveChangesAsync();
    }
    static async Task SeedMenuItems( EfContext context, int itemsPerCategory, int maxOptionsPerItem )
    {
        List<MenuCategory> categories = await context.MenuCategories.ToListAsync();
        List<MenuOptionGroup> options = await context.MenuOptionGroups.ToListAsync();
        List<MenuItemOptionGroup> groups = [ ];
        List<MenuItem> items = [ ];
        Random rnd = new();

        foreach ( MenuCategory c in categories )
        {
            for ( int i = 0; i < itemsPerCategory; i++ )
            {
                items.Add( new MenuItem
                {
                    Name = $"{c.Name} : {i}",
                    ImageUrl = "",
                    Price = 9.99,
                    SalePrice = 5.99m,
                    MenuCategory = c,
                    MenuCategoryId = c.Id
                } );
            }
        }

        await context.MenuItems.AddRangeAsync( items );
        await context.SaveChangesAsync();

        foreach ( MenuItem i in items )
        {
            List<MenuOptionGroup> optionGroups = GetRandomGroups();

            foreach ( MenuOptionGroup o in optionGroups )
            {
                groups.Add( new MenuItemOptionGroup
                {
                    MenuItemId = i.Id,
                    MenuOptionGroupId = o.Id
                } );
            }
        }

        await context.MenuItemOptionGroups.AddRangeAsync( groups );
        await context.SaveChangesAsync();
        
        return;
        
        List<MenuOptionGroup> GetRandomGroups()
        {
            List<MenuOptionGroup> rndGroups = [ ];

            int count = rnd.Next( 1, maxOptionsPerItem + 1 ); // Ensure you're getting a number within the desired range

            while ( rndGroups.Count < count )
            {
                MenuOptionGroup option = options[ rnd.Next( options.Count ) ];

                if ( !rndGroups.Contains( option ) )
                    rndGroups.Add( option );
            }

            return rndGroups;
        }
    }
}