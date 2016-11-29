using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models.RouteListerViewModels
{
    public class OrderRowViewModel
    {
        public int OrderRowId { get; set; }
        public int Count { get; set; }
        public bool OrderRowStatus { get; set; }
        public string ParcelName { get; set; }
        public string ParcelNumber { get; set; }
    }
}
