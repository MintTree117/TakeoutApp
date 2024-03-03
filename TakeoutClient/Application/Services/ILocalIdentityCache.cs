using Application.Dtos;

namespace Application.Services;

public interface ILocalIdentityCache
{
    Task<UserDto?> GetIdentity();
    Task SetIdentity( UserDto? user );
}