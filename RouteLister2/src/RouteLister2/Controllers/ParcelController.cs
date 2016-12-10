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
    public class ParcelController : Controller
    {
        private SignalRBusinessLayer _businessLayer;

        public ParcelController([FromServices] SignalRBusinessLayer businessLayer)
        {
            _businessLayer = businessLayer;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _businessLayer.Get<Parcel>(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
        public IActionResult Create()
        {
            SetDropDowns();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Parcel model)
        {

            if (!ModelState.IsValid)
            {
                SetDropDowns();
                return View(model);
            }
            await _businessLayer.Insert(model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        private void SetDropDowns(int? id = null)
        {

        }
    }
}
