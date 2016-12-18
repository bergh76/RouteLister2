using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using RouteLister2.Models;
using RouteLister2.Models.MapRouteViewModel;
using System.Threading.Tasks;

namespace RouteLister2.Controllers
{
    public class RouteListController : Controller
    {
        private IConnectionManager _connectionManager;
        private SignalRBusinessLayer _businessLayer;

        public RouteListController(
            IConnectionManager connectionManager,
            [FromServices] SignalRBusinessLayer businessLayer
            )
        {
            _connectionManager = connectionManager;
            _businessLayer = businessLayer;
        }

        public async Task<IActionResult> Index(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var viewModel = await _businessLayer.GetRouteListViewModelByRegistrationNumber(id);
                return View(viewModel);
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Create(string title)
        {
            RouteList model = new RouteList() { Title = title };
            await SetUserDropDown();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RouteList model)
        {
            await SetUserDropDown();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _businessLayer.Insert(model);
            return RedirectToAction("Edit", new { id = model.Id });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _businessLayer.GetRouteList(id: id);
            if (model == null)
            {
                return NotFound();
            }
            await SetUserDropDown(model.ApplicationUserId);
            return View(model);
        }

        //[HttpPost]
        public IActionResult MapRoute(string name, string address, string postnr, string city)
        {

            string destination = address + "," + postnr + " " + city + " ,Sverige";
            var rmodel = new MapRouteViewModel(destination);
            return View(rmodel);
        }


        public async Task<IActionResult> List()
        {
            var result = await _businessLayer.GetAllRouteLists();
            return View(result);
        }

        private async Task SetUserDropDown(string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.VehicleDropDown = await _businessLayer.GetRegistrationNumberDropDown(id);
            }
            else
            {
                ViewBag.VehicleDropDown = await _businessLayer.GetRegistrationNumberDropDown(id);
            }
        }
    }
}