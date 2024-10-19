namespace ProductManager.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public void AddProduct_ShouldReturnTrue_WhenProductIsValid()
        {
            // Arrange
            var product = new Product { Name = "Laptop", Price = 1000m, Quantity = 10 };

            // Act
            var productService = new ProductService(); // Este servicio aún no está implementado
            var result = productService.AddProduct(product);

            // Assert
            Assert.True(result);
        }
    }
}
