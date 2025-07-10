using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IProductRepository repo) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
            => Ok(await repo.GetProductsAsync(brand, type, sort));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await repo.GetProductByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> Getbrands()
        => Ok(await repo.GetBrandsAsync());

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        => Ok(await repo.GetTypesAsync());

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repo.AddProduct(product);
            return await repo.SaveChangesAsync()
                ? CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product)
                : BadRequest("Problem creating product!");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
            if (product.Id != id) return BadRequest("Id mismatch");
            if (!await repo.ExistsAsync(id)) return NotFound();
            repo.UpdateProduct(product);
            return await repo.SaveChangesAsync()
                ? NoContent()
                : BadRequest("Problem updating product!");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await repo.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            repo.DeleteProduct(product);
            return await repo.SaveChangesAsync()
                ? NoContent()
                : BadRequest("Problem deleting product!");
        }
    }
}