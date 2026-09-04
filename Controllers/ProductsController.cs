using Microsoft.AspNetCore.Mvc;
using ProductsApi.Application.DTOs;
using ProductsApi.Application.Interfaces;

namespace ProductsApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // POST /api/products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new { error = "El nombre es obligatorio." });

            var product = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // GET /api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            return Ok(product);
        }

        // GET /api/products?page=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var result = await _service.GetPagedAsync(page, pageSize);
            return Ok(result);
        }

        // PATCH /api/products/{id}/stock
        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockRequest request)
        {
            var product = await _service.UpdateStockAsync(id, request.Quantity);
            return Ok(product);
        }
    }
}