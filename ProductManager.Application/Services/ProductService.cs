using ProductManager.Application.Interfaces;
using ProductManager.Domain.Entities;

namespace ProductManager.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public bool AddProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name) || product.Price < 0 || product.Quantity < 0)
                return false;

            return _productRepository.Create(product);
        }

        public List<Product> GetAllProducts(int userId)
        {
            return _productRepository.GetAll(userId);
        }

        public Product GetProductById(int id)
        {
            return _productRepository.GetById(id);
        }

        public bool UpdateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name) || product.Price < 0 || product.Quantity < 0)
                return false;

            return _productRepository.Update(product);
        }

        public bool DeleteProduct(int id)
        {
            return _productRepository.Delete(id);
        }
    }
}
