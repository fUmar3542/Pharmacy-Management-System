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
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using CrystalDecisions.CrystalReports.Engine;

namespace Rasheed_Traders
{
    /// <summary>
    /// Interaction logic for SaleItems.xaml
    /// </summary>
    public partial class SaleItems : Window
    {
        int saleId = 0;

        List<SaleInfo> ticketsList = new List<SaleInfo>();
        public SaleItems(int a)
        {
            saleId = a;
            InitializeComponent();
            loadData();
        }

        private void loadData()
        {
            ticketsList.Clear();
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var data = (from c in db.SaleItems
                        from f in db.Medicines.Where(y => y.id == c.medicineId).DefaultIfEmpty()
                        where c.isDeleted == false && c.saleId == saleId
                        select new
                        {
                            Name = f.name.ToUpper(),
                            Potency = f.potency.ToUpper(),
                            Quanitity = c.quantity,
                            Discount = c.discountAmount,
                            SubTotal = c.subTotal,
                            Total = c.total,
                            typeId =f.typeId
                        });;
            foreach (var item in data)
            {
                var tp = (from d in db.Types
                          where d.id == item.typeId && d.isDeleted == false
                          select new
                          {
                              name = d.name,
                          }).SingleOrDefault();
                ticketsList.Add(new SaleInfo() { Name = tp.name + " - " +  item.Name, Potency = item.Potency, Quantity = item.Quanitity, Discount = Convert.ToDouble(item.Discount), SubTotal = item.SubTotal, Total = Convert.ToInt32(item.Total) });
            }
            table.ItemsSource = ticketsList;
        }

        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = ItemsControl.ContainerFromElement((DataGrid)sender,
                                                e.OriginalSource as DependencyObject) as DataGridRow;
            if (row == null)
                return;
            else
            {
                int a = table.SelectedIndex, index = 0;
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                var doc2 = from d in db.SaleItems
                           where d.isDeleted == false && d.saleId == saleId 
                           select d;
                foreach (var item in doc2)
                {
                    if (a == index)
                    {
                        string title = "ParticularSaleItem";  /*Your Window Instance Name*/
                        var existingWindow = Application.Current.Windows.
                        Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                        if (existingWindow == null)
                        {
                            ParticularSaleItem newWindow = new ParticularSaleItem(saleId,item.id,true); /* Give Your window Instance */
                            newWindow.Title = title;
                            newWindow.Show();
                        }
                    }
                    index++;
                }
                loadData();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(removeRow))
            {
                var selectedItem = table.SelectedItem;
                int a = table.SelectedIndex,
                    index = 0,
                    medId = 0,
                    medQuantity = 0;
                if (selectedItem != null)
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                        var data = (from p in db.SaleItems
                                    from f in db.Medicines.Where(y => y.id == p.medicineId).DefaultIfEmpty()
                                    where p.isDeleted == false && p.saleId == saleId
                                    select p);
                        var data1 = (from c in db.Sales
                                     where c.isDeleted == false && c.id == saleId
                                     select c).SingleOrDefault();
                        foreach (var item in data)
                        {
                            if (a == index)
                            {
                                item.isDeleted = true;     
                                data1.items--;
                                data1.subTotal -= item.subTotal;
                                data1.discountAmount = (int)(data1.subTotal * data1.discount / 100);
                                data1.total = data1.subTotal - (double)(data1.discountAmount);
                                medId = item.medicineId;
                                medQuantity = item.quantity;
                            }
                            index++;
                        }
                        if (data1.items == 0)               // if there is only a single item in sale
                            data1.isDeleted = true;
                        var data2 = (from c in db.Stocks
                                     where c.isDeleted == false && c.medicineId == medId
                                     select c).SingleOrDefault();
                        data2.quantity += medQuantity;     // Stock updationn
                        db.SaveChanges();
                        if (data1.items == 0)
                           this.Close();
                        else
                            loadData();
                        MessageBox.Show("SaleItem removed successfully");
                        updateWindow();
                    }
                }
            }
            else if (sender.Equals(invoice))
            {
                int subtotal = 0, total = 0;
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                var dat = (from d in db.Sales where d.id == saleId && d.isDeleted == false select d).SingleOrDefault();
                var data1 = from d in db.Invoices select d;
                if (db != null)
                {
                    foreach (var item in data1)
                        db.Invoices.Remove(item);
                }
                var data = (from p in db.SaleItems
                            from f in db.Medicines.Where(y => y.id == p.medicineId).DefaultIfEmpty()
                            where p.isDeleted == false && p.saleId == saleId
                            select p);
                double total1 = Math.Round(Convert.ToDouble(dat.total)),subTota = Math.Round(Convert.ToDouble(dat.subTotal)),inTotal = 0;
                string tr = total1.ToString("N2");
                string top1 = saleId.ToString();
                while(top1.Length != 5)
                {
                    top1 = "0" + top1;
                }
                string bonu = "",s = "";
                string quantity = "";
                int bonusValue = 0;
                foreach(var item in data)
                {
                    var medicine = (from d in db.Medicines
                                   where d.id == item.medicineId && d.isDeleted == false
                                    select d).SingleOrDefault();
                    var type = (from d in db.Types
                                where d.id == medicine.typeId && d.isDeleted == false
                                select d).SingleOrDefault();
                    quantity = item.quantity.ToString();
                    if (item.bonusId != null || item.bonusId == -1)
                    {
                        var bonus = (from d in db.Bonus where d.id == item.bonusId && d.isDeleted == false select d).SingleOrDefault();
                        bonu = bonus.name;
                        bonusValue = Convert.ToInt32(item.bonus);
                        quantity = (item.quantity - item.bonus).ToString();
                    }
                    inTotal = Math.Round(Convert.ToDouble(item.total));
                    s = DateTime.Now.ToString("dd/MM/yyyy");
                    Invoice invoice = new Invoice() { Total = inTotal.ToString("N2"), R_Price = item.Price.ToString(), SubTotal = subTota.ToString("N2"),Discount = item.discount + "%",Quantity = quantity, Date = s, OverallTotal = tr,Bonus = bonu,Name = dat.Name.ToUpper(),SaleType = "Customer",SaleId = top1, TotalDiscount = dat.discount.ToString() + "%", Item = medicine.name, Type = type.name };
                    db.Invoices.Add(invoice);
                    subtotal += Convert.ToInt32(item.subTotal);
                    total += Convert.ToInt32(item.total);
                }
                db.SaveChanges();
                ReportViewer rp = new ReportViewer();           
                rp.Show();
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
    }

    public class SaleInfo
    {
        private string name ;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string potency;
        public string Potency
        {
            get { return potency; }
            set { potency = value; }
        }
        public ObservableCollection<string> mediCombo { get; set; }

        private double discount;
        public double Discount
        {
            get { return discount; }
            set { discount = value; }
        }
        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        private double subTotal;
        public double SubTotal
        {
            get { return subTotal; }
            set { subTotal = value; }
        }

        private int total;
        public int Total
        {
            get { return total; }
            set { total = value; }
        }
    }
}
