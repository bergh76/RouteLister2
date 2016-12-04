

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RouteLister2.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models
{
    public class UserSettings
    {

        IServiceProvider _serviceProvider;
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSettings(
            IServiceProvider serviceProvider,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context, string email, string [] roles
            )
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _userManager = userManager;
        }
        internal async Task<ApplicationUser> SetNewPassword(ApplicationDbContext context,ApplicationUser user,  string newPW, string userID)
        {
            var password = new PasswordHasher<ApplicationUser>();
            var hashed = password.HashPassword(user, newPW);
            user.PasswordHash = hashed;
            var userStore = new UserStore<ApplicationUser>(context);
            context.Entry(user.PasswordHash).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await context.SaveChangesAsync();
            return user;
        }


        internal static async Task<IdentityResult> AssignRoles(UserManager<ApplicationUser> userManager, string email, string[] roles)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(email);
            var result = await userManager.AddToRolesAsync(user, roles);

            return result;
        }

    }
}

