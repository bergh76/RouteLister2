using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RouteLister2.Models;
using Microsoft.AspNetCore.Identity;
using RouteLister2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using RouteLister2.Services;
using RouteLister2.Models.ParcelListFromCompanyViewModel;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using RouteLister2.Models.ManageViewModels;
using static RouteLister2.Controllers.ManageController;
using Microsoft.Extensions.Logging;
using RouteLister2.Models.AccountViewModels;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RouteLister2.Controllers
{
    [Authorize(Roles = "Admin")]
    //[Route("Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private IMapper _mapper;
        private readonly ILogger _logger;
        private IHostingEnvironment _host;

        // GET: /<controller>/
        //[Authorize(Roles ="Admin")]
        public AdminController(
            [FromServices] ApplicationDbContext context,
            [FromServices] UserManager<ApplicationUser> userManager,
            [FromServices]SignInManager<ApplicationUser> signInManager,
            [FromServices] IMapper mapper,
            ILoggerFactory loggerFactory,
            IHostingEnvironment host)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _host = host;
        }


        public async Task<IActionResult> Index()
        {
            if (ModelState.IsValid)
            {
                // ToDo: Implement the IDataImport Interface correctly
                IDataImports data = new DataImports(_context, _host);
                await data.GetParcelData();
                //await data.GetCoordinates();
                var result = _context.OrderRows.ProjectTo<ParcelListFromCompanyViewModel>(_mapper.ConfigurationProvider);
                List<ParcelListFromCompanyViewModel> outResult = await result.ToListAsync();
                List<SelectListItem> dropDown = await _context.Users.Select(x => new SelectListItem() { Text = x.RegistrationNumber, Value = x.RegistrationNumber }).ToListAsync();
                foreach (var item in outResult)
                {
                    //Hack to set index for dropdown, haven't figured out how to map a dropdown with automapper yet or if its possible at all
                    dropDown[dropDown.IndexOf(dropDown.Where(x => x.Value == item.RegistrationNumber).SingleOrDefault())].Selected = true;
                    item.RegNrDropDown = dropDown.ToList();
                }
                return View(outResult);
            }

            return RedirectToAction("Error");
        }

        public IActionResult ShowCarLists()
        {
            return View();
        }
        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["UserRole"] = new SelectList(_context.Roles, "Name", "Name");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string role, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, RegistrationNumber = model.RegNr };
                var exist = _context.Users.Any(x => x.UserName == user.UserName);
                var emailExists = _context.Users.Any(x => x.Email == user.Email);
                var regnrExists = _context.Users.Any(x => x.RegistrationNumber == user.RegistrationNumber);
                if (exist == true)
                {
                    ViewData["UserRole"] = new SelectList(_context.Roles, "Name", "Name");
                    ViewBag.UserExists = "Användaren finns redan";
                    return View(model);
                }
                else if (emailExists == true)
                {
                    ViewData["UserRole"] = new SelectList(_context.Roles, "Name", "Name");
                    ViewBag.EmailExists = "Eposten finns redan";
                    return View(model);
                }
                else if (regnrExists == true)
                {
                    ViewData["UserRole"] = new SelectList(_context.Roles, "Name", "Name");
                    ViewBag.RegnrExists = "Bilen är redan registrerad";
                    return View(model);
                }

                var result = await _userManager.CreateAsync(user, model.Password);

                // Gets role from View and pars to Array  
                string tempRole = role;
                string[] roleArray = new string[] { tempRole };
                var addRole = await _userManager.AddToRoleAsync(user, role.ToUpper());
                // Calls the method for setting the role to actual useraccount
                await UserSettings.AssignRoles(_userManager, user.Email, roleArray);

                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToAction(nameof(AdminController.Register), "Admin");
                    //return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public async Task<IActionResult> UpdateUser(ApplicationUser appUser, UserSettings user, string Id, string username, string email, string phone, string roles, bool islocked)
        {
            await user.UpdateUserData(_userManager, appUser, _context, Id, username, email, phone, roles, islocked);

            return RedirectToAction(nameof(AdminController.Register), "Admin");

        }

        public IActionResult Message()
        {
            return View();
        }

        public async Task<IActionResult> DeleteUser(RemoveLoginViewModel account, string Id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ManageMessageId? message = ManageMessageId.Error;
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(Id);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
                return RedirectToLocal(returnUrl);

            }
            return RedirectToAction(nameof(AdminController.Register), new { Message = message });
        }

        public IActionResult Error()
        {
            return View();
        }

        //GET: /Admin/ManageLogins
        [HttpGet]
        public void ManageLogins(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.RemoveLoginSuccess ? "Användaren är borttagen."
                : message == ManageMessageId.AddLoginSuccess ? "Användare lades till!"
                : message == ManageMessageId.Error ? "Ett fel vid borttagandet av användaren uppstod."
                : "";
        }

        #region Helpers
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(AdminController.Register), "Admin");
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        #endregion 
    }
}