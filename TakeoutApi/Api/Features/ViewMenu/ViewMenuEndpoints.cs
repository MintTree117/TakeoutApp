using API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.ViewMenu;

public static class ReadMenuEndpoints
{
    public static void MapReadMenuEndpoints( this IEndpointRouteBuilder app )
    {
        const string baseRoute = "api/get";
        
        app.MapGet( $"{baseRoute}/menu-categories",
            async ( EfContext context ) => Results.Ok( await context.MenuCategories.ToListAsync() ) );

        app.MapGet( $"{baseRoute}/menu-items",
            async ( EfContext context ) => Results.Ok( await context.MenuItems.ToListAsync() ) );

        app.MapGet( $"{baseRoute}/menu-options",
            async ( EfContext context ) => Results.Ok( await context.MenuOptions.ToListAsync() ) );

        app.MapGet( $"{baseRoute}/menu-option-groups",
            async ( EfContext context ) => Results.Ok( await context.MenuOptionGroups.ToListAsync() ) );

        app.MapGet( $"{baseRoute}/menu-category/{{id}}",
            async ( string id, EfContext context ) =>
                !int.TryParse( id, out int parsedId )
                    ? null
                    : Results.Ok( ( object? )
                        await context.MenuItems.FindAsync( parsedId ) ) );

        app.MapGet( $"{baseRoute}/menu-item/{{id}}",
            async ( string id, EfContext context ) =>
                !int.TryParse( id, out int parsedId )
                    ? null
                    : Results.Ok( ( object? )
                        await context.MenuItems.FindAsync( parsedId ) ) );

        app.MapGet( $"{baseRoute}/menu-option/{{id}}",
            async ( string id, EfContext context ) =>
                !int.TryParse( id, out int parsedId )
                    ? null
                    : Results.Ok( ( object? )
                        await context.MenuItems.FindAsync( parsedId ) ) );

        app.MapGet( $"{baseRoute}/menu-option-group/{{id}}",
            async ( string id, EfContext context ) =>
                !int.TryParse( id, out int parsedId )
                    ? null
                    : Results.Ok( ( object? )
                        await context.MenuItems.FindAsync( parsedId ) ) );
    }
}