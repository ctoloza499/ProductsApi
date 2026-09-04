using ProductsApi.Application.DTOs;
using ProductsApi.Application.Interfaces;
using ProductsApi.Domain.Entities;
using ProductsApi.Domain.Exceptions;

namespace ProductsApi.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
        {
            if (request.Price < 0)
                throw new BusinessRuleException("El precio no puede ser negativo.");
            if (request.InitialStock < 0)
                throw new BusinessRuleException("El stock inicial no puede ser negativo.");

            var product = new Product(request.Name, request.Description, request.Price, request.InitialStock);
            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            return ToResponse(product);
        }

        public async Task<ProductResponse> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Producto con id {id} no encontrado.");
            return ToResponse(product);
        }

        public async Task<PagedResponse<ProductResponse>> GetPagedAsync(int page, int pageSize)
        {
            var (items, total) = await _repository.GetPagedAsync(page, pageSize);
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);

            return new PagedResponse<ProductResponse>(
                items.Select(ToResponse).ToList(), page, pageSize, total, totalPages);
        }

        public async Task<ProductResponse> UpdateStockAsync(int id, int quantity)
        {
            var product = await _repository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Producto con id {id} no encontrado.");

            product.AdjustStock(quantity); // lanza excepción si queda negativo
            await _repository.SaveChangesAsync();

            return ToResponse(product);
        }

        private static ProductResponse ToResponse(Product p) =>
            new(p.Id, p.Name, p.Description, p.Price, p.Stock, p.CreatedAt);
    }
}