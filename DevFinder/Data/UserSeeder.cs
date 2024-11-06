using DevFinder.Constants;
using Microsoft.AspNetCore.Identity;

namespace DevFinder.Data
{
    public class UserSeeder
    {
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            await CreateUserWithRoleAsync(userManager, "admin@devfinder.com", "Admin123!", Roles.Admin);
            await CreateUserWithRoleAsync(userManager, "jobseeker@devfinder.com", "Jobseeker123!", Roles.JobSeeker);
            await CreateUserWithRoleAsync(userManager, "employer@devfinder.com", "Employer123!", Roles.Employer);

        }
        private static async Task CreateUserWithRoleAsync(UserManager<IdentityUser> userManager, string email, string password, string role)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser
                {
                    Email = email,
                    EmailConfirmed = true,
                    UserName = email
                };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    throw new Exception($"Failed Creating user with email {user.Email}. Errors: {string.Join(",", result.Errors)}");
                }
            }
        }
    }
}
