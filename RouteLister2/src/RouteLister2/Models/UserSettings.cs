using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RouteLister2.Data;
using RouteLister2.Models.AccountViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using RouteLister2.Models.ManageViewModels;
using Newtonsoft.Json;
using static RouteLister2.Controllers.ManageController;

namespace RouteLister2.Models
{
    public class UserSettings
    {
        public static string _message;
        private ApplicationDbContext _context;
        public UserSettings() { }

        public UserSettings([FromServices] ApplicationDbContext context)
        {
            _context = context;
        }

        internal async Task<ApplicationUser> RegisterUser(
            ApplicationDbContext context,
            RegisterViewModel model,
            UserManager<ApplicationUser> userManager,
            string role, string returnUrl = null)
        {
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, RegistrationNumber = model.RegNr };
            bool exist = context.Users.Any(x => x.UserName == user.UserName);
            bool emailExists = context.Users.Any(x => x.Email == user.Email);
            bool regnrExists = context.Users.Any(x => x.RegistrationNumber == user.RegistrationNumber);

            var result = await userManager.CreateAsync(user, model.Password);

            //// Gets role from View and pars to Array  
            string tempRole = role;
            string[] roleArray = new string[] { tempRole };
            var addRole = await userManager.AddToRoleAsync(user, role.ToUpper());
            // Calls the method for setting the role to actual useraccount
            if (result.Succeeded) { _message = ""; return user; }
            return user;
        }

        internal async Task<ApplicationUser> SetNewPassword(ApplicationDbContext context, ApplicationUser user, string newPW, string userID)
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


        internal static async Task<IdentityResult> UpdateRole(UserManager<ApplicationUser> userManager, string Id, string[] roles)
        {
            ApplicationUser user = await userManager.FindByIdAsync(Id);
            var result = await userManager.AddToRolesAsync(user, roles);

            return result;
        }

        internal async Task<ApplicationUser> UpdateUserData(UserManager<ApplicationUser> userManager, ApplicationUser user, ApplicationDbContext context, string Id, string username, string email, string phone, string roles, bool islocked)
        {
            user = await userManager.FindByIdAsync(Id);
            user.UserName = username;
            user.NormalizedUserName = username.ToUpper();
            user.Email = email;
            user.NormalizedEmail = email.ToUpper();
            user.PhoneNumber = phone;
            user.LockoutEnabled = islocked;
            string tempRole = roles;
            string[] roleArray = new string[] { tempRole };
            var addRole = await userManager.AddToRoleAsync(user, roles.ToUpper());
            await UserSettings.UpdateRole(userManager, user.Id, roleArray);
            await context.SaveChangesAsync();
            return user;

        }

        internal async Task DeleteUser(UserManager<ApplicationUser> userManager, RemoveLoginViewModel account, string id, string returnUrl)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    _message = user.UserName + " Användaren är borttagen";
                }
                else if (!result.Succeeded)
                {
                    _message = "Ett fel uppstod " + user.UserName + result.ToString();
                }
                else
                {
                    _message = "";
                }
            }
        }
    }
}