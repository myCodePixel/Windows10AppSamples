using System;
using System.Collections.Generic;
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
using Windows.Storage.Pickers;
using Windows.Storage.FileProperties;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
//using Bing.Maps;
//using Windows.Devices.Geolocation;
using System.Net.Http;
using Newtonsoft.Json;
using Windows.World;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ImageData
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

        ImageProperties[] imgProp = new ImageProperties[100000];
        BitmapImage[] bitImg = new BitmapImage[100000];
        static int count = -1;
        IReadOnlyList<StorageFile> sfiles;
        async private void Button_Click(object sender, RoutedEventArgs e)
        {


            //FileOpenPicker fop = new FileOpenPicker();
            //fop.ViewMode = PickerViewMode.Thumbnail;
            //fop.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            //fop.FileTypeFilter.Add(".jpeg");
            //fop.FileTypeFilter.Add(".jpg");
            //fop.FileTypeFilter.Add(".png");
            //StorageFile sf =await fop.PickSingleFileAsync();
            FolderPicker fp = new FolderPicker();

            fp.ViewMode = PickerViewMode.List;
            fp.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fp.FileTypeFilter.Add(".jpeg");
            fp.FileTypeFilter.Add(".jpg");
            fp.FileTypeFilter.Add(".png");
            var sfs = await fp.PickSingleFolderAsync();
            //FlipView fv = new FlipView();
            try
            {
                if (sfs != null)
                {
                    sfiles = await sfs.GetFilesAsync();

                    foreach (StorageFile f in sfiles)
                    {
                        count++;
                        using (IRandomAccessStream ir = await f.OpenAsync(FileAccessMode.Read))
                        {
                            BitmapImage bi = new BitmapImage();
                            BitmapImage lbi = bi;


                            bi.SetSource(ir);
                            fv.Items.Add(bi);
                            lv.Items.Add(bi);
                            imgProp[count] = await f.Properties.GetImagePropertiesAsync();
                            //sp.Children.Add(img);



                        }

                    }
                    // sp.Children.Add(fv);
                }
            }
            catch (Exception exc)
            {

            }


        }

        async private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {



                if (lv.SelectedItem != null)
                {

                    int i = lv.SelectedIndex;
                    lv.ScrollIntoView(lv.SelectedItem);
                    fv.SelectedItem = fv.Items[i];
                    StringBuilder outputText = new StringBuilder();


                    Properties.Text = "     Properties of Image     ";
                    DateTaken.Text = "Date Taken             : ";
                    vDate.Text = imgProp[i].DateTaken.ToString();

                    Rating.Text = "Rating                     : ";
                    vRating.Text = imgProp[i].Rating.ToString();

                    CameraManufacturer.Text = "Camera Manufacturer        : ";
                    vCameraManufacturer.Text = imgProp[i].CameraManufacturer;

                    CameraModel.Text = "Camera Model      : ";
                    vCameraModel.Text = imgProp[i].CameraModel.ToString();

                    Place.Text = "Place                       : ";

                    //Location location = new Location(Convert.ToDouble(imgProp[i].Latitude), Convert.ToDouble(imgProp[i].Longitude));
                    // myMap.Center = new Location(Convert.ToDouble(imgProp[i].Latitude), Convert.ToDouble(imgProp[i].Longitude));

                    myMap.ZoomLevel = 15.0;
                    //var pp = myMap.Children.First(p => (p.GetType() == typeof(Pushpin)));
                    //myMap.Children.Remove(pp);
                 //   Pushpin pushpin = new Pushpin();


                  //  MapLayer.SetPosition(pushpin, location);
                   // myMap.Children.Add(pushpin);
                    //myMap.Center = new Location(Convert.ToDouble(imgProp[i].Latitude), Convert.ToDouble(imgProp[i].Longitude));

                    Uri u = new Uri("http://dev.virtualearth.net/REST/v1/Locations/" + imgProp[i].Latitude + "," + imgProp[i].Longitude + "?o=json&key=	Asoh6QzcSn15uP_cEYhkFDJmAiyurB3qdmIPzJYHtTCN_ZElhSC0Y89xjcT7GlO3");
                    HttpClient client = new HttpClient();


                    string jsondata = await client.GetStringAsync(u);
                    ImageData.jsonParse.RootObject myobj = JsonConvert.DeserializeObject<ImageData.jsonParse.RootObject>(jsondata);
                    vPlace.Text = myobj.resourceSets[0].resources[0].name.ToString();

                }

            }
            catch (Exception e1)
            {

            }

        }

        private void fv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                StringBuilder sb = new StringBuilder();
                int i = ((FlipView)sender).SelectedIndex;
                lv.SelectedItem = lv.Items[i];

                //sb.AppendLine("*****Properties of Image***** ");
                //sb.AppendLine("");
                //sb.AppendLine("Date taken                              : " + imgProp[i].DateTaken);

                //sb.AppendLine("");
                //sb.AppendLine("Rating                                    : " + imgProp[i].Rating);

                //sb.AppendLine("");
                //sb.AppendLine("Camera Model                       :  " + imgProp[i].CameraModel);

                //sb.AppendLine("");



                //sb.AppendLine("Latitude of Place taken          : " + imgProp[i].Latitude);

                //sb.AppendLine("");

                //sb.AppendLine("Longitude of the Place taken : " + imgProp[i].Longitude);
                //sb.AppendLine("");

                //tb.Text = sb.ToString();
            }
            catch (Exception ex)
            {

            }
        }
        

}
}
