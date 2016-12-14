using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models
{
    public class MapRouteViewModel
    {
        public string Url { get; set; }
    
        public MapRouteViewModel(string url)
        {
            Url = url;
        }
    }
}
