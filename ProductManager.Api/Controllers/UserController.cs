using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductManager.Api.Config;
using ProductManager.Api.DTOs;
using ProductManager.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductManager.Api.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtSettings _jwtSettings;
        public UserController(UserService userService, JwtSettings jwtSettings)
        {
            _userService = userService;
            _jwtSettings = jwtSettings;
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

            // Generar el token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, request.UserName)
                }),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiresInHours),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
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
                Name = user.UserName
            };

            return Ok(userDto);
        }
    }
}
