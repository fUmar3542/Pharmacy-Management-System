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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public string userName; 
        public Window1( string s)
        {
            InitializeComponent();
            userName = s;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(cancel))
                this.Close();
            else if(sender.Equals(change))
            {
                if (previousPassword.Password == null || password.Password == null || confirmPassword.Password == null)
                {
                    MessageBox.Show("Please fill out all the fields first");
                    return;
                }
                Rasheed_TradersEntities db = new Rasheed_TradersEntities();
                var doc = from d in db.Users
                          where userName == d.username
                          select d;
                foreach (var item in doc)
                {
                    if(item.password != previousPassword.Password)
                    {
                        MessageBox.Show("Enter the valid previous password");
                        return;
                    }
                    if (password.Password != confirmPassword.Password)
                    {
                        MessageBox.Show("Password must match the confirm password");
                        return;
                    }
                    item.password = password.Password;
                }
                db.SaveChanges();
                MessageBox.Show("Password changed successfully!!!");
                this.Close();
            }
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
