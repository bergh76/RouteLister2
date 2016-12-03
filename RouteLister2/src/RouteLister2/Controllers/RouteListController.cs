using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Microsoft.EntityFrameworkCore;
using RouteLister2.Data;
using RouteLister2.Models;
using RouteLister2.Models.RouteListerViewModels;
using RouteLister2.Services;
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
        private IConnectionManager _connectionManager;

        public RouteListController(
            [FromServices] ApplicationDbContext context, 
            [FromServices] IMapper mapper, 
            [FromServices] UnitOfWork unitOfWork,
            IConnectionManager connectionManager
            )
        {
            _context = context;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _connectionManager = connectionManager;
        }

        public IActionResult Index(string id)
        {

            var result = _unitOfWork.GenericRepository<RouteList>().GetIncluded(
                included: x => x.ApplicationUser, 
                filter: x => x.ApplicationUser.RegistrationNumber == id
                ).ProjectTo<RouteListViewModel>(_mapper.ConfigurationProvider).FirstOrDefault();
            return View(result);
        }

        public async Task<IActionResult> List(string id)
        {

            var result = await _unitOfWork.GenericRepository<RouteList>().GetIncluded(
                included: x => x.ApplicationUser,
                filter: x => x.ApplicationUser.RegistrationNumber == id
                ).ProjectTo<RouteListViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return View(result);
        }


        //Example SignalR flow action
        //If this method returns false, the client does not change its status. If true, the client is allowed to change its status
        //Ideally one should return a object with why the client wasn't allowed to change status for more detailed message to the user
        [HttpPost]
        public bool ChangeStatusOnOrderRow(int id)
        {
            //Bizniz layah at work
            return _unitOfWork.ChangeStatusOnOrderRow(id);
        }

        //In this method we push the change to client with their method
        [HttpGet]
        public void ChangeStatusOnClientsRow(int id,string clientId)
        {
            //Bizniz layah at work
            if (_unitOfWork.ChangeStatusOnOrderRow(id))
            {
                _connectionManager.GetHubContext<DriverHub>().Clients.User(clientId).ChangeStatus(id);
            }
            else
            {
                _connectionManager.GetHubContext<DriverHub>().Clients.User(clientId).SendMessage("Cant touch this,dumdumdum psch");
            }
        }



    }
}
