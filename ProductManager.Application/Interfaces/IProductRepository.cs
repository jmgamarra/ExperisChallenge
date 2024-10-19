using ProductManager.Domain.Entities;

namespace ProductManager.Application.Interfaces
{
    public interface IProductRepository
    {
        bool Create(Product product);
        List<Product> GetAll(int userId);
        Product GetById(int id);
        bool Update(Product product);
        bool Delete(int id);
    }
}
