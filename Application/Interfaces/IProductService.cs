using ProductsApi.Application.DTOs;

namespace ProductsApi.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse> CreateAsync(CreateProductRequest request);
        Task<ProductResponse> GetByIdAsync(int id);
        Task<PagedResponse<ProductResponse>> GetPagedAsync(int page, int pageSize);
        Task<ProductResponse> UpdateStockAsync(int id, int quantity);
    }
}