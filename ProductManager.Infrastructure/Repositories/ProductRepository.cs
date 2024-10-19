using Dapper;
using ProductManager.Application.Interfaces;
using ProductManager.Domain.Entities;
using System.Data;

namespace ProductManager.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _connection;

        public ProductRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public bool Create(Product product)
        {
            var query = "INSERT INTO Products (Name, Price, Quantity, UserId) VALUES (@Name, @Price, @Quantity, @UserId);";
            var result = _connection.Execute(query, product);
            return result > 0;
        }

        public List<Product> GetAll(int userId)
        {
            var query = "SELECT * FROM Products WHERE UserId = @UserId;";
            return _connection.Query<Product>(query, new { UserId = userId }).ToList();
        }

        public Product GetById(int id)
        {
            var query = "SELECT * FROM Products WHERE ProductId = @Id;";
            return _connection.QueryFirstOrDefault<Product>(query, new { Id = id });
        }

        public bool Update(Product product)
        {
            var query = "UPDATE Products SET Name = @Name, Price = @Price, Quantity = @Quantity WHERE ProductId = @Id;";
            var result = _connection.Execute(query, product);
            return result > 0;
        }

        public bool Delete(int id)
        {
            var query = "DELETE FROM Products WHERE ProductId = @Id;";
            var result = _connection.Execute(query, new { Id = id });
            return result > 0;
        }
    }
}
