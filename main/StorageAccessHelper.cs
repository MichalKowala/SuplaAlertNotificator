using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using SuplaNotificationIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuplaNotificationIntegration
{
    public class StorageAccessHelper : IStorageAccessHelper
    {
        private readonly CloudStorageAccount _storage;
        private readonly CloudBlobClient _client;
        private readonly CloudBlobContainer _container;
        public StorageAccessHelper()
        {
            _storage = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable(EnvKeys.StorageConnString));
            _client = _storage.CreateCloudBlobClient();
            _container = _client.GetContainerReference(Environment.GetEnvironmentVariable(EnvKeys.SniContainer));
        }
        public CloudBlockBlob GetSNIContainerBlockBlobReference(string filename)
        {
            return _container.GetBlockBlobReference(filename);
        }
        
    }
}
