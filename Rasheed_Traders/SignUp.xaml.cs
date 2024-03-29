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
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        public SignUp()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(Cancel))
                this.Close();
            else if(sender.Equals(Create))
            {
                if(name.Text == "" || username.Text == "" || password.Password == "" || confirmPassword.Password == "")
                {
                    MessageBox.Show("Please fill out all the fields first");
                    return;
                }
                if (password.Password != confirmPassword.Password)
                {
                    MessageBox.Show("Password must match the confirm password");
                    return;
                }
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                var doc = from d in db.Users
                          select new
                          {
                              username = d.username,
                          };
                foreach (var item in doc)
                {
                    if(item.username == username.Text)
                    {
                        MessageBox.Show("Username already exist");
                        return;
                    }
                }
                User u = new User {name = name.Text, username = username.Text, password = password.Password, };
                db.Users.Add(u);
                db.SaveChanges();
                MessageBox.Show("Username created successfully");
                this.Close();
            }
        }
    }
}
