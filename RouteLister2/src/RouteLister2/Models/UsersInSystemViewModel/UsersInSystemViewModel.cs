using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models.UsersInSystemViewModel
{
    public class SystemUsersViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> UserRole { get; set; }
        public bool IsAccountLocked { get; set; }
        public string Phone { get; set; }
        public int LoginCount { get; set; }
    }
}
