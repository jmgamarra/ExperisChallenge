using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public void CreateUser_ShouldReturnTrue_WhenUserIsValid()
        {
            // Arrange
            var userService = new UserService(); // No hay inyección ni repositorios aún
            var user = new User { UserName = "testuser", Password = "password123" };

            // Act
            var result = userService.Create(user);

            // Assert
            Assert.True(result); // Este test fallará 
        }
    }
}
