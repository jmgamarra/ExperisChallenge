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
                // Inserta el usuario en la tabla Users
                var insertUserQuery =
                    "INSERT INTO Users (UserName) OUTPUT INSERTED.UserId VALUES (@UserName);";
                var userId = _connection.ExecuteScalar<int>(insertUserQuery, new { user.UserName }, transaction);
                Console.WriteLine($"User ID: {userId}");
                // Hash del password
                var passwordHash = HashPassword(password);

                // Inserta la seguridad del usuario en UserSecurity
                var insertSecurityQuery =
                    "INSERT INTO UserSecurity (UserId, PasswordHash, IsActive) VALUES (@UserId, @PasswordHash, @IsActive);";
                _connection.Execute(insertSecurityQuery, new
                {
                    UserId = userId,
                    PasswordHash = passwordHash,
                    IsActive = true
                }, transaction);

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return false;
            }
        }

        public User GetByName(string name)
        {
            var query = "SELECT UserId as Id,UserName FROM Users WHERE UserName = @UserName;";
            return _connection.QueryFirstOrDefault<User>(query, new { UserName = name });
        }

        public UserSecurity GetUserSecurity(int userId)
        {
            var query = "SELECT * FROM UserSecurity WHERE UserId = @UserId;";
            return _connection.QueryFirstOrDefault<UserSecurity>(query, new { UserId = userId });
        }

        private string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
