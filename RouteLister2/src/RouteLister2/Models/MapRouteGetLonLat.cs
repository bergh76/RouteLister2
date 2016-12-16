using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteLister2.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RouteLister2.Models
{
    public class Coordinat
    {
        public int Id { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }

    public class MapRouteGetLonLat
    {
        public List<Coordinat> _coordinatsList { get; set; }
        //public string path = "http://maps.google.com/maps/api/geocode/xml?address=" + _adress + "&sensor=false";
        private readonly ApplicationDbContext _context;

        public MapRouteGetLonLat([FromServices] ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task GetCoordinates(ApplicationDbContext context)
        {

            string _address = "";
            var address = _context.Address.ToList();
            foreach (var item in address)
            {
                _address = item.Street + "+" + item.PostNumber + "+" + item.City;
                string url = "http://maps.google.com/maps/api/geocode/xml?address=" + _address + "&sensor=false";

                //ingen serialisering här. Hämta datat och returna som long och lat
                ApiDeserializer dserial = new ApiDeserializer();
                var result = await dserial.GetPositionCoordinats(url);
                //_coordinatsList = result;
                if (_coordinatsList.Count() != 0)
                {
                    for (int i = 0; i < _coordinatsList.Count(); i++)
                    {
                        Coordinat coordinats = _context.Coordinats.SingleOrDefault(x => x.AddressId == item.Id);
                        if (coordinats != null)
                        {
                            coordinats = AddCoordinatsToDb(i, _context);
                            context.Add(coordinats);
                        }
                    }
                }
                await context.SaveChangesAsync();
            }
        }

        private Coordinat AddCoordinatsToDb(int i, ApplicationDbContext context)
        {
            return new Coordinat()
            {
                AddressId = _coordinatsList[i].AddressId,
                Longitude = _coordinatsList[i].Longitude,
                Latitude = _coordinatsList[i].Latitude,
            };
        }
    }
}