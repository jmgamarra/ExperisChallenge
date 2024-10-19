using ProductManager.Application.Interfaces;
using ProductManager.Domain.Entities;

namespace ProductManager.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new List<Product>();

        public bool Create(Product product)
        {
            product.Id = _products.Count + 1; // Id
            _products.Add(product);
            return true;
        }

        public List<Product> GetAll(int userId)
        {
            return _products.Where(p => p.UserId == userId).ToList();
        }

        public Product GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public bool Update(Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null) return false;

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;
            return true;
        }

        public bool Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return false;

            _products.Remove(product);
            return true;
        }
    }
}
