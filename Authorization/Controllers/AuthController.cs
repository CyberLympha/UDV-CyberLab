using Authorization.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest userDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = userDto.UserName,
                FirstName = userDto.FirstName,
                SecondName = userDto.SecondName,
                Email = userDto.Email
            };
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
                return BadRequest(result);

            await _userManager.AddToRoleAsync(user, UserRole.Student);
            
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest userDto)
        {
            var user = await _userManager.FindByNameAsync(userDto.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, userDto.Password))
                return Unauthorized();

            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id)
            };
            foreach (var userRole in userRoles)
                claims.Add(new Claim(ClaimTypes.Role, userRole));

            var accessToken = CreateAccessToken(claims);
            
            return Ok(new { accessToken = new JwtSecurityTokenHandler().WriteToken(accessToken) });
        }

        [HttpGet("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            return Ok();
        }

        [HttpGet("hello")]
        [Authorize]
        public async Task<IActionResult> Hello()
        {
            var id = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return BadRequest();
            return Ok(new { firstName = user.FirstName, secondName = user.SecondName });
        }

        private JwtSecurityToken CreateAccessToken(List<Claim> userClaims)
        {
            var authSigningKey = AuthOptions.GetSymmetricSecurityKey();

            var token = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                expires: DateTime.Now.AddMinutes(AuthOptions.EXPIRES_MINUTES),
                claims: userClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
