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
using Newtonsoft.Json;
using System.Net.Http;
using System.Xml.Linq;
using System.Net;
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.DataTransfer;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWA_JSON
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        public List<Photo> photoList = new List<Photo>();

        public MainPage()
        {
            this.InitializeComponent();
            GetImages();
        }
        public async void GetImages()
        {
            // For the Api Url written below, just get the new Api Url from the flickr api link the blog
            string provideUri = "https://api.flickr.com/services/rest/?method=flickr.photos.getRecent&api_key=98e8be2e7b751241cab220aff0117ee6&format=json&nojsoncallback=1";

            HttpClient client = new HttpClient();
            string jsonstring = await client.GetStringAsync(provideUri);
            var obj = JsonConvert.DeserializeObject<RootObject>(jsonstring);
            if (obj.stat == "ok")
            {
                FlickrGridView.ItemsSource = obj.photos.photo;
            }
            
        }
    }
}
