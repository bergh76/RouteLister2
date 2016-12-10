using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RouteLister2.Data;
using AutoMapper;
using RouteLister2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace RouteLister2.Controllers
{
    public class OrderController : Controller
    {
        private SignalRBusinessLayer _businessLayer;

        public OrderController([FromServices] SignalRBusinessLayer businessLayer)
        {
            _businessLayer = businessLayer;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _businessLayer.Get<Order>(id);
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
        public async Task<IActionResult> Create(Order model)
        {
            
            if (!ModelState.IsValid)
            {
                await SetDropDowns();
                return View(model);
            }
            await _businessLayer.Insert(model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        private async Task SetDropDowns(int? id=null)
        {

            var one = await _businessLayer.GetDestinationDropDown(id);
            ViewBag.DestinationDropDown = one.Count() == 0 ? null : one;
            var two = await _businessLayer.GetOrderStatusDropDown(id);
            ViewBag.OrderStatusDropDown = two.Count() == 0 ? null : two;
            var three = await _businessLayer.GetOrderTypDropDown(id);
            ViewBag.OrderTypeDropDown = three.Count() == 0 ? null : three;
            var four = await _businessLayer.GetRouteListDropDown(id);
            ViewBag.RouteListDropDown = four.Count() == 0 ? null : four;
        }
    }
}
