
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using RouteLister2.Models;

namespace RouteLister2.Controllers
{
    public class OrderRowStatusController : Controller
    {
        private SignalRBusinessLayer _businessLayer;

        public OrderRowStatusController([FromServices] SignalRBusinessLayer businessLayer)
        {
            _businessLayer = businessLayer;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _businessLayer.Get<OrderRowStatus>(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderRowStatus model)
        {

            if (!ModelState.IsValid)
            {
                SetDropDowns();
                return View(model);
            }
            await _businessLayer.Insert(model);
            return RedirectToAction("Edit", new { id = model.Id });
        }

        private void SetDropDowns()
        {
            
        }
    }
}
