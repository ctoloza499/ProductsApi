using Microsoft.EntityFrameworkCore;
using ProductsApi.Application.Interfaces;
using ProductsApi.Domain.Entities;
using ProductsApi.Infrastructure.Data;

namespace ProductsApi.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetByIdAsync(int id) =>
            await _context.Products.FindAsync(id);

        public async Task<(List<Product>, int)> GetPagedAsync(int page, int pageSize)
        {
            var total = await _context.Products.CountAsync();
            var items = await _context.Products
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task AddAsync(Product product) =>
            await _context.Products.AddAsync(product);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}