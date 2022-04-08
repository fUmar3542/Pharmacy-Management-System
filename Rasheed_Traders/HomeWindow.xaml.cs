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
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var data = (from c in db.Medicines
                        //from e in db.Stocks.Where(x => x.medicineId == c.id).DefaultIfEmpty()
                        from f in db.Types.Where(y => y.id == c.typeId).DefaultIfEmpty()
                        where c.isDeleted == false
                        select new
                        {
                            Id = c.id,
                            Name = c.name,
                            Type = f.name,
                            Potency = c.potency,
                            Price = c.priceSell,
                            Date = c.createdAt,
                            //quantity = e.quantity,
                        }).ToList();
            table.ItemsSource = data;
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
}
