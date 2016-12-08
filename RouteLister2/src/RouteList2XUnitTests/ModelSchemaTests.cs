using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RouteLister2.Data;
using RouteLister2.Models;
using RouteLister2.Models.RouteListerViewModels;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace RouteList2XUnitTests
{
    public class ModelSchemaTests
    {
        [Fact]
        public void MustFail()
        {
            Assert.False(true);
        }

        [Fact]
        //[InlineData("frk", " Kalle Anka\nParadisäppelvägen 111\n12345 Ankeborg")]
        public void TestApplicationDBContextSchema()
        {
            //Arrange
            var options = CreateNewContextOptions();
            RouteList listOfDeliveries = FakeDataSets.DeliveryListFactory();



            //Act
            int result = 0;
            using (var context = new ApplicationDbContext(options))
            {
                context.RouteLists.Add(listOfDeliveries);
                result = context.SaveChanges();

            }

            //Assert
            Assert.Equal(13, result);
        }
        [Theory, MemberData(nameof(DeliveryLists), MemberType = typeof(ModelSchemaTests))]
        public void DeliveryListViewModelShouldHaveCorrectData(RouteList deliveryList)
        {
            //Arrange
            int Id = FakeDataSets.Id;
            RouteListViewModel deliveryListViewModel = new RouteListViewModel()
            {
                Title = FakeDataSets.DeliveryListFactory().Title,
                DeliveryListId = FakeDataSets.Id
            };
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                context.RouteLists.Add(deliveryList);
                int changes = context.SaveChanges();
                RouteListViewModel viewModelResult = null;
                //Act
                viewModelResult = context.RouteLists.Where(x => x.Id == Id).Select(
                  x => new RouteListViewModel()
                  {
                      DeliveryListId = x.Id,
                      Title = x.Title

                  }).FirstOrDefault();
                //Assert
                Type t = viewModelResult.GetType();
                var properties = t.GetProperties();

                foreach (var item in properties)
                {
                    Assert.Equal(item.GetValue(viewModelResult), item.GetValue(deliveryListViewModel));
                }
            }
        }
        [Theory, MemberData(nameof(DeliveryLists), MemberType = typeof(ModelSchemaTests))]
        public void OrderListViewModelShouldHaveCorrectData(RouteList deliveryList)
        {

            //Arrange
            int Id = FakeDataSets.Id;
            OrderDetailViewModel orderListViewModel = new OrderDetailViewModel()
            {
                FirstName = FakeDataSets.ContactFactory().FirstName,
                LastName = FakeDataSets.ContactFactory().LastName,
                Address = FakeDataSets.AddressFactory().Street,
                PostNumber = FakeDataSets.AddressFactory().PostNumber,
                City = FakeDataSets.AddressFactory().City,
                PhoneNumbers = FakeDataSets.PhoneNumberFactory().Select(x => x.Number).ToList(),
                OrderId = FakeDataSets.OrderFactory().Id,
                DeliveryTypeName = FakeDataSets.OrderFactory().OrderType.Name
            };
            //TODO 
            //Act
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                context.RouteLists.Add(deliveryList);
                context.SaveChanges();
                OrderDetailViewModel resultingViewModel = null;
                var result = from order in context.Orders.Where(x => x.Id == Id)
                             from dest in context.Destinations.Where(x => x.Id == order.DestinationId)
                             from cont in context.Contacts.Where(x => x.Id == dest.ContactId)
                             from addr in context.Address.Where(x => x.Id == dest.AddressId)
                             from orderType in context.OrderType.Where(x => x.Id == order.OrderTypeId)
                             select new OrderDetailViewModel()
                             {
                                 FirstName = cont.FirstName,
                                 LastName = cont.LastName,
                                 Address = addr.Street,
                                 City = addr.City,
                                 PostNumber = addr.PostNumber,
                                 PhoneNumbers = FakeDataSets.PhoneNumberFactory().Select(x => x.Number).ToList(),
                                 OrderId = order.Id,
                                 DeliveryTypeName = orderType.Name,
                             };
                resultingViewModel = result.FirstOrDefault();
                //Assert
                ReflectEvaluateObjectProperties(resultingViewModel, orderListViewModel);
            }

        }

        private void ReflectEvaluateObjectProperties<T>(T t1, T t2)
        {
            Type t = t1.GetType();
            var properties = t.GetProperties();


            foreach (var item in properties)
            {
                Assert.Equal(item.GetValue(t1), item.GetValue(t2));
            }
        }

        [Theory, MemberData(nameof(DeliveryLists), MemberType = typeof(ModelSchemaTests))]
        public void OrderRowViewModelShouldHaveCorrectData(RouteList deliveryList)
        {
            //Arrange
            OrderRowViewModel orderRowViewModel = new OrderRowViewModel()
            {
                OrderRowId = FakeDataSets.Id,
                Count = FakeDataSets.OrderRowFactory().Count,
                ParcelNumber = FakeDataSets.ParcelFactory().ParcelNumber,
                ParcelName = FakeDataSets.ParcelFactory().Name,
                //will probably want to add status like broken, so bool might be off the table
                OrderRowStatus = FakeDataSets.OrderRowFactory().OrderRowStatus.Name == "Plockad"
            };
            //TODO
            //Act
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                context.RouteLists.Add(deliveryList);
                context.SaveChanges();
                OrderRowViewModel orderRowViewModelResult = null;
                var result = from orderRow in context.OrderRows.Include(x => x.OrderRowStatus).Include(x => x.Parcel).Where(x => x.Id == FakeDataSets.OrderRowFactory().Id)
                             select new OrderRowViewModel()
                             {
                                 Count = orderRow.Count,
                                 OrderRowId = orderRow.Id,
                                 OrderRowStatus = orderRow.OrderRowStatus.Name == "Plockad" ? true : false,
                                 ParcelName = orderRow.Parcel.Name,
                                 ParcelNumber = orderRow.Parcel.ParcelNumber
                             };
                orderRowViewModelResult = result.FirstOrDefault();

                //Assert
                ReflectEvaluateObjectProperties(orderRowViewModel, orderRowViewModelResult);

            }
        }


        [Fact]
        public void TestingProjectTo()
        {
            MapperConfiguration configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfileConfiguration>();

            });
            RouteList deliveryList = FakeDataSets.DeliveryListFactory();
            //Arrange
            OrderRowViewModel orderRowViewModel = new OrderRowViewModel()
            {
                OrderId=FakeDataSets.Id,
                OrderRowId = FakeDataSets.Id,
                Count = FakeDataSets.OrderRowFactory().Count,
                ParcelNumber = FakeDataSets.ParcelFactory().ParcelNumber,
                ParcelName = FakeDataSets.ParcelFactory().Name,
                //will probably want to add status like broken, so bool might be off the table
                OrderRowStatus = FakeDataSets.OrderRowFactory().OrderRowStatus.Name == "Plockad"
            };
            //TODO
            //Act
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                context.RouteLists.Add(deliveryList);
                context.SaveChanges();
                OrderRowViewModel orderRowViewModelResult = null;
                var result = context.OrderRows.Where(x => x.Id == FakeDataSets.OrderRowFactory().Id).ProjectTo<OrderRowViewModel>(configuration);
                            
                orderRowViewModelResult = result.FirstOrDefault();

                //Assert
                ReflectEvaluateObjectProperties(orderRowViewModel, orderRowViewModelResult);

            }
        }




        public static object[] DeliveryLists
        {
            get
            {
                return new[] { new object[] { FakeDataSets.DeliveryListFactory() } };
            }
        }

        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
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
