using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;

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
        public async void AddProduct_ShouldReturnCreated_WhenProductIsValid()
        {
            // Arrange
            var product = new { Name = "Test Product", Price = 10.0 };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(product),
                System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/products", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }
    }
}
