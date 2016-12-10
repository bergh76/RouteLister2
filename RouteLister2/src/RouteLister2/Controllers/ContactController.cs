using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RouteLister2.Data;
using AutoMapper;
using RouteLister2.Models;

namespace RouteLister2.Controllers
{
    public class ContactController : Controller
    {
        private SignalRBusinessLayer _businessLayer;

        public ContactController([FromServices] SignalRBusinessLayer businessLayer)
        {
            _businessLayer = businessLayer;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _businessLayer.Get<Contact>(id);
            if (model == null)
            {
                return NotFound();
            }
            await SetDropDowns();
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            await SetDropDowns();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Contact model)
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
            
        }
    }
}
