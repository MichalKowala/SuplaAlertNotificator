using System;
using System.Collections.Generic;
using System.Text;

namespace SNIClassLibrary
{
    public class DailyReport
    {
        public string Id { get; set; }
        public string Date { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public string DeviceName { get; set; }
       


    }
}
