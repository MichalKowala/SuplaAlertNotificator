using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
