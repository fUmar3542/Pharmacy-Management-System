﻿using System;
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
using System.Globalization;
using System.Threading;

namespace Rasheed_Traders
{
    /// <summary>
    /// Interaction logic for Ledger.xaml
    /// </summary>
    public partial class Ledger : Window
    {
        List<ledger> ticketsList = new List<ledger>();

        public Ledger()
        {
            InitializeComponent();
            loadData();
        }

        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            List<ledger> list = new List<ledger>() { };
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var doc2 = from d in db.Sales
                       where d.isDeleted == false
                       select new
                       {
                           Total_Items = d.items,
                           Partner_Name = d.Name.ToUpper(),
                           SubTotal = d.subTotal,
                           Is_Purchase = d.isPurchase,
                           Total = d.total,
                           Date = d.createdAt,
                           Discount_Percentage = d.discount,
                           Discount_Amount = d.discountAmount,
                       };
            int totalPurchase = 0, totalSale = 0;
            string s = "";
            foreach (var item in doc2)
            {
                if (item.Date >= fromDate.SelectedDate && item.Date <= to.SelectedDate.Value.AddDays(1))
                {
                    s = item.Date.ToString("dd/MM/yyyy HH:mm:ss");
                    ledger g = new ledger { Partner_Name = item.Partner_Name, TOTAL = Convert.ToInt32(item.Total), Total_Items = item.Total_Items, SUBTOTAL = item.SubTotal, DATE = s, Discount_Amount = Convert.ToInt32(item.Discount_Amount), Discount_Percentage = Convert.ToDouble(item.Discount_Percentage) };
                    if (item.Is_Purchase == true)
                    {
                        g.Type = "Purchase";
                        totalPurchase += Convert.ToInt32(item.Total);
                    }
                    else if (item.Is_Purchase == false)
                    {
                        g.Type = "Sale";
                        totalSale += Convert.ToInt32(item.Total);
                    }
                    list.Add(g);
                }
            }
            sale1.Text = totalSale.ToString();
            purchase.Text = totalPurchase.ToString();
            table.ItemsSource = list;         
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void loadData()
        {
            //fromDate.Text = Convert.ToDateTime(fromDate.Text).ToString("yyyy/MM/dd");
            System.Globalization.CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            DateTime from = DateTime.Now;
            var doc2 = from d in db.Sales
                       where d.isDeleted == false
                       select new
                       {
                           Total_Items = d.items,
                           Partner_Name = d.Name.ToUpper(),
                           SubTotal = d.subTotal,
                           Is_Purchase = d.isPurchase,
                           Total = d.total,
                           Date = d.createdAt,
                           Discount_Percentage = d.discount,
                           Discount_Amount = d.discountAmount,
                       };
            int totalPurchase = 0,totalSale = 0;
            string s = "";
            foreach(var item in doc2)
            {
                s = item.Date.ToString("dd/MM/yyyy HH:mm:ss");
                ledger g = new ledger { Partner_Name = item.Partner_Name, TOTAL = Convert.ToInt32(item.Total), Total_Items = item.Total_Items, SUBTOTAL = item.SubTotal, DATE = s, Discount_Amount = Convert.ToInt32(item.Discount_Amount), Discount_Percentage = Convert.ToDouble(item.Discount_Percentage) };
                if (item.Is_Purchase == true)
                {
                    g.Type = "Purchase";
                    totalPurchase += Convert.ToInt32(item.Total);
                }
                else if (item.Is_Purchase == false)
                {
                    g.Type = "Sale";
                    totalSale += Convert.ToInt32(item.Total);
                }
                ticketsList.Add(g);
                if (from > item.Date)
                    from = item.Date;
            }
            sale1.Text = totalSale.ToString();
            purchase.Text = totalPurchase.ToString();
            to.SelectedDate = DateTime.Now;
            fromDate.SelectedDate = from;
            table.ItemsSource = ticketsList;
        }
    }

    public class ledger
    {
        private int items;
        public int Total_Items
        {
            get { return items; }
            set { items = value; }
        }
        private string name;
        public string Partner_Name
        {
            get { return name; }
            set { name = value; }
        }
        private double subTotal;
        public double SUBTOTAL
        {
            get { return subTotal; }
            set { subTotal = value; }
        }
        private string type;
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        private int total;
        public int TOTAL
        {
            get { return total; }
            set { total = value; }
        }
        private string date;
        public string DATE
        {
            get { return date; }
            set { date = value; }
        }
        private double dPercentage;
        public double Discount_Percentage
        {
            get { return dPercentage; }
            set { dPercentage = value; }
        }
        private int dAmount;
        public int Discount_Amount
        {
            get { return dAmount; }
            set { dAmount = value; }
        }
    }
}


//int totalPurchase = 0, totalSale = 0;
//string date = to.SelectedDate.Value.AddDays(1).ToString("dd/MM/yyyy HH:mm:ss");
//string fromD = fromDate.SelectedDate.Value.ToString("dd/MM/yyyy HH:mm:ss");
//List<ledger> list = new List<ledger>() { };
//foreach (var item in ticketsList)
//{
//    if ((String.Compare(item.DATE, fromD) >= 0) && (String.Compare(item.DATE, date) <= 0))
//    {
//        ledger g = new ledger { Partner_Name = item.Partner_Name, TOTAL = Convert.ToInt32(item.TOTAL), Total_Items = item.Total_Items, SUBTOTAL = item.SUBTOTAL, DATE = item.DATE, Discount_Amount = Convert.ToInt32(item.Discount_Amount), Discount_Percentage = Convert.ToDouble(item.Discount_Percentage) };
//        if (item.Type == "Purchase")
//        {
//            g.Type = "Purchase";
//            totalPurchase += Convert.ToInt32(item.TOTAL);
//        }
//        else if (item.Type == "Sale")
//        {
//            g.Type = "Sale";
//            totalSale += Convert.ToInt32(item.TOTAL);
//        }
//        list.Add(g);
//    }
//}
//sale1.Text = totalSale.ToString();
//purchase.Text = totalPurchase.ToString();
//table.ItemsSource = list;