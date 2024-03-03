using Api.Features.Identity;
using Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Extentions;

public static class ApplicationExtentions
{
    public static IServiceCollection AddApplicationServices( this IServiceCollection services, IConfiguration config )
    {
        services.AddDbContext<EfContext>( opt => {
            opt.UseSqlite( config.GetConnectionString( "DefaultConnection" ) );
        } );

        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}