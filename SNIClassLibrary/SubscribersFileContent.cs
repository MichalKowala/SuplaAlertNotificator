using Newtonsoft.Json;
using System.Collections.Generic;

namespace SNIClassLibrary
{
    public class SubscribersFileContent
    {
        [JsonProperty("email")]
        public List<string> Emails { get; set; }
        [JsonProperty("sms")]
        public List<string> SMSes { get; set; }
    }
}
