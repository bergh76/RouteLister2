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
        private ApplicationDbContext _context;
        private IMapper _mapper;

        public DestinationController([FromServices] ApplicationDbContext context, [FromServices] IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _context.FindAsync<Destination>(id);
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
        public async Task<IActionResult> Create(Destination model)
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
            var one = _context.Contacts.Select(x => new SelectListItem() { Text = x.FirstName + " " + x.LastName, Value = x.Id.ToString(), Selected = x.Id == id });
            ViewBag.ContactDropDown = one.Count() == 0 ? null : one;
            var two = _context.Address.Select(x => new SelectListItem() { Text = x.PostNumber + " " + x.Street, Value = x.Id.ToString(), Selected = x.Id == id });
            ViewBag.AddressDropDown = two.Count() == 0 ? null : two;
        }
    }
}
