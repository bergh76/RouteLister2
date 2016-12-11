﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models.RouteListerViewModels
{
    public class RouteListCreateViewModel
    {
        
        public IEnumerable<SelectListItem> VehicleDropDown { get; set; }
        public int VehicleId { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
    }
}