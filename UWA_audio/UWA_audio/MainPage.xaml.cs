using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWA_audio
{
    public enum AudioEncodingFormat
    {
        Mp3,
        M4a,
        Wav,
        Wma
    };
    public static class AudioEncodingFormatExtensions
    {
        public static string ToFileExtension(this AudioEncodingFormat encodingFormat)
        {
            switch (encodingFormat)
            {
                case AudioEncodingFormat.Mp3:
                    return ".mp3";
                case AudioEncodingFormat.M4a:
                    return ".M4a";
                case AudioEncodingFormat.Wav:
                    return ".wav";
                case AudioEncodingFormat.Wma:
                    return ".wma";
                default:
                    throw new ArgumentOutOfRangeException("encodingFormat"); // to show that no format is being selected.
            }
        }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private MediaCapture CaptureMedia; // To record the audio in this object
        private IRandomAccessStream AudioStream; // To save audio in this stream so that it can be saved at a particular location
        private FileSavePicker FileSave; // To open the file save picker option
        private DispatcherTimer DishTImer; // For timer and using it
        private TimeSpan SpanTime; // To count the time for which the timer is working 
        private AudioEncodingFormat selectedFormat; // Enum type to check for the format
        private AudioEncodingQuality SelectedQuality; // Declaring a pre defined enum to go for quality of audio being caputed.
        public MainPage()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await InitMediaCapture();
            LoadAudioEncodings();
            LoadAudioQualities();
            InitTimer();

        }

        private void InitTimer()
        {
            DishTImer = new DispatcherTimer();
            DishTImer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            DishTImer.Tick += DishTImer_Tick; // Adding the event to the DishTImer object while it hits tick event
        }

        private void DishTImer_Tick(object sender, object e)
        {
            SpanTime = SpanTime.Add(DishTImer.Interval);
            Duration.DataContext = SpanTime;
        }

        private void LoadAudioQualities()
        {
            var audioQualities = Enum.GetValues(typeof(AudioEncodingQuality)).Cast<AudioEncodingQuality>();
            AudioQuality.ItemsSource = audioQualities;
            AudioQuality.SelectedItem = AudioEncodingQuality.Auto;
        }

        private void LoadAudioEncodings()
        {
            var audioEncodingFormats = Enum.GetValues(typeof(AudioEncodingFormat)).Cast<AudioEncodingFormat>();
            AudioFormat.ItemsSource = audioEncodingFormats;
            AudioFormat.SelectedItem = AudioEncodingFormat.Mp3;
        }

        private async Task InitMediaCapture()
        {
            CaptureMedia = new MediaCapture();
            var captureSettings = new MediaCaptureInitializationSettings();
            captureSettings.StreamingCaptureMode = StreamingCaptureMode.Audio;
            await CaptureMedia.InitializeAsync(captureSettings);
            CaptureMedia.Failed += CaptureMedia_Failed;
            CaptureMedia.RecordLimitationExceeded += CaptureMedia_RecordLimitationExceeded;
        }

        private async void CaptureMedia_RecordLimitationExceeded(MediaCapture sender)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                await sender.StopRecordAsync();
                var warningMessage = new MessageDialog("The media recording has been stopper as you have exceeded the maximum length", "Recording Stopped");
                await warningMessage.ShowAsync();

            });
        }

        private async void CaptureMedia_Failed(MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                var warningMessage = new MessageDialog("The media recording Failed " + errorEventArgs.Message, "Capture Failed");
                await warningMessage.ShowAsync();
            });
        }




        private async void Record_click(object sender, RoutedEventArgs e)
        {
            MediaEncodingProfile encodingProfile = null;
            switch (selectedFormat)
            {
                case AudioEncodingFormat.Mp3:
                    encodingProfile = MediaEncodingProfile.CreateMp3(SelectedQuality);
                    break;
                case AudioEncodingFormat.M4a:
                    encodingProfile = MediaEncodingProfile.CreateM4a(SelectedQuality);
                    break;
                case AudioEncodingFormat.Wma:
                    encodingProfile = MediaEncodingProfile.CreateWav(SelectedQuality);
                    break;
                case AudioEncodingFormat.Wav:
                    encodingProfile = MediaEncodingProfile.CreateWav(SelectedQuality);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();

            }

            AudioStream = new InMemoryRandomAccessStream();
            await CaptureMedia.StartRecordToStreamAsync(encodingProfile, AudioStream);
            DishTImer.Start();

        }

        private async void Stop_Click(object sender, RoutedEventArgs e)
        {
            await CaptureMedia.StopRecordAsync();
            DishTImer.Stop();
        }

        private async void Save_click(object sender, RoutedEventArgs e)
        {
            var mediaFile = await FileSave.PickSaveFileAsync();
            if (mediaFile != null)
            {
                using (var dataReader = new DataReader(AudioStream.GetInputStreamAt(0)))
                {
                    await dataReader.LoadAsync((uint)AudioStream.Size);
                    byte[] buffer = new byte[(int)AudioStream.Size];
                    dataReader.ReadBytes(buffer);
                    await FileIO.WriteBytesAsync(mediaFile, buffer);
                }
            }
        }


        private void AudioQuality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedQuality = (AudioEncodingQuality)AudioQuality.SelectedItem;
        }

        private void AudioFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedFormat = (AudioEncodingFormat)AudioFormat.SelectedItem;
            InitFileSavePicker();
        }

        private void InitFileSavePicker()
        {
            FileSave = new FileSavePicker();
            FileSave.FileTypeChoices.Add("Encoding", new List<string>() { selectedFormat.ToFileExtension() });
            FileSave.SuggestedStartLocation = PickerLocationId.MusicLibrary;

        }
    }
}
