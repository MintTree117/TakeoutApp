using Application.Dtos;

namespace Application.Services;

public interface IIdentityManager
{
    Task<UserDto?> GetIdentity();
    Task<UserDto?> Login( LoginDto login );
    Task Logout();
}