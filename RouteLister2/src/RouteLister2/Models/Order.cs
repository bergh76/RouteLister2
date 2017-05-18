using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RouteLister2.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int? RouteListId { get; set; }
        public RouteList RouteList { get; set; }
        public List<OrderRow> OrderRows { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public OrderType OrderType { get; set; }
        public int OrderTypeId { get; set; }
        public Destination Destination { get; set; }
        public int DestinationId { get; set; }
        public int TotalProductCount {
            get {
                if (OrderRows != null)
                {
                    return OrderRows.Sum(x => x.Count);
                }
                return 0;
            }
        }

        public int OrderStatusId { get; set; }
    }
}