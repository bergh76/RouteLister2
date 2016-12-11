using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RouteLister2.Data;
using RouteLister2.Models;
using RouteLister2.Models.ParcelListFromCompanyViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RouteLister2.Services
{
    public class DataImports : IDataImports
    {
        private ApplicationDbContext _context;
        
        // Url to i.e external API/Json
        private const string path = "http://localhost:5000/TestData/jsonParcels.json";

        private static List<ParcelListFromCompanyViewModel> _parcelList = new List<ParcelListFromCompanyViewModel>();

        public DataImports([FromServices] ApplicationDbContext context)
        {
            _context = context;
        }

        public DataImports() { }
        public async Task GetParcelData()
        {
            ApiDeserializer dserial = new ApiDeserializer();
            var dataOut = await dserial.JsonDserializer<ApiDeserializer>(path);
            _parcelList = dataOut.ParcelListImport;
            await JsonApiDataImport();
        }
        
        private async Task JsonApiDataImport()
        {
            if (_parcelList.Count() != 0)
                await ImportData(_context);
            return;
        }

        private async Task ImportData(ApplicationDbContext context)
        {
            for (int i = 0; i < _parcelList.Count; i++)
            {
                // checks if collieId exists in imported list,  if not import to db.
                if (!context.Parcels.Where(x => x.ParcelNumber == _parcelList[i].CollieId).Any())// && !context.Address.Where(x => x.Street == _parcelList[i].Adress).Any())
                {
                    Contact contact = AddContactToDb(i);
                    context.Add(contact);
                    await context.SaveChangesAsync();

                    Address address = AddAddressToDb(i);
                    context.Add(address);
                    await context.SaveChangesAsync();                   

                    Parcel parcel = AddParcelToDb(i);
                    context.Add(parcel);
                    await context.SaveChangesAsync();

                    Destination destination = AddDestinationToDb(contact, address);
                    context.Add(destination);
                    await context.SaveChangesAsync();

                    RouteList route = AddRouteToDb();
                    context.Add(route);
                    await context.SaveChangesAsync();

                    OrderStatus status = AddOrderStatusToDb();
                    context.Add(status);
                    await context.SaveChangesAsync();

                    OrderType ordertyp = AddOrderTypToDb(i);
                    context.Add(ordertyp);
                    await context.SaveChangesAsync();

                    Order order = AddOrderToDb(i);
                    context.Add(order);
                    await context.SaveChangesAsync();

                    OrderRow orderRow = AddOrderRowToDb(i);
                    context.Add(orderRow);
                    await context.SaveChangesAsync();
                }
            };

        }


        private RouteList AddRouteToDb()
        {
            return new RouteList()
            {
                Created = DateTime.Now,
            };
        }

        private OrderStatus AddOrderStatusToDb()
        {
            return new OrderStatus()
            {
                Description = "Ordern finns på lager",
                Name = "Lagerförd",
                Priority = 3
            };
        }

        private Order AddOrderToDb(int i)
        {
            return new Order()
            {
                DestinationId = _context.Destinations.Select(x => x.Id).Last(),
                OrderStatusId = 1,
                OrderTypeId = _context.OrderType.Select(x => x.Id).Last(),
                RouteListId = _context.RouteLists.Select(x => x.Id).Last(),
            };
        }

        private OrderRow AddOrderRowToDb(int i)
        {
            return new OrderRow()
            {
                Count = _parcelList[i].ArticleAmount,
                OrderId = _context.Orders.Select(x => x.Id).Last(),
                OrderRowStatusId = 1,
                ParcelId = _context.Parcels.Select(x => x.Id).Last(),
            };
        }

        private OrderType AddOrderTypToDb(int i)
        {
            return new OrderType()
            {
                Description = _parcelList[i].DeliveryType,
                Name = _parcelList[i].DeliveryType,
            };
        }

        private Destination AddDestinationToDb(Contact contact, Address address)
        {
            return new Destination()
            {
                AddressId = address.Id,
                ContactId = contact.Id,

            };
        }

        private Parcel AddParcelToDb(int i)
        {
            return new Parcel()
            {
                Name = _parcelList[i].ArticleName,
                ParcelNumber = _parcelList[i].CollieId,
                PickedStatus = false,
                Distributor = _parcelList[i].Distributor,
            };
        }

        private Address AddAddressToDb(int i)
        {
            //if (!context.Address.Where(x => x.Street == _parcelList[i].Adress).Any())
            //{
                return new Address()
                {
                    City = _parcelList[i].City,
                    Street = _parcelList[i].Adress,
                    County = _parcelList[i].Country,
                    PostNumber = _parcelList[i].PostNr
                };
            //}
            // ToDo: Need return value so function can return 
        }

        private Contact AddContactToDb(int i)
        {
            return new Contact
            {
                FirstName = _parcelList[i].FirstName,
                LastName = _parcelList[i].LastName,
                PhoneNumbers = new List<PhoneNumber>()
                        {
                                new PhoneNumber() { Number = _parcelList[i].PhoneOne},
                                new PhoneNumber() { Number = _parcelList[i].PhoneTwo}
                        }
            };
        }
    }
}