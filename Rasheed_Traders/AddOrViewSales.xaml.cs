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
    /// Interaction logic for AddOrViewSales.xaml
    /// </summary>
    public partial class AddOrViewSales : Window
    {
        public AddOrViewSales()
        {
            InitializeComponent();
            searchedContent.Text = "Search";
        }
        public void loadData()
        {
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var doc2 = from d in db.Sales
                       where d.isDeleted == false && d.isPurchase == false
                       select new
                       {
                           TOTAL_ITEMS = d.items,
                           BUYER_NAME = d.Name.ToUpper(),
                           SUBTOTAL = d.subTotal,
                           TOTAL = d.total,
                           DATE = d.createdAt,
                           Discount_Percentage = d.discount,
                           Discount_Amount = d.discountAmount,
                       };
            table.ItemsSource = doc2.ToList();
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
                           where d.isDeleted == false && d.isPurchase == false
                           select d;
                foreach (var item in doc2)
                {
                    if (a == index)
                    {
                        string title = "SaleItems";  /*Your Window Instance Name*/
                        var existingWindow = Application.Current.Windows.
                        Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                        if (existingWindow == null)
                        {
                            SaleItems newWindow = new SaleItems(item.id); /* Give Your window Instance */
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
            if (sender.Equals(createSale))
            {
                string title = "CreateSale";  /*Your Window Instance Name*/
                var existingWindow = Application.Current.Windows.
                Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                if (existingWindow == null)
                {
                    CreateSale newWindow = new CreateSale(); /* Give Your window Instance */
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
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var doc2 = from d in db.Sales
                    where d.isDeleted == false && d.isPurchase == false && d.Name.Contains(searchedContent.Text)
                    select new
                    {
                        TOTAL_ITEMS = d.items,
                        BUYER_NAME = d.Name.ToUpper(),
                        SUBTOTAL = d.subTotal,
                        TOTAL = d.total,
                        DATE = d.createdAt,
                        Discount_Percentage = d.discount,
                        Discount_Amount = d.discountAmount,
                    };
            table.ItemsSource = doc2.ToList();
        }
    }
}
