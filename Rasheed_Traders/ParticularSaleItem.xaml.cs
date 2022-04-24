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
                            bId = c.buyerId
                        }).SingleOrDefault();
            if (data != null)
            {
                quantity.Text = data.Quanitity.ToString();
                dPercentage.Text = data.Discount.ToString();
                price.Text = Convert.ToInt32((data.SubTotal / data.Quanitity)).ToString();
                int index = 0;
                var med = (from c in db.Medicines
                           where c.isDeleted == false
                           select c);
                foreach (var item in med)
                {
                    mediCombo.Items.Add(item.name.ToUpper());
                    if (item.id == data.medId)
                    {
                        mediCombo.SelectedItem = data.Name;
                        mediCombo.SelectedIndex = index;
                    }
                    index++;
                }
                index = 0;
                var type = (from c in db.Types
                            where c.isDeleted == false
                            select c);
                foreach (var item in type)
                {
                    typeCombo.Items.Add(item.name.ToUpper());
                    if (item.id == data.Tpid)
                    {
                        typeCombo.SelectedItem = item.name.ToUpper();
                        typeCombo.SelectedIndex = index;
                    }
                    index++;
                }
                var bonus = (from c in db.Bonus
                             where c.isDeleted == false
                             select c);
                foreach (var item in bonus)
                {
                    bonusCombo.Items.Add(item.name);
                }
                item1.mediId = data.medId;
                item1.typeId = data.Tpid;
                item1.discount = Convert.ToDouble(data.Discount);
                item1.price = Convert.ToInt32((data.SubTotal / data.Quanitity));
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
                int i1 = mediCombo.SelectedIndex, i2 = typeCombo.SelectedIndex,i3 = 0,discountAm =0,bonus = 0, sId = 0;
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
                        item2.mediId = item.id;
                    index++;
                }
                var type = (from c in db.Types
                            where c.isDeleted == false
                            select c);
                index = 0;
                foreach (var item in type)
                {
                    if (index == i2)
                        item2.typeId = item.id;
                    index++;
                }
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
                    }
                    char[] num = bonusCombo.Text.ToCharArray();
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
                    bonus = (item2.quantity / num3) * num4; // bonus items
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
                    SaleItem item = new SaleItem() { saleId = sId, medicineId = item2.mediId, typeId = item2.typeId, buyerId = item2.buyerId, bonusId = item2.bonusId, total = item2.total, subTotal = item2.subTotal, discount = item2.discount, discountAmount = discountAm, quantity = item2.quantity + bonus, createdAt = DateTime.Now, isDeleted = false };
                    db.SaleItems.Add(item);
                }
                else
                {
                    SaleItem item = new SaleItem() { saleId = sId, medicineId = item2.mediId, typeId = item2.typeId, buyerId = item2.buyerId, total = item2.total, subTotal = item2.subTotal, discount = item2.discount, discountAmount = discountAm, quantity = item2.quantity + bonus, createdAt = DateTime.Now, isDeleted = false };
                    db.SaleItems.Add(item);
                }
                sale.updatedAt = DateTime.Now;
                var saleI = (from c in db.SaleItems
                             where c.isDeleted == false && c.id == itemId
                             select c).SingleOrDefault();
                saleI.isDeleted = true;
                if (sale.items == 1)
                {
                    sale.discount = item2.discount;
                    sale.discountAmount = discountAm;                   
                    sale.subTotal = item2.subTotal;
                    sale.total = item2.total;
                }
                else
                {
                    sale.total -= item1.total;
                    sale.subTotal -= item1.subTotal;
                    sale.subTotal += item2.subTotal;
                    sale.total += item2.total - Convert.ToDouble(item2.total * (sale.discount / 100));
                }
                if (isSale)
                {
                    if (updateStock1(item1.mediId, item1.buyerId, item1.quantity, item1.price) && updateStock(item2.mediId, item2.quantity, item2.price))
                    {
                        db.SaveChanges();
                        this.Close();
                        updateWindow();
                    }
                }
                else
                {
                    if (updateStock(item1.mediId, item1.quantity, item1.price) && updateStock1(item2.mediId,item2.buyerId, item2.quantity, item2.price))
                    {
                        db.SaveChanges();
                        this.Close();
                        updateWindow();
                    }
                }
            }
        }

        private bool updateStock1(int a, int b, int c, int price)     // Add Stock
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
                    isDeleted = false
                };
                db.Stocks.Add(stk);
                var s = (from d in db.Medicines
                         where d.isDeleted == false && d.id == a
                         select d).SingleOrDefault();
                s.priceBuy = price;
            }
            else
                st.quantity += c;
            db.SaveChanges();
            return true;
        }
        private bool updateStock(int a, int c, int price)     // Remove Stock
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
                         select d).SingleOrDefault();
                s.priceSell = price;
                db.SaveChanges();
            }
            return true;
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
