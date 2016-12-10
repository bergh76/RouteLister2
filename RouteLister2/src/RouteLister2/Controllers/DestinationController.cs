using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RouteLister2.Data;
using AutoMapper;
using RouteLister2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RouteLister2.Controllers
{
    public class DestinationController : Controller
    {
        private SignalRBusinessLayer _businessLayer;

        public DestinationController([FromServices] SignalRBusinessLayer businessLayer)
        {
            _businessLayer = businessLayer;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _businessLayer.Get<Destination>(id);
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
        public async Task<IActionResult> Create(Destination model)
        {

            if (!ModelState.IsValid)
            {
                await SetDropDowns();
                return View(model);
            }
            await _businessLayer.Insert(model);
            return RedirectToAction("Edit", new { id = model.Id });
        }


        private async Task SetDropDowns(int? id = null)
        {
            var one = await _businessLayer.GetContactsDropDown(id);
            ViewBag.ContactDropDown = one.Count() == 0 ? null : one;
            var two = await _businessLayer.GetAddressDropDown(id);
            ViewBag.AddressDropDown = two.Count() == 0 ? null : two;
        }
    }
}
