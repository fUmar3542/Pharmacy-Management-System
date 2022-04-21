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
    /// Interaction logic for Partner.xaml
    /// </summary>
    public partial class Partner : Window
    {
        public bool isBuyer = false;
        public Partner(bool s)
        {
            InitializeComponent();
            isBuyer = s;
            name.Text = location.Text = phone.Text = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(cancel))
                this.Close();
            else if (sender.Equals(createButton))
            {
                if (name.Text == "" || location.Text == "" || phone.Text == "" )
                {
                    MessageBox.Show("Please fill out all the fields first");
                    return;
                }
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                var doc = from d in db.TradingParteners
                          select new
                          {
                              name = d.name,
                          };
                foreach (var item in doc)
                {
                    if (item.name == name.Text)
                    {
                        MessageBox.Show("Trading partner already exist");
                        return;
                    }
                }
                TradingPartener u = new TradingPartener { name = name.Text, location = location.Text,phoneNo = phone.Text, createdAt = DateTime.Now, isDeleted = false, isBuyer = isBuyer, isSeller = !isBuyer};
                db.TradingParteners.Add(u);
                db.SaveChanges();
                MessageBox.Show("Trading partner added successfully");
                this.Close();
                updateWindow();
            }
        }
        public void updateWindow()
        {
            string title = "CreateSale";  /*Your Window Instance Name*/
            var existingWindow = Application.Current.Windows.
            Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
            if (existingWindow != null)
            {
                CreateSale newWindow1 = new CreateSale(); /* Give Your window Instance */
                List<TicketInfo> list = ((CreateSale)existingWindow).table.Items.OfType<TicketInfo>().ToList();
                newWindow1.table.ItemsSource = list;
                existingWindow.Close();
                newWindow1.Title = title;
                newWindow1.Show();
            }
            string title1 = "CreatePurchase";  /*Your Window Instance Name*/
            var existingWindow1 = Application.Current.Windows.
            Cast<Window>().SingleOrDefault(x => x.Title.Equals(title1));
            if (existingWindow1 != null)
            {
                CreatePurchase newWindow = new CreatePurchase(); /* Give Your window Instance */
                List<TicketInfo> list = ((CreatePurchase)existingWindow1).table.Items.OfType<TicketInfo>().ToList();
                newWindow.table.ItemsSource = list;
                existingWindow1.Close();
                newWindow.Title = title1;
                newWindow.Show();
            }
        }
    }
}
