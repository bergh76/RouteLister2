using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RouteLister2.Data;
using RouteLister2.Models;
using Microsoft.AspNetCore.Builder;

namespace RouteLister2.Data
{
    public class SeedDefaultData
    {
        //private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private IServiceProvider _serviceProvider;
        public SeedDefaultData(UserManager<ApplicationUser> userManager, IServiceProvider serviceProvider)
        { _serviceProvider = serviceProvider; _userManager = userManager; }

        private static string[] GetRoles()
        {
            return new string[] { "Admin", "User" }; // add the users to seed "Editor", "Buyer", "Business", "Seller", "Subscriber" };
        }
        public static async void SeedAdminUser(IApplicationBuilder app)
        {
            var dbContext = app.ApplicationServices.GetService<ApplicationDbContext>();

            //var dbContext = _serviceProvider.GetService<ApplicationDbContext>();
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

            await AssignRoles(app, user.Email, roles);
            await dbContext.SaveChangesAsync();
            await SeedOrderRowStatusToDbAsync(app);
            await SeedOrderStatusToDbAsync(app);


        }
        public static async Task<IdentityResult> AssignRoles(IApplicationBuilder app, string email, string[] roles)
        {
            UserManager<ApplicationUser> _userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.AddToRolesAsync(user, roles);

            return result;
        
        }


        public static async Task SeedOrderRowStatusToDbAsync(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetService<ApplicationDbContext>();
            if (context.OrderRowStatus.Count() == 0)
            {
                var addOrderStatus = new OrderRowStatus();
                addOrderStatus = new OrderRowStatus { Name = "I Lager" };
                context.Add(addOrderStatus);
                await context.SaveChangesAsync();
                addOrderStatus = new OrderRowStatus { Name = "Plockad" };
                context.Add(addOrderStatus);
                await context.SaveChangesAsync();
                
            }

        }

        public static async Task SeedOrderStatusToDbAsync(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetService<ApplicationDbContext>();
            if (context.OrderStatus.Count() == 0)
            {
                var addOrderStatus = new OrderStatus();
                addOrderStatus = new OrderStatus { Description = "Artikel saknas på lager", Name = "Saknas", Priority = 1 };
                context.Add(addOrderStatus);
                await context.SaveChangesAsync();
                addOrderStatus = new OrderStatus { Description = "Artikel finns på lager", Name = "Lagerförd", Priority = 2 };
                context.Add(addOrderStatus);
                await context.SaveChangesAsync();
                addOrderStatus = new OrderStatus { Description = "Artikel är restad", Name = "Lagerförd", Priority = 3 };
                context.Add(addOrderStatus);
                await context.SaveChangesAsync();
            };
        }
    }
}