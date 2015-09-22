using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.Core;
using Windows.Media.Editing;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWA_Video_Capture
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

        private async void Capture_button_Click(object sender, RoutedEventArgs e)
        {
            // Declaring CameraCaptureUI to call the default Video capturing tool in the system
            CameraCaptureUI cc = new CameraCaptureUI();
            cc.VideoSettings.Format = CameraCaptureUIVideoFormat.Mp4;
            cc.VideoSettings.MaxResolution = CameraCaptureUIMaxVideoResolution.HighDefinition;

            /* saving the video temporarily in the storage file object 
            and setting the streaming source for our media element to show the information*/
            sf = await cc.CaptureFileAsync(CameraCaptureUIMode.Video);
            if (sf != null)
            {
                rs = await sf.OpenAsync(FileAccessMode.Read);
                MediaClip mc = await MediaClip.CreateFromFileAsync(sf);
                MediaComposition mcomp = new MediaComposition();
                mcomp.Clips.Add(mc);
                MediaStreamSource mss = mcomp.GeneratePreviewMediaStreamSource((int) video.ActualWidth, (int)video.ActualHeight);
                video.SetMediaStreamSource(mss);
            }
        }

        private async void save_Video(object sender, RoutedEventArgs e)
        {
            // Adding try catch block in case of occurence of an exception
            try
            {
                // Creating object of FileSavePicker and adding few values to the properties of the object.
                FileSavePicker fs = new FileSavePicker();
                fs.FileTypeChoices.Add("Video", new List<string>() { ".mp4",".3gp" });
                fs.DefaultFileExtension = ".mp4";
                fs.SuggestedFileName = "Video" + DateTime.Today.ToString();
                fs.SuggestedStartLocation = PickerLocationId.VideosLibrary;

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
