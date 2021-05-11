using SNIClassLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Newtonsoft.Json;
using SuplaNotificationIntegration.Interfaces;

namespace SuplaNotificationIntegration
{
    public class ReportsManager : IReportsManager
    {
        private readonly IStorageAccessHelper _storageAccessHelper;
        public ReportsManager(IStorageAccessHelper storageAccessHelper)
        {
            _storageAccessHelper = storageAccessHelper;
        }
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
                    report.IncorrectReadings.Add($"{DateTime.Now.ToString("HH:mm")} Cant connect to the {device.Name} device");
                }
                foreach (MeasuredProperty property in device.MeasuredProperties)
                {
                    string message = "";
                    if (property.Actual != null)
                        if (property.Actual > property.Max || property.Actual < property.Min)
                        {
                            message += $"{DateTime.Now.ToString("HH:mm")} {device.Name} {property.Name} threshold exceeded " +
                                $"Max: {property.Max} Min: {property.Min} Actual: {property.Actual}";
                            report.IncorrectReadings.Add(message);
                        }
                        else
                        {
                            message += $"{DateTime.Now.ToString("HH:mm")} {device.Name} {property.Name} " +
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
            var devicesAsJson = _storageAccessHelper
                .GetSNIContainerBlockBlobReference(EnvKeys.DevicesFile)
                .DownloadText();
            var fileContent = JsonConvert.DeserializeObject<DevicesFileContent>(devicesAsJson);
            foreach (Device d in fileContent.Devices)
            {
                //We're doing this because cosmos db containers cant have '/' in their names
                //and each device will have a container with its corresponding name
                d.Name = d.Name.Replace('/', '-');
            }
            return fileContent.Devices;
        }


        private List<string> GetSubscribersList(SubscriptionType subType)
        {
            var devicesAsJson = _storageAccessHelper
                .GetSNIContainerBlockBlobReference(EnvKeys.SubscribersFile)
                .DownloadText();
            var fileContent = JsonConvert.DeserializeObject<SubscribersFileContent>(devicesAsJson);

            if (subType == SubscriptionType.Mail)
                return fileContent.Emails;
            else
                return fileContent.SMSes;
        }

        public async Task MailTheReports(List<QuarterlyReport> reports)
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
            var apiKey = EnvKeys.SNISendGridApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(EnvKeys.SNISendGridMail);
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

        public async Task SMSTheReports(List<QuarterlyReport> reports)
        {
            throw new NotImplementedException();
        }
    }
}
