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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RouteLister2.Controllers
{
    [Authorize(Roles = "Admin")]
    //[Route("Admin")]
    public class AdminController : Controller
    {
         // GET: /<controller>/
        //[Authorize(Roles ="Admin")]
       
        public async Task<IActionResult> Index(JsonDataListImports jsonData, ParcelListFromCompanyViewModel parcel)
        {
            if (ModelState.IsValid)
            {
                var data = jsonData.getParcelData();
                var list = JsonDataListImports.ParcelList;
                return View(list);
            }
            
            return RedirectToAction("Error");
        }
        public IActionResult ShowCarLists()
        {
            return View();
        }

        public IActionResult Message()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }

    }

}
