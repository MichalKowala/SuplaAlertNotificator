using System;
using System.Collections.Generic;
using System.Text;

namespace SNIClassLibrary
{
    public class Device
    {
        public string Link { get; set; }
        public bool IsConnected { get; set; }
        public List<MeasuredProperty> MeasuredProperties { get; set; }
    }
}
