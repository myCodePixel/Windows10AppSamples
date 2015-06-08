using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UploadandDownloadBlob
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            bar.IsIndeterminate = true;
            bar.Visibility = Visibility.Visible;
           


            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpeg");

            picker.FileTypeFilter.Add(".jpg");
            picker.SuggestedStartLocation = PickerLocationId.Desktop;
            picker.ViewMode = PickerViewMode.List;
            StorageFile file = await picker.PickSingleFileAsync();
            IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

          

            Helper obj = new Helper();
            CloudBlobContainer blobContainer = await obj.GetCloudBobContainer();
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(Guid.NewGuid().ToString() + ".jpg");


            await blob.UploadFromStreamAsync(stream);
            bar.IsIndeterminate = false;
            bar.Visibility = Visibility.Collapsed;
            MessageDialog msd = new MessageDialog("The image uploaded succesfully");
            await msd.ShowAsync();


        }

        private async void Get_Images_Click(object sender, RoutedEventArgs e)
        {
            Helper obj = new Helper();

            CloudBlobContainer blobContainer = await obj.GetCloudBobContainer();

            BlobContinuationToken continuationToken = null;
            string prefix = null;
            bool useFlatBlobListing = true;
            BlobListingDetails blobListingDetails = BlobListingDetails.All;
            int maxBlobsPerRequest = 10;
            List<IListBlobItem> blobs = new List<IListBlobItem>();


            do
            {
                var listresult = await blobContainer.ListBlobsSegmentedAsync(prefix, useFlatBlobListing, blobListingDetails, maxBlobsPerRequest, continuationToken, null, null);
                continuationToken = listresult.ContinuationToken;
                blobs.AddRange(listresult.Results);
            }
            while (continuationToken != null);
            List<Img> b = new List<Img>();
            foreach (var item in blobs)
            {

                Img im = new Img();
                string str = item.Uri.ToString();
                BitmapImage bmp = new BitmapImage();
                bmp.UriSource = new Uri(str);
                im.bmpImg = bmp;
                b.Add(im);


            }
            gv.ItemsSource = b;


        }



    }
}
