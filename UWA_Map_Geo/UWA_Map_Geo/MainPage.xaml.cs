using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWA_Map_Geo
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

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    Geolocator gl = new Geolocator();
                    Geoposition gp = await gl.GetGeopositionAsync();
                    Geopoint myloc = gp.Coordinate.Point;
                    map1.Center = myloc;
                    map1.ZoomLevel = 15;
                    map1.LandmarksVisible = true;
                    MapIcon mi = new MapIcon();
                    mi.Location = myloc;
                    mi.NormalizedAnchorPoint = new Point(0.5, 1.0);
                    mi.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/pin.png"));
                    mi.ZIndex = 0;
                    map1.MapElements.Add(mi);
                    break;
                case GeolocationAccessStatus.Denied:
                    break;
                case GeolocationAccessStatus.Unspecified:
                    break;
            }

        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (comboBox.SelectedValue.ToString())
            {
                case "None":
                    map1.Style = MapStyle.None;
                    break;
                case "Aerial":
                    map1.Style = MapStyle.Aerial;
                    break;
                case "Aerial3D":
                    map1.Style = MapStyle.Aerial3D;
                    break;
                case "Aerial3DWithRoads":
                    map1.Style = MapStyle.Aerial3DWithRoads;
                    break;
                case "AerialWithRoads":
                    map1.Style = MapStyle.AerialWithRoads;
                    break;
                case "Road":
                    map1.Style = MapStyle.Road;
                    break;
                case "Terrain":
                    map1.Style = MapStyle.Terrain;
                    break;
            }
        }
    }
}