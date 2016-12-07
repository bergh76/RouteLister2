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
            string path = JsonUrl;
            var data = await jsonSerializer<JsonDataListImports>(path);
            ParcelList = data.ParcelListImports;
            await setDataToPersonIfNotExists(context);
            //await GetCurrentList();
        }

        private async Task GetCurrentList()
        {
            var result = from contact in _context.Contacts
                             //join destination in context.Destinations on contact.Id equals destination.ContactId
                         //join phone in context.PhoneNumbers on contact.Id equals phone.ContactId
                         //join address in context.Address on destination.AddressId equals address.Id
                         //join parcel in context.Parcels on contact.Id equals parcel.Id
                         //join ordertype in context.OrderType on contact.Id equals ordertype.Id

                         select new ParcelListFromCompanyViewModel
                         {
                             FirstName = contact.FirstName,
                             LastName = contact.LastName,
                             Adress = "",
                             City = "",
                             PostNr = "",
                             Country = "",
                             ArticleName = "",
                             CollieId = "",
                             Distributor = "",
                             DeliveryType = "",
                             PhoneOne = _context.PhoneNumbers
                                        .Where(x => x.ContactId == contact.Id).ToList()
                                        .Select(x => x.Number).First(),
                             PhoneTwo = _context.PhoneNumbers
                                        .Where(x => x.ContactId == contact.Id).ToList()
                                        .Select(x => x.Number).Last(),
                             ArticleAmount = 1,
                             DeliveryDate = DateTime.Now
                         };
            ParcelList = result.ToList();
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
            var contacts  =_mapper.Map<IEnumerable<Contact>>(ParcelList);
            List<Contact> test = new List<Contact>();
            
            await context.AddRangeAsync(contacts);

            for (int i = 0; i < ParcelList.Count; i++)
            {
                var contact = new Contact
                {
                    FirstName = ParcelList[i].FirstName,
                    LastName = ParcelList[i].LastName,
                    PhoneNumbers = new List<PhoneNumber>()
                        {
                                new PhoneNumber() { Number = ParcelList[i].PhoneOne},
                                new PhoneNumber() { Number = ParcelList[i].PhoneTwo}
                        }
                };
                context.Add(contact);
                await context.SaveChangesAsync();

                var address = new Address()
                {
                    City = ParcelList[i].City,
                    Street = ParcelList[i].Adress,
                    County = ParcelList[i].Country,
                    PostNumber = ParcelList[i].PostNr
                };
                context.Add(address);
                await context.SaveChangesAsync();

                var parcel = new Parcel()
                {
                    Name = ParcelList[i].ArticleName,
                    ParcelNumber = ParcelList[i].CollieId,
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
                    Description = ParcelList[i].DeliveryType,
                    Name = ParcelList[i].DeliveryType,
                };
                context.Add(ordertyp);
                await context.SaveChangesAsync();

            }
        }

        //internal async Task CheckIfCollieExists(ApplicationDbContext context)
        //{
        //    for (int i = 0; i < ParcelList.Count; i++)
        //    {

        //        var contact = new Contact
        //        {
        //            FirstName = ParcelList[i].FirstName,
        //            LastName = ParcelList[i].LastName,
        //            PhoneNumbers = new List<PhoneNumber>()
        //                {
        //                        new PhoneNumber() { Number = ParcelList[i].PhoneOne},
        //                        new PhoneNumber() { Number = ParcelList[i].PhoneTwo}
        //                }
        //        };
        //        context.Add(contact);
        //        await context.SaveChangesAsync();

        //        var address = new Address()
        //        {
        //            City = ParcelList[i].City,
        //            Street = ParcelList[i].Adress,
        //            County = ParcelList[i].Country,
        //            PostNumber = ParcelList[i].PostNr
        //        };
        //        context.Attach(address);
        //        await context.SaveChangesAsync();

        //        var parcel = new Parcel()
        //        {
        //            Name = ParcelList[i].ArticleName,
        //            ParcelNumber = ParcelList[i].CollieId,
        //            PickedStatus = false,
        //        };
        //        context.Attach(parcel);
        //        await context.SaveChangesAsync();

        //        var destination = new Destination()
        //        {
        //            AddressId = address.Id,
        //            ContactId = contact.Id,
        //        };
        //        context.Attach(destination);
        //        await context.SaveChangesAsync();

        //        var ordertyp = new OrderType()
        //        {
        //            Description = ParcelList[i].DeliveryType,
        //            Name = ParcelList[i].DeliveryType,
        //        };
        //        context.Attach(ordertyp);
        //        await context.SaveChangesAsync();
        //    }
        //}
    }
}
