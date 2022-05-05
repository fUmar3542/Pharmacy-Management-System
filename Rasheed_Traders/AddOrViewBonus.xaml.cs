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
    /// Interaction logic for AddOrViewBonus.xaml
    /// </summary>
    public partial class AddOrViewBonus : Window
    {
        List<Bonus> list = new List<Bonus>() { };
        public AddOrViewBonus()
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
                if (bonusName.Text == "")
                {
                    MessageBox.Show("Enter bonus name");
                    return;
                }
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                var doc = from d in db.Bonus where d.isDeleted == false
                          select d;
                foreach (var item in doc)
                {
                    if (item.name == bonusName.Text)
                    {
                        MessageBox.Show("Bonus name already exist");
                        return;
                    }
                }
                Bonu b = new Bonu { createdAt = DateTime.Now, name = bonusName.Text, description = description.Text, isDeleted = false };
                db.Bonus.Add(b);
                db.SaveChanges();
                MessageBox.Show("Bonus Created successfully");
                loadData();
                bonusName.Text = "";
                description.Text = "";
                updateWindow();
            }
            else if (sender.Equals(Delete))
            {
                if (bonusName.Text == "")
                    return;   
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                var doc = (from d in db.Bonus
                            where d.name == bonusName.Text && d.isDeleted == false
                            select d).SingleOrDefault();
                if(doc == null)
                {
                    MessageBox.Show("Item not found");
                        return;
                }    
                if (doc.SaleItems.Count > 0)
                {
                    MessageBox.Show("Bonus can't be deleted. Delete the respective Sale/Purchase first");
                    return;
                }
                if (doc != null)
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        doc.isDeleted = true;
                        db.SaveChanges();
                        loadData();
                        MessageBox.Show("Bonus Deleted successfully");
                        bonusName.Text = "";
                        description.Text = "";
                        updateWindow();
                    }
                }
                
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
        private void loadData()
        {
            list.Clear();
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var doc = from d in db.Bonus
                      where d.isDeleted == false
                      select new
                      {
                          Name = d.name,
                          Date = d.createdAt,
                          Description = d.description
                      };
            string s = "";
            foreach(var item in doc)
            {
                s = item.Date.ToString("dd/MM/yyyy HH:MM:ss");
                list.Add(new Bonus() {Name = item.Name,Dt = s,Description = item.Description});
            }
            tableData.ItemsSource = "";
            tableData.ItemsSource = list;
        }

        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = ItemsControl.ContainerFromElement((DataGrid)sender,
                                                 e.OriginalSource as DependencyObject) as DataGridRow;
            if (row == null)
                return;
            else
            {
                int a = tableData.SelectedIndex, index = 0;
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                var doc2 = from d in db.Bonus
                           where d.isDeleted == false
                           select d;
                foreach (var item in doc2)
                {
                    if (a == index)
                    {
                        bonusName.Text = item.name;
                        description.Text = item.description;
                    }
                    index++;
                }
            }
        }
        public class Bonus
        {
            private string name;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            private string description;
            public string Description
            {
                get { return description; }
                set { description = value; }
            }
            private string date;
            public string Dt
            {
                get { return date; }
                set { date = value; }
            }
        }
    }
}
