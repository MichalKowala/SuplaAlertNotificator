using SNIClassLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;
using System;
using System.IO;
using Newtonsoft.Json;

namespace SuplaNotificationIntegration
{
    public class ReportManager
    {
        public List<QuarterlyReport> GenerateQuarterlyReports()
        {
            List<Device> devices = GetDevicesList();
            List<QuarterlyReport> reports = new List<QuarterlyReport>();
            foreach (Device device in devices)
            {
                device.CheckMeasures();
            }
            foreach (Device device in devices)
            {
                QuarterlyReport report = new QuarterlyReport();
                report.DeviceName = device.Link;
                if (device.IsConnected == false)
                {
                    report.IncorrectReadings.Add("Cant connect to the device");
                }
                foreach (MeasuredProperty property in device.MeasuredProperties)
                {
                    var sb = new StringBuilder();
                    if (property.Actual != null)
                        if (property.Actual > property.Max || property.Actual < property.Min)
                        {
                            sb.AppendLine("Threshold Exceeded");
                            sb.AppendLine($"Max: {property.Max}");
                            sb.AppendLine($"Min: {property.Min}");
                            sb.AppendLine($"Actual: {property.Actual}");
                            report.IncorrectReadings.Add(sb.ToString());
                        }
                        else
                        {
                            sb.AppendLine($"Max: {property.Max}");
                            sb.AppendLine($"Min: {property.Min}");
                            sb.AppendLine($"Actual: {property.Actual}");
                            report.CorrectReadings.Add(sb.ToString());
                        }
                }
                reports.Add(report);
            }
            return reports;
        }

        private List<Device> GetDevicesList()
        {
            StorageAccessHelper storage = new StorageAccessHelper();
            var devicesAsJson = storage
                .GetSNIContainerBlockBlobReference(Environment.GetEnvironmentVariable(EnvKeys.DevicesFile))
                .DownloadText();
            var fileContent = JsonConvert.DeserializeObject<DevicesFileContent>(devicesAsJson);
            return fileContent.Devices;
        }

        private List<string> GetSubscribersList(SubscriptionType subType)
        {
            StorageAccessHelper storage = new StorageAccessHelper();
            var devicesAsJson = storage
                .GetSNIContainerBlockBlobReference(Environment.GetEnvironmentVariable(EnvKeys.SubscribersFile))
                .DownloadText();
            var fileContent = JsonConvert.DeserializeObject<SubscribersFileContent>(devicesAsJson);

            if (subType == SubscriptionType.Mail)
                return fileContent.Emails;
            if (subType == SubscriptionType.SMS)
                return fileContent.SMSes;

            else return new List<string>();
        }

        public async Task MailTheReport(string message)
        {
            var mailSubscribers = GetSubscribersList(SubscriptionType.Mail);

            var apiKey = Environment.GetEnvironmentVariable("test_klucz");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("romthannatar@gmail.com");
            var subject = $"Report {DateTime.Now}";
            var plainTextContent = message;
            var htmlContent = "<strong> test siemaneczko </strong>";

            foreach (var subscriber in mailSubscribers)
            {
                var msg = MailHelper.CreateSingleEmail(
                from,
                new EmailAddress(subscriber),
                subject,
                plainTextContent,
                plainTextContent);
                var response = await client.SendEmailAsync(msg);
            }

        }
    }
}
