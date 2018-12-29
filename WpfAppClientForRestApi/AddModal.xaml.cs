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
    /// Interaction logic for AddModal.xaml
    /// </summary>
    public partial class AddModal : Window
    {
        string itemSource = null;

        //url http source web api, you should change port number to suit your own
        private static string url = "http://localhost:57447/api/RestAPI";

        public AddModal()
        {
            InitializeComponent();     
        }

        private async void btnYes_Click(object sender, RoutedEventArgs e)
        {
            if (tbName.Text != ""&&(itemSource!=""||itemSource!=null))
            {
                string SourceURL = String.Concat("wwwroot/images/", itemSource);
                
                Mobile t = new Mobile(tbName.Text.ToString(), SourceURL);

                //send data to rest api and it will rewrite json file data
                using (HttpClient client = new HttpClient())
                {
                    string con = JsonConvert.SerializeObject(t);

                    var content = new StringContent(con, Encoding.UTF8, "application/json");

                    var responseMessage = await client.PostAsync(url, content);  
                }

                // Dialog box accepted
                DialogResult = true;
            }
            else MessageBox.Show("Input all data please");
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
            }
        }
    }
}
