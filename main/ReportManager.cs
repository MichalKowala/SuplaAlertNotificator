using SNIClassLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SuplaNotificationIntegration
{
    public class ReportManager
    {
        public string PrepareReportMessage(List<Device> devices)
        {
            StringBuilder message = new StringBuilder();
            foreach (Device device in devices)
            {
                if (device.IsConnected==false)
                {
                    message.AppendLine(DateTime.Now.ToString());
                    message.AppendLine($"Warning: could not connect to {device.Link}");
                    message.AppendLine();
                }
                foreach (MeasuredProperty property in device.MeasuredProperties)
                {
                    if (property.Actual!=null)
                    {
                        if (property.Actual>property.Max || property.Actual<property.Min)
                        {
                            message.AppendLine(DateTime.Now.ToString());
                            message.AppendLine($"Warning: Threshold exceeded at {device.Link}");
                            message.AppendLine($"{property.Name} - Expected MAX:{property.Max} MIN:{property.Min} ACTUAL:{property.Actual}");
                            message.AppendLine();
                        }
                    }
                }
            }
            if (message.Length != 0)
                return message.ToString();
            else
                return "ok";
        }
        public async Task MailTheReport(string message)
        {
            message = "ALERT Kto gra w dotkę?";
            var apiKey = Environment.GetEnvironmentVariable("test_klucz");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("");
            var to = new EmailAddress("");
            var subject = $"Report {DateTime.Now}";
            var plainTextContent = message;
            var htmlContent = "<strong> test siemaneczko </strong>";
            var msg = MailHelper.CreateSingleEmail(
                from,
                to,
                subject,
                plainTextContent,
                plainTextContent);
            var response = await client.SendEmailAsync(msg);
            Console.WriteLine();
        }
    }
}
