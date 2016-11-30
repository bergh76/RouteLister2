using System.Collections.Generic;

namespace RouteLister2.Models
{
    public class Vehicle
    {
        public int Id { get;  set; }
        public string RegistrationNumber { get;  set; }
        public IEnumerable<RouteList> RouteLists { get; set; }
    }
}