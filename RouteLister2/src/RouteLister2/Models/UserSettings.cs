using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RouteLister2.Data;
using System;
using System.Threading.Tasks;

namespace RouteLister2.Models
{
    public class UserSettings
    {

        private ApplicationDbContext _context;
        //private readonly UserManager<ApplicationUser> _userManager;

        public UserSettings() { }
        public UserSettings([FromServices] ApplicationDbContext context)
        {
            _context = context;
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

        public static async Task<IdentityResult> AssignRoles(UserManager<ApplicationUser> userManager, string email, string[] roles)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(email);
            var result = await userManager.AddToRolesAsync(user, roles);
            return result;
        }

        //internal static async Task<IdentityResult> UpdateRole(UserManager<ApplicationUser> userManager, string Id, string[] roles)
        //{
        //    ApplicationUser user = await _userManager.FindByEmailAsync(email);
        //    var result = await _userManager.AddToRolesAsync(user, roles);

        //    return result;
        //}
        //internal async Task<ApplicationUser> UpdateUserData(UserManager<ApplicationUser> userManager, ApplicationUser user,  ApplicationDbContext context,string Id, string username, string email, string phone, string roles, bool islocked)
        //{
        //    user = await userManager.FindByIdAsync(Id);
        //    user.UserName = username;
        //    user.NormalizedUserName = username.ToUpper();
        //    user.Email = email;
        //    user.NormalizedEmail = email.ToUpper();
        //    user.PhoneNumber = phone;
        //    user.LockoutEnabled = islocked;
        //    string tempRole = roles;
        //    string[] roleArray = new string[] { tempRole };
        //    var addRole = await userManager.AddToRoleAsync(user, roles.ToUpper());
        //    await UserSettings.UpdateRole(userManager, user.Id, roleArray);

        //    return user;

        //}

    }
}

