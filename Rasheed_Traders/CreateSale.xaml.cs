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
                saleDone();
            }
        }

        private void saleDone()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Sale Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                List<TicketInfo> list = table.Items.OfType<TicketInfo>().ToList();
                if (list.Count() == 0)
                    return;
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                List<string> mediId = new List<string>();
                int id1 = 0, id2 = 0, id3 = 0, id4 = -1, i = 0;
                double total1 = 0, subTotal = 0, iTotal = 0, iSubtotal = 0, dAmount = 0;
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
                int bonus = 0;
                double dPercentage = 0;

                // Searching of medicineId and typeId of each record
                foreach (var index in list)
                {
                    bonus = 0;
                    id1 = returnId("Medicine", index.mediStatus);
                    if (id1 == -1)
                    {
                        MessageBox.Show("Select items first");
                        return;
                    }
                    id2 = returnId("Type", index.typeStatus);
                    if (id2 == -1)
                    {
                        MessageBox.Show("Select items first");
                        return;
                    }
                    if (index.bonusStatus != "0")
                    {
                        id4 = returnId("Bonus", index.bonusStatus);
                        char[] num = index.bonusStatus.ToCharArray();
                        int len = num.Length;
                        int num2 = 0, num3 = 0, num4 = 0;
                        while (num[num2] != '+')
                        {
                            num3 += (num[num2] - '0');
                            num2++;
                        }
                        num2++;
                        while (num2 != len)
                        {
                            num4 += (num[num2] - '0');
                            num2++;
                        }
                        bonus = (index.Quantity / num3) * num4; // bonus items
                    }
                    if (updateStock(id1,  bonus + index.Quantity,index.Price) == false)
                        return;
                    iSubtotal = index.Price * (index.Quantity);
                    dAmount = (index.DiscountPercentage / 100) * iSubtotal;
                    iTotal = iSubtotal - dAmount;
                    total1 += iTotal;
                    subTotal += iSubtotal;
                    if (bonus != 0)   // if bonusId selected
                    {
                        SaleItem item = new SaleItem() { saleId = s.id, medicineId = id1, typeId = id2, buyerId = id3, bonusId = id4, total = iTotal, subTotal = iSubtotal, discount = index.DiscountPercentage, discountAmount = dAmount, quantity = index.Quantity + bonus, createdAt = DateTime.Now, isDeleted = false };
                        db.SaleItems.Add(item);
                    }
                    else
                    {
                        SaleItem item = new SaleItem() { saleId = s.id, medicineId = id1, typeId = id2, buyerId = id3, total = iTotal, subTotal = iSubtotal, discount = index.DiscountPercentage, discountAmount = dAmount, quantity = index.Quantity, createdAt = DateTime.Now, isDeleted = false };
                        db.SaleItems.Add(item);
                    }
                    dPercentage = index.DiscountPercentage;
                    i++;
                }
                if (percentage.Text != "")
                {
                    double number = Convert.ToDouble(percentage.Text);
                    s.discount = number;
                    s.discountAmount = total1 * (number / 100);
                    total1 = total1 - Convert.ToDouble(s.discountAmount);
                }
                s.subTotal = subTotal;
                s.total = total1;
                s.createdAt = DateTime.Now;
                s.isDeleted = false;
                s.items = i;
                if (i == 1)       // if it is only 1 item
                {
                    s.discountAmount = dAmount;
                    s.discount = dPercentage;
                }
                s.Name = name;
                s.isPurchase = false;
                db.Sales.Add(s);
                db.SaveChanges();
                this.Close();
                updateWindow();
            }
        }
        private void updateWindow()
        {
            string title1 = "HomeWindow";
            var e = Application.Current.Windows.
            Cast<Window>().SingleOrDefault(x => x.Title.Equals(title1));
            if (e != null)
            {
                e.Close();
                HomeWindow newWindow = new HomeWindow(); /* Give Your window Instance */
                newWindow.Title = title1;
                newWindow.Show();
            }
            string title = "AddOrViewSales";  /*Your Window Instance Name*/
            var existingWindow = Application.Current.Windows.
            Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
            if (existingWindow != null)
            {
                existingWindow.Close();
                AddOrViewSales newWindow1 = new AddOrViewSales(); /* Give Your window Instance */
                newWindow1.Title = title;
                newWindow1.Show();
            }
        }
        private bool updateStock(int a,  int c,int price)
        {
            // Updating stock
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var st = (from d in db.Stocks
                     where d.isDeleted == false && d.medicineId == a
                     select d).FirstOrDefault();
            if (st == null)
            {
                MessageBox.Show("Stock item not exist");
                return false;
            }
            if (st.quantity < c)
            {
                MessageBox.Show("Stock not available");
                return false;
            }
            else
            {
                st.quantity -= c;
                var s = (from d in db.Medicines
                          where d.isDeleted == false && d.id == a
                          select d).FirstOrDefault();
                s.priceSell = price;
                db.SaveChanges();
            }
            return true;
        }

        public int returnId(string a,string b)
        {
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            if (b == "")
                return -1;
            if(a == "Medicine")
            {
                var doc = (from d in db.Medicines
                          where d.isDeleted == false && d.name == b
                          select new
                          {
                              id = d.id
                          });
                foreach(var item in doc)
                     return item.id;
            }
            else if(a == "Bonus")
            {
                var doc = (from d in db.Bonus
                          where d.isDeleted == false  && d.name == b
                          select new
                          {
                              id = d.id
                          });
                foreach (var item in doc)
                    return item.id;
            }
            else if(a == "Type")
            {
                var doc = (from d in db.Types
                          where d.isDeleted == false && d.name == b
                          select new
                          {
                              id = d.id
                          });
                foreach (var item in doc)
                    return item.id;
            }
            return -1;
        }
        private void loadData()
        {
            // Select Buyer/Seller
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
                //ticketsList[0].mediCombo.Add(item.name);
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
