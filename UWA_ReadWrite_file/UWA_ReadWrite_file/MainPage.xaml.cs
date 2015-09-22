using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWA_ReadWrite_file
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        StorageFile sampleFile;
        StorageFile sampleFile1;

        public MainPage()
        {
            this.InitializeComponent();
            Func();
        }

        public async void Func()
        {
            sampleFile = await storageFolder.GetFileAsync("sample.txt");
            sampleFile1 = await storageFolder.CreateFileAsync("sample.txt", CreationCollisionOption.OpenIfExists);
        }

        private async void ReadTextAsync_Click(object sender, RoutedEventArgs e)
        {
            string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            txtbox.Text = text;
        }

        private async void ReadBufferAsync_Click(object sender, RoutedEventArgs e)
        {
            var buffer = await Windows.Storage.FileIO.ReadBufferAsync(sampleFile);
            DataReader dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer);
            string text = dataReader.ReadString(buffer.Length);
            txtbox.Text = text;
        }

        private async void ReadviaStream_Click(object sender, RoutedEventArgs e)
        {
            var stream = await sampleFile.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

            var size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0))
            {
                // Add code to use the stream to read your file
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                string text = dataReader.ReadString(numBytesLoaded);
                txtbox.Text = text;
            }

        }

        private async void WriteTextAsync_Click(object sender, RoutedEventArgs e)
        {
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile1, txtbox.Text);
            txtbox.Text = "";
        }

        private async void WriteBufferAsync_Click(object sender, RoutedEventArgs e)
        {
            var buffer = Windows.Security.Cryptography.CryptographicBuffer.ConvertStringToBinary(
                        txtbox.Text, Windows.Security.Cryptography.BinaryStringEncoding.Utf8);
            await Windows.Storage.FileIO.WriteBufferAsync(sampleFile1, buffer);
            txtbox.Text = "";
        }

        private async void WriteviaStream_Click(object sender, RoutedEventArgs e)
        {
            var stream = await sampleFile1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
            using (var outputStream = stream.GetOutputStreamAt(0))
            {

                // Add code to use the stream to write to your file
                DataWriter dataWriter = new DataWriter(outputStream);
                dataWriter.WriteString(txtbox.Text);
                txtbox.Text = "";
                await dataWriter.StoreAsync();
                await outputStream.FlushAsync();
            }

        }
    }
}
