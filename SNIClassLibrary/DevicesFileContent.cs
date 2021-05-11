using Newtonsoft.Json;
using System.Collections.Generic;

namespace SNIClassLibrary
{
    public class DevicesFileContent
    {

        [JsonProperty("devices")]
        public List<Device> Devices { get; set; }
    }
}
