using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RouteLister2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace RouteList2XUnitTests
{
    public static class Utilities
    {

        public static void ReflectEvaluateObjectProperties<T>(T t1, T t2)
        {
            Type t = t1.GetType();
            var properties = t.GetProperties();


            foreach (var item in properties)
            {
                Assert.Equal(item.GetValue(t1), item.GetValue(t2));
            }
        }
        public static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();



            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider(serviceProvider);
            
            



            return builder.Options;
        }
    }
}
