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
                report.DeviceName = device.Name;
                if (device.IsConnected == false)
                {
                    report.IncorrectReadings.Add($"{DateTime.Now.ToString("HH: mm")} Cant connect to the {device.Name} device");
                }
                foreach (MeasuredProperty property in device.MeasuredProperties)
                {
                    string message = "";
                    if (property.Actual != null)
                        if (property.Actual > property.Max || property.Actual < property.Min)
                        {
                            message += $"{DateTime.Now.ToString("HH:mm")} {property.Name} threshold exceeded " +
                                $"Max: {property.Max} Min: {property.Min} Actual: {property.Actual}";
                            report.IncorrectReadings.Add(message);
                        }
                        else
                        {
                            message += $"{DateTime.Now.ToString("HH:mm")} {property.Name} " +
                                $"Max: {property.Max} Min: {property.Min} Actual: {property.Actual}";
                            report.CorrectReadings.Add(message.ToString());
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

        public async Task MailTheReport(List<QuarterlyReport> reports)
        {
            
            StringBuilder htmlMessage = new StringBuilder();

            foreach (QuarterlyReport report in reports)
            {
                htmlMessage.AppendLine($"{report.DeviceName} <br>");
                foreach (string alert in report.IncorrectReadings)
                {
                    htmlMessage.AppendLine($"{alert} <br>");
                }
            }
            var mailToList = GetSubscribersList(SubscriptionType.Mail);
            var apiKey = Environment.GetEnvironmentVariable(EnvKeys.SNI_SendGrid_ApiKey);
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(Environment.GetEnvironmentVariable(EnvKeys.SNI_SendGrid_Mail));
            var subject = $"SNI Report {DateTime.Now.ToString("dd MMMMM")}";
            var htmlContent = htmlMessage.ToString();
            var plainTextContent = htmlContent.Replace("<br","");

            foreach (var subscriber in mailToList)
            {
                var msg = MailHelper.CreateSingleEmail(
                from,
                new EmailAddress(subscriber),
                subject,
                plainTextContent,
                htmlContent);
                var response = await client.SendEmailAsync(msg);
            }

        }
    }
}
