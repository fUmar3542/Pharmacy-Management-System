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
using System.Collections.ObjectModel;

namespace Rasheed_Traders
{
    /// <summary>
    /// Interaction logic for CreatePurchase.xaml
    /// </summary>
    /// 

    public partial class CreatePurchase : Window
    {
        public ObservableCollection<string> Positions { get; set; }

        //List<TicketInfo> ticketsList = new List<TicketInfo>
        //{
        //    new TicketInfo{ mediStatus="True",mediCombo = new ObservableCollection<string>() { "Forward", "Defense", "Goalie" },typeStatus="True", typeCombo=new ObservableCollection<string>() { "Forward", "Defense", "Goalie" },Quantity=1}
        //};
        public CreatePurchase()
        {
            InitializeComponent();
            loadData();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(createSeller))
            {
                string title = "Partner";  /*Your Window Instance Name*/
                var existingWindow = Application.Current.Windows.
                Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                if (existingWindow == null)
                {
                    Partner newWindow = new Partner(false); /* Give Your window Instance */
                    newWindow.Title = title;
                    newWindow.Show();
                }
            }
            else if (sender.Equals(addRow))
            {
                //TicketInfo t = new TicketInfo { mediStatus = "True", mediCombo = new ObservableCollection<string>() { "Forward", "Defense", "Goalie" }, typeStatus = "True", typeCombo = new ObservableCollection<string>() { "Forward", "Defense", "Goalie" }, Quantity = 1 };
                //List<TicketInfo> list = table.Items.OfType<TicketInfo>().ToList();
                //list.Add(t);
                //table.ItemsSource = null;
                //table.ItemsSource = list;
            }
            else if (sender.Equals(removeRow))
            {
                var selectedItem = table.SelectedItem;
                if (selectedItem != null)
                {
                    //List<TicketInfo> list = table.Items.OfType<TicketInfo>().ToList();
                    //for (int i = 0; i < list.Count; i++)
                    //{
                    //    if (list[i] == selectedItem)
                    //        list.Remove(list[i]);
                    //}
                    //table.ItemsSource = null;
                    //table.ItemsSource = list;
                }
            }
            else if (sender.Equals(done))
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Sale Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)  // error is here
                {
                    Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                    var doc = from d in db.TradingParteners
                              select new
                              {
                                  name = d.name,
                              };
                    foreach (var item in doc)
                    {
                        combobox.Items.Add(item.name);
                    }
                }
            }
        }
        private void loadData()
        {
            //Positions = new ObservableCollection<string>() { "Forward", "Defense", "Goalie" };
            //combobox.ItemsSource = Positions;
            //table.Visibility = Visibility.Visible;
            //table.RowHeight = 28;
            //table.ItemsSource = ticketsList;
            //cm.ItemsSource = Positions;
            //cm1.ItemsSource = Positions;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
