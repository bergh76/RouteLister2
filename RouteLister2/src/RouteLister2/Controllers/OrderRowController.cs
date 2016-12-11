using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RouteLister2.Data;
using AutoMapper;
using RouteLister2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using RouteLister2.Models.ParcelListFromCompanyViewModel;
using RouteLister2.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RouteLister2.Controllers
{
    public class OrderRowController : Controller
    {
        private SignalRBusinessLayer _businessLayer;
        private IMapper _mapper;
        private DriverHub _driversHub;

        public OrderRowController([FromServices] SignalRBusinessLayer businessLayer, [FromServices] IMapper mapper, [FromServices] DriverHub driversHub)
        {
            _businessLayer = businessLayer;
            _mapper = mapper;
            _driversHub = driversHub;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _businessLayer.Get<OrderRow>(id);
            if (model == null)
            {
                return NotFound();
            }
            await SetDropDowns(id);
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            await SetDropDowns();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderRow model)
        {
           
            if (!ModelState.IsValid)
            {
                await SetDropDowns();
                return View(model);
            }
            await _businessLayer.Insert(model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        public async Task<IActionResult> AdminRowPartial([Bind("ParcelListFromCompanyViewModel") ] ParcelListFromCompanyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                
                return View(viewModel);
            }
            //How to organize this?, lots of things that needs to be done
            //Check if a routeList exists already (Date will be set then)
            if (viewModel.DeliveryDate.HasValue)
            {
                //await _businessLayer.MoveOrderRowToOtherDriver(viewModel.Id, viewModel.RegistrationNumber);
               
                //TODO
                //await _businessLayer.Update<RouteList>(newEntry,viewModel.Id);
                //Notify old routeList driver of change
                //await _driversHub.Clients.Groups(new List<string>() { oldRouteList.ApplicationUser.RegistrationNumber }).RemovedOrderRow();
                //TODO notify driver that he has a new OrderRow on his existing routelist
                //await _driversHub.Clients.Groups(viewModel.RegistrationNumber).AddedOrderRow();
            }
           
            //If there is no existing routeList
            else 
            {
                //Creating new RouteList, and maybe order if it does not exist
                //await _businessLayer.CreateNewRouteListAndOrder(RegistrationNumber:viewModel.RegistrationNumber,CollieId:viewModel.CollieId, RowNumberId:viewModel.Id);

                //TODO
                //notify relevant driver that he has a new routeList
                //await _driversHub.Clients.Groups(viewModel.RegistrationNumber).NewRouteList();
            }
    
            return PartialView(viewModel);
        }



        private async Task SetDropDowns(int? id=null)
        {
            List<SelectListItem> one = await _businessLayer.GetOrderRowStatusDropDown(id);
            ViewBag.OrderRowStatusDropDown = one.Count() == 0 ? null : one;
            List<SelectListItem> two = await _businessLayer.GetParcelDropDown(id);
            ViewBag.ParcelDropDown = two.Count() == 0 ? null : two;
            List<SelectListItem> three = await _businessLayer.GetOrdersDropDown(id);
            ViewBag.OrderDropDown = two.Count() == 0 ? null : three;
        }
        
    }
}
