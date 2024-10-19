using ProductManager.Application.Services;

namespace ProductManager.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public void CreateUser_ShouldReturnTrue_WhenUserIsValid()
        {
            // Arrange
            var userService = new UserService();
            var name = "testuser";
            var password = "password123";

            // Act
            var result = userService.Create(name, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CreateUser_ShouldReturnFalse_WhenUserNameIsDuplicate()
        {
            // Arrange
            var userService = new UserService();
            userService.Create("testuser", "password123");

            // Act
            var result = userService.Create("testuser", "anotherpassword");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Login_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            // Arrange
            var userService = new UserService();
            userService.Create("testuser", "password123");

            // Act
            var result = userService.Login("testuser", "password123");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Login_ShouldReturnFalse_WhenPasswordIsIncorrect()
        {
            // Arrange
            var userService = new UserService();
            userService.Create("testuser", "password123");

            // Act
            var result = userService.Login("testuser", "wrongpassword");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DeactivateUser_ShouldReturnTrue_WhenUserIsDeactivated()
        {
            // Arrange
            var userService = new UserService();
            userService.Create("testuser", "password123");
            var user = userService.GetAllUsers().First();

            // Act
            var result = userService.DeactivateUser(user.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DeactivateUser_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var userService = new UserService();

            // Act
            var result = userService.DeactivateUser(99); // Usuario inexistente

            // Assert
            Assert.False(result);
        }
    }

}
