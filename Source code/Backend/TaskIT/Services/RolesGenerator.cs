using Microsoft.AspNetCore.Identity;

namespace TaskIT.Services
{
    public static class RolesGenerator
    {
        private static readonly string[] roleNames = { "USER", "WORKER", "EMPLOYER" };
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
