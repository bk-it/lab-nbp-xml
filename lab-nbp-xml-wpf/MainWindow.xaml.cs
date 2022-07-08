using System;
using System.Windows;
using System.Windows.Controls;
using lab_nbp_xml;

namespace lab_nbp_xml_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RadioButton selectedCurrency;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void changeCurr(object sender, RoutedEventArgs e)
        {
            selectedCurrency = (RadioButton)sender;
        }

        private void download(object sender, RoutedEventArgs e)
        {
            reset();

            try
            {
                DateTime fromRange = DateTime.Parse(from.Text);
                DateTime toRange = DateTime.Parse(to.Text);
                info.Text = "Retrieving data... Please wait";
                var XMLdata = new CurrencyXML(selectedCurrency.Name, fromRange, toRange);
                info.Text = selectedCurrency.Name + " currency info";
                maxPurchase.Text = "Purchase: " + XMLdata.getMax("purchase");
                maxSale.Text = "Sale: " + XMLdata.getMax("sale");
                minPurchase.Text = "Purchase: " + XMLdata.getMin("purchase");
                minSale.Text = "Sale: " + XMLdata.getMin("sale");
                avgPurchase.Text = "Purchase: " + XMLdata.getAvg("purchase");
                avgSale.Text = "Sale: " + XMLdata.getAvg("sale");
                deviationPurchase.Text = "Purchase: " + XMLdata.getDeviation("purchase");
                deviationSale.Text = "Sale: " + XMLdata.getDeviation("sale");
                MessageBox.Show("Data loaded successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void reset()
        {
            string nodata = "No data to display";

            info.Text = nodata;
            maxPurchase.Text = "";
            maxSale.Text = nodata;
            minPurchase.Text = "";
            minSale.Text = nodata;
            avgPurchase.Text = "";
            avgSale.Text = nodata;
            deviationPurchase.Text = "";
            deviationSale.Text = nodata;
        }
    }
}
