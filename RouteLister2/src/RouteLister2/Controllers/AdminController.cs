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
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using System.Security.Claims;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RouteLister2.Controllers
{
    [Authorize(Roles = "Admin")]
    //[Route("Admin")]
    public class AdminController : Controller
    {
        private SignalRBusinessLayer _businessLayer;
        private IMapper _mapper;
        private IConnectionManager _connectionManager;
        private ConnectionMapping<string> _mapping;

        // GET: /<controller>/
        //[Authorize(Roles ="Admin")]
        public AdminController(
            [FromServices] SignalRBusinessLayer businessLayer,
            [FromServices] IMapper mapper,
            [FromServices] IConnectionManager connectionManager,
            [FromServices] ConnectionMapping<string> connectionMapping
            )
        {
            _businessLayer = businessLayer;
            _mapper = mapper;
            _connectionManager = connectionManager;
            _mapping = connectionMapping;

        }
        public async Task<IActionResult> Index(
            JsonDataListImports jsonData, 
            ParcelListFromCompanyViewModel parcel)
        {
            if (ModelState.IsValid)
            {
                //Uncommented import since webapi is non-existant.
                //await jsonData.GetParcelData(_context);

                var result = _businessLayer.GetAll<OrderRow>().Include(x=>x.Order).Include(x=>x.Parcel).ProjectTo<ParcelListFromCompanyViewModel>(_mapper.ConfigurationProvider);
               
                List<ParcelListFromCompanyViewModel> outResult = await result.ToListAsync();
                List<SelectListItem> dropDown = await _businessLayer.GetRegistrationNrOnlyDropDown();
                foreach (var item in outResult)
                {
                    //Hack to set index for dropdown, haven't figured out how to map a dropdown with automapper yet or if its possible at all
                    dropDown[dropDown.IndexOf(dropDown.Where(x => x.Value == item.RegistrationNumber).SingleOrDefault())].Selected = true;
                    item.RegNrDropDown = dropDown.ToList();
                }
                return View(outResult);
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
        [HttpPost]
        public async Task<IActionResult> AdminRowPartial(ParcelListFromCompanyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {

                return View(viewModel);
            }

            var hubContext = _connectionManager.GetHubContext<DriverHub>();

            
            //Getting applicationUser
            bool newRouteList = await _businessLayer.RegisterOrderToDriver(viewModel.RegistrationNumber, viewModel.OrderId);

            if (newRouteList)
            {
                //notify relevant driver that he has a new routeList
                await hubContext.Clients.Groups(new List<string>() { viewModel.RegistrationNumber }).NewRouteListAdded();
            }
            else
            {
                //Notify old client that he has lost a order
                var oldClientId = await _businessLayer.GetUser(orderId: viewModel.OrderId);
                await hubContext.Clients.Groups(new List<string>() { oldClientId.UserName }).RemovedOrder(viewModel.OrderId);
            }
            //
            //var clientNameToMessage = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //User assigned
            var newUser = await _businessLayer.GetUser(regNr: viewModel.RegistrationNumber);
            //Build url for getter
            var orderUrl = Url.Action("Order", "RouteList", new { id=viewModel.OrderId });
            //Tells client to add order
            await hubContext.Clients.Clients(_mapping.GetConnections(newUser.UserName).ToList()).AddedOrder(orderUrl);
            //Send a message to all involved clients that a order has been added
            await hubContext.Clients.Clients(_mapping.GetConnections(newUser.UserName).ToList()).Message(HttpContext.User.Identity.Name+" har lagt till en order till" + newUser.UserName);
            viewModel.RegNrDropDown = await _businessLayer.GetRegistrationNrOnlyDropDown(SelectedRegnr:viewModel.RegistrationNumber);
            return PartialView(viewModel);
        }

    }

}
