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
        List<Type1> list = new List<Type1>() { };
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
                if (typeName.Text == "")
                {
                    MessageBox.Show("Enter the type name");
                    return;
                }
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                var doc = from d in db.Types where d.isDeleted == false
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
            else if (sender.Equals(delete))
            {
                if (typeName.Text == "")
                    return;

                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                var doc = (from d in db.Types
                            where d.name == typeName.Text && d.isDeleted == false
                            select d).SingleOrDefault();
                if (doc == null)
                {
                    MessageBox.Show("Item not found");
                    return;
                }
                if (doc.Medicines.Count > 0)
                {
                    MessageBox.Show("Type can't be deleted. Delete the respective Medicines first");
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
                        MessageBox.Show("Type Deleted successfully");
                        typeName.Text = "";
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
            list.Clear();
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var doc = from d in db.Types
                      where d.isDeleted == false
                      select new
                      {
                          Name = d.name.ToUpper(),
                          Date = d.createdAt,
                      };
            string s = "";
            foreach (var item in doc)
            {
                s = item.Date.ToString("dd/MM/yyyy HH:MM:ss");
                list.Add(new Type1() { Name = item.Name, Dt = s});
            }
            table.ItemsSource = "";
            table.ItemsSource = list;
        }

        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = ItemsControl.ContainerFromElement((DataGrid)sender,
                                                 e.OriginalSource as DependencyObject) as DataGridRow;
            if (row == null)
                return;
            else
            {
                int a = table.SelectedIndex, index = 0;
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                var doc2 = from d in db.Types
                           where d.isDeleted == false
                           select d;
                foreach (var item in doc2)
                {
                    if (a == index)
                    {
                        typeName.Text = item.name;
                    }
                    index++;
                }
            }
        }

        public class Type1
        {
            private string name;
            public string Name
            {
                get { return name; }
                set { name = value; }
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