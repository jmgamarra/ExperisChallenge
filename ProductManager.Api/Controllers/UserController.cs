using Microsoft.AspNetCore.Mvc;
using ProductManager.Api.DTOs;
using ProductManager.Application.Services;

namespace ProductManager.Api.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] UserRegistrationRequest request)
        {
            var result = _userService.Create(request.UserName, request.Password);
            if (!result)
                return BadRequest(new { Message = "User registration failed. Either the user already exists or invalid data was provided." });

            return CreatedAtAction(nameof(GetUserByName), new { userName = request.UserName }, request);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] UserLoginRequest request)
        {
            var result = _userService.Login(request.UserName, request.Password);
            if (!result)
                return Unauthorized(new { Message = "Invalid credentials or user is inactive." });

            return Ok(new { Message = "Login successful." });
        }

        [HttpGet("{userName}")]
        public IActionResult GetUserByName(string userName)
        {
            var user = _userService.GetUser(userName);
            if (user == null)
                return NotFound(new { Message = "User not found." });

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name
            };

            return Ok(userDto);
        }
    }
}
