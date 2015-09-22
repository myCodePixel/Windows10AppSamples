using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace UploadandDownloadBlob
{
    class Helper
    {

        public async Task<CloudBlobContainer> GetCloudBobContainer()
        {
            var credentials = new StorageCredentials("demotrilok", "f11NOwXeBMd7zX29MhKWTZ+++kgiIKa+HuLPbYFbdK7jHquMUKf4M+mpYO2ANdW5ouwrWqUPRiAAZx+sd8Rz9A==");
            var account = new CloudStorageAccount(credentials, true);
            var blobClient = account.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("democontainer");
            if (await container.CreateIfNotExistsAsync())
            {
                await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
            return container;
        }
    }

    class Img
    {
        public BitmapImage bmpImg { set; get; }
    }
}
