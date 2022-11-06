using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Cloudy.CMS.UI.FieldTypes.MediaPicker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

        public async Task<MediaProviderListResult> List(string path)
        {
            var container = BlobServiceClient.GetBlobContainerClient("media");

            var result = new List<MediaItem>();

            await foreach (var blobPage in container.GetBlobsByHierarchyAsync(prefix: path ?? string.Empty, delimiter: "/").AsPages())
            {
                foreach(var item in blobPage.Values)
                {
                    if (item.IsPrefix)
                    {
                        var prefix = item.Prefix.TrimEnd('/').Split('/').Last();
                        result.Add(new MediaItem(
                            prefix,
                            "folder",
                            prefix
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

            return new MediaProviderListResult(result);
        }

        public async Task<MediaProviderUploadResult> Upload(string path, IFormFile file)
        {
            using var read = file.OpenReadStream();

            var container = BlobServiceClient.GetBlobContainerClient("media");
            var blobClient = container.GetBlobClient($"{path}/{file.FileName}");

            var result = await blobClient.UploadAsync(read, false);

            return new MediaProviderUploadResult(blobClient.Uri.ToString());
        }
    }
}
