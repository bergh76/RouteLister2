﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RouteLister2.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public IEnumerable<RouteList> RouteLists { get; set; }
        public IEnumerable<UserConnectionStatus> UserConnectionHistory { get; set; }
        public string RegistrationNumber { get; set; }
    }
}
