using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models.RouteListerViewModels
{
    public class RouteListViewModel
    {
        public DateTime Assigned { get; set; }
        public int DeliveryListId { get; set; }
        public string RegNr { get; set; }
        public string Title { get; set; }
        public List<OrderDetailViewModel> Orders { get; set; }
    }
}
