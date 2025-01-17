﻿using Microsoft.Azure.Cosmos;
using SNIClassLibrary;
using SuplaNotificationIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuplaNotificationIntegration
{
    public class ReportsArchivizer : IReportsArchivizer
    {
        private readonly string CosmosUrl = EnvKeys.SNIDbUri;
        private readonly string DbKey = EnvKeys.SNIDbKey;
        private CosmosClient cosmosClient;
        private Database database;
        private Container container;
        private string databaseId = EnvKeys.SNIDbName;
        public async Task ArchivizeTheReports(List<QuarterlyReport> quarterlyReports)
        {
            this.cosmosClient = new CosmosClient(CosmosUrl, DbKey);
            await this.CreateDatabaseAsync();
            foreach (QuarterlyReport quarterlyReport in quarterlyReports)
            {
                await this.CreateContainerAsync(quarterlyReport.DeviceName);
                await this.CreateDailyReportIfNotExisting(quarterlyReport.DeviceName);
                await this.AppendQuearterlyReportToDailyReport(quarterlyReport);
            }
        }
        private async Task CreateDatabaseAsync()
        {
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        }
        private async Task CreateContainerAsync(string deviceName)
        {
            this.container = await this.database.CreateContainerIfNotExistsAsync(deviceName, "/id");
        }
        private async Task CreateDailyReportIfNotExisting (string deviceName)
        {
            DailyReport dailyReport = new DailyReport();
            dailyReport.DeviceName = deviceName;
            var queryText = $"SELECT * FROM c WHERE c.Date=\"{dailyReport.Date}\"";
            QueryDefinition queryDefinition = new QueryDefinition(queryText);
            FeedIterator<DailyReport> queryResultSetIterator = this.container.GetItemQueryIterator<DailyReport>(queryDefinition);
            if (queryResultSetIterator.HasMoreResults)
            {
                if (!queryResultSetIterator.ReadNextAsync().Result.Any())
                    await this.container.CreateItemAsync<DailyReport>(dailyReport, new PartitionKey(dailyReport.Id.ToString()));
            }
        }
        private async Task AppendQuearterlyReportToDailyReport(QuarterlyReport quarterlyReport)
        {
            var queryText = $"SELECT * FROM c WHERE c.Date=\"{DateTime.Now.ToString("dd/MM/yyyy")}\"";
            QueryDefinition queryDefinition = new QueryDefinition(queryText);
            FeedIterator<DailyReport> queryResultSetIterator = this.container.GetItemQueryIterator<DailyReport>(queryDefinition);
            var todaysReport = queryResultSetIterator.ReadNextAsync().Result.First();
            todaysReport.Messages.AddRange(quarterlyReport.CorrectReadings);
            todaysReport.Messages.AddRange(quarterlyReport.Alerts);
            await this.container.ReplaceItemAsync<DailyReport>(todaysReport, todaysReport.Id, new PartitionKey(todaysReport.Id));
        }
    }
}
