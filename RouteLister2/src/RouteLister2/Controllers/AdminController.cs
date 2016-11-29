using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RouteLister2.Models;
using Microsoft.AspNetCore.Identity;
using RouteLister2.Data;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RouteLister2.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: /<controller>/
        [Authorize(Roles ="Admin")]
        //[Route("Admin")]
        public IActionResult Index()
        { 
            //ToDo: return custom errorpage if user not allowed
            return View();
        }
       
    }

}
