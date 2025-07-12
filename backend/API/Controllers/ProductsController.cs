using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IGenericRepository<Product> repo) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductSpecification(specParams);
            return await CreatePagedResult(repo, spec, specParams.PageIndex, specParams.PageSize);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product?>> GetProductById(int id) => await repo.GetByIdAsync(id);

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> Getbrands()
        {
            var spec = new BrandListSpecification();
            return Ok(await repo.ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            return Ok(await repo.ListAsync(spec));
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repo.Add(product);
            return await repo.SaveChangesAsync()
                ? CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product)
                : BadRequest("Problem creating product!");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
            if (product.Id != id) return BadRequest("Id mismatch");
            if (!await repo.ExistsAsync(id)) return NotFound();
            repo.Update(product);
            return await repo.SaveChangesAsync()
                ? NoContent()
                : BadRequest("Problem updating product!");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await repo.GetByIdAsync(id);
            if (product == null) return NotFound();
            repo.Remove(product);
            return await repo.SaveChangesAsync()
                ? NoContent()
                : BadRequest("Problem deleting product!");
        }
    }
}