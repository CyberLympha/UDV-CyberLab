using Microsoft.AspNetCore.Identity;

namespace Authorization
{
    public static class RolesConfig
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roleNames = { UserRole.Student, UserRole.Teacher, UserRole.Admin };
                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task CreateAdmin(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                
                var superUser = new User
                {
                    Id = new Guid().ToString(),
                    UserName = "Super",
                    FirstName = "Super",
                    SecondName = "Super",
                    Email = "super@email.com"
                };
                var password = "super123";
                var foundUser = userManager.FindByEmailAsync(superUser.Email);

                if (foundUser == null)
                {
                    var creationUserResult = await userManager.CreateAsync(superUser, password);
                    if (creationUserResult.Succeeded
                        && await roleManager.RoleExistsAsync(UserRole.Admin))
                        await userManager.AddToRoleAsync(superUser, UserRole.Admin);
                }
            }
        }
    }
}
