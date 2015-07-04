using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WindowsPreview.Media.Ocr;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace OCRReader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WriteableBitmap bitmap;
        // OCR engine instance used to extract text from images.
        private OcrEngine ocrEngine;

        public MainPage()
        {
            this.InitializeComponent();
            ocrEngine = new OcrEngine(OcrLanguage.English);
            TextOverlay.Children.Clear();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("TestImages\\SQuotes.jpg");
            await LoadImage(file);
        }
        private async Task LoadImage(StorageFile file)
        {
            ImageProperties imgProp = await file.Properties.GetImagePropertiesAsync();

            using (var imgStream = await file.OpenAsync(FileAccessMode.Read))
            {
                bitmap = new WriteableBitmap((int)imgProp.Width, (int)imgProp.Height);
                bitmap.SetSource(imgStream);
                PreviewImage.Source = bitmap;
            }
        }
        private async void ExtractText_Click(object sender, RoutedEventArgs e)
        {
            //// Prevent another OCR request, since only image can be processed at the time at same OCR engine instance.
            //ExtractTextButton.IsEnabled = false;

            // Check whether is loaded image supported for processing.
            // Supported image dimensions are between 40 and 2600 pixels.
            if (bitmap.PixelHeight < 40 ||
                bitmap.PixelHeight > 2600 ||
                bitmap.PixelWidth < 40 ||
                bitmap.PixelWidth > 2600)
            {
                ImageText.Text = "Image size is not supported." +
                                    Environment.NewLine +
                                    "Loaded image size is " + bitmap.PixelWidth + "x" + bitmap.PixelHeight + "." +
                                    Environment.NewLine +
                                    "Supported image dimensions are between 40 and 2600 pixels.";
                //ImageText.Style = (Style)Application.Current.Resources["RedTextStyle"];

                return;
            }

            // This main API call to extract text from image.
            var ocrResult = await ocrEngine.RecognizeAsync((uint)bitmap.PixelHeight, (uint)bitmap.PixelWidth, bitmap.PixelBuffer.ToArray());

            // OCR result does not contain any lines, no text was recognized. 
            if (ocrResult.Lines != null)
            {
                // Used for text overlay.
                // Prepare scale transform for words since image is not displayed in original format.
                var scaleTrasform = new ScaleTransform
                {
                    CenterX = 0,
                    CenterY = 0,
                    ScaleX = PreviewImage.ActualWidth / bitmap.PixelWidth,
                    ScaleY = PreviewImage.ActualHeight / bitmap.PixelHeight,
                };

                if (ocrResult.TextAngle != null)
                {

                    PreviewImage.RenderTransform = new RotateTransform
                    {
                        Angle = (double)ocrResult.TextAngle,
                        CenterX = PreviewImage.ActualWidth / 2,
                        CenterY = PreviewImage.ActualHeight / 2
                    };
                }

                string extractedText = "";

                // Iterate over recognized lines of text.
                foreach (var line in ocrResult.Lines)
                {
                    // Iterate over words in line.
                    foreach (var word in line.Words)
                    {
                        var originalRect = new Rect(word.Left, word.Top, word.Width, word.Height);
                        var overlayRect = scaleTrasform.TransformBounds(originalRect);

                        var wordTextBlock = new TextBlock()
                        {
                            Height = overlayRect.Height,
                            Width = overlayRect.Width,
                            FontSize = overlayRect.Height * 0.8,
                            Text = word.Text,

                        };

                        // Define position, background, etc.
                        var border = new Border()
                        {
                            Margin = new Thickness(overlayRect.Left, overlayRect.Top, 0, 0),
                            Height = overlayRect.Height,
                            Width = overlayRect.Width,
                            Background = new SolidColorBrush(Colors.Orange),
                            Opacity = 0.5,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            Child = wordTextBlock,

                        };
                        OverlayTextButton.IsEnabled = true;
                        // Put the filled textblock in the results grid.
                        TextOverlay.Children.Add(border);
                        extractedText += word.Text + " ";
                    }
                    extractedText += Environment.NewLine;
                }

                ImageText.Text = extractedText;

            }
            else
            {
                ImageText.Text = "No text.";

            }
        }

        private void OverlayText_Click(object sender, RoutedEventArgs e)
        {
            if (TextOverlay.Visibility == Visibility.Visible)
            {
                TextOverlay.Visibility = Visibility.Collapsed;
            }
            else
            {
                TextOverlay.Visibility = Visibility.Visible;
            }
        }

    }
}
