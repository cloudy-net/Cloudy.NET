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

        public async Task<MediaProviderResult> List(string path)
        {
            var container = BlobServiceClient.GetBlobContainerClient("media");

            var result = new List<MediaItem>();

            await foreach (var blobPage in container.GetBlobsByHierarchyAsync(prefix: path ?? string.Empty, delimiter: "/").AsPages())
            {
                foreach(var item in blobPage.Values)
                {
                    if (item.IsPrefix)
                    {
                        result.Add(new MediaItem(
                            item.Prefix.TrimEnd('/').Split('/').Last(),
                            "folder",
                            item.Prefix
                        ));
                    }
                    else
                    {
                        result.Add(new MediaItem(
                            item.Blob.Name.Split('/').Last(),
                            "file",
                            container.GetBlobClient(item.Blob.Name).Uri.ToString()
                        ));
                    }
                }
            }

            return new MediaProviderResult(result);
        }
    }
}
