using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Web;
using System.Net;
using System.Net.Http;
using ClassLibraryWpfRest.Models;
using Newtonsoft.Json;

namespace WpfAppClientForRestApi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //url http source web api, you should change port number to suit your own
        private static string url = "http://localhost:57447/api/RestAPI";

        private static HttpClient client = new HttpClient();

        public  MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;

            listBox.SelectionChanged += ListBox_SelectionChanged;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = listBox.SelectedItem as Mobile;

            if (item != null)
            {
                Uri uri = new Uri(item.SourceURL, UriKind.Relative);

                itemImage.Source = new BitmapImage(uri);
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //refresh listbox items
                HelperRefreshListBox();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            var item = listBox.Items.CurrentItem as Mobile;

            if (item != null)
            {
                Uri uri = new Uri(item.SourceURL, UriKind.Relative);

                itemImage.Source = new BitmapImage(uri);
            }
        }



        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            client.Dispose();
            this.Close();
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var item = listBox.SelectedItem as Mobile;

            //send request to rest api to delete item
            if (item != null)
            {               
                    var responseMessage = await client.SendAsync(new HttpRequestMessage()
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(url + "/" + item.Name)
                    });
            }

            //refresh listbox items
            HelperRefreshListBox();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
                // Instantiate the dialog box
                AddModal dlg = new AddModal();

                // Configure the dialog box
                dlg.Owner = this;

                // Open the dialog box modally 
                if (dlg.ShowDialog() == true)
                {
                    MessageBox.Show("data were successfully added");

                    //refresh listbox items
                    HelperRefreshListBox();
                }
            
        }

        //helper method
        private async void HelperRefreshListBox()
        {
            //refresh listbox items
            HttpResponseMessage response = await client.GetAsync(url);

            byte[] arr = await response.Content.ReadAsByteArrayAsync();

            List<Mobile> result = new List<Mobile>();

            result = JsonConvert.DeserializeObject(System.Text.Encoding.UTF8.GetString(arr), typeof(List<Mobile>)) as List<Mobile>;

            listBox.ItemsSource = result;
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            //taking current selected item
            var item = listBox.SelectedItem as Mobile;

            if (item!=null)
            {
                // Instantiate the dialog box
                UpdateModal dlg = new UpdateModal(item);

                // Configure the dialog box
                dlg.Owner = this;

                // Open the dialog box modally 
                if (dlg.ShowDialog() == true)
                {
                    MessageBox.Show("data were successfully updated");

                    //refresh listbox items
                    HelperRefreshListBox();
                }
            }
        }
    }
}
