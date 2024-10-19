using ProductManager.Domain.Entities;

namespace ProductManager.Application.Services
{
    public class UserService
    {
        private readonly List<User> _users = new List<User>();
        private readonly List<UserSecurity> _userSecurities = new List<UserSecurity>();

        public bool Create(string name, string password)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
                return false;

            // Verificar si el usuario ya existe
            if (_users.Any(u => u.Name == name))
                return false;

            // Crear usuario y agregarlo a la lista
            var user = new User { Id = _users.Count + 1, Name = name };
            _users.Add(user);

            // Crear la seguridad del usuario con la contraseña encriptada
            var passwordHash = HashPassword(password);
            var userSecurity = new UserSecurity
            {
                UserId = user.Id,
                SecurityId = _userSecurities.Count + 1,
                PasswordHash = passwordHash,
                IsActive = true
            };
            _userSecurities.Add(userSecurity);

            return true;
        }

        public bool Login(string name, string password)
        {
            // Buscar el usuario por nombre
            var user = _users.FirstOrDefault(u => u.Name == name);
            if (user == null) return false;

            // Obtener la seguridad del usuario
            var userSecurity = _userSecurities.FirstOrDefault(us => us.UserId == user.Id);
            if (userSecurity == null || !userSecurity.IsActive)
                return false;

            // Verificar la contraseña
            var passwordHash = HashPassword(password);
            return userSecurity.PasswordHash == passwordHash;
        }

        public bool DeactivateUser(int userId)
        {
            var userSecurity = _userSecurities.FirstOrDefault(us => us.UserId == userId);
            if (userSecurity == null) return false;

            userSecurity.IsActive = false;
            return true;
        }

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public User GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        // Método auxiliar para simular el hash de la contraseña
        private string HashPassword(string password)
        {
            // Usar una simulación de hash. En producción, usar un algoritmo seguro como BCrypt.
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

}
