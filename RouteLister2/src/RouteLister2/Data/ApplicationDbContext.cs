using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RouteLister2.Models;

namespace RouteLister2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<RouteList> RouteLists { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<OrderRow> OrderRows { get; set; }
        public DbSet<OrderRowStatus> OrderRowStatus { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<OrderType> OrderType { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<UserConnectionStatus> UsersConnectionStatus { get; set; }
    }
}
