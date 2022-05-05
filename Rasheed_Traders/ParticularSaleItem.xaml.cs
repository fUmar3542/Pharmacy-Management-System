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
    /// Interaction logic for ParticularSaleItem.xaml
    /// </summary>
    public partial class ParticularSaleItem : Window
    {
        int saleId = 0,itemId =0;
        itemInfo item1 = new itemInfo();
        bool isSale = false;
        public ParticularSaleItem(int a,int b,bool t)
        {
            InitializeComponent();
            saleId = a;
            itemId = b;
            isSale = t;
            loadData();
        }
        private void loadData()
        {
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var data = (from c in db.SaleItems
                        from f in db.Medicines.Where(y => y.id == c.medicineId).DefaultIfEmpty()
                        where c.isDeleted == false && itemId == c.id
                        select new
                        {
                            Name = f.name.ToUpper(),
                            Quanitity = c.quantity,
                            Discount = c.discount,
                            SubTotal = c.subTotal,
                            Total = c.total,
                            Tpid = c.typeId,
                            medId = c.medicineId,
                            bId = c.buyerId,
                            bonusId = c.bonusId,
                            price = c.Price,  
                            bonus = c.bonus
                        }).SingleOrDefault();
            string bonusName = "";
            if (data != null)
            {
                if (data.bonusId == null)
                {
                    quantity.Text = data.Quanitity.ToString();
                }
                else
                {
                    var b = (from c in db.Bonus
                               where c.isDeleted == false && c.id == data.bonusId
                               select c).SingleOrDefault();
                    int bonu = (int)(data.bonus);
                    bonusName = b.name;
                    quantity.Text = (data.Quanitity - bonu).ToString();
                }
                price.Text = data.price.ToString();
                dPercentage.Text = data.Discount.ToString();
                int index = 0;
                var med = (from c in db.Medicines
                           where c.isDeleted == false
                           select c);
                foreach (var item in med)
                {
                    var tp = (from d in db.Types
                              where d.id == item.typeId && d.isDeleted == false
                              select new
                              {
                                  name = d.name,
                              }).SingleOrDefault();
                    mediCombo.Items.Add(tp.name.ToUpper() + " - " + item.name.ToUpper());
                    if (item.id == data.medId)
                    {
                        mediCombo.SelectedItem = tp.name.ToUpper() + " - " +  data.Name;
                        mediCombo.SelectedIndex = index;
                    }
                    index++;
                }
                index = 0;
                var bonus = (from c in db.Bonus
                             where c.isDeleted == false
                             select c);
                foreach (var item in bonus)
                {
                    bonusCombo.Items.Add(item.name);
                }
                if(bonusName != "")
                    bonusCombo.SelectedItem = bonusName;
                item1.mediId = data.medId;
                item1.typeId = data.Tpid;
                item1.discount = Convert.ToDouble(data.Discount);
                item1.price = Convert.ToInt32(price.Text);
                item1.quantity = data.Quanitity;
                item1.buyerId = data.bId;
                item1.total = data.Total;
                item1.subTotal = Convert.ToInt32(data.SubTotal);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(cancel))
                this.Close();
            else if(sender.Equals(update))
            {
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                int i1 = mediCombo.SelectedIndex, i3 = 0,discountAm =0,bonus1 = 0, sId = 0;
                if (!validateInput())
                    return;
                itemInfo item2 = new itemInfo();
                item2.quantity = Convert.ToInt32(Double.Parse(quantity.Text));
                item2.price = Convert.ToInt32(Double.Parse(price.Text));
                item2.discount = Double.Parse(dPercentage.Text);
                item2.buyerId = item1.buyerId;          
                int index = 0;
                var med = (from c in db.Medicines
                           where c.isDeleted == false
                           select c);
                foreach (var item in med)
                {
                    if (index == i1)
                    {
                        item2.mediId = item.id;
                        item2.typeId = item.typeId;
                    }
                    index++;
                }
                index = 0;
                if (bonusCombo.SelectedItem != null)
                {
                    i3 = bonusCombo.SelectedIndex;
                    var bonu = (from c in db.Bonus
                                 where c.isDeleted == false
                                 select c);
                    index = 0;
                    foreach (var item in bonu)
                    {
                        if (index == i3)
                            item2.bonusId = item.id;
                        index++;
                    }
                    char[] num = bonusCombo.Text.ToCharArray();
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
                    if(num3 != 0)
                        bonus1 = (item2.quantity / num3) * num4; // bonus items
                }
                var sale = (from c in db.Sales
                            where c.isDeleted == false && c.id == saleId
                            select c).SingleOrDefault();
                item2.subTotal = item2.price * (item2.quantity);
                discountAm = Convert.ToInt32((item2.discount / 100) * item2.subTotal);
                item2.total = item2.subTotal - discountAm;
                sId = saleId;
                if (bonusCombo.SelectedItem != null)
                {
                    SaleItem item = new SaleItem() {bonus = bonus1, Price = item2.price, saleId = sId, medicineId = item2.mediId, typeId = item2.typeId, buyerId = item2.buyerId, bonusId = item2.bonusId, total = item2.total, subTotal = item2.subTotal, discount = item2.discount, discountAmount = discountAm, quantity = (item2.quantity + bonus1), createdAt = DateTime.Now, isDeleted = false };
                    db.SaleItems.Add(item);
                }
                else
                {
                    SaleItem item = new SaleItem() {bonus = bonus1, Price = item2.price, saleId = sId, medicineId = item2.mediId, typeId = item2.typeId, buyerId = item2.buyerId, total = item2.total, subTotal = item2.subTotal, discount = item2.discount, discountAmount = discountAm, quantity = item2.quantity, createdAt = DateTime.Now, isDeleted = false };
                    db.SaleItems.Add(item);
                }
                sale.updatedAt = DateTime.Now;
                var saleI = (from c in db.SaleItems       // For deleting old item
                             where c.isDeleted == false && c.id == itemId
                             select c).SingleOrDefault();
                saleI.isDeleted = true;
                if (sale.items == 1)
                {
                    sale.subTotal = item2.subTotal;
                }
                else
                {
                    sale.subTotal -= item1.total;
                    sale.subTotal += item2.total;
                }
                if (sale.discount != null)
                {
                    sale.discountAmount = (int)(sale.subTotal * (Convert.ToDouble(sale.discount) / 100));
                    sale.total = sale.subTotal - (double)(sale.discountAmount);
                }
                else
                    sale.total = sale.subTotal;
                if (isSale)
                {
                    if (addStock(item1.mediId, item1.buyerId, item1.quantity, item1.price) && removeStock(item2.mediId, item2.quantity + bonus1, item2.price))
                    {
                        db.SaveChanges();
                        this.Close();
                        updateWindow();
                    }
                }
                else
                {
                    if (addStock(item2.mediId,item2.buyerId, item2.quantity + bonus1, item2.price) && removeStock(item1.mediId, item1.quantity, item1.price))
                    {
                        db.SaveChanges();
                        this.Close();
                        updateWindow1();
                    }
                }
            }
        }

        private bool validateInput()
        {
            double number1 = 0;
            int number2 = 0;
            if (price.Text == "" || quantity.Text == "")
            {
                MessageBox.Show("Enter fields first");
                return false;
            }
            if (Double.TryParse(dPercentage.Text, out number1))
            {
                if (number1 < 0)
                {
                    MessageBox.Show("Enter the valid Discount Percentage");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Enter the valid Discount Percentage");
                return false;
            }
            if (int.TryParse(quantity.Text, out number2))
            {
                if (number2 < 0)
                {
                    MessageBox.Show("Enter valid Quantity");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Enter valid Quantity");
                return false;
            }
            if (int.TryParse(price.Text, out number2))
            {
                if (number2 < 0)
                {
                    MessageBox.Show("Enter valid Price");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Enter valid Price");
                return false;
            }
            return true;
        }

        private bool addStock(int a, int b, int c, int price)     // Add Stock
        {
            // Updating stock
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var st = (from d in db.Stocks
                      where d.isDeleted == false && d.medicineId == a
                      select d).SingleOrDefault();
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
                         select d).SingleOrDefault();
                s.priceBuy = price;
            }
            else
            {
                st.updatedAt = DateTime.Now;
                st.quantity += c;
            }
            db.SaveChanges();
            return true;
        }
        private bool removeStock(int a, int c, int price)     // Remove Stock
        {
            // Updating stock
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var st = (from d in db.Stocks
                      where d.isDeleted == false && d.medicineId == a
                      select d).SingleOrDefault();
            if (st == null)
            {
                MessageBox.Show("Stock item not exist");
                return false;
            }
            if (isSale == true)
            {
                if (st.quantity < c)
                {
                    var med = (from d in db.Medicines
                               where d.isDeleted == false && d.id == a
                               select d).FirstOrDefault();
                    MessageBox.Show(med.name + " stock not available");
                    return false;
                }
            }
            st.quantity -= c;
            if (st.quantity < 0)
                st.quantity = 0;
            var s = (from d in db.Medicines
                        where d.isDeleted == false && d.id == a
                        select d).SingleOrDefault();
            s.priceSell = price;
            st.updatedAt = DateTime.Now;
            db.SaveChanges();
            return true;
        }

        private void updateWindow1()      // For purchase
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
            string title = "AddOrViewPurchase";  /*Your Window Instance Name*/
            var existingWindow = Application.Current.Windows.
            Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
            if (existingWindow != null)
            {
                existingWindow.Close();
                AddOrViewPurchase newWindow1 = new AddOrViewPurchase(); /* Give Your window Instance */
                newWindow1.Title = title;
                newWindow1.Show();
            }
            string title2 = "purchaseItems";  /*Your Window Instance Name*/
            var eWindow = Application.Current.Windows.
            Cast<Window>().SingleOrDefault(x => x.Title.Equals(title2));
            if (eWindow != null)
            {
                eWindow.Close();
                purchaseItems newWindow1 = new purchaseItems(saleId); /* Give Your window Instance */
                newWindow1.Title = title2;
                newWindow1.Show();
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
            string title2 = "SaleItems";  /*Your Window Instance Name*/
            var eWindow = Application.Current.Windows.
            Cast<Window>().SingleOrDefault(x => x.Title.Equals(title2));
            if (eWindow != null)
            {
                eWindow.Close();
                SaleItems newWindow1 = new SaleItems(saleId); /* Give Your window Instance */
                newWindow1.Title = title2;
                newWindow1.Show();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public class itemInfo
    {
        public int mediId;
        public int typeId;
        public int bonusId;
        public int buyerId;
        public int quantity = 1;
        public int price = 0;
        public double discount = 0;
        public double total = 0;
        public int subTotal = 0;
    }
}
