using VirtualLab.Domain.Interfaces.Proxmox;

namespace VirtualLab.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class IpController : ControllerBase
{
    private readonly IProxmoxVm
        _service; // Замените YourService на имя вашего сервиса, который содержит метод GetIp

    public IpController(IProxmoxVm service) // Конструктор для внедрения зависимостей
    {
        _service = service;
    }

    [HttpGet("{node}/{qemu:int}")]
    public async Task<IActionResult> Get(string node, int qemu)
    {
        try
        {
            var result = await _service.GetIp(node, qemu);
            if (result.IsSuccess)
            {
                return Ok(result.Value); // Возвращает IP-адрес, если найден
            }

            return NotFound(result.Errors); // Возвращает сообщение об ошибке, если IP-адрес не найден
        }
        catch (Exception ex)
        {
            // Обработка исключений
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}