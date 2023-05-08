using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet(Name = "GetUser"), Authorize(Roles = "User")]
        public async Task<ActionResult<User>> GetUser()
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            var userId = httpContext?.User.FindFirst(claim => claim.Type == "Id")?.Value;
            var candidate = await _userService.GetAsyncById(userId);
            return candidate;
        }
    }
}