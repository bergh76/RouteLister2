using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RouteLister2.Data;
using RouteLister2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace RouteList2XUnitTests
{
    public class BusinessLayerTests
    {
     
        

        [Fact]
        public async Task TestingBusinessLayer()
        {



            //Arrange
            using (var context = new ApplicationDbContext(Utilities.CreateNewContextOptions()))
            {
                MapperConfiguration configuration = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfileConfiguration>();

                });
                IMapper mapper = configuration.CreateMapper();
                SignalRBusinessLayer businessLayer = new SignalRBusinessLayer(new RouteListerRepository(context), mapper);
                ApplicationUser user = FakeDataSets.ApplicationUserFactory();
                //context.Database.ExecuteSqlCommand(string.Format("Set identity_insert off"));
                await businessLayer.AddUser(FakeDataSets.ApplicationUserFactory());
                //Act
                ApplicationUser userGotten = await businessLayer.GetUser(id: FakeDataSets.Id.ToString());
                //Assert

                Assert.Equal(userGotten.Id, user.Id);
            } 
        }

        [Fact]
        public async Task TestingNullValueInGetUserId()
        {
            //Arrange
            MapperConfiguration configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfileConfiguration>();

            });
            IMapper mapper = configuration.CreateMapper();
            using (var context = new ApplicationDbContext(Utilities.CreateNewContextOptions()))
            {
                SignalRBusinessLayer businessLayer = new SignalRBusinessLayer(new RouteListerRepository(context),mapper);
                ApplicationUser user = FakeDataSets.ApplicationUserFactory();
              
                await businessLayer.AddUser(FakeDataSets.ApplicationUserFactory());
                //Act
                ApplicationUser userGotten = await businessLayer.GetUser(id: null,name:FakeDataSets.ApplicationUserFactory().UserName);
                //Assert

                Assert.Equal(userGotten.Id, user.Id);
            }
        }

        [Fact]
        public async Task TestingGetRouteList()
        {

            MapperConfiguration configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfileConfiguration>();

            });
            IMapper mapper = configuration.CreateMapper();

            //Arrange
            using (var context = new ApplicationDbContext(Utilities.CreateNewContextOptions()))
            {
                SignalRBusinessLayer businessLayer = new SignalRBusinessLayer(new RouteListerRepository(context),mapper);
                ApplicationUser user = FakeDataSets.ApplicationUserFactory();
  
                await businessLayer.Insert(FakeDataSets.DeliveryListFactory());
                //Act

                var routelist = await businessLayer.GetRouteList(RegistrationNumber: FakeDataSets.ApplicationUserFactory().RegistrationNumber);
                //Assert
                Assert.NotNull(routelist);
           
                
            }
        }



    }
}
