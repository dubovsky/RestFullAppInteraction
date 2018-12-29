using Microsoft.Win32;
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
using System.Windows.Shapes;
using System.Net.Http;
using ClassLibraryWpfRest.Models;
using Newtonsoft.Json;

namespace WpfAppClientForRestApi
{
    /// <summary>
    /// Interaction logic for UpdateModal.xaml
    /// </summary>
    public partial class UpdateModal : Window
    {
        string itemSource = null;
        string lateName = null;
        string lateItemSource = null;
        bool ImageNameChangedflag = false;

        //url http source web api, you should change port number to suit your own
        private static string url = "http://localhost:57447/api/RestAPI";

        public UpdateModal(Mobile t)
        {
            InitializeComponent();

            lateName = t.Name;
            tbName.Text = t.Name;
            itemSource = t.SourceURL;
            lateItemSource = t.SourceURL;
        }

        private async void btnYes_Click(object sender, RoutedEventArgs e)
        {
            if (tbName.Text != "")
            {             
                //Before update we should delete first late item,
                //send request to rest api to delete item
                Mobile item = new Mobile(lateName, lateItemSource);
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var responseMessage = await client.SendAsync(new HttpRequestMessage()
                        {
                            Method = HttpMethod.Delete,
                            RequestUri = new Uri(url + "/" + item.Name)
                        });
                    }
                }

                Mobile mobile = new Mobile();
                mobile.Name = tbName.Text.ToString();

                if (ImageNameChangedflag)
                {
                    mobile.SourceURL = String.Concat("wwwroot/images/", itemSource);
                }
                else mobile.SourceURL = itemSource;

                    //send data to rest api and it will rewrite json file data
                    using (HttpClient client = new HttpClient())
                {
                    string con = JsonConvert.SerializeObject(mobile);

                    var content = new StringContent(con, Encoding.UTF8, "application/json");

                    var responseMessage = await client.PutAsync(url, content);
                }

                // Dialog box accepted
                DialogResult = true;
            }
            else MessageBox.Show("You must feel name field please");
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            // Dialog box canceled
            DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".jpg"; // Default file extension

            dlg.Filter = "Image documents (.jpg)|*.jpg"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // file name
                itemSource = dlg.SafeFileName;
                ImageNameChangedflag = true;
            }
        }
    }
}
