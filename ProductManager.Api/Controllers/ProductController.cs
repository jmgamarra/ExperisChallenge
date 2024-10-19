using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.Services;
using ProductManager.Domain.Entities;

namespace ProductManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{userId}")]
        public ActionResult<List<Product>> GetAllProducts(int userId)
        {
            var products = _productService.GetAllProducts(userId);
            if (products == null || products.Count == 0)
                return NotFound();

            return Ok(products);
        }

        [HttpGet("details/{id}")]
        public ActionResult<Product> GetProductById(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public ActionResult AddProduct([FromBody] Product product)
        {
            var result = _productService.AddProduct(product);
            if (!result)
                return BadRequest("Invalid product data.");

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            product.Id = id;
            var result = _productService.UpdateProduct(product);
            if (!result)
                return BadRequest("Failed to update product.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var result = _productService.DeleteProduct(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
