namespace ProductsApi.Application.DTOs
{
    public record CreateProductRequest(string Name, string Description, decimal Price, int InitialStock);

    public record UpdateStockRequest(int Quantity); // positivo suma, negativo resta

    public record ProductResponse(int Id, string Name, string Description, decimal Price, int Stock, DateTime CreatedAt);

    public record PagedResponse<T>(List<T> Items, int Page, int PageSize, int TotalItems, int TotalPages);
}