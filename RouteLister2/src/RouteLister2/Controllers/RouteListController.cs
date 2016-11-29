using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RouteLister2.Data;
using RouteLister2.Models;
using RouteLister2.Models.RouteListerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RouteLister2.Controllers
{
    public class RouteListController : Controller
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;

        public RouteListController([FromServices] ApplicationDbContext context, [FromServices] IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index(string id)
        {

            //Using Automapper
            var result = from vehicles in _context.Vehicles.Where(x => x.RegistrationNumber == id)
                         from routeLists in _context.RouteLists.Where(x => x.VehicleId == vehicles.Id)
                         select routeLists;
            var viewModels = result.ProjectTo<RouteListViewModel>(_mapper.ConfigurationProvider).FirstOrDefault();
            return View(viewModels);
        }
        public IActionResult Index2()

        {
            var result = _context.RouteLists.ProjectTo<RouteList>();
            return View(result);
        }

        [HttpGet]
        public IActionResult Create(string title)
        {
            RouteList model = new RouteList() { Title = title };
            SetVehicleDropDown();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RouteList model)
        {
            SetVehicleDropDown();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _context.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit",new { id=model.Id });
        }
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _context.FindAsync<RouteList>(id);
            if (model == null)
            {
                return NotFound();
            }
            SetVehicleDropDown(id);
            return View(model);
        }

        public async Task<IActionResult> List()
        {
            var result = await _context.RouteLists.ProjectTo<RouteListViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return View(result);
        }
        private void SetVehicleDropDown(int? id = null)
        {
            if (id.HasValue) { 
            ViewBag.VehicleDropDown = _context.Vehicles.Select(x => new SelectListItem() { Text = x.RegistrationNumber, Value = x.Id.ToString(), Selected = x.Id == id });
            }
            else
            {
                ViewBag.VehicleDropDown = _context.Vehicles.Select(x => new SelectListItem() { Text = x.RegistrationNumber, Value = x.Id.ToString() });
            }
        }
       
    }
}
