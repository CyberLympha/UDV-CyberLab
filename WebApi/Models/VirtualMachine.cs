using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class Vm
{
    [Required] public int Vmid { get; set; } = 0;
    [Required] public string Name { get; set; } = null!;
}