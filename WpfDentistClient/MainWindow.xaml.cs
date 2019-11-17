using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
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
using WpfDentistClient.ViewModels;

namespace WpfDentistClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<CountryVM> _countries = new ObservableCollection<CountryVM>();
        public MainWindow()
        {
            InitializeComponent();
            //_countries.Add(new CountryVM
            //{
            //    Id = 1,
            //    Name = "jon"
            //});

            string privatUrl = "http://localhost:51160/api/country?page=1&name=jon";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(privatUrl);
            request.Method = "GET";
            String resultStr = String.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                resultStr = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
            }
            _countries = JsonConvert.DeserializeObject<ObservableCollection<CountryVM>>(resultStr);

            dgSimple.ItemsSource = _countries;
        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://localhost:51160/api/country";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            CountryAddVM country = new CountryAddVM
            {
                Name = "Niger",
                FlagImage = "https://static.drukukr.com/catalog/14/world-00286__1446562318__615.jpg"
            };

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(country);
                    //"{\"user\":\"test\"," +
                              //"\"password\":\"bla\"}";
                streamWriter.Write(json);
            }
            try
            {
                String resultStr = String.Empty;
                using (HttpWebResponse response = 
                    (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    resultStr = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                }
                var resCountry = JsonConvert.DeserializeObject<CountryVM>(resultStr);
                _countries.Add(resCountry);
            }
            catch
            {
                MessageBox.Show("Помилка додавання");
            }
            
        }
    }
}
