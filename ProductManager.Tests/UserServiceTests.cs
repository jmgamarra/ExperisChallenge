using Moq;
using ProductManager.Application.Interfaces;
using ProductManager.Application.Services;
using ProductManager.Domain.Entities;

namespace ProductManager.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public void CreateUser_ShouldReturnTrue_WhenUserIsValid()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockSecurityRepository = new Mock<IUserSecurityRepository>();

            mockUserRepository.Setup(repo => repo.GetByName(It.IsAny<string>())).Returns((User)null);
            mockUserRepository.Setup(repo => repo.CreateUser(It.IsAny<User>(), "password123")).Returns(true);

            mockSecurityRepository
                .Setup(repo => repo.Create(It.IsAny<UserSecurity>()))
                .Returns(true);

            var userService = new UserService(mockUserRepository.Object, mockSecurityRepository.Object);

            // Act
            var result = userService.Create("testuser", "password123");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CreateUser_ShouldReturnFalse_WhenUserAlreadyExists()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockSecurityRepository = new Mock<IUserSecurityRepository>();

            var existingUser = new User { Name = "testuser" };
            mockUserRepository.Setup(repo => repo.GetByName(existingUser.Name)).Returns(existingUser);

            var userService = new UserService(mockUserRepository.Object, mockSecurityRepository.Object);

            // Act
            var result = userService.Create(existingUser.Name, "password123");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Login_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockSecurityRepository = new Mock<IUserSecurityRepository>();

            var user = new User { Id = 1, Name = "testuser" };
            var userSecurity = new UserSecurity
            {
                UserId = 1,
                PasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("password123")),
                IsActive = true
            };

            mockUserRepository.Setup(repo => repo.GetByName(user.Name)).Returns(user);
            mockSecurityRepository.Setup(repo => repo.GetByUserId(user.Id)).Returns(userSecurity);

            var userService = new UserService(mockUserRepository.Object, mockSecurityRepository.Object);

            // Act
            var result = userService.Login(user.Name, "password123");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Login_ShouldReturnFalse_WhenPasswordIsIncorrect()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockSecurityRepository = new Mock<IUserSecurityRepository>();

            var user = new User { Id = 1, Name = "testuser" };
            var userSecurity = new UserSecurity
            {
                UserId = 1,
                PasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("password123")),
                IsActive = true
            };

            mockUserRepository.Setup(repo => repo.GetByName(user.Name)).Returns(user);
            mockSecurityRepository.Setup(repo => repo.GetByUserId(user.Id)).Returns(userSecurity);

            var userService = new UserService(mockUserRepository.Object, mockSecurityRepository.Object);

            // Act
            var result = userService.Login(user.Name, "wrongpassword");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Login_ShouldReturnFalse_WhenUserIsInactive()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockSecurityRepository = new Mock<IUserSecurityRepository>();

            var user = new User { Id = 1, Name = "testuser" };
            var userSecurity = new UserSecurity
            {
                UserId = 1,
                PasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("password123")),
                IsActive = false
            };

            mockUserRepository.Setup(repo => repo.GetByName(user.Name)).Returns(user);
            mockSecurityRepository.Setup(repo => repo.GetByUserId(user.Id)).Returns(userSecurity);

            var userService = new UserService(mockUserRepository.Object, mockSecurityRepository.Object);

            // Act
            var result = userService.Login(user.Name, "password123");

            // Assert
            Assert.False(result);
        }
    }
}
