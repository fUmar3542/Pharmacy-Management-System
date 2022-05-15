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
            new TicketInfo{ mediStatus="True",mediCombo = new ObservableCollection<string>() { "Forward", "Defense", "Goalie" }, typeCombo=new ObservableCollection<string>() { "Forward", "Defense", "Goalie" },Quantity=1}
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
                List<TicketInfo> list = table.Items.OfType<TicketInfo>().ToList();
                foreach (var item1 in list)
                {
                    if (item1.mediStatus == "True" || item1.Quantity < 0 || item1.DiscountPercentage < 0)
                    {
                        MessageBox.Show("Not a valid input. Please enter valid input");
                        return;
                    }
                }
                TicketInfo t = new TicketInfo { mediStatus = "True", mediCombo = new ObservableCollection<string>() { "Forward", "Defense", "Goalie" },  typeCombo = new ObservableCollection<string>() { "Forward", "Defense", "Goalie" }, Quantity = 1 };
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

        private bool checkStock(int a,int c)
        {
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
                var med = (from d in db.Medicines
                           where d.isDeleted == false && d.id == a
                           select d).FirstOrDefault();
                MessageBox.Show(med.name + " stock not available");
                return false;
            }
            return true;
        }
        private void saleDone()
        {
            double number1;
            List<TicketInfo> list = table.Items.OfType<TicketInfo>().ToList();
            if (list.Count() == 0)
            {
                this.Close();
                return;
            }
            foreach (var item1 in list)
            {
                if (item1.mediStatus == "True" || item1.Quantity < 0 || item1.DiscountPercentage < 0)
                {
                    MessageBox.Show("Not a valid input. Please enter valid input");
                    return;
                }
            }
            if (combobox.Text == "")
            {
                MessageBox.Show("Select Partner first");
                return;
            }
            if (percentage.Text != "")
            {
                if (Double.TryParse(percentage.Text, out number1))
                {
                    if (number1 < 0)
                    {
                        MessageBox.Show("Enter the valid Discount Percentage");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Discount Percentage is outside the range of a Double.");
                    return;
                }
            }
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Sale Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                List<string> mediId = new List<string>();
                int id1 = 0, id2 = 0, id3 = 0, id4 = -1, i = 0;
                double total1 = 0, iTotal = 0, iSubtotal = 0, dAmount = 0;
                int bonus1 = 0;
                foreach (var item in list)
                {
                    bonus1 = 0;
                    int index3 = item.mediStatus.IndexOf('-');
                    string tp = item.mediStatus.Substring(0, index3 - 1),
                        md = item.mediStatus.Substring(index3 + 2);
                    id2 = returnId("Type", tp);
                    if (id2 == -1)
                    {
                        MessageBox.Show("Select items first");
                        return;
                    }
                    id1 = returnMedicineId(md, id2);
                    if (item.bonusStatus != "0")
                    {
                        id4 = returnId("Bonus", item.bonusStatus);
                        char[] num = item.bonusStatus.ToCharArray();
                        int len = num.Length;
                        int num2 = 0, num3 = 0, num4 = 0;
                        while (num[num2] != '+')
                        {
                            num3 *= 10;
                            num3 += (num[num2] - '0');
                            num2++;
                        }
                        num2++;
                        while (num2 != len)
                        {
                            num4 *= 10;
                            num4 += (num[num2] - '0');
                            num2++;
                        }
                        if (num3 != 0)
                            bonus1 = (item.Quantity / num3) * num4; // bonus items
                    }
                    if (checkStock(id1, bonus1 + item.Quantity) == false)
                        return;
                }
                int pIndex = combobox.Text.IndexOf('-');
                string name = "", pName = combobox.Text.Substring(0, (pIndex - 1)), pLocation = combobox.Text.Substring(pIndex + 2);
                // Partner id searching
                var doc2 = from d in db.TradingParteners
                           where d.isDeleted == false && d.isBuyer == true && d.name == pName && d.location == pLocation
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
                double dPercentage = 0;
                foreach(var item in list)
                {
                    bonus1 = 0;
                    int index3 = item.mediStatus.IndexOf('-');
                    string tp = item.mediStatus.Substring(0, index3 - 1),
                        md = item.mediStatus.Substring(index3 + 2);
                    id2 = returnId("Type", tp);
                    if (id2 == -1)
                    {
                        MessageBox.Show("Select items first");
                        return;
                    }
                    id1 = returnMedicineId(md, id2);
                    if (item.bonusStatus != "0")
                    {
                        id4 = returnId("Bonus", item.bonusStatus);
                        char[] num = item.bonusStatus.ToCharArray();
                        int len = num.Length;
                        int num2 = 0, num3 = 0, num4 = 0;
                        while (num[num2] != '+')
                        {
                            num3 *= 10;
                            num3 += (num[num2] - '0');
                            num2++;
                        }
                        num2++;
                        while (num2 != len)
                        {
                            num4 *= 10;
                            num4 += (num[num2] - '0');
                            num2++;
                        }
                        if (num3 != 0)
                            bonus1 = (item.Quantity / num3) * num4; // bonus items
                    }
                    if (checkStock(id1,bonus1 + item.Quantity) == false)
                        return;
                }

                // Searching of medicineId and typeId of each record
                foreach (var index in list)
                {
                    bonus1 = 0;
                    int index3 = index.mediStatus.IndexOf('-');
                    string tp = index.mediStatus.Substring(0, index3 - 1), 
                        md = index.mediStatus.Substring(index3+2);
                    id2 = returnId("Type",tp);
                    id1 = returnMedicineId( md, id2);
                    if (index.bonusStatus != "0")
                    {
                        id4 = returnId("Bonus", index.bonusStatus);
                        char[] num = index.bonusStatus.ToCharArray();
                        int len = num.Length;
                        int num2 = 0, num3 = 0, num4 = 0;
                        while (num[num2] != '+')
                        {
                            num3 *= 10;
                            num3 += (num[num2] - '0');
                            num2++;
                        }
                        num2++;
                        while (num2 != len)
                        {
                            num4 *= 10;
                            num4 += (num[num2] - '0');
                            num2++;
                        }
                        if (num3 != 0)
                            bonus1 = (index.Quantity / num3) * num4; // bonus items
                    }
                    else
                        id4 = -1;
                    if (updateStock(id1,  bonus1 + index.Quantity,index.Price) == false)
                        return;
                    iSubtotal = index.Price * (index.Quantity);
                    dAmount = (int)((index.DiscountPercentage / 100) * iSubtotal);
                    iTotal = iSubtotal - dAmount;
                    total1 += iTotal;
                    if (bonus1 != 0)   // if bonusId selected
                    {
                        SaleItem item = new SaleItem() {Price = index.Price, saleId = s.id, medicineId = id1, typeId = id2, buyerId = id3, bonusId = id4, total = iTotal, subTotal = iSubtotal, discount = index.DiscountPercentage, discountAmount = dAmount, quantity = index.Quantity + bonus1,bonus = bonus1, createdAt = DateTime.Now, isDeleted = false };
                        db.SaleItems.Add(item);
                    }
                    else
                    {
                        SaleItem item = new SaleItem() {Price = index.Price, saleId = s.id, medicineId = id1, typeId = id2, buyerId = id3, total = iTotal, subTotal = iSubtotal, discount = index.DiscountPercentage, discountAmount = dAmount, quantity = index.Quantity, createdAt = DateTime.Now,bonus = bonus1, isDeleted = false };
                        db.SaleItems.Add(item);
                    }
                    dPercentage = index.DiscountPercentage;
                    i++;
                }
                s.subTotal = total1;
                if (percentage.Text != "")
                {
                    double number = Convert.ToDouble(percentage.Text);
                    s.discount = number;
                    s.discountAmount = (int)(total1 * (number / 100));
                    total1 = total1 - Convert.ToDouble(s.discountAmount);
                }
                else
                {
                    s.discount = 0;
                    s.discountAmount = 0;
                }
                s.total = total1;
                s.createdAt = DateTime.Now;
                s.isDeleted = false;
                s.items = i;  
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
            string t = "Ledger";
            var ex = Application.Current.Windows.
                  Cast<Window>().SingleOrDefault(x => x.Title.Equals(t));
            if (ex != null)
            {
                ex.Close();
                Ledger newWindow1 = new Ledger(); /* Give Your window Instance */
                newWindow1.Title = t;
                newWindow1.Show();
            }
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
           st.updatedAt = DateTime.Now;
           st.quantity -= c;
           var s = (from d in db.Medicines
                    where d.isDeleted == false && d.id == a
                    select d).FirstOrDefault();
           s.priceSell = price;
           db.SaveChanges();
            return true;
        }

        public int returnMedicineId(string s,int i)
        {
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            if (s == "")
                return -1;
           var doc = (from d in db.Medicines
                    where d.isDeleted == false && d.name == s && d.typeId == i
                    select new
                    {
                        id = d.id
                    }).FirstOrDefault();
            if (doc != null)
                return doc.id;
            else
                return -1;
        }
        public int returnId(string a,string b)
        {
            if (b == "")
                return -1;
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            if (a == "Bonus")
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
                      where d.isDeleted == false
                      where d.isBuyer == true
                      select new
                      {
                          name = d.name,
                          loc = d.location
                      };
            foreach (var item in doc)
            {
                combobox.Items.Add(item.name + " - " + item.loc);
            }

            // Select Medicine
            var doc1 = from d in db.Medicines
                       where d.isDeleted == false
                       select new
                       {
                           name = d.name,
                           typeId = d.typeId
                      };
            List<string> m = new List<string>();
            List<string> m1 = new List<string>();
            List<string> m2 = new List<string>();
            foreach (var item in doc1)
            {
                var tp = (from d in db.Types
                          where d.isDeleted == false && d.id == item.typeId
                         select new
                         {
                            name = d.name,
                         }).SingleOrDefault();
                m.Add(tp.name + " - " + item.name);
            }
            cm1.ItemsSource = m;

            // Select Type
            var doc3 = from d in db.Bonus
                       where d.isDeleted == false
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
        private string mStatus = "0";
        public string mediStatus
        {
            get { return mStatus; }
            set { mStatus = value; }
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
