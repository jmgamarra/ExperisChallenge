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
            var parameters = new DynamicParameters();
            parameters.Add("@Name", product.Name);
            parameters.Add("@Price", product.Price);
            parameters.Add("@Quantity", product.Quantity);
            parameters.Add("@UserId", product.UserId);

            var result = _connection.Execute("CreateProduct", parameters, commandType: CommandType.StoredProcedure);
            return result > 0;
        }

        public List<Product> GetAll(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            return _connection.Query<Product>("GetAllProductsByUser", parameters, commandType: CommandType.StoredProcedure).ToList();
        }

        public Product GetById(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", id);

            return _connection.QueryFirstOrDefault<Product>("GetProductById", parameters, commandType: CommandType.StoredProcedure);
        }

        public bool Update(Product product)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", product.Id);
            parameters.Add("@Name", product.Name);
            parameters.Add("@Price", product.Price);
            parameters.Add("@Quantity", product.Quantity);

            var result = _connection.Execute("UpdateProduct", parameters, commandType: CommandType.StoredProcedure);
            return result > 0;
        }

        public bool Delete(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", id);

            var result = _connection.Execute("DeleteProduct", parameters, commandType: CommandType.StoredProcedure);
            return result > 0;
        }
    }
}
