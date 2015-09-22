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
using System.Xml.Linq;
using Windows.Data.Xml.Dom;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWA_XML_Binding
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string Mediaurl;
        private string title;
        public MainPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        private void xmlbutton_Click(object sender, RoutedEventArgs e)
        {
            XDocument xdoc = XDocument.Load("xmlFile.xml");
            IEnumerable<flipimage> images = from img in xdoc.Descendants("Image") select new flipimage(img.Element("ImageTitle").Value, img.Element("ImageUrl").Value);
            PhotoGrid.DataContext = images;
        }
    }
}
