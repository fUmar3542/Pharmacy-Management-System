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
        bool isMaximized = false;
        public HomeWindow()
        {
            InitializeComponent();
            loadData();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            if (isMaximized)
            {
                isMaximized = false;
                menu.Margin = new Thickness(0, 0, 0, 630);
                menu.Width = ActualWidth - 100;
            }
            else
            {
                isMaximized = true;
                menu.Margin = new Thickness(0, 0, 0, 375);
            }
        }

        private void loadData()
        {
            List<Home> a = new List<Home>();
            a.Add(new Home() { ID = 1, Type = "Syrup", Name = "Serum", Potency = "100mg", Quantity = 20 });
            a.Add(new Home() { ID = 2, Type = "Tab", Name = "Panadol", Potency = "10mg", Quantity = 200 });
            a.Add(new Home() { ID = 3, Type = "Syrup", Name = "Bronocol", Potency = "80mg", Quantity = 30 });
            table.ItemsSource = a;
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
    }
    public class Home
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Potency { get; set; }
        public int Quantity { get; set; }
    }
}
