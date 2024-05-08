using Authorization.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabsController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "Authenticated")]
        public IActionResult GetLabs()
        {
            return Ok(new LabsResponse
            {
                Labs = new List<LabDto>
                {
                    new LabDto
                    {
                        Title = "Тестирование уязвимостей",
                        Deadline = new DateOnly(2024, 5, 12),
                        ShortDescription = "Короткое описание"
                    }
                }
            });
        }

        [HttpPatch("{id:int}")]
        [Authorize(Policy = "Teacher")]
        public IActionResult Edit()
        {
            return Ok();
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "Student")]
        public IActionResult Start()
        {
            return Ok();
        }
    }
}
