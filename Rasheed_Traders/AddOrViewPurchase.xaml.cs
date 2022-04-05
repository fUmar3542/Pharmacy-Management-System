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
    /// Interaction logic for AddOrViewPurchase.xaml
    /// </summary>
    public partial class AddOrViewPurchase : Window
    {
        public AddOrViewPurchase()
        {
            InitializeComponent();
            loadData();
        }

        private void loadData()
        {
            List<Data1> a = new List<Data1>();
            a.Add(new Data1() { ID = 1, BuyerID = 1, Items = 30, Date = new DateTime(2000, 12, 21), DiscountPercentage = "20%", DiscountAmount = 1290, SubTotal = 5150, Total = 4650 });
            a.Add(new Data1() { ID = 2, BuyerID = 2, Items = 37, Date = new DateTime(2010, 12, 21), DiscountPercentage = "40%", DiscountAmount = 1350, SubTotal = 5440, Total = 4890 });
            a.Add(new Data1() { ID = 3, BuyerID = 3, Items = 40, Date = new DateTime(2020, 12, 21), DiscountPercentage = "23%", DiscountAmount = 1200, SubTotal = 6540, Total = 6050 });
            table.ItemsSource = a;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(createPurchase))
            {
                string title = "CreatePurchase";  /*Your Window Instance Name*/
                var existingWindow = Application.Current.Windows.
                Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                if (existingWindow == null)
                {
                    CreatePurchase newWindow = new CreatePurchase(); /* Give Your window Instance */
                    newWindow.Title = title;
                    newWindow.Show();
                }
            }
            else if (sender.Equals(searchButton))
            {

            }
        }
    }
    public class Data1
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
