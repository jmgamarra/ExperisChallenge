using ProductManager.Domain.Entities;

namespace ProductManager.Application.Interfaces
{
    public interface IUserSecurityRepository
    {
        bool Create(UserSecurity userSecurity);
        UserSecurity GetByUserId(int userId);
    }
}
