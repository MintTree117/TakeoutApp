using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Api.Features.Identity;

public class JwtService : IJwtService
{
    readonly IConfiguration _configuration;
    readonly SymmetricSecurityKey _securityKey;

    public JwtService( IConfiguration configuration )
    {
        _configuration = configuration;
        _securityKey =
            new SymmetricSecurityKey( Encoding.UTF8.GetBytes(
                _configuration[ "Token:Key" ] ?? throw new Exception( "Failed to get Jwt key!" ) ) );
    }

    public string CreateToken( IdentityUser user )
    {
        List<Claim> claims =
        [
            new Claim( ClaimTypes.Email, user.Email! ),
            new Claim( ClaimTypes.Name, user.UserName! )
        ];

        SigningCredentials creds = new( _securityKey, SecurityAlgorithms.HmacSha512Signature );

        SecurityTokenDescriptor descr = new()
        {
            Subject = new ClaimsIdentity( claims ),
            Expires = DateTime.Now.AddDays( 1 ),
            SigningCredentials = creds,
            Issuer = _configuration[ "Token:Issuer" ]
        };

        JwtSecurityTokenHandler handler = new();

        SecurityToken token = handler.CreateToken( descr );

        return handler.WriteToken( token );
    }
}