using System.ComponentModel.DataAnnotations;

namespace Authorization.Dto
{
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string SecondName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Role { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
