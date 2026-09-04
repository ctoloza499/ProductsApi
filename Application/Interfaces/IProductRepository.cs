using ProductsApi.Domain.Entities;

namespace ProductsApi.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<(List<Product> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
        Task AddAsync(Product product);
        Task SaveChangesAsync();
    }
}