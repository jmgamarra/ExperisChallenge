using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.Services;
using ProductManager.Domain.Entities;

namespace ProductManager.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public IActionResult AddProduct([FromBody] Product product)
        {
            var result = _productService.AddProduct(product);
            if (!result) return BadRequest("Invalid product data.");

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpGet("user/{userId}")]
        public ActionResult<List<Product>> GetAllProducts(int userId)
        {
            var products = _productService.GetAllProducts(userId);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();

            return Ok(product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id) return BadRequest();

            var result = _productService.UpdateProduct(product);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var result = _productService.DeleteProduct(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
