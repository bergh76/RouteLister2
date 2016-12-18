using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models.MapRouteViewModel
{
    public class MapRouteViewModel
    {
        public string Url { get; set; }
        public string Position { get; set; }
        public string Destination { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    
        public MapRouteViewModel(string destination)
        {
            Destination = destination;
            //Url = url;
        }
    }
}
