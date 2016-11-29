using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RouteLister2.Data;
using AutoMapper;
using RouteLister2.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RouteLister2.Controllers
{
    public class VehicleController : Controller
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;

        public VehicleController([FromServices] ApplicationDbContext context, [FromServices] IMapper mapper){
            _context = context;
            _mapper = mapper;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Vehicle model, string title)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _context.Vehicles.AddAsync(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Create","RouteList", new {Title=title });
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Vehicle model = await _context.Vehicles.FindAsync(id);
            if (model!=null)
            {
                return View(model);
            }
            return NotFound();
        }
    }
}
