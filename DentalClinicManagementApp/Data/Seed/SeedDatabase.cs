using DentalClinicManagementApp.Lib;
using Microsoft.AspNetCore.Identity;

namespace DentalClinicManagementApp.Data.Seed
{
    public class SeedDatabase
    {
        public static void Seed(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager,
                               RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager).Wait();
            SeedUsers(userManager).Wait();
        }

        //Create a 'admin' and 'worker' roles
        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            try
            {
                var roleCheck = await roleManager.RoleExistsAsync(AppConstants.ADMIN_ROLE);

                if (!roleCheck)
                {
                    var adminRole = new IdentityRole
                    {
                        Name = AppConstants.ADMIN_ROLE
                    };
                    var roleResult = await roleManager.CreateAsync(adminRole);
                }

                roleCheck = await roleManager.RoleExistsAsync(AppConstants.WORKER_ROLE);

                if (!roleCheck)
                {
                    var operativeRole = new IdentityRole
                    {
                        Name = AppConstants.WORKER_ROLE
                    };
                    var roleResult = await roleManager.CreateAsync(operativeRole);
                }
            }
            catch (Exception ex)
            {

            }
        }

        //Creat one 'admin' and 'worker' user with respective roles
        private static async Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            try
            {
                IdentityUser adminUser = new IdentityUser
                {
                    UserName = AppConstants.ADMIN_USER,
                    Email = AppConstants.ADMIN_EMAIL
                };
                var dbAdmin = await userManager.FindByNameAsync(adminUser.UserName);

                if (dbAdmin == null)
                {
                    var result = await userManager.CreateAsync(adminUser, AppConstants.ADMIN_PWD);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, AppConstants.ADMIN_ROLE);
                        dbAdmin = await userManager.FindByNameAsync(adminUser.UserName);
                        dbAdmin!.EmailConfirmed = true;
                        await userManager.UpdateAsync(dbAdmin);
                    }
                }

                IdentityUser workerUser = new IdentityUser
                {
                    UserName = AppConstants.WORKER_USER,
                    Email = AppConstants.WORKER_EMAIL
                };
                var dbWorker = await userManager.FindByNameAsync(workerUser.UserName);

                if (dbWorker == null)
                {
                    var result = await userManager.CreateAsync(workerUser, AppConstants.WORKER_PWD);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(workerUser, AppConstants.WORKER_ROLE);
                        dbWorker = await userManager.FindByNameAsync(workerUser.UserName);
                        dbWorker!.EmailConfirmed = true;
                        await userManager.UpdateAsync(dbWorker);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
