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
        List<saleView> list = new List<saleView>() { };
        public AddOrViewPurchase()
        {
            InitializeComponent();
            loadData();
            searchedContent.Text = "Search";
        }

        private void loadData()
        {
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var doc2 = from d in db.Sales
                       where d.isDeleted == false && d.isPurchase == true
                       select new
                       {
                           TOTAL_ITEMS = d.items,
                           SELLER_NAME = d.Name.ToUpper(),
                           SUBTOTAL = d.subTotal,
                           TOTAL = d.total,
                           DATE = d.createdAt,
                           Discount_Percentage = d.discount,
                           Discount_Amount = d.discountAmount,
                       };
            list.Clear();
            String s = "";
            foreach (var item in doc2)
            {
                s = item.DATE.ToString("dd/MM/yyyy HH:mm:ss");
                list.Add(new saleView() { Items = item.TOTAL_ITEMS, Name = item.SELLER_NAME, SubTotal = item.SUBTOTAL, Total = item.TOTAL, Dt = s, DiscountPercentage = Convert.ToDouble(item.Discount_Percentage), DiscountAmount = Convert.ToDouble(item.Discount_Amount) });
            }
            table.ItemsSource = list;
        }

        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = ItemsControl.ContainerFromElement((DataGrid)sender,
                                                e.OriginalSource as DependencyObject) as DataGridRow;
            if (row == null)
                return;
            else
            {
                var s = table.SelectedItem;
                int a = table.SelectedIndex, index = 0;
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                var doc2 = from d in db.Sales
                           where d.isDeleted == false && d.isPurchase == true
                           select d;
                foreach (var item in doc2)
                {
                    if (a == index)
                    {
                        string title = "purchaseItems";  /*Your Window Instance Name*/
                        var existingWindow = Application.Current.Windows.
                        Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                        if (existingWindow == null)
                        {
                            purchaseItems newWindow = new purchaseItems(item.id); /* Give Your window Instance */
                            newWindow.Title = title;
                            newWindow.Show();
                        }
                    }
                    index++;
                }

            }
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
        }

        private void searchedContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchedContent.Text == "" || searchedContent.Text == "Search")
            {
                loadData();
                return;
            }
            List<saleView> list1 = new List<saleView>() { };
            foreach (var item in list)
            {
                if (item.Name.Contains(searchedContent.Text.ToUpper()))
                    list1.Add(new saleView() { Items = item.Items, Name = item.Name, SubTotal = item.SubTotal, Total = item.Total, Dt = item.Dt, DiscountPercentage = Convert.ToDouble(item.DiscountPercentage), DiscountAmount = Convert.ToDouble(item.DiscountAmount) });
            }
            table.ItemsSource = list1;
        }
    }
}
