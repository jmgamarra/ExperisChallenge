using ProductManager.Domain.Entities;

namespace ProductManager.Application.Interfaces
{
    public interface IUserRepository
    {
        bool CreateUser(User user, string password);
        User GetByName(string name);
        UserSecurity GetUserSecurity(int userId);
    }
}
