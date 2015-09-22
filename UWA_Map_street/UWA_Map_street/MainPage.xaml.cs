using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWA_Map_street
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
  

        private async void Panorama_Click(object sender, RoutedEventArgs e)
        {
            BasicGeoposition cityPosition = new BasicGeoposition() { Latitude = 48.858, Longitude = 2.295 };
            Geopoint cityCenter = new Geopoint(cityPosition);
            StreetsidePanorama panoramaNearCity = await StreetsidePanorama.FindNearbyAsync(cityCenter);
            StreetsideExperience ssView = new StreetsideExperience(panoramaNearCity);
            ssView.OverviewMapVisible = true;
            MapControl1.CustomExperience = ssView;
        }

        private async void threeD_View_Click(object sender, RoutedEventArgs e)
        {
            MapControl1.Style = MapStyle.Aerial3D;
            BasicGeoposition hwGeoposition = new BasicGeoposition() { Latitude = 48.858, Longitude = 2.295 };
            Geopoint hwPoint = new Geopoint(hwGeoposition);


            MapScene hwScene = MapScene.CreateFromLocationAndRadius(hwPoint, 80, 0, 60);


            await MapControl1.TrySetSceneAsync(hwScene, MapAnimationKind.Bow);
        }
    }
}
