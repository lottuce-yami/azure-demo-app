using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Azure.Functions.Worker;
using AzureDemoAppAzureFunction.Services;

namespace AzureDemoAppAzureFunction
{
    public class BlobStorageTrigger
    {
        private readonly EmailService _emailService;

        public BlobStorageTrigger(EmailService emailService)
        {
            _emailService = emailService;
        }

        [Function(nameof(BlobStorageTrigger))]
        public Task Run([BlobTrigger("documents/{name}")] BlobClient blobClient, string name)
        {
            var recipient = GetMetadata(blobClient, "recipient");
            var uri = blobClient.GenerateSasUri(
                BlobSasPermissions.Read,
                DateTimeOffset.UtcNow.AddHours(1)
            );
            
            _emailService.SendMail(
                recipient: recipient,
                subject: "Blob Storage Notification",
                message: $"Your file <b>{name}</b> has been successfully uploaded.<br>Link (valid for 1 hour): {uri}"
            );
            
            return Task.CompletedTask;
        }

        public static string GetMetadata(BlobClient blobClient, string key)
        {
            return blobClient.GetPropertiesAsync().Result.Value.Metadata[key];
        }
    }
}
