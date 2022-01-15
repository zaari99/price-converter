using Newtonsoft.Json;
using price_converter.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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

namespace price_converter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Bind_ComboBoxDest();
            Bind_ComboBoxSource();
        }

        private void Bind_ComboBoxDest()
        {
            DataTable Destination  = new DataTable();
            Destination.Columns.Add("Nom");
            Destination.Columns.Add("ID");
            Destination.Rows.Add("USD", "usd-us-dollars");
            Destination.Rows.Add("Ethereum", "eth-ethereum");
            Destination.Rows.Add("XRP", "xrp-xrp");

            //Remplir la liste déroulante des devises (destination)
            destinationComboBox.ItemsSource = Destination.DefaultView;
            destinationComboBox.DisplayMemberPath = "Nom";
            destinationComboBox.SelectedValuePath = "ID";
            destinationComboBox.SelectedIndex = 0;
        }

        private void Bind_ComboBoxSource()
        {
            DataTable Source = new DataTable();
            Source.Columns.Add("Nom");
            Source.Columns.Add("ID");
            Source.Rows.Add("Bitcoin", "btc-bitcoin");
            Source.Rows.Add("Euro", "eur-euro-token");
            Source.Rows.Add("Neurochain", "ncc-neurochain");

            //Remplir la liste déroulante des devises (source)
            sourceComboBox.ItemsSource = Source.DefaultView;
            sourceComboBox.DisplayMemberPath = "Nom";
            sourceComboBox.SelectedValuePath = "ID";
            sourceComboBox.SelectedIndex = 0;
        }

      
        private void Button_Click_Convertir(object sender, RoutedEventArgs e)
        {
            string quote_currency_id = (string)destinationComboBox.SelectedValue;
            string base_currency_id = (string)sourceComboBox.SelectedValue;
            string amount = montant.Text;


            String url= $"http://api.coinpaprika.com/v1/price-converter?base_currency_id={base_currency_id}&&quote_currency_id={quote_currency_id}&&amount={amount}";

            try {
                //envoyer une requête HTTP pour récupérer le fichier json  
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.ContentType = "application/json; charset=utf-8";
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);

                        //CONVERTIR le json vers un objet

                        Response R = JsonConvert.DeserializeObject<Response>(reader.ReadToEnd());

                        //l’affichage du résultat

                        resultat.Content = R.price;
               
                    }
            }
            catch (System.Net.WebException x) {
                //CONVERTIR le json vers un objet qui contient un attribut error 
                BadResponse BR = JsonConvert.DeserializeObject<BadResponse>(new StreamReader(x.Response.GetResponseStream()).ReadToEnd());

                //Afficher le message d'erreur si on a une WebException
                MessageBox.Show($"erreur :  {BR.error }",
                    x.Message, 
                    MessageBoxButton.OK, MessageBoxImage.Information); 

            }
            catch (Exception a)
            {
                //Afficher le message d'erreur (cas generale)
                MessageBox.Show("erreur", a.Message, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        // la méthode est utilisée pour effacer la valeur de toutes les champs
        private void Button_Click_Effacer(object sender, RoutedEventArgs e)
        {
            resultat.Content = "00.00";
            montant.Text = String.Empty;
            
            if (sourceComboBox.Items.Count > 0)
                sourceComboBox.SelectedIndex = 0;
            if (destinationComboBox.Items.Count > 0)
                destinationComboBox.SelectedIndex = 0;
            montant.Focus();
        }

        //regex pour accepter seulement les chiffres dans  l'element  TextBox
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
