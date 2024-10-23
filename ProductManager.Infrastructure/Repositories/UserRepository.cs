using Dapper;
using ProductManager.Application.Interfaces;
using ProductManager.Domain.Entities;
using System.Data;

namespace ProductManager.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public bool CreateUser(User user, string password)
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            using var transaction = _connection.BeginTransaction();
            try
            {
                var passwordHash = HashPassword(password);

                var parameters = new DynamicParameters();
                parameters.Add("@UserName", user.UserName);
                parameters.Add("@PasswordHash", passwordHash);
                parameters.Add("@IsActive", true);

                // Llamar al procedimiento almacenado CreateUser
                _connection.Execute(
                    "CreateUser",
                    parameters,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure
                );

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

        public User GetByName(string name)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserName", name);

            return _connection.QueryFirstOrDefault<User>(
                "GetUserByName",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public UserSecurity GetUserSecurity(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            return _connection.QueryFirstOrDefault<UserSecurity>(
                "GetUserSecurityByUserId",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        private string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
