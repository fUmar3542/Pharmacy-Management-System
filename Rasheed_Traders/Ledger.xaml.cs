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

namespace Rasheed_Traders
{
    /// <summary>
    /// Interaction logic for Ledger.xaml
    /// </summary>
    public partial class Ledger : Window
    {
        public Ledger()
        {
            InitializeComponent();
            loadData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void loadData()
        {
            List<Data> a = new List<Data>();
            a.Add(new Data() { ID = 1, BuyerID = 1, Items = 30, Date = new DateTime(2000, 12, 21), DiscountPercentage = "20%", DiscountAmount = 1290, SubTotal = 5150, Total = 4650 });
            a.Add(new Data() { ID = 2, BuyerID = 2, Items = 37, Date = new DateTime(2010, 12, 21), DiscountPercentage = "40%", DiscountAmount = 1350, SubTotal = 5440, Total = 4890 });
            a.Add(new Data() { ID = 3, BuyerID = 3, Items = 40, Date = new DateTime(2020, 12, 21), DiscountPercentage = "23%", DiscountAmount = 1200, SubTotal = 6540, Total = 6050 });
            table.ItemsSource = a;
        }
    }
    public class Data
    {
        public int ID { get; set; }
        public int BuyerID { get; set; }
        public int Items { get; set; }
        public DateTime Date { get; set; }
        public string DiscountPercentage { get; set; }
        public int DiscountAmount { get; set; }
        public int SubTotal { get; set; }
        public int Total { get; set; }
    }
}
