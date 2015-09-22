using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWA_Xaml_DataBind
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<Recording> MyMusic = new ObservableCollection<Recording>();
        public MainPage()
        {
            this.InitializeComponent();
            // Set the data context to a new Recording.
            textBox1.DataContext = new Recording("Chris Sells", "Chris Sells Live",
                new DateTime(2008, 2, 5));
            // Add items to the collection.
            MyMusic.Add(new Recording("Chris Sells", "Chris Sells Live",
                new DateTime(2008, 2, 5)));
            MyMusic.Add(new Recording("Luka Abrus",
                "The Road to Redmond", new DateTime(2007, 4, 3)));
            MyMusic.Add(new Recording("Jim Hance",
                "The Best of Jim Hance", new DateTime(2007, 2, 6)));

            // Set the data context for the combo box.
            ListBox1.DataContext = MyMusic;
           // load();
        }

        void load()
        {
            // Set the data context to a new Recording.
            textBox1.DataContext = new Recording("Chris Sells", "Chris Sells Live",
                new DateTime(2008, 2, 5));
            // Add items to the collection.
            MyMusic.Add(new Recording("Chris Sells", "Chris Sells Live",
                new DateTime(2008, 2, 5)));
            MyMusic.Add(new Recording("Luka Abrus",
                "The Road to Redmond", new DateTime(2007, 4, 3)));
            MyMusic.Add(new Recording("Jim Hance",
                "The Best of Jim Hance", new DateTime(2007, 2, 6)));

            // Set the data context for the combo box.
            ListBox1.DataContext = MyMusic;
        }

    }

    // A simple business object
    public class Recording
    {
        public Recording() { }

        public Recording(string artistName, string cdName, DateTime release)
        {
            Artist = artistName;
            Name = cdName;
            ReleaseDate = release;
        }

        public string Artist { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }

        // Override the ToString method.
        public override string ToString()
        {
            return Name + " by " + Artist + ", Released: " + ReleaseDate.ToString("d");
        }
    }
}
