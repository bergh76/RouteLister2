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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RouteLister2.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
         // GET: /<controller>/
        [Authorize(Roles ="Admin")]
        [Route("Admin")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShowCarLists()
        {
            return View();
        }

       
    }

}
