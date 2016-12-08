using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models
{
    public class UserConnectionStatus
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public bool Status { get; set; }
        public string Reason { get; set; } = "No reason";
    }
}
