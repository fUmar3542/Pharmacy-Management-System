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
    /// Interaction logic for CreateSale.xaml
    /// </summary>
    public partial class CreateSale : Window
    {
        public ObservableCollection<string> Positions { get; set; }

        List<TicketInfo> ticketsList = new List<TicketInfo>
        {
            new TicketInfo{ mediStatus="True",mediCombo = new ObservableCollection<string>() { "Forward", "Defense", "Goalie" },typeStatus="True", typeCombo=new ObservableCollection<string>() { "Forward", "Defense", "Goalie" },Quantity=1}
        };
        public CreateSale()
        {
            InitializeComponent();
            loadData();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(createBuyer))
            {
                string title = "Partner";  /*Your Window Instance Name*/
                var existingWindow = Application.Current.Windows.
                Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                if (existingWindow == null)
                {
                    Partner newWindow = new Partner(); /* Give Your window Instance */
                    newWindow.Title = title;
                    newWindow.Show();
                }
            }
            else if (sender.Equals(addRow))
            {
                TicketInfo t = new TicketInfo { mediStatus = "True", mediCombo = new ObservableCollection<string>() { "Forward", "Defense", "Goalie" }, typeStatus = "True", typeCombo = new ObservableCollection<string>() { "Forward", "Defense", "Goalie" }, Quantity = 1 };
                List<TicketInfo> list = table.Items.OfType<TicketInfo>().ToList();
                list.Add(t);
                table.ItemsSource = null;
                table.ItemsSource = list;
            }
            else if (sender.Equals(removeRow))
            {
                var selectedItem = table.SelectedItem;
                if (selectedItem != null)
                {
                    List<TicketInfo> list = table.Items.OfType<TicketInfo>().ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] == selectedItem)
                            list.Remove(list[i]);
                    }
                    table.ItemsSource = null;
                    table.ItemsSource = list;
                }
            }
            else if (sender.Equals(done))
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Sale Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)  // error is here
                {

                }
            }
        }
        private void loadData()
        {
            Positions = new ObservableCollection<string>() { "Forward", "Defense", "Goalie" };
            combobox.ItemsSource = Positions;
            table.Visibility = Visibility.Visible;
            table.RowHeight = 28;
            table.ItemsSource = ticketsList;
            cm.ItemsSource = Positions;
            cm1.ItemsSource = Positions;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
    public class TicketInfo
    {
        private string mStatus;
        public string mediStatus
        {
            get { return mStatus; }
            set { mStatus = value; }
        }
        private string tStatus;
        public string typeStatus
        {
            get { return tStatus; }
            set { tStatus = value; }
        }

        public ObservableCollection<string> mediCombo { get; set; }
        public ObservableCollection<string> typeCombo { get; set; }

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
        private double discountPercentage;
        public double DiscountPercentage
        {
            get { return discountPercentage; }
            set { discountPercentage = value; }
        }
    }
}
