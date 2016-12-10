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

    public class JsonDataListImports
    {
        private ApplicationDbContext _context;
        private const string JsonUrl = "http://localhost:5000/TestData/jsonParcels.json";
        public List<ParcelListFromCompanyViewModel> ParcelListImports { get; set; }
        public static List<ParcelListFromCompanyViewModel> _parcelList = new List<ParcelListFromCompanyViewModel>();
        private IMapper _mapper;

        public JsonDataListImports(ApplicationDbContext context, [FromServices] IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public JsonDataListImports() { }

        private async Task<T> JsonSerializer<T>(string path) where T : new()
        {
            using (var http = new HttpClient())
            {
                var data = string.Empty;
                {
                    try
                    {
                        data = await http.GetStringAsync(path);
                    }
                    catch (Exception) { }
                    return !string.IsNullOrEmpty(path) ? JsonConvert.DeserializeObject<T>(data) : new T();
                }

            }
        }

        public async Task GetParcelData(ApplicationDbContext context)
        {
            string path = JsonUrl.ToString();
            var data = await JsonSerializer<JsonDataListImports>(path);

            _parcelList = data.ParcelListImports;
            await JsonApiDataImport(context);
            //await GetCurrentList();
        }

        public async Task JsonApiDataImport(ApplicationDbContext context)
        {
            for (int i = 0; i < _parcelList.Count; i++)
            {
                // First applyment to db from API and checks if collieId exists in db.
                if (!context.Parcels.Where(x => x.ParcelNumber == _parcelList[i].CollieId).Any())
                {
                    var contact = new Contact
                    {
                        FirstName = _parcelList[i].FirstName,
                        LastName = _parcelList[i].LastName,
                        PhoneNumbers = new List<PhoneNumber>()
                        {
                                new PhoneNumber() { Number = _parcelList[i].PhoneOne},
                                new PhoneNumber() { Number = _parcelList[i].PhoneTwo}
                        }
                    };
                    context.Add(contact);
                    await context.SaveChangesAsync();

                    var address = new Address()
                    {
                        City = _parcelList[i].City,
                        Street = _parcelList[i].Adress,
                        County = _parcelList[i].Country,
                        PostNumber = _parcelList[i].PostNr
                    };
                    context.Add(address);
                    await context.SaveChangesAsync();

                    var parcel = new Parcel()
                    {
                        Name = _parcelList[i].ArticleName,
                        ParcelNumber = _parcelList[i].CollieId,
                        PickedStatus = false,
                    };
                    context.Add(parcel);
                    await context.SaveChangesAsync();

                    var destination = new Destination()
                    {
                        AddressId = address.Id,
                        ContactId = contact.Id,

                    };
                    context.Add(destination);
                    await context.SaveChangesAsync();

                    var ordertyp = new OrderType()
                    {
                        Description = _parcelList[i].DeliveryType,
                        Name = _parcelList[i].DeliveryType,
                    };
                    context.Add(ordertyp);
                    await context.SaveChangesAsync();
                }
            };
        }
    }
}
