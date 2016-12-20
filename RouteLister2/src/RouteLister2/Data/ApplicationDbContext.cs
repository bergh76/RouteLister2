using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RouteLister2.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace RouteLister2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        private static string[] GetRoles()
        {
            return new string[] { "Admin", "User" }; // add the users to seed "Editor", "Buyer", "Business", "Seller", "Subscriber" };
        }

        public static async void SeedDefaultData(IApplicationBuilder app)
        {
            using (var context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>())
            {
                //var dbContext = _serviceProvider.GetService<ApplicationDbContext>();
                string[] roles = GetRoles();
                foreach (string role in roles)
                {
                    var roleStore = new RoleStore<IdentityRole>(context);
                    if (!context.Roles.Any(r => r.Name == role))
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

                if (!context.Users.Any(u => u.UserName == user.UserName))
                {
                    var password = new PasswordHasher<ApplicationUser>();
                    var hashed = password.HashPassword(user, "Asdf1234*");
                    user.PasswordHash = hashed;
                    var userStore = new UserStore<ApplicationUser>(context);
                    var result = userStore.CreateAsync(user);
                    // ToDo: Add claims for Users
                    //user.Claims.Add(new IdentityUserClaim<string>
                    //{
                    //    ClaimType = "AdminOnly",
                    //    ClaimValue = "Admin"
                    //});
                }

                await AssignRoles(app, user.Email, roles);
                await context.SaveChangesAsync();
                await SeedOrderRowStatusToDbAsync(app);
                await SeedOrderStatusToDbAsync(app);

            }


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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        //public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<RouteList> RouteLists { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<OrderRow> OrderRows { get; set; }
        public DbSet<OrderRowStatus> OrderRowStatus { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<OrderType> OrderType { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<UserConnectionStatus> UsersConnectionStatus { get; set; }
    }
}
