using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using RouteLister2.Data;
using RouteLister2.Models.RouteListerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
            RouteListViewModel viewModel = result.ProjectTo<RouteListViewModel>(_mapper.ConfigurationProvider).FirstOrDefault();
            return View(viewModel);
        }
    }
}
