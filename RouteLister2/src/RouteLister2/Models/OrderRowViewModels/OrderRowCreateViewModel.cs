using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models.OrderRowViewModels
{
    public class OrderRowCreateViewModel
    {

        public int Count { get; set; }
        public bool OrderRowStatus { get; set; }
        public string ParcelName { get; set; }
        public string ParcelNumber { get; set; }
    }
}
