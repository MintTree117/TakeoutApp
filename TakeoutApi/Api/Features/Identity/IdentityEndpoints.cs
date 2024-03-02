using Api.Errors;
using Api.Features.Identity.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Api.Features.Identity;

public static class IdentityEndpoints
{
    public static void MapReadMenuEndpoints( this IEndpointRouteBuilder app )
    {
        app.MapPost( "api/login", 
            async ( LoginDto login, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IJwtService jwtService ) => {
                IdentityUser? user = await userManager.FindByEmailAsync( login.Email );

                if ( user is null )
                    return Results.NotFound( new ApiError( ApiErrorType.NotFound, "User with email wasn't found" ) );

                SignInResult result = await signInManager.CheckPasswordSignInAsync( user, login.Password, false );

                if ( !result.Succeeded )
                    return Results.Unauthorized();

                return Results.Ok( new UserDto
                {
                    Email = user.Email ?? "",
                    Username = user.UserName ?? "",
                    Token = jwtService.CreateToken( user )
                } );
            } );

        app.MapPost( "api/register",
            async ( RegisterDto register, UserManager<IdentityUser> userManager, IJwtService jwtService ) => {
                IdentityUser user = new()
                {
                    UserName = register.Username,
                    Email = register.Email
                };

                IdentityResult result = await userManager.CreateAsync( user, register.Password );

                if ( !result.Succeeded )
                    return Results.BadRequest();

                return Results.Ok( new UserDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = jwtService.CreateToken( user )
                } );
            } );
    }
}