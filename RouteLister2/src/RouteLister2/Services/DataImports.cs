using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

                    Contact contact = context.Contacts.Where(x => x.PhoneNumbers.Select(p => p.Number).Contains(_parcelList[i].PhoneOne)).SingleOrDefault();
                    if (contact == null)
                    {
                        contact = AddContactToDb(i);
                        context.Add(contact);
                        await context.SaveChangesAsync();
                    }

                    Address address = context.Address.Where(x => x.Street == _parcelList[i].Adress).SingleOrDefault();
                    if (address == null)
                    {
                        address = AddAddressToDb(i);
                        context.Add(address);
                        await context.SaveChangesAsync();
                    }

                    Destination destination = context.Destinations.SingleOrDefault(x => x.AddressId == address.Id && x.ContactId == contact.Id);
                    if (destination == null)
                    {
                        destination = AddDestinationToDb(contact, address);
                        context.Add(destination);
                        await context.SaveChangesAsync();
                    }

                    Parcel parcel = context.Parcels.SingleOrDefault(x => x.ParcelNumber == _parcelList[i].CollieId);
                    if (parcel == null)
                    {
                        parcel = AddParcelToDb(i);
                        context.Add(parcel);
                        await context.SaveChangesAsync();
                    }

                    OrderType ordertyp = context.OrderType.SingleOrDefault();
                    if (ordertyp == null)
                    {
                        ordertyp = AddOrderTypToDb(i);
                        context.Add(ordertyp);
                        await context.SaveChangesAsync();
                    }

                    RouteList route = context.RouteLists
                        .Include(x => x.Orders)
                        .SingleOrDefault(x => !x.Assigned.HasValue && x.Orders
                            .Select(d => d.DestinationId == destination.Id)
                            .Any());
                    if (route == null)
                    {
                        route = AddRouteToDb();
                        context.Add(route);
                        await context.SaveChangesAsync();
                    }

                    // check !x.RouteList om ordern inte har någon routelist smäller det!
                    Order order = context.Orders.Include(x => x.RouteList).SingleOrDefault(x => x.DestinationId == destination.Id && !x.RouteList.Assigned.HasValue);
                    if (order == null)
                    {
                        order = AddOrderToDb(i, destination.Id, ordertyp.Id, route.Id);
                        context.Add(order);
                        await context.SaveChangesAsync();
                    }
                    OrderRow orderRow = context.OrderRows.SingleOrDefault(x => x.ParcelId == parcel.Id);

                    if (orderRow == null)
                    {
                        orderRow = AddOrderRowToDb(i, order.Id, parcel.Id);
                        context.Add(orderRow);
                        await context.SaveChangesAsync();
                    }
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

        //ToDo: Add seed to service in
        //private OrderStatus AddOrderStatusToDb()
        //{
        //    return new OrderStatus()
        //    {
        //        Description = "Ordern finns på lager",
        //        Name = "Lagerförd",
        //        Priority = 3
        //    };
        //}

        private Order AddOrderToDb(int i, int destinId, int orderTypeId, int routeListId)
        {
            return new Order()
            {
                DestinationId = destinId,
                OrderStatusId = 1,
                OrderTypeId = orderTypeId,
                RouteListId = routeListId,
            };
        }

        private OrderRow AddOrderRowToDb(int i, int OrderId, int ParcelId)
        {
            return new OrderRow()
            {
                Count = _parcelList[i].ArticleAmount,
                OrderId = _context.Orders.SingleOrDefault(x => x.Id == OrderId).Id,
                OrderRowStatusId = 1,
                ParcelId = _context.Parcels.SingleOrDefault(x => x.Id == ParcelId).Id,
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