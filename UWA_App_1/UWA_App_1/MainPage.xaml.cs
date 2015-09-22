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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWA_App_1
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
        private async void Add_image(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fp = new FileOpenPicker();

            // Adding filters for the file type to access.
            fp.FileTypeFilter.Add(".jpeg");
            fp.FileTypeFilter.Add(".png");
            fp.FileTypeFilter.Add(".bmp");
            fp.FileTypeFilter.Add(".jpg");

            // Using PickSingleFileAsync() will return a storage file which can be saved into an object of storage file class.

            StorageFile sf = await fp.PickSingleFileAsync();

            // Adding bitmap image object to store the stream provided by the object of StorageFile defined above.
            BitmapImage bmp = new BitmapImage();

            // Reading file as a stream and saving it in an object of IRandomAccess.
            IRandomAccessStream stream = await sf.OpenAsync(FileAccessMode.Read);

            // Adding stream as source of the bitmap image object defined above
            bmp.SetSource(stream);

            // Adding bmp as the source of the image in the XAML file of the document.
            image.Source = bmp;
        }
    }
}
