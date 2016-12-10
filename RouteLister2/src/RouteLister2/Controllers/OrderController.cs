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
        private ApplicationDbContext _context;
        private IMapper _mapper;

        public OrderController([FromServices] ApplicationDbContext context, [FromServices] IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _context.FindAsync<Order>(id);
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
        public async Task<IActionResult> Create(Order model)
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
            
            var one = _context.Destinations.Include(x => x.Address).Include(x => x.Contact).Select(x => new SelectListItem() { Text = x.Contact.FirstName + " " + x.Contact.LastName + " " + x.Address.City + " " + x.Address.Street, Value = x.Id.ToString(), Selected = x.Id == id });
            ViewBag.DestinationDropDown = one.Count() == 0 ? null : one;
            var two = _context.OrderStatus.Select(x => new SelectListItem() { Text = x.Name,Value=x.Id.ToString(),Selected = x.Id == id });
            ViewBag.OrderStatusDropDown = two.Count() == 0 ? null : two;
            var three = _context.OrderType.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == id });
            ViewBag.OrderTypeDropDown = three.Count() == 0 ? null : three;
            var four = _context.RouteLists.Select(x => new SelectListItem() { Text = x.Title, Value = x.Id.ToString(), Selected = x.Id == id });
            ViewBag.RouteListDropDown = four.Count() == 0 ? null : four;
        }
    }
}
