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
                    Partner newWindow = new Partner(true); /* Give Your window Instance */
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
                if (messageBoxResult == MessageBoxResult.Yes)  
                {
                    List<TicketInfo> list = table.Items.OfType<TicketInfo>().ToList();                  
                    Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                    List<string> mediId = new List<string>();
                    int id1 = 0, id2 = 0, id3 = 0, id4 = -1, i = 0;
                    double total1 = 0,subTotal = 0,iTotal = 0,iSubtotal,dAmount = 0;
                    string name = "";

                    // Partner id searching
                    var doc2 = from d in db.TradingParteners
                               where d.isDeleted == false && d.isBuyer == true && d.name == combobox.Text
                               select new
                               {
                                   id = d.id,
                                   nm = d.name
                                  
                               };
                    foreach (var m in doc2)
                    {
                        id3 = m.id;
                        name = m.nm;
                    }

                    Sale s = new Sale();

                   // Searching of medicineId and typeId of each record
                    foreach (var index in list)
                    {
                        var doc = from d in db.Medicines
                                  where d.isDeleted == false && index.typeStatus != null && d.name == index.mediStatus
                                  select new
                                  {
                                      id = d.id
                                  };
                        foreach (var m in doc)
                        {
                            id1 = m.id;
                        }
                        var doc1 = from d in db.Types
                                   where d.isDeleted == false && index.typeStatus != null && d.name == index.typeStatus
                                   select new
                                   {
                                       id = d.id
                                   };
                        foreach (var m in doc1)
                        {
                            id2 = m.id;
                        }
                        if (index.bonusStatus != "0")
                        {
                            var doc3 = from d in db.Bonus
                                       where d.isDeleted == false && d.name == index.bonusStatus
                                       select new
                                       {
                                           id = d.id
                                       };
                            foreach (var m in doc3)
                            {
                                id4 = m.id;
                            }
                        }
                        iSubtotal = index.Price * index.Quantity;
                        iTotal = subTotal - dAmount;
                        dAmount = (index.DiscountPercentage / 100) * subTotal;
                        total1 += iTotal;
                        subTotal += iSubtotal;
                        if (id4 != -1)
                        {
                            SaleItem item = new SaleItem() { saleId = s.id, medicineId = id1, typeId = id2, buyerId = id3, bonusId = id4, total = iTotal, subTotal = iSubtotal, discount = index.DiscountPercentage, discountAmount = dAmount, quantity = index.Quantity, createdAt = DateTime.Now };
                            db.SaleItems.Add(item);
                        }
                        else
                        {
                            SaleItem item = new SaleItem() { saleId = s.id, medicineId = id1, typeId = id2, buyerId = id3,  total = iTotal, subTotal = iSubtotal, discount = index.DiscountPercentage, discountAmount = dAmount, quantity = index.Quantity, createdAt = DateTime.Now };
                            db.SaleItems.Add(item);
                        }
                        i++;
                    }
                    s.subTotal = subTotal;
                    s.total = total1;
                    s.createdAt = DateTime.Now;
                    s.isDeleted = false;
                    s.items = i;
                    //s.name = name;
                    db.Sales.Add(s);
                    db.SaveChanges();
                    this.Close();
                }
            }
        }
        private void loadData()
        {
            // Slect Buyer/Seller
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var doc = from d in db.TradingParteners
                      where d.isBuyer == true
                      select new
                      {
                          name = d.name,
                      };
            foreach (var item in doc)
            {
                combobox.Items.Add(item.name);
            }

            // Select Medicine
            var doc1 = from d in db.Medicines
                       select new
                       {
                           name = d.name,
                      };
            List<string> m = new List<string>();
            List<string> m1 = new List<string>();
            List<string> m2 = new List<string>();
            foreach (var item in doc1)
            {
                m.Add(item.name);
                ticketsList[0].mediCombo.Add(item.name);
            }
            cm1.ItemsSource = m;

            // Select Type
            var doc2 = from d in db.Types
                       select new
                       {
                           name = d.name,
                       };
            foreach (var item in doc2)
            {
                m1.Add(item.name);
            }
            cm.ItemsSource = m1;

            // Select Type
            var doc3 = from d in db.Bonus
                       select new
                       {
                           name = d.name,
                       };
            foreach (var item in doc3)
            {
                m2.Add(item.name);
            }
            cm2.ItemsSource = m2;
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
        private string bonuStatus = "0";
        public string bonusStatus
        {
            get { return bonuStatus; }
            set { bonuStatus = value; }
        }

        public ObservableCollection<string> mediCombo { get; set; }
        public ObservableCollection<string> typeCombo { get; set; }
        public ObservableCollection<string> bonusCombo { get; set; }

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
