using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SNIClassLibrary
{
    public class DailyReport
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Date { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public string DeviceName { get; set; }
        public List<string> Messages = new List<string>();


    }
}
