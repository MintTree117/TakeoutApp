using Microsoft.AspNetCore.Identity;

namespace Api.Features.Identity;

public interface IJwtService
{
    string CreateToken( IdentityUser user );
}