using Api.Errors;
using API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Menu;

public static class ReadMenuEndpoints
{
    public static void MapReadMenuEndpoints( this IEndpointRouteBuilder app )
    {
        const string baseRoute = "api/get";
        
        // Lists
        app.MapGet( $"{baseRoute}/menu-categories",
                async ( EfContext context ) => Results.Ok( await context.MenuCategories.ToListAsync() ) )
            .Produces( 200 )
            .Produces( 404 ).Produces<ApiError>( 404 );
        
        app.MapGet( $"{baseRoute}/menu-items",
            async ( EfContext context ) => Results.Ok( await context.MenuItems.ToListAsync() ) );
        
        app.MapGet( $"{baseRoute}/menu-options",
            async ( EfContext context ) => Results.Ok( await context.MenuOptions.ToListAsync() ) );
        
        app.MapGet( $"{baseRoute}/menu-option-groups",
            async ( EfContext context ) => Results.Ok( await context.MenuOptionGroups.ToListAsync() ) );
        
        // Singles
        app.MapGet( $"{baseRoute}/menu-category/{{id:int}}",
            async ( int id, EfContext db ) =>
                await db.MenuItems.FindAsync( id )
                    is { } category
                    ? Results.Ok( category )
                    : Results.NotFound() );

        app.MapGet( $"{baseRoute}/menu-item/{{id:int}}",
            async ( int id, EfContext db ) =>
                await db.MenuItems.FindAsync( id )
                    is { } item
                    ? Results.Ok( item )
                    : Results.NotFound() );

        app.MapGet( $"{baseRoute}/menu-option/{{id:int}}",
            async ( int id, EfContext db ) =>
                await db.MenuItems.FindAsync( id )
                    is { } option
                    ? Results.Ok( option )
                    : Results.NotFound() );

        app.MapGet( $"{baseRoute}/menu-option-group/{{id:int}}",
            async ( int id, EfContext db ) =>
                await db.MenuItems.FindAsync( id )
                    is { } group
                    ? Results.Ok( group )
                    : Results.NotFound() );
    }
}