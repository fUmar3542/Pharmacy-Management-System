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

        List<saleView> list = new List<saleView>() { };
        public AddOrViewSales()
        {
            InitializeComponent();
            searchedContent.Text = "Search";
        }

        public void Focus(object sender, RoutedEventArgs e)
        {
            searchedContent.Text = "";
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
            list.Clear();
            String s = "";
            foreach (var item in doc2)
            {
                s = item.DATE.ToString("dd/MM/yyyy HH:mm:ss");
                list.Add(new saleView() { Items = item.TOTAL_ITEMS, Name = item.BUYER_NAME, SubTotal = item.SUBTOTAL, Total = Math.Round(Convert.ToDouble(item.TOTAL)), Dt = s, DiscountPercentage = Convert.ToDouble(item.Discount_Percentage), DiscountAmount = Convert.ToDouble(Math.Round(Convert.ToDouble(item.Discount_Amount))) });
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
            List<saleView> list1 = new List<saleView>() { };
            foreach (var item in list)
            {
                if(item.Name.Contains(searchedContent.Text.ToUpper()))
                    list1.Add(new saleView() { Items = item.Items, Name = item.Name, SubTotal = item.SubTotal, Total = Math.Round(Convert.ToDouble(item.Total)), Dt = item.Dt, DiscountPercentage = Convert.ToDouble(item.DiscountPercentage), DiscountAmount = Convert.ToDouble(Math.Round(Convert.ToDouble(item.DiscountAmount))) });
            }
            table.ItemsSource = list1;
        }
    }

    public class saleView
    {
        private int items;
        public int Items
        {
            get { return items; }
            set { items = value; }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private double subTotal;
        public double SubTotal
        {
            get { return subTotal; }
            set { subTotal = value; }
        }
        private double total;
        public double Total
        {
            get { return total; }
            set { total = value; }
        }

        private string dt;
        public string Dt
        {
            get { return dt; }
            set { dt = value; }
        }

        private double discountAmount;
        public double DiscountAmount
        {
            get { return discountAmount; }
            set { discountAmount = value; }
        }
        private double discountPercentage;
        public double DiscountPercentage
        {
            get { return discountPercentage; }
            set { discountPercentage = value; }
        }
    }
}
