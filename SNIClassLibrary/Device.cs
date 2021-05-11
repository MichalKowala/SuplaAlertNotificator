using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SNIClassLibrary
{
    public class Device
    {
        public string Link { get; set; }
        public string Name { get; set; }
        public bool IsConnected { get; set; }
        public List<MeasuredProperty> MeasuredProperties { get; set; }
        public void CheckMeasures()
        {
            string output = GetResponseFromSupla();
            JObject jObject = JsonConvert.DeserializeObject<JObject>(output);
            IsConnected = (bool)jObject.Property("connected").Value;
            if (IsConnected==true)
            {
                foreach (JProperty p in jObject.Properties())
                { 
                    if (MeasuredProperties.Where(x=>x.Name.ToLower()==p.Name).Any())
                    {
                        MeasuredProperties.Where(x => x.Name.ToLower() == p.Name).First().Actual = (double)p.Value;
                    }
                }
            }
        }
        public string GetResponseFromSupla()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage httpResponseResult = client.GetAsync(Link).Result;
            var output = httpResponseResult.Content.ReadAsStringAsync().Result;
            return output;
        }
    }
   
}
