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

        services.AddCors( options => {
            options.AddPolicy( "CorsPolicy", policy => {
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins( "https://localhost:4200" );
            } );
        } );
        
        return services;
    }
}