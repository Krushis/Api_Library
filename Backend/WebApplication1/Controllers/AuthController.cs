using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryBackend.Services;

namespace LibraryBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;

        public AuthController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _jwtKey = configuration["Jwt:Key"];
            _jwtIssuer = configuration["Jwt:Issuer"];
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            var user = _userService.ValidateUser(model.Username, model.Password);

            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var key = Encoding.UTF8.GetBytes(_jwtKey); // Make sure this is 16+ bytes since I ran into a problem with that

            if (key.Length < 16)
            {
                return BadRequest("JWT key must be at least 128 bits.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.UserName)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _jwtIssuer,
                Audience = _jwtIssuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
