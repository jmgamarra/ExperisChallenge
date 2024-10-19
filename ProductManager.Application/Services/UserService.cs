using ProductManager.Application.Interfaces;
using ProductManager.Domain.Entities;

namespace ProductManager.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserSecurityRepository _userSecurityRepository;

        public UserService(IUserRepository userRepository, IUserSecurityRepository userSecurityRepository)
        {
            _userRepository = userRepository;
            _userSecurityRepository = userSecurityRepository;
        }
        public User GetUser(string userName)
        {
            // Llamamos al repositorio para obtener el usuario por nombre
            return _userRepository.GetByName(userName);
        }

        public bool Create(string name, string password)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
                return false;

            var existingUser = _userRepository.GetByName(name);
            if (existingUser != null)
                return false; // Usuario duplicado

            var user = new User { Name = name };
            _userRepository.CreateUser(existingUser, password);

            var passwordHash = HashPassword(password);
            var userSecurity = new UserSecurity
            {
                UserId = user.Id,
                PasswordHash = passwordHash,
                IsActive = true
            };
            return _userSecurityRepository.Create(userSecurity);
        }

        public bool Login(string userName, string password)
        {
            var user = _userRepository.GetByName(userName);
            if (user == null) return false;

            var userSecurity = _userSecurityRepository.GetByUserId(user.Id);
            if (userSecurity == null || !VerifyPassword(password, userSecurity.PasswordHash))
                return false;

            return userSecurity.IsActive;
        }

        private string HashPassword(string password)
        {
            // Implementación simple de hashing (no segura, solo ejemplo)
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == hash;
        }
    }

}
