using Newtonsoft.Json;
using RouteLister2.Models.ParcelListFromCompanyViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RouteLister2.Data
{
    public class ApiDeserializer
    {
        public List<ParcelListFromCompanyViewModel> ParcelListImports { get; set; }

        public ApiDeserializer() { }
        public async Task<T> JsonSerializer<T>(string path) where T : new()
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
    }
}
