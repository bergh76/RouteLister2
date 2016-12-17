using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RouteLister2.Models;
using RouteLister2.Models.ParcelListFromCompanyViewModel;
using RouteLister2.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace RouteLister2.Data
{
    public class ApiDeserializer
    {
        //public List<ParcelListFromCompanyViewModel> ParcelListImport { get; set; }
        //public List<Coordinat> CoordinatsList { get; set; }
        public ApiDeserializer() { }

        // Returns a list with parcel data
        public async Task<List<ParcelListFromCompanyViewModel>> GetApiListItems(string url)
        {
            var obj = new ParcelListFromCompanyViewModel();
            // Customize URL according to obj parameters
            var data = string.Format(url, obj);

            // Syncronious Consumption
            var syncClient = new HttpClient();
            var content = await syncClient.GetStringAsync(data);
            return JsonConvert.DeserializeObject<List<ParcelListFromCompanyViewModel>>(content);
        }

        // ToDo: Get the coordinates from google.map.api
        // Returns a list with coordinates data
        //public async Task<List<Coordinat>> GetPositionCoordinats(string url)
        //{
        //    var obj = new Coordinat();
        //    // Customize URL according to obj parameters
        //    var data = string.Format(url, obj);

        //    // Asyncronious Consumption
        //    var syncClient = new HttpClient();
        //    var content = await syncClient.GetStringAsync(data);
        //    return JsonConvert.DeserializeObject<List<Coordinat>>(content);

        //}

        //public async Task<List<T>> JsonDserializer<T>(string api) where T : new()
        //{
        //    using (var http = new HttpClient())
        //    {
        //        string data = null;
        //        {
        //            try { data = await http.GetStringAsync(api); }
        //            catch (Exception ex) { new ArgumentException(ex.Message, ex.StackTrace); }
        //            //ToDo: Fix the json string
        //            return !string.IsNullOrEmpty(api) ? JsonConvert.DeserializeObject<List<T>>(data) : new List<T>();
        //        }
        //    }
        //}
    }
    
    //public class RootObject
    //{
    //    public int id { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string City { get; set; }
    //    public string PostNr { get; set; }
    //    public string Adress { get; set; }
    //    public string PhoneOne { get; set; }
    //    public string PhoneTwo { get; set; }
    //    public string Distributor { get; set; }
    //    public string ArticleName { get; set; }
    //    public string CollieId { get; set; }
    //    public string Country { get; set; }
    //    public int ArticleAmount { get; set; }
    //    public string DeliveryType { get; set; }
    //    public string DeliveryDate { get; set; }
    //    public object ParcelListData { get; set; }
    //    ICollection<ParcelListFromCompanyViewModel> RootObjectList { get; set; }
    //}
}
