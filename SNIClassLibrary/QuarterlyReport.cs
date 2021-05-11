using System;
using System.Collections.Generic;

namespace SNIClassLibrary
{
    public class QuarterlyReport
    {
        public string Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        public string DeviceName { get; set; }
        public List<string> CorrectReadings { get; set; } = new List<string>();
        public List<string> IncorrectReadings { get; set; } = new List<string>();
    }
}
