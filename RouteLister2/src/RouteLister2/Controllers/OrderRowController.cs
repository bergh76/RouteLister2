using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RouteLister2.Data;
using AutoMapper;
using RouteLister2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RouteLister2.Controllers
{
    public class OrderRowController : Controller
    {

        private ApplicationDbContext _context;
        private IMapper _mapper;

        public OrderRowController([FromServices] ApplicationDbContext context, [FromServices] IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _context.FindAsync<OrderRow>(id);
            if (model == null)
            {
                return NotFound();
            }
            SetDropDowns(id);
            return View(model);
        }
        public IActionResult Create()
        {
            SetDropDowns();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderRow model)
        {
           
            if (!ModelState.IsValid)
            {
                SetDropDowns();
                return View(model);
            }
            _context.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = model.Id });
        }

        private void SetDropDowns(int? id=null)
        {
            var one = _context.OrderRowStatus.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == id });
            ViewBag.OrderRowStatusDropDown = one.Count() == 0 ? null : one;
            var two = _context.Parcels.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == id });
            ViewBag.ParcelDropDown = two.Count() == 0 ? null : two;
            var three = _context.Orders.Select(x => new SelectListItem() { Text = x.DestinationId.ToString()+x.OrderTypeId.ToString()+x.OrderStatusId.ToString(), Value = x.Id.ToString(), Selected = x.Id == id });
            ViewBag.OrderDropDown = two.Count() == 0 ? null : three;
        }
    }
}
