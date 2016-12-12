using RouteLister2.Models.OrderRowViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models.RouteListerViewModels
{
    public class OrderDetailViewModel
    {

     
        public string Address { get; set; }
        public string City { get; set; }
        public string Description { get {
                return Name + ", " +
                        Address + "," +
                        PostNumber + " " +
                        City;
            } }
        public string Name { get {
                return FirstName + " " + LastName;
                    }
        }
        public string FirstName {get;set;}
        public string LastName { get; set; }
        public int OrderId { get; set; }
        public int? RouteListId { get; set; }
        public List<string> PhoneNumbers { get; set; }
        public string PostNumber { get; set; }
        public string DeliveryTypeName { get; set; }
        public int TotalCount { get; set; }
        public List<OrderRowViewModel> OrderRows { get; set; }
    }
}
