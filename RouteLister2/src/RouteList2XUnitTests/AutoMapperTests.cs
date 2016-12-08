using AutoMapper;
using RouteLister2.Data;
using Xunit;

namespace RouteList2XUnitTests
{
    public class AutoMapperTests
    {
        [Fact]
        public void TestMappings()
        {
            MapperConfiguration configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfileConfiguration>();

            });
            configuration.AssertConfigurationIsValid();
        }
    }
}
