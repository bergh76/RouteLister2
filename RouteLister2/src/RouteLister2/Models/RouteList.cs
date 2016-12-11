using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models
{
    public class RouteList
    {
        public int Id { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public string Title { get;set;}
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public DateTime? Assigned { get; set; }
    }
}
