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

        public static async Task CreateTeacher(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                
                var teacher = new User
                {
                    Id = new Guid().ToString(),
                    UserName = "teacher",
                    FirstName = "Teacher",
                    SecondName = "Teacher",
                    Email = "teacher@email.com"
                };
                var password = "teacher123";
                var foundUser = await userManager.FindByEmailAsync(teacher.Email);
                if (foundUser == null)
                {
                    var creationUserResult = await userManager.CreateAsync(teacher, password);
                    if (creationUserResult.Succeeded
                        && await roleManager.RoleExistsAsync(UserRole.Teacher))
                        await userManager.AddToRoleAsync(teacher, UserRole.Teacher);
                }
            }
        }
    }
}
