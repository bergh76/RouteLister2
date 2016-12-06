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

        public JsonDataListImports(ApplicationDbContext context)
        {
            _context = context;
        }

        public JsonDataListImports() { }
        private async Task<T> jsonSerializer<T>(string path) where T : new()
        {
            using(var http = new HttpClient())
            {
                var data = string.Empty;
                {
                    try
                    {
                        data = await http.GetStringAsync(path);
                    }
                    catch (Exception) { }
                    return !string.IsNullOrEmpty(path) 
                        ? JsonConvert.DeserializeObject<T>(data) : new T();
                }

            }
        }

        public async Task getParcelData()
        {
            string path = JsonUrl;

            var data = await jsonSerializer<JsonDataListImports>(path);
            ParcelList = data.ParcelListImports;
            await setDataToPersonIfNotExists();
        }

        public async Task setDataToPersonIfNotExists()
        {
            for (int i = 0; i < ParcelList.Count; i++)
            {
                var person = new Contact
                {
                    FirstName = ParcelList[i].FirstName,
                    LastName = ParcelList[i].LastName,
                };
                //await _context.SaveChangesAsync();
                //await context.AddAsync(person);
                var phone = new List<PhoneNumber>()
                        {
                            new PhoneNumber() { Number = ParcelList[i].PhoneOne, ContactId = person.Id },
                            new PhoneNumber() { Number = ParcelList[i].PhoneTwo , ContactId = person.Id}
                        };
                //await context.AddAsync(phone);
                //await _context.SaveChangesAsync();
                var address = new Address()
                {
                    City = ParcelList[i].City,
                    Street = ParcelList[i].Adress,
                    County = ParcelList[i].Country,
                    PostNumber = ParcelList[i].PostNr
                };
                //await context.AddAsync(address);
                //await _context.SaveChangesAsync();
                var parcel = new Parcel()
                {
                    Name = ParcelList[i].ArticleName,
                    ParcelNumber = ParcelList[i].CollieId,
                    PickedStatus = false,
                };
                //await context.AddAsync(parcel);
                //await _context.SaveChangesAsync();
                var destination = new Destination()
                {
                    AddressId = address.Id,
                    ContactId = person.Id,
                };
                //await context.AddAsync(destination); 
                //await context.AddRangeAsync(person, address, parcel, destination);

            };
            await _context.SaveChangesAsync();

        }

        //public Contact addContact(Contact person)
        //{
        //    if (!_context.Contacts.Where(x => x.FirstName + x.LastName == person.FirstName + person.LastName).Any())
        //    {
        //        _context.Attach(person);
        //    }
        //    return person;
        //}
        //public Address addAdress(Address address)
        //{
        //    if (!_context.Address.Where(x => x.Street == address.Street).Any())
        //    {
        //        _context.Attach(address);
        //    }
        //    return address;
        //}
    }
}
