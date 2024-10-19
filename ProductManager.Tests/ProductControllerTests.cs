using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;

namespace ProductManager.Tests
{
    public class ProductControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnOk_WhenProductsExist()
        {
            var response = await _client.GetAsync("/api/Product/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AddProduct_ShouldReturnCreated_WhenProductIsValid()
        {
            var product = new { Name = "Laptop", Price = 1000m, Quantity = 10, UserId = 1 };
            var response = await _client.PostAsJsonAsync("/api/Product", product);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
