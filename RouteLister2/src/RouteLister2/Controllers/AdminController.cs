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
        private IDataImports _data;


        // GET: /<controller>/
        //[Authorize(Roles ="Admin")]
        public AdminController(
            [FromServices] ApplicationDbContext context,
            [FromServices] UserManager<ApplicationUser> userManager,
            [FromServices]SignInManager<ApplicationUser> signInManager,
            [FromServices] IMapper mapper,
            ILoggerFactory loggerFactory,
            IHostingEnvironment host,
            IDataImports data)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _host = host;
            _data = data;
        }


        public async Task<IActionResult> Index()
        {           
            if (ModelState.IsValid)
            {                
                var result = _context.OrderRows.ProjectTo<ParcelListFromCompanyViewModel>(_mapper.ConfigurationProvider);                
                List<ParcelListFromCompanyViewModel> outResult = await result.ToListAsync();
                List<SelectListItem> dropDown = await _context.Users.Select(x => new SelectListItem() { Text = x.RegistrationNumber, Value = x.RegistrationNumber }).ToListAsync();
                foreach (var item in outResult)
                {
                    //Hack to set index for dropdown, haven't figured out how to map a dropdown with automapper yet or if its possible at all
                    dropDown[dropDown.IndexOf(dropDown.Where(x => x.Value == item.RegistrationNumber).SingleOrDefault())].Selected = true;
                    item.RegNrDropDown = dropDown.ToList();
                }
                //ViewBag.ImportedParcels = count.Result;
                return View(outResult);
            }
            return RedirectToAction("Error");
        }


        public async Task<IActionResult> DownloadRouteList()
        {
            await _data.ImportParcelData();
            return RedirectToAction("Index");
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
        
        [AllowAnonymous]
        public virtual JsonResult CheckUserNameExists(string userName)
        {
            //Check in database via Usermanager that particular user name exists or not
            bool userExists = _userManager.Users.Any(u => u.UserName == userName);
            if(userExists !=false)
                return Json("Användarnamnet är redan registrerat!");
            return Json(true);
        }

        [AllowAnonymous]
        public virtual JsonResult CheckEmailExists(string email)
        {
            //Check in database via Usermanager that particular email exists or not
            bool emailExists = _userManager.Users.Any(u => u.Email == email);
            if (emailExists != false)
                return Json("Eposten är redan registrerad!");
            return Json(true);
        }

        [AllowAnonymous]
        public virtual JsonResult CheckRegNrExists(string regnr)
        {
            //Check in database via Usermanager that particular email exists or not
            bool regnrExists = _userManager.Users.Any(u => u.RegistrationNumber == regnr);
            if(regnrExists !=false)
                return Json("Registreringsnumret är redan registrerat!");
            return Json(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string role, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var reg = new UserSettings();
                await reg.RegisterUser(_context, model,_userManager, role, returnUrl);
                return RedirectToAction(nameof(AdminController.Register), "Admin");
            }

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
            //ManageMessageId? message = ManageMessageId.Error;
            if (ModelState.IsValid)
            {
                var user = new UserSettings();
                await user.DeleteUser(_userManager, account, Id, returnUrl);
                return RedirectToLocal(returnUrl);

            }
            return RedirectToAction(nameof(AdminController.Register), "Admin");
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