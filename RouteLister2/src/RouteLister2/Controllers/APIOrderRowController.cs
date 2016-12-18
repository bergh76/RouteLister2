using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RouteLister2.Models;
using RouteLister2.Models.RouteListerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using RouteLister2.Models.OrderRowViewModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RouteLister2.Controllers
{
    [Route("/api/v1/orderRow")]
    public class APIOrderRowController : Controller
    {
        private SignalRBusinessLayer _businessLayer;
        private IMapper _mapper;

        public APIOrderRowController(
            [FromServices] SignalRBusinessLayer businessLayer, 
            [FromServices] IMapper mapper)
        {
            _businessLayer = businessLayer;
            _mapper = mapper;
        }
        // GET: api/values
        [HttpGet]
        [Authorize]
        public async Task<JsonResult> Get()
        {
            var result = _businessLayer.GetOrderRows().ProjectTo<OrderRowViewModel>(_mapper.ConfigurationProvider);
            return Json(await result.ToListAsync());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            string regNr = (await _businessLayer.GetUser(name: HttpContext.User.Identity.Name)).RegistrationNumber;
           
                var viewModel = await _businessLayer.GetDriversRouteListForToday(regNr);
                return Ok(viewModel);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]string value)
        {
            //TODO
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]string value)
        {
            //TODO
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            //TODO
            return Ok();
        }
    }
}
