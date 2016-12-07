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
        public static List<ParcelListFromCompanyViewModel> ParcelList = new List<ParcelListFromCompanyViewModel>();
        private IMapper _mapper;

        public JsonDataListImports(ApplicationDbContext context, [FromServices] IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public JsonDataListImports() { }

        private async Task<T> jsonSerializer<T>(string path) where T : new()
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

        public async Task getParcelData(ApplicationDbContext context)
        {
            string path = JsonUrl.ToString();
            var data = await jsonSerializer<JsonDataListImports>(path);
            ParcelList = data.ParcelListImports;
            await setDataToPersonIfNotExists(context);
            //await GetCurrentList();
        }


        public async Task setDataToPersonIfNotExists(ApplicationDbContext context)
        {
            for (int i = 0; i < ParcelList.Count; i++)
            {                
                // First applyment to db from API and checks if collieId exists in db.
                if (!context.Parcels.Where(x => x.ParcelNumber == ParcelList[i].CollieId).Any())
                {
                    await APIListImport(context);
                }
            };
        }

        internal async Task APIListImport(ApplicationDbContext context)
        {
            for (int i = 0; i < ParcelList.Count; i++)
            {
                var contacts = _mapper.Map<IEnumerable<Contact>>(ParcelList);
                await context.AddRangeAsync(contacts);
                var address = _mapper.Map<IEnumerable<Address>>(ParcelList);
                await context.AddRangeAsync(address);
                var parcel = _mapper.Map<IEnumerable<Parcel>>(ParcelList);
                await context.AddRangeAsync(parcel);
                var destination = _mapper.Map<IEnumerable<Destination>>(ParcelList);
                await context.AddRangeAsync(destination);
                var ordertype = _mapper.Map<IEnumerable<OrderType>>(ParcelList);
                await context.AddRangeAsync(ordertype);
                await context.SaveChangesAsync();

            }
                //var contact = new Contact
                //{
                //    FirstName = ParcelList[i].FirstName,
                //    LastName = ParcelList[i].LastName,
                //    PhoneNumbers = new List<PhoneNumber>()
                //        {
                //                new PhoneNumber() { Number = ParcelList[i].PhoneOne},
                //                new PhoneNumber() { Number = ParcelList[i].PhoneTwo}
                //        }
                //};
                //context.Add(contact);
                //await context.SaveChangesAsync();

                //var address = new Address()
                //{
                //    City = ParcelList[i].City,
                //    Street = ParcelList[i].Adress,
                //    County = ParcelList[i].Country,
                //    PostNumber = ParcelList[i].PostNr
                //};
                //context.Add(address);
                //await context.SaveChangesAsync();

                //var parcel = new Parcel()
                //{
                //    Name = ParcelList[i].ArticleName,
                //    ParcelNumber = ParcelList[i].CollieId,
                //    PickedStatus = false,
                //};
                //context.Add(parcel);
                //await context.SaveChangesAsync();

                //var destination = new Destination()
                //{
                //    AddressId = address.Id,
                //    ContactId = contact.Id,

                //};
                //context.Add(destination);
                //await context.SaveChangesAsync();

                //var ordertyp = new OrderType()
                //{
                //    Description = ParcelList[i].DeliveryType,
                //    Name = ParcelList[i].DeliveryType,
                //};
                //context.Add(ordertyp);
                //await context.SaveChangesAsync();


        }

    }
}
