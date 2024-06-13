using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Model.AuthModels;
using WebApi.Model.AuthModels.Requests;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public AuthController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<int>> Register(RegistrationRequest request)
        {
            string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User()
            {
                Email = request.Email,
                Password = passwordHash,
                Role = request.Role,
                FirstName = request.FirstName,
                SecondName = request.SecondName,
                IsApproved = true
            };
            try
            {
                await _userService.CreateAsync(user);
            }
            catch (Exception e)
            {
                return Conflict(new { message = $"An existing user with email '{request.Email}' was already found." });
            }

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            var user = await _userService.GetAsync(request.Email);
            
            if (user == null )
            {
                return StatusCode(400, "User doesnt found");
            };

            if (user.IsApproved == false)
            {
                return StatusCode(403);
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(user);

            return Ok(new
            {
                token,
                user,
            });
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("Id", user.Id),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Key").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}