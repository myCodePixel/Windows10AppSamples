using System;

using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using System.Text;
using System.Collections;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWA_Capture_Image
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private StorageFile sf;
        private IRandomAccessStream rs;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Capture_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI cc = new CameraCaptureUI();
            cc.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            cc.PhotoSettings.CroppedAspectRatio = new Size(3, 4);
            cc.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;
            sf = await cc.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (sf != null)
            {
                BitmapImage bmp = new BitmapImage();
                rs = await sf.OpenAsync(FileAccessMode.Read);
                bmp.SetSource(rs);
                image.Source = bmp;
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            // Adding try catch block in case of occurence of an exception
            try
            {
                // Creating object of FileSavePicker and adding few values to the properties of the object.
                FileSavePicker fs = new FileSavePicker();
                fs.FileTypeChoices.Add("Image", new List<string>() { ".jpeg" });
                fs.DefaultFileExtension = ".jpeg";
                fs.SuggestedFileName = "Image" + DateTime.Today.ToString();
                fs.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

                // Using storagefile object defined earlier in above method to save the file using filesavepicker.
                fs.SuggestedSaveFile = sf;

                // Saving the file
                var s = await fs.PickSaveFileAsync();
                if (s != null)
                {
                    using (var dataReader = new DataReader(rs.GetInputStreamAt(0)))
                    {
                        await dataReader.LoadAsync((uint)rs.Size);
                        byte[] buffer = new byte[(int)rs.Size];
                        dataReader.ReadBytes(buffer);
                        await FileIO.WriteBytesAsync(s, buffer);
                    }
                }
            }
            catch (Exception ex)
            {
                var messageDialog = new MessageDialog("Something went wrong.");
                await messageDialog.ShowAsync();
            }

           
        }
    }
}
