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
    /// Interaction logic for AddOrViewSales.xaml
    /// </summary>
    public partial class AddOrViewSales : Window
    {
        public AddOrViewSales()
        {
            InitializeComponent();
            loadData();
        }
        public void loadData()
        {
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var doc2 = from d in db.Sales
                       where d.isDeleted == false && d.isPurchase == false
                       select new
                       {
                           TOTAL_ITEMS = d.items,
                           BUYER_NAME = d.Name.ToUpper(),
                           SUBTOTAL = d.subTotal,
                           TOTAL = d.total,
                           DATE = d.createdAt,
                           Discount_Percentage = d.discount,
                           Discount_Amount = d.discountAmount,
                       };
            table.ItemsSource = doc2.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(createSale))
            {
                string title = "CreateSale";  /*Your Window Instance Name*/
                var existingWindow = Application.Current.Windows.
                Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                if (existingWindow == null)
                {
                    CreateSale newWindow = new CreateSale(); /* Give Your window Instance */
                    newWindow.Title = title;
                    newWindow.Show();
                }
            }
            else if (sender.Equals(searchButton))
            {
                if (searchedContent.Text == "")
                {
                    loadData();
                    return;
                }
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                var doc2 = from d in db.Sales
                           where d.isDeleted == false && d.isPurchase == false && d.Name.Contains(searchedContent.Text)
                           select new
                           {
                               TOTAL_ITEMS = d.items,
                               BUYER_NAME = d.Name.ToUpper(),
                               SUBTOTAL = d.subTotal,
                               TOTAL = d.total,
                               DATE = d.createdAt,
                               Discount_Percentage = d.discount,
                               Discount_Amount = d.discountAmount,
                           };
                table.ItemsSource = doc2.ToList();
            }
        }
    }
}
