using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Microsoft.EntityFrameworkCore;
using RouteLister2.Data;
using RouteLister2.Models;
using RouteLister2.Models.RouteListerViewModels;
using RouteLister2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RouteLister2.Controllers
{
    public class RouteListController : Controller
    {
        private IConnectionManager _connectionManager;
        private SignalRBusinessLayer _businessLayer;
        private ConnectionMapping<string> _mapping;

        public RouteListController(
            [FromServices] IConnectionManager connectionManager,
            [FromServices] SignalRBusinessLayer businessLayer
            
            )
        {

            _connectionManager = connectionManager;
            _businessLayer = businessLayer;

        }
        /// <summary>
        /// Getting todays routelist for driver
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Index()
        {
            
            string regNr = (await _businessLayer.GetUser(name: HttpContext.User.Identity.Name)).RegistrationNumber;
            if (!string.IsNullOrEmpty(regNr)) {
                var viewModel = await _businessLayer.GetDriversRouteListForToday(regNr);
                return View(viewModel);
            }
            return RedirectToAction("Index","Admin");
     
        }
        public async Task<IActionResult> IndexPartial()
        {
            string regNr = (await _businessLayer.GetUser(name: HttpContext.User.Identity.Name)).RegistrationNumber;


            if (!string.IsNullOrEmpty(regNr))
            {
                var viewModel = await _businessLayer.GetDriversRouteListForToday(regNr);
                return View(viewModel);
            }
            return View();

        }


        [HttpGet]
        public async Task<IActionResult> Create(string title)
        {
            RouteList model = new RouteList() { Title = title };
            await SetUserDropDown();

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Order(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            //var model = await _businessLayer.GetOrderRowViewModel(id.Value);
            var model = await _businessLayer.GetOrderViewModel(id.Value);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RouteList model)
        {
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _businessLayer.Insert(model);
            await SetUserDropDown();
            return RedirectToAction("Edit",new { id=model.Id });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _businessLayer.GetRouteList(id:id);
            if (model == null)
            {
                return NotFound();
            }
            await SetUserDropDown(model.ApplicationUserId);
            return View(model);
        }

        public async Task<IActionResult> List()
        {
            var result = await _businessLayer.GetAllRouteLists();
            return View(result);
        }

        public IActionResult Test()
        {
            var result = _businessLayer.GetOrderRowViewModel();
            return View(result);
        }

        public async Task<IActionResult> Test2()
        {
            ViewBag.Test = "Funkar";
            return PartialView();
        }
        private async Task SetUserDropDown(string id = null)
        {
      
                ViewBag.UserDropDown = await _businessLayer.GetApplicationUserDropDown(ApplicationUserId: id);
        }


        

    }
}
