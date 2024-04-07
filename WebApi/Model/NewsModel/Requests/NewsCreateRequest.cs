using Microsoft.Build.Framework;

namespace WebApi.Model.NewsModel.Requests;

public class NewsCreateRequest
{
        [Required] public string title { get; set; } = null!;
        [Required] public string text { get; set; } = null!;
        [Required] public string createdAt { get; set; } = null!;
}