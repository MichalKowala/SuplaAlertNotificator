using Microsoft.Azure.Storage.Blob;

namespace SuplaNotificationIntegration.Interfaces
{
    public interface IStorageAccessHelper
    {
        CloudBlockBlob GetSNIContainerBlockBlobReference(string filename);
    }
}
