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
    }
}