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
    /// Interaction logic for AddOrViewType.xaml
    /// </summary>
    public partial class AddOrViewType : Window
    {
        public AddOrViewType()
        {
            InitializeComponent();
            loadData();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(cancel))
                this.Close();
            else if (sender.Equals(create))
            {
                if (typeName.Text == null )
                {
                    MessageBox.Show("Enter the type name");
                    return;
                }
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                var doc = from d in db.Types
                          select d;
                foreach (var item in doc)
                {
                    if (item.name == typeName.Text)
                    {
                        MessageBox.Show("Type name already exist");
                        return;
                    }
                }
                Type u = new Type { name = typeName.Text.ToUpper(), createdAt = DateTime.Now };
                db.Types.Add(u);
                db.SaveChanges();
                MessageBox.Show("Type Created successfully");
                loadData();
                typeName.Text = "";
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
            string t = "NewMedicine";
            var ex = Application.Current.Windows.
                  Cast<Window>().SingleOrDefault(x => x.Title.Equals(t));
            if (ex != null)
            {
                ex.Close();
                NewMedicine newWindow1 = new NewMedicine(); /* Give Your window Instance */
                newWindow1.Title = t;
                newWindow1.Show();
            }
        }
        private void loadData()
        {
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var doc = from d in db.Types
                      where d.isDeleted == false
                      select new
                      {
                          Name = d.name.ToUpper(),
                          Date = d.createdAt
                      };
            table.ItemsSource = doc.ToList();
        }
    }
}