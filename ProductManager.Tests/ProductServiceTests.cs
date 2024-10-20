﻿using Moq;
using ProductManager.Application.Interfaces;
using ProductManager.Application.Services;
using ProductManager.Domain.Entities;

namespace ProductManager.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public void AddProduct_ShouldReturnTrue_WhenProductIsValid()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.Create(It.IsAny<Product>())).Returns(true);

            var productService = new ProductService(mockRepository.Object);
            var product = new Product { Name = "Laptop", Price = 1000m, Quantity = 10, UserId = 1 };

            // Act
            var result = productService.AddProduct(product);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AddProduct_ShouldReturnFalse_WhenProductNameIsEmpty()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var productService = new ProductService(mockRepository.Object);
            var product = new Product { Name = "", Price = 1000m, Quantity = 10, UserId = 1 };

            // Act
            var result = productService.AddProduct(product);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetAllProducts_ShouldReturnProductsForSpecificUser()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var userId = 1;
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 1000m, Quantity = 10, UserId = userId },
                new Product { Id = 2, Name = "Phone", Price = 500m, Quantity = 5, UserId = userId }
            };

            mockRepository.Setup(repo => repo.GetAll(userId)).Returns(products);

            var productService = new ProductService(mockRepository.Object);

            // Act
            var result = productService.GetAllProducts(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetProductById_ShouldReturnCorrectProduct_WhenProductExists()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var product = new Product { Id = 1, Name = "Laptop", Price = 1000m, Quantity = 10, UserId = 1 };

            mockRepository.Setup(repo => repo.GetById(1)).Returns(product);

            var productService = new ProductService(mockRepository.Object);

            // Act
            var result = productService.GetProductById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Laptop", result.Name);
        }

        [Fact]
        public void UpdateProduct_ShouldReturnTrue_WhenProductIsValid()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var product = new Product { Id = 1, Name = "Laptop", Price = 1000m, Quantity = 10, UserId = 1 };

            mockRepository.Setup(repo => repo.Update(It.IsAny<Product>())).Returns(true);

            var productService = new ProductService(mockRepository.Object);

            // Act
            var result = productService.UpdateProduct(product);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteProduct_ShouldReturnTrue_WhenProductExists()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();

            mockRepository.Setup(repo => repo.Delete(1)).Returns(true);

            var productService = new ProductService(mockRepository.Object);

            // Act
            var result = productService.DeleteProduct(1);

            // Assert
            Assert.True(result);
        }
    }
}
