using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Cloudy.CMS.UI.FieldTypes.MediaPicker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.AzureMediaPicker
{
    public class AzureMediaProvider : IMediaProvider
    {
        BlobServiceClient BlobServiceClient { get; }

        public AzureMediaProvider(IConfiguration configuration)
        {
            BlobServiceClient = new BlobServiceClient(configuration.GetConnectionString("azuremedia"));
        }

        public async Task<MediaProviderResult> List(int pageSize, int page)
        {
            var container = BlobServiceClient.GetBlobContainerClient("media");

            await foreach (var blobPage in container.GetBlobsAsync().AsPages(default, pageSize))
            {
                var items = new List<MediaProviderResultItem>();
                foreach (BlobItem item in blobPage.Values)
                {
                    items.Add(new MediaProviderResultItem(
                        item.Name,
                        container.GetBlobClient(item.Name).Uri.ToString()
                    ));
                }
                return new MediaProviderResult(items, !string.IsNullOrEmpty(blobPage.ContinuationToken) ? blobPage.ContinuationToken : null, null);
            }

            return null;
        }
    }
}
