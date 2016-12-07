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
        private SignalRBusinessLayer _businessLayer;

        public RouteListController(
            [FromServices] ApplicationDbContext context, 
            [FromServices] IMapper mapper, 
            [FromServices] UnitOfWork unitOfWork,
            IConnectionManager connectionManager,
            [FromServices] SignalRBusinessLayer businessLayer
            )
        {
            _context = context;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _connectionManager = connectionManager;
            _businessLayer = businessLayer;
        }

        public IActionResult Index(string id)
        {
            var result = _businessLayer.GetRouteList(id);
           
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


        ////Example SignalR flow action
        ////If this method returns false, the client does not change its status. If true, the client is allowed to change its status
        ////Ideally one should return a object with why the client wasn't allowed to change status for more detailed message to the user
        //[HttpPost]
        //public bool ChangeStatusOnOrderRow(int id)
        //{
        //    //Bizniz layah at work
        //    //return _unitOfWork.ChangeStatusOnOrderRow(id);
        //    return _connectionManager.GetHubContext<DriverHub>().
        //}

        ////In this method we push the change to client with their method
        //[HttpGet]
        //public async Task ChangeStatusOnClientsRow(int id,string clientId)
        //{
        //    //Bizniz layah at work
        //    ApplicationUser user = await _unitOfWork.GenericRepository<ApplicationUser>().GetAsync(clientId);
        //    if (user == null)
        //    {
        //        //Do nothing or block user ip or something
        //    }
        //    OrderRow orderRow = await _unitOfWork.GenericRepository<OrderRow>().GetAsync(id);
        //    if (orderRow == null)
        //    {
        //        //Row doesn exist, notify user of that and logg this
        //    }
            
            

        //    if ()
        //    {
        //        _connectionManager.GetHubContext<DriverHub>().Clients.User(clientId).ChangeStatus(id);
        //    }
        //    else
        //    {
        //        _connectionManager.GetHubContext<DriverHub>().Clients.User(clientId).SendMessage("Cant touch this,dumdumdum psch");
        //    }
        //}



    }
}
