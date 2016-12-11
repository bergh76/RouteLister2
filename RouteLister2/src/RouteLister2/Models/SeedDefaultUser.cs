using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RouteLister2.Data;

namespace RouteLister2.Models
{
    public class SeedDefaultUser
    {
        private ApplicationDbContext _context;
        private IServiceProvider _serviceProvider;

        public SeedDefaultUser(
            ApplicationDbContext context,
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _context = context;
        }

        private static string[] GetRoles()
        {
            return new string[] { "Admin", "User" }; // add the users to seed "Editor", "Buyer", "Business", "Seller", "Subscriber" };
        }
        public async void SeedAdminUser()
        {
            var dbContext = _serviceProvider.GetService<ApplicationDbContext>();
            string[] roles = GetRoles();
            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(dbContext);
                if (!dbContext.Roles.Any(r => r.Name == role))
                {
                    await roleStore.CreateAsync(new IdentityRole { Name = role, NormalizedName = role.ToUpper() });
                }

            }

            var user = new ApplicationUser
            {
                UserName = "default",
                NormalizedUserName = "DEFAULT",
                Email = "default@default.se",
                NormalizedEmail = "DEFAULT@DEFAULT.SE",
                EmailConfirmed = true,
                LockoutEnabled = false,
                 RegistrationNumber ="bbb222",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if (!dbContext.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "Asdf1234*");
                user.PasswordHash = hashed;
                var userStore = new UserStore<ApplicationUser>(dbContext);
                var result = userStore.CreateAsync(user);
                // ToDo: Add claims for Users
                //user.Claims.Add(new IdentityUserClaim<string>
                //{
                //    ClaimType = "AdminOnly",
                //    ClaimValue = "Admin"
                //});
            }

            await SeedAssignRoles(_serviceProvider, user.Email, roles);
            await dbContext.SaveChangesAsync();
        }
        
        internal static async Task<IdentityResult> SeedAssignRoles(IServiceProvider services, string email, string[] roles)
        {
            UserManager<ApplicationUser> _userManager = services.GetService<UserManager<ApplicationUser>>();
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.AddToRolesAsync(user, roles);

            return result;
        }
    }
}