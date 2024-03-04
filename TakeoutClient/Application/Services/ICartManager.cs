using Application.Dtos;

namespace Application.Services;

public interface ICartManager
{
    Task<CartSummary> GetCartSummary();
    Task UpdateServerCart( UserDto? user );
}