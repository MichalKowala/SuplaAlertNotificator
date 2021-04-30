using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SNIClassLibrary
{
    public class DevicesFileContent
    {

        [JsonProperty("devices")]
        public List<Device> Devices { get; set; }
    }
}
