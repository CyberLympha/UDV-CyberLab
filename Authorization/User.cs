using Microsoft.AspNetCore.Identity;

namespace Authorization
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
    }
}
