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
        public string PrepareReportMessage()
        {
            List<Device> devices = GetDevicesList();
            foreach (Device d in devices)
                d.CheckMeasures();

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
