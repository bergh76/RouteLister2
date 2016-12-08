using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Microsoft.EntityFrameworkCore;
using RouteLister2.Data;
using RouteLister2.Models;
using RouteLister2.Models.RouteListerViewModels;
using RouteLister2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Controllers
{
    public class RouteListController : Controller
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
 
        private IConnectionManager _connectionManager;
        private SignalRBusinessLayer _businessLayer;

        public RouteListController(
            [FromServices] ApplicationDbContext context, 
            [FromServices] IMapper mapper, 

            IConnectionManager connectionManager,
            [FromServices] SignalRBusinessLayer businessLayer
            )
        {
            _context = context;
            _mapper = mapper;

            _connectionManager = connectionManager;
            _businessLayer = businessLayer;
        }

        public async Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id)) { 
                var result = await _businessLayer.GetRouteList(id);
                RouteListViewModel viewModel = _mapper.Map<RouteListViewModel>(result);
                return View(viewModel);
            }
            return View();
        }

        public async Task<IActionResult> List(string id)
        {
            var result = await _businessLayer.GetAllRouteLists().ProjectTo<RouteListViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return View(result);
        }
    }
}
