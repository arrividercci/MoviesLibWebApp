using Microsoft.AspNetCore.Identity;
using MoviesWebAspNetMVC.Models;

namespace MoviesWebAspNetMVC
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = AdminData.AdminEmail;
            string adminName = AdminData.AdminName;
            string adminPassword = AdminData.AdminPassword;

            if(await roleManager.FindByNameAsync(RolesString.AdminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(RolesString.AdminRole));
            }

            if(await roleManager.FindByNameAsync(RolesString.UserRole)== null) 
            {
                await roleManager.CreateAsync(new IdentityRole(RolesString.UserRole));
            }

            if(await userManager.FindByNameAsync(adminName) == null)
            {
                User admin = new User()
                {
                    Email = adminEmail,
                    UserName = adminName,
                };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, RolesString.AdminRole);
                }
            }
        }
    }
}
