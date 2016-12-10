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

        private static List<ParcelListImport> _parcelList = new List<ParcelListImport>();

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
                if (!context.Parcels.Where(x => x.ParcelNumber == _parcelList[i].CollieId).Any())
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

                    OrderType ordertyp = AddOrderTypToDb(i);
                    context.Add(ordertyp);
                    await context.SaveChangesAsync();
                }
            };

        }

        private static OrderType AddOrderTypToDb(int i)
        {
            return new OrderType()
            {
                Description = _parcelList[i].DeliveryType,
                Name = _parcelList[i].DeliveryType,
            };
        }

        private static Destination AddDestinationToDb(Contact contact, Address address)
        {
            return new Destination()
            {
                AddressId = address.Id,
                ContactId = contact.Id,

            };
        }

        private static Parcel AddParcelToDb(int i)
        {
            return new Parcel()
            {
                Name = _parcelList[i].ArticleName,
                ParcelNumber = _parcelList[i].CollieId,
                PickedStatus = false,
            };
        }

        private static Address AddAddressToDb(int i)
        {
            return new Address()
            {
                City = _parcelList[i].City,
                Street = _parcelList[i].Adress,
                County = _parcelList[i].Country,
                PostNumber = _parcelList[i].PostNr
            };
        }

        private static Contact AddContactToDb(int i)
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