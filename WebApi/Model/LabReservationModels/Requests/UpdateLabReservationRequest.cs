using Microsoft.Build.Framework;
using WebApi.Model.LabModels;

namespace WebApi.Model.LabReservationModels.Requests;

public class UpdateLabReservationRequest
{
    [Required] public string Id { get; set; } = null!;
    [Required] public string Theme { get; set; } = null!;
    [Required] public string Description { get; set; } = null!;
    [Required] public long TimeStart { get; set; }
    [Required] public long TimeEnd { get; set; }
    [Required] public string ReservorId { get; set; } = null!;
    [Required] public Lab Lab { get; set; } = null!;
    [Required] public string CurrentUserId { get; set; } = null!;
}