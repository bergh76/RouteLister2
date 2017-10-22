using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using WebApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using WebApi.Controllers;
//using WebApi.Models;
//using WebApi.Service;
//using Xunit;
//using Microsoft.AspNetCore.Mvc;
//using RouteLister2.Data;

//namespace RouteList2XUnitTests
//{
//    public class WebApiTest
//    {
//        private readonly IHostingEnvironment hostEnvironment;
//        private readonly IStringLocalizer<ParcelsApiController> localizer;
//        public static IParcelRepository ParcelData { get; set; }
//        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
//        {
//            // Create a fresh service provider, and therefore a fresh 
//            // InMemory database instance.
//            var serviceProvider = new ServiceCollection()
//                .AddEntityFrameworkInMemoryDatabase()
//                .BuildServiceProvider();

//            // Create a new options instance telling the context to use an
//            // InMemory database and the new service provider.
//            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
//            builder.UseInMemoryDatabase()
//                   .UseInternalServiceProvider(serviceProvider);

//            return builder.Options;
//        }


//        //[Fact]
//        //public void GetAllItems_ShouldReturnAllItems_Fail()
//        //{
//        //    var options = CreateNewContextOptions();
//        //    using (var context = new ApplicationDbContext(options))
//        //    {
//        //        var testProducts = GetTestParcelData();
//        //        var ctlr = new ParcelRepository(context);
//        //        int errvalue = 2;
//        //        var result = ctlr.GetAll() as List<ParcelDataModel>;
//        //        Assert.Equal(result.Count(), errvalue);
//        //    }

//        //}

//        //[Fact]
//        //public void GetAllItems_ShouldReturnAllItems_OK()
//        //{
//        //    var options = CreateNewContextOptions();
//        //    using (var context = new ApplicationDbContext(options))
//        //    {
//        //        var addItems = GetTestParcelData();
//        //        var ctlr = new ParcelController(context);
//        //        var result = ctlr.Index();
//        //        //var result = ctlr.GetAll() as List<ParcelDataModel>;
//        //        var viewResult = Assert.IsType<ViewResult>(result);
//        //        var model = Assert.IsAssignableFrom<IEnumerable<ParcelDataModel>>(
//        //            viewResult.ViewData.Model);
//        //        Assert.Equal(4, model.Count());
//        //        //Assert.Equal("Test Product 1", model.ElementAt(0).ArticleName);
//        //        //Assert.Equal(testProducts.Count, result.Count);
//        //    }

//        //}

//        //[Fact]
//        //public async Task GetAllItemsAsync_ShouldReturnAllItems()
//        //{
//        //    var testProducts = GetTestParcelData();
//        //    var controller = new ParcelsApiController(testProducts);

//        //    var result = await controller.GetAllProductsAsync() as List<ParcelDataModel>;
//        //    Assert.AreEqual(testProducts.Count, result.Count);
//        //}

//        //[Fact]
//        //public void GetItem_ShouldReturnCorrectItem()
//        //{
//        //    var testProducts = GetTestParcelData();
//        //    var controller = new ParcelsApiController(testProducts);

//        //    var result = controller.Get as OkNegotiatedContentResult<ParcelDataModel>;
//        //    Assert.IsNotNull(result);
//        //    Assert.AreEqual(testProducts[3].Name, result.Content.Name);
//        //}

//        //[Fact]
//        //public async Task GetItemAsync_ShouldReturnCorrectItem()
//        //{
//        //    var testProducts = GetTestParcelData();
//        //    var controller = new ParcelsApiController(testProducts);

//        //    var result = await controller.GetProductAsync(4);
//        //    Assert.IsNotNull(result);
//        //    Assert.AreEqual(testProducts[3].Name, result.Content.Name);
//        //}

//        //[Fact]
//        //public void GetItem_ShouldNotFindItem()
//        //{
//        //    var controller = new ParcelsApiController(GetTestParcelData());

//        //    var result = controller.GetProduct(999);
//        //    Assert.IsInstanceOfType(result, typeof(NotFoundResult));
//        //}

//        private void GetTestParcelData()
//        {
//            var options = CreateNewContextOptions();
//            using (var context = new ApplicationDbContext(options))
//            {
//                var persOne = new ParcelDataModel()
//                {
//                    Id = 1,
//                    FirstName = "Kalle",
//                    LastName = "Anka",
//                    Adress = "Ankeborg 32",
//                    PostNr = "38245",
//                    City = "Nybro",
//                    PhoneOne = "0481 - 12548",
//                    PhoneTwo = "0481 - 12548",
//                    Country = "Sverige",
//                    Distributor = "Elgiganten",
//                    CollieId = "1SE",
//                    ArticleAmount = 1,
//                    ArticleName = "Diskmaskin",
//                    DeliveryType = "Curbeside",
//                    DeliveryDate = new DateTime(2016 - 12 - 01),
//                };
//                context.Add(persOne);
//                var persTwo = new ParcelDataModel()
//                {
//                    Id = 2,
//                    FirstName = "Knatte",
//                    LastName = "Anka",
//                    Adress = "Ankeborg 32",
//                    PostNr = "38245",
//                    City = "Nybro",
//                    PhoneOne = "0481 - 12548",
//                    PhoneTwo = "0481 - 12548",
//                    Country = "Sverige",
//                    Distributor = "Elgiganten",
//                    CollieId = "2SE",
//                    ArticleAmount = 1,
//                    ArticleName = "Kylskåp",
//                    DeliveryType = "Curbeside",
//                    DeliveryDate = new DateTime(2016 - 12 - 01),
//                    ParcelListData = null,
//                };
//                context.Add(persTwo);
//                var persThree = new ParcelDataModel()
//                {
//                    Id = 3,
//                    FirstName = "Fnatte",
//                    LastName = "Anka",
//                    Adress = "Ankeborg 32",
//                    PostNr = "38245",
//                    City = "Nybro",
//                    PhoneOne = "0481 - 12548",
//                    PhoneTwo = "0481 - 12548",
//                    Country = "Sverige",
//                    Distributor = "Elgiganten",
//                    CollieId = "3SE",
//                    ArticleAmount = 1,
//                    ArticleName = "Tvättmaskin",
//                    DeliveryType = "Curbeside",
//                    DeliveryDate = new DateTime(2016 - 12 - 01),
//                    ParcelListData = null,
//                };
//                context.Add(persThree);
//                var persFour = new ParcelDataModel()

//                {
//                    Id = 4,
//                    FirstName = "Tjatte",
//                    LastName = "Anka",
//                    Adress = "Ankeborg 32",
//                    PostNr = "38245",
//                    City = "Nybro",
//                    PhoneOne = "0481 - 12548",
//                    PhoneTwo = "0481 - 12548",
//                    Country = "Sverige",
//                    Distributor = "Elgiganten",
//                    CollieId = "4SE",
//                    ArticleAmount = 1,
//                    ArticleName = "Torktumlare",
//                    DeliveryType = "Curbeside",
//                    DeliveryDate = new DateTime(2016 - 12 - 01),
//                    ParcelListData = null,
//                };
//                context.Add(persFour);
//                context.SaveChanges();
//            }
//        }
//    }
//}