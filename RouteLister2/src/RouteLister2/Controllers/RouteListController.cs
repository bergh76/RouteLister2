using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        public RouteListController(
            IConnectionManager connectionManager,
            [FromServices] SignalRBusinessLayer businessLayer
            )
        {

            _connectionManager = connectionManager;
            _businessLayer = businessLayer;
        }

        public async Task<IActionResult> Index(string id)
        {
#if DEBUG
            id = "aaa111";
#endif
            if (!string.IsNullOrEmpty(id)) {
                var viewModel = await _businessLayer.GetRouteListViewModelByRegistrationNumber(id);
                return View(viewModel);
            }
            return View();
     
        }
        public async Task<IActionResult> IndexPartial(string id)
        {
#if DEBUG
            id = "aaa111";
#endif
            if (!string.IsNullOrEmpty(id))
            {
                var viewModel = await _businessLayer.GetRouteListViewModelByRegistrationNumber(id);
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

        [HttpPost]
        public async Task<IActionResult> Create(RouteList model)
        {
            await SetUserDropDown();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _businessLayer.Insert(model);
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
            if (string.IsNullOrEmpty(id)) {
                ViewBag.VehicleDropDown = await _businessLayer.GetRegistrationNumberDropDown(id);
            }
            else
            {
                ViewBag.VehicleDropDown = await _businessLayer.GetRegistrationNumberDropDown(id);
            }
        }


        

    }
}
