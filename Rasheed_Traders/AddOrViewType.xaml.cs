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
    /// Interaction logic for AddOrViewType.xaml
    /// </summary>
    public partial class AddOrViewType : Window
    {
        public AddOrViewType()
        {
            InitializeComponent();
            loadData();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(cancel))
                this.Close();
            else if (sender.Equals(create))
            {
                if (typeName.Text == null )
                {
                    MessageBox.Show("Enter the type name");
                    return;
                }
                Rasheed_TradersEntities db = new Rasheed_TradersEntities();
                var doc = from d in db.Types
                          select d;
                foreach (var item in doc)
                {
                    if (item.name == typeName.Text)
                    {
                        MessageBox.Show("Type name already exist");
                        return;
                    }
                }
                Type u = new Type { name = typeName.Text, createdAt = DateTime.Now };
                db.Types.Add(u);
                db.SaveChanges();
                MessageBox.Show("Type Created successfully");
            }
        }

        private void loadData()
        {
            //List<Type> a = new List<Type>();
            //a.Add(new Type() { ID = 1, Name = "Serum", createdAt = new DateTime(1974, 1, 17), updatedAt = new DateTime(1974, 1, 17) });
            //a.Add(new Type() { ID = 2, Name = "Panadol", createdAt = new DateTime(1974, 1, 17), updatedAt = new DateTime(1974, 1, 17) });
            //a.Add(new Type() { ID = 3, Name = "Bronocol", createdAt = new DateTime(1974, 1, 17), updatedAt = new DateTime(1974, 1, 17) });
            //table.ItemsSource = a;
        }
    }
    public class Type1
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}