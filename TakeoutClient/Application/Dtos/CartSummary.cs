namespace Application.Dtos;

public sealed record CartSummary
{
    public int Count { get; init; }
    public decimal Price { get; init; }
}