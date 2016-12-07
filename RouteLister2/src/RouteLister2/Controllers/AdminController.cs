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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RouteLister2.Controllers
{
    [Authorize(Roles = "Admin")]
    //[Route("Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private UnitOfWork _unitOfWork;

        // GET: /<controller>/
        //[Authorize(Roles ="Admin")]
        public AdminController(
            [FromServices]ApplicationDbContext context,
            [FromServices] IMapper mapper,
            [FromServices] UnitOfWork unitOfWork
            )
        {
            _context = context;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(
            JsonDataListImports jsonData, 
            ParcelListFromCompanyViewModel parcel)
        {
            if (ModelState.IsValid)
            {
                await jsonData.getParcelData(_context);
                var result = await _unitOfWork.GenericRepository<ParcelListFromCompanyViewModel>().GetAsyncIncluded();

                return View(result);
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
