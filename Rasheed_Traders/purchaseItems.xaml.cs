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

namespace Rasheed_Traders
{
    /// <summary>
    /// Interaction logic for purchaseItems.xaml
    /// </summary>
    public partial class purchaseItems : Window
    {
        int saleId = 0;
        public purchaseItems(int a)
        {
            saleId = a;
            InitializeComponent();
            loadData();
        }

        private void loadData()
        {
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
                        });
            if (data != null)
                table.ItemsSource = data.ToList();
            else
                table.ItemsSource = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(removeRow))
            {
                var selectedItem = table.SelectedItem;
                int a = table.SelectedIndex,
                    index = 0,
                    medQuantity = 0,
                    medId = 0;
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
                                data1.total -= item.total;
                                medQuantity = item.quantity;
                                medId = item.medicineId;
                            }
                            index++;
                        }
                        if (data1.items == 0)               // if there is only a single item in sale
                            data1.isDeleted = true;
                        var data2 = (from c in db.Stocks
                                     where c.isDeleted == false && c.medicineId == medId
                                     select c).SingleOrDefault();
                        data2.quantity -= medQuantity;
                        db.SaveChanges();
                        if (data1.items == 0)
                            this.Close();
                        else
                            loadData();
                        MessageBox.Show("PurchaseItem removed successfully");
                        updateWindow();
                    }
                }
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
        }
    }
}