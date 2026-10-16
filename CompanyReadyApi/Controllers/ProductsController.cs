
using CompanyReadyApi.DTOs;
using CompanyReadyApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyReadyApi.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponseDto>> GetProductById(int id)
        {
            var product = await _service.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> CreateProduct(
            [FromBody] CreateProductDto dto)
        {
            var product = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = product.Id },
                product);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(
            int id, [FromBody] UpdateProductDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);

            if (!updated)
                return NotFound();

            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
