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

        List<TicketInfo> ticketsList = new List<TicketInfo>
        {
            new TicketInfo{ mediStatus="True",mediCombo = new ObservableCollection<string>() { "Forward", "Defense", "Goalie" }, typeCombo=new ObservableCollection<string>() { "Forward", "Defense", "Goalie" },Quantity=1}
        };
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
                TicketInfo t = new TicketInfo { mediStatus = "True", mediCombo = new ObservableCollection<string>() { "Forward", "Defense", "Goalie" }, typeCombo = new ObservableCollection<string>() { "Forward", "Defense", "Goalie" }, Quantity = 1 };
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
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Purchase Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                List<TicketInfo> list = table.Items.OfType<TicketInfo>().ToList();
                if (list.Count() == 0)
                    return;
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                List<string> mediId = new List<string>();
                int id1 = 0, id2 = 0, id3 = 0, id4 = -1, i = 0;
                double total1 = 0, subTotal = 0, iTotal = 0, iSubtotal, dAmount = 0;
                string name = "";
                // Partner id searching
                var doc2 = (from d in db.TradingParteners
                            where d.isDeleted == false && d.isSeller == true && d.name == combobox.Text
                            select new
                            {
                                id = d.id,
                                nm = d.name
                            }).FirstOrDefault();
                id3 = doc2.id;
                name = doc2.nm;

                Sale s = new Sale();
                int bonus = 0;
                double dPercentage = 0;

                // Searching of medicineId and typeId of each record
                foreach (var index in list)
                {
                    bonus = 0;
                    int index3 = index.mediStatus.IndexOf('-');
                    string tp = index.mediStatus.Substring(0, index3 - 1),
                              md = index.mediStatus.Substring(index3 + 2);
                    id2 = returnId("Type", tp);
                    if (id2 == -1)
                    {
                        MessageBox.Show("Select items first");
                        return;
                    }
                    id1 = returnMedicineId(md, id2);
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
                    updateStock(id1, id3, bonus + index.Quantity,index.Price);
                    iSubtotal = index.Price * (index.Quantity);
                    dAmount = (index.DiscountPercentage / 100) * iSubtotal;
                    iTotal = iSubtotal - dAmount;
                    total1 += iTotal;
                    subTotal += iSubtotal;
                    if (bonus != 0)   // if bonusId selected
                    {
                        SaleItem item = new SaleItem() { saleId = s.id, medicineId = id1, typeId = id2, buyerId = id3, bonusId = id4, total = iTotal, subTotal = iSubtotal, discount = index.DiscountPercentage, discountAmount = dAmount, quantity = index.Quantity + bonus, createdAt = DateTime.Now };
                        db.SaleItems.Add(item);
                    }
                    else
                    {
                        SaleItem item = new SaleItem() { saleId = s.id, medicineId = id1, typeId = id2, buyerId = id3, total = iTotal, subTotal = iSubtotal, discount = index.DiscountPercentage, discountAmount = dAmount, quantity = index.Quantity, createdAt = DateTime.Now };
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
                if(i == 1)
                {
                    s.discountAmount = dAmount;
                    s.discount = dPercentage;
                }
                s.Name = name;
                s.isPurchase = true;
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
            string title = "AddOrViewPurchase";  /*Your Window Instance Name*/
            var existingWindow = Application.Current.Windows.
            Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
            if (existingWindow != null)
            {
                existingWindow.Close();
                AddOrViewPurchase newWindow = new AddOrViewPurchase(); /* Give Your window Instance */
                newWindow.Title = title;
                newWindow.Show();
            }
        }
        private void updateStock(int a, int b, int c,int price)
        {
            // Updating stock
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var st = (from d in db.Stocks
                     where d.isDeleted == false && d.medicineId == a 
                     select d).FirstOrDefault();
            if (st == null)
            {
                Stock stk = new Stock()
                {
                    medicineId = a,
                    tpId = b,
                    quantity = c,
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now,
                    isDeleted = false
                };
                db.Stocks.Add(stk);
                var s = (from d in db.Medicines
                         where d.isDeleted == false && d.id == a
                         select d).FirstOrDefault();
                s.priceBuy = price;
            }
            else
            {
                st.quantity += c;
                st.updatedAt = DateTime.Now;
            }
            db.SaveChanges();
        }

        public int returnMedicineId(string s, int i)
        {
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            if (s == "")
                return -1;
            var doc = (from d in db.Medicines
                       where d.isDeleted == false && d.name == s && d.typeId == i
                       select new
                       {
                           id = d.id
                       }).SingleOrDefault();
            if (doc != null)
                return doc.id;
            else
                return -1;
        }
        public int returnId(string a, string b)
        {
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            if (b == "")
                return -1;
           if (a == "Bonus")
            {
                var doc = (from d in db.Bonus
                           where d.isDeleted == false && d.name == b
                           select new
                           {
                               id = d.id
                           }).FirstOrDefault();
                return doc.id;
            }
            else if (a == "Type")
            {
                var doc = (from d in db.Types
                          where d.isDeleted == false && d.name == b
                          select new
                          {
                              id = d.id
                          }).FirstOrDefault();
                return doc.id;
            }
            return -1;
        }
        private void loadData()
        {
            // Select Seller
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var doc = from d in db.TradingParteners
                      where d.isSeller == true
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
                           typeId = d.typeId
                       };
            List<string> m = new List<string>();
            List<string> m1 = new List<string>();
            List<string> m2 = new List<string>();
            foreach (var item in doc1)
            {
                var tp = (from d in db.Types
                          where d.id == item.typeId
                          select new
                          {
                              name = d.name,
                          }).SingleOrDefault();
                m.Add(tp.name + " - " + item.name);
            }
            cm1.ItemsSource = m;
            
            // Select Bonus
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
}