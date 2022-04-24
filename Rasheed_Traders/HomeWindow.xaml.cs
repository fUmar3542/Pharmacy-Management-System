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
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        List<homeData> list = new List<homeData>() { };
        public HomeWindow()
        {
            InitializeComponent();
            loadData();
            searchedContent.Text = "Search";
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        private void loadData()
        {
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var data = (from c in db.Medicines
                        from e in db.Stocks.Where(x => x.medicineId == c.id).DefaultIfEmpty()
                        from f in db.Types.Where(y => y.id == c.typeId).DefaultIfEmpty()
                        where c.isDeleted == false && e.id != null                      // Stock id exists
                        select new
                        {
                            Name = c.name.ToUpper(),
                            Type = f.name.ToUpper(),
                            Potency = c.potency.ToUpper(),
                            Unit_Price = c.priceBuy,
                            Date = c.createdAt,
                            Quantity = e.quantity,
                        });
            if (data != null)
            {
                list.Clear();
                foreach (var item in data)
                {
                    list.Add(new homeData() { Name = item.Name, Type = item.Type, Potency = item.Potency, Price = item.Unit_Price, Dt = item.Date,Quantity = item.Quantity });
                }
                table.ItemsSource = list;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs r)
        {
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(addBonus))
            {
                string title = "AddOrViewBonus";  /*Your Window Instance Name*/
                var existingWindow = Application.Current.Windows.
                Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                if (existingWindow == null)
                {
                    AddOrViewBonus newWindow = new AddOrViewBonus(); /* Give Your window Instance */
                    newWindow.Title = title;
                    newWindow.Show();
                }
            }
            else if (sender.Equals(addMedicine))
            {
                string title = "NewMedicine";  /*Your Window Instance Name*/
                var existingWindow = Application.Current.Windows.
                Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                if (existingWindow == null)
                {
                    NewMedicine newWindow = new NewMedicine(); /* Give Your window Instance */
                    newWindow.Title = title;
                    newWindow.Show();
                }
            }
            else if (sender.Equals(addSale))
            {
                string title = "AddOrViewSales";  /*Your Window Instance Name*/
                var existingWindow = Application.Current.Windows.
                Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                if (existingWindow == null)
                {
                    AddOrViewSales newWindow = new AddOrViewSales(); /* Give Your window Instance */
                    newWindow.Title = title;
                    newWindow.Show();
                }
            }
            else if (sender.Equals(addType))
            {
                string title = "AddOrViewType";  /*Your Window Instance Name*/
                var existingWindow = Application.Current.Windows.
                Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                if (existingWindow == null)
                {
                    AddOrViewType newWindow = new AddOrViewType(); /* Give Your window Instance */
                    newWindow.Title = title;
                    newWindow.Show();
                }
            }
            else if (sender.Equals(addPurchase))
            {
                string title = "AddOrViewPurchase";  /*Your Window Instance Name*/
                var existingWindow = Application.Current.Windows.
                Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                if (existingWindow == null)
                {
                    AddOrViewPurchase newWindow = new AddOrViewPurchase(); /* Give Your window Instance */
                    newWindow.Title = title;
                    newWindow.Show();
                }
            }
            else if (sender.Equals(ledger))
            {
                string title = "Ledger";  /*Your Window Instance Name*/
                var existingWindow = Application.Current.Windows.
                Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                if (existingWindow == null)
                {
                    Ledger newWindow = new Ledger(); /* Give Your window Instance */
                    newWindow.Title = title;
                    newWindow.Show();
                }
            }
        }

        private void searchedContent_TextChanged(object sender, TextChangedEventArgs ex)
        {
            if (searchedContent.Text == "" || searchedContent.Text == "Search")
            {
                loadData();
                return;
            }
            List<homeData> list1 = new List<homeData>() { };
            foreach (var item in list)
            {
                if(item.Name.Contains(searchedContent.Text.ToUpper()) || item.Type.Contains(searchedContent.Text.ToUpper()))
                    list1.Add(new homeData() { Name = item.Name, Type = item.Type, Potency = item.Potency, Price = item.Price, Dt = item.Dt, Quantity = item.Quantity });
            }
            table.ItemsSource = null;
            table.ItemsSource = list1;
        }
    }

    public class homeData
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string type;
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        private string potency;
        public string Potency
        {
            get { return potency; }
            set { potency = value; }
        }

        private DateTime dt;
        public DateTime Dt
        {
            get { return dt; }
            set { dt = value; }
        }
        private int price;
        public int Price
        {
            get { return price; }
            set { price = value; }
        }
        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
    }
}
