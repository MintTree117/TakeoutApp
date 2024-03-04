using Api.Errors;
using Api.Features.Identity.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Api.Features.Identity;

public static class IdentityEndpoints
{
    public static void MapIdentityEndpoints( this IEndpointRouteBuilder app )
    {
        app.MapPost( "api/identity/emailexists",
            async ( string email, UserManager<IdentityUser> userManager ) => 
                Results.Ok( await userManager.FindByEmailAsync( email ) is not null ) );
        
        app.MapPost( "api/identity/login", 
            async ( LoginDto login, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IJwtService jwtService ) => {
                IdentityUser? user =
                    await userManager.FindByEmailAsync( login.EmailOrUsername ) ??
                    await userManager.FindByNameAsync( login.EmailOrUsername );

                if ( user is null )
                    return Results.NotFound( new ApiError( ApiErrorType.NotFound, "User wasn't found" ) );

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
        
        app.MapPost( "api/identity/logout",
            [Authorize] async ( HttpContext http, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IJwtService jwtService ) => {
                IdentityUser? user = await userManager.FindByEmailAsync( http.User.Identity?.Name ?? "" );

                if ( user is null )
                    return Results.NotFound( new ApiError( ApiErrorType.NotFound, "User wasn't found" ) );

                await signInManager.SignOutAsync();

                return Results.Ok( "User signed out." );
            } );

        app.MapPost( "api/identity/register",
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