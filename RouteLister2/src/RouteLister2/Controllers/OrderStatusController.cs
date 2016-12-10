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
    public class OrderStatusController : Controller
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;

        public OrderStatusController([FromServices] ApplicationDbContext context, [FromServices] IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _context.FindAsync<OrderStatus>(id);
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
        public async Task<IActionResult> Create(OrderStatus model)
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

        private void SetDropDowns(int? id = null)
        {
            
        }
    }
}
