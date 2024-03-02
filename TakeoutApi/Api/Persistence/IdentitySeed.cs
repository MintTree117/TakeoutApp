using Microsoft.AspNetCore.Identity;

namespace API.Persistence;

public class IdentitySeed
{
    public static async Task SeedUsersAsync( UserManager<IdentityUser> userManager )
    {
        if ( userManager.Users.Any() )
            return;

        IdentityUser user = new()
        {
            UserName = "Martin",
            Email = "martygrof3708@gmail.com"
        };

        await userManager.CreateAsync( user, "Password1?" );
    }
}