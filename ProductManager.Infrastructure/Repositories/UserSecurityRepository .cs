using Dapper;
using ProductManager.Application.Interfaces;
using ProductManager.Domain.Entities;
using System.Data;

namespace ProductManager.Infrastructure.Repositories
{
    public class UserSecurityRepository : IUserSecurityRepository
    {
        private readonly IDbConnection _connection;

        public UserSecurityRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public bool Create(UserSecurity userSecurity)
        {
            var query = @"
                INSERT INTO UserSecurity (UserId, PasswordHash, IsActive) 
                VALUES (@UserId, @PasswordHash, @IsActive);";

            var result = _connection.Execute(query, userSecurity);
            return result > 0;
        }

        public UserSecurity GetByUserId(int userId)
        {
            var query = "SELECT * FROM UserSecurity WHERE UserId = @UserId;";
            return _connection.QueryFirstOrDefault<UserSecurity>(query, new { UserId = userId });
        }
    }

}
