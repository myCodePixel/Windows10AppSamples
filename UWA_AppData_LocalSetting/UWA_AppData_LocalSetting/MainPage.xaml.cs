using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWA_AppData_LocalSetting
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (background.Values["bcolor"] != null)
            {
                g1.Background = backcolor(background.Values["bcolor"].ToString());
            }
        }
        Windows.Storage.ApplicationDataContainer background = Windows.Storage.ApplicationData.Current.LocalSettings;

        // Accessing Local Temporary folder
        Windows.Storage.StorageFolder TempFolder = ApplicationData.Current.TemporaryFolder;        
        private void button_Click(object sender, RoutedEventArgs e)
        {

            string co = comboBox.SelectedValue.ToString();
            g1.Background = backcolor(co);
            background.Values["bcolor"] = co;
        }
        public SolidColorBrush backcolor(string s)
        {
            SolidColorBrush sbc = new SolidColorBrush();
            switch (s)
            {
                case "Red":
                    sbc = new SolidColorBrush(Colors.Red);
                    break;
                case "Green":
                    sbc = new SolidColorBrush(Colors.Green);
                    break;
                case "Yellow":
                    sbc = new SolidColorBrush(Colors.Yellow);
                    break;
                case "Aqua":
                    sbc = new SolidColorBrush(Colors.Aqua);
                    break;
                case "Bisque":
                    sbc = new SolidColorBrush(Colors.Bisque);
                    break;
                case "Blue":
                    sbc = new SolidColorBrush(Colors.Blue);
                    break;
                default:
                    sbc = new SolidColorBrush(Colors.White);
                    break;

            }
            return sbc;
        }
    }
}
