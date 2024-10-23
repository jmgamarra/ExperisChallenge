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
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userSecurity.UserId);
            parameters.Add("@PasswordHash", userSecurity.PasswordHash);
            parameters.Add("@IsActive", userSecurity.IsActive);

            var result = _connection.Execute(
                "CreateUserSecurity",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result > 0;
        }

        public UserSecurity GetByUserId(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            return _connection.QueryFirstOrDefault<UserSecurity>(
                "GetUserSecurityByUserId",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }

}
