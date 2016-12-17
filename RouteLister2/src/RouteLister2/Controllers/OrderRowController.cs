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

        public OrderRowController([FromServices] SignalRBusinessLayer businessLayer, [FromServices] IMapper mapper)
        {
            _businessLayer = businessLayer;
            _mapper = mapper;

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
