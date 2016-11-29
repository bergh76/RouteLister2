using RouteLister2.Models.OrderRowViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models.OrderViewModels
{
    public class OrderCreateViewModel
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> PhoneNumbers { get; set; }
        public string PostNumber { get; set; }
        public string DeliveryTypeName { get; set; }
        public List<OrderRowCreateViewModel> OrderRows { get; set; }
    }
}
