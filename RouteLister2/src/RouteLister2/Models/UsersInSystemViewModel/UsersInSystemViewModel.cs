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
        public List<string> _role { get; set; }
        public string UserRole {
            get {
                string[] role = _role.ToArray();
                string result = string.Join(",", role);
                return result;
            }
            set { UserRole = value; }
        }
        public bool IsAccountLocked { get; set; }
        public string Phone { get; set; }
        public int LoginCount { get; set; }
    }
}
