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

            if (userDto.Role == UserRole.Teacher)
                await _userManager.AddToRoleAsync(user, UserRole.Teacher);
            else if (userDto.Role == UserRole.Student)
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
            HttpContext.Response.Cookies
                .Append("access_token", 
                new JwtSecurityTokenHandler().WriteToken(accessToken), 
                new CookieOptions { HttpOnly = true, Expires = accessToken.ValidTo, Secure = true });
            
            return Ok();
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            var token = HttpContext.Request.Cookies["access_token"];
            if (token != null)
            {
                HttpContext.Response.Cookies
                    .Delete("access_token", 
                    new CookieOptions { 
                        HttpOnly = true, 
                        Expires = new JwtSecurityTokenHandler().ReadToken(token).ValidTo, 
                        Secure = true 
                    });
                return Ok();
            }
            return BadRequest();
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
