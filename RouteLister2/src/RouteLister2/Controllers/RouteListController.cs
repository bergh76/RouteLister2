using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteLister2.Data;
using RouteLister2.Models;
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
        private UnitOfWork _unitOfWork;

        public RouteListController([FromServices] ApplicationDbContext context, [FromServices] IMapper mapper, [FromServices] UnitOfWork unitOfWork)
        {
            _context = context;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string id)
        {

            //Using Automapper
       
            var result = _context.Vehicles.Include(x => x.RouteLists).Where(x => x.RegistrationNumber == id);
            result = _unitOfWork.GenericRepository<Vehicle>().GetIncluded(x => x.Id.ToString() == id, included: z => z.RouteLists).AsQueryable();
            
            RouteListViewModel viewModel = result.ProjectTo<RouteListViewModel>(_mapper.ConfigurationProvider).FirstOrDefault();
            return View(viewModel);
        }

        //Example SignalR flow action
        [HttpPost]
        public bool ChangeStatusOnOrderRow(int id)
        {
            return false;
        }

    }
}
