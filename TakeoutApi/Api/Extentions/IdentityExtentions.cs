using System.Text;
using API.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Api.Extentions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices( this IServiceCollection services, IConfiguration config )
        {
            services.AddIdentityCore<IdentityUser>( opt => { opt.Password.RequireDigit = true; } )
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<EfContext>()
                .AddSignInManager<SignInManager<IdentityUser>>()
                .AddRoleManager<RoleManager<IdentityRole>>();

            services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
                .AddJwtBearer( options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes( config[ "Token:Key" ] ?? throw new Exception( "Failed to get Jwt Key!" ) ) ),
                        ValidIssuer = config[ "Token:Issuer" ],
                        ValidateIssuer = true,
                        ValidateAudience = false
                    };
                } );

            services.AddAuthorization();

            return services;
        }
    }
}