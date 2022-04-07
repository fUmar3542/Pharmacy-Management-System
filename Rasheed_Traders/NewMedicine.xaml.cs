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
    /// Interaction logic for NewMedicine.xaml
    /// </summary>
    public partial class NewMedicine : Window
    {
        public NewMedicine()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (sender.Equals(Cancel))
            //    this.Close();
            //else if (sender.Equals(createButton))
            //{
            //    if (name.Text == null || username.Text == null || password.Password == null || confirmPassword.Password == null)
            //    {
            //        MessageBox.Show("Please fill out all the fields first");
            //        return;
            //    }
            //    if (password.Password != confirmPassword.Password)
            //    {
            //        MessageBox.Show("Password must match the confirm password");
            //        return;
            //    }
            //    Rasheed_TradersEntities db = new Rasheed_TradersEntities();
            //    var doc = from d in db.Users
            //              select new
            //              {
            //                  username = d.username,
            //              };
            //    foreach (var item in doc)
            //    {
            //        if (item.username == username.Text)
            //        {
            //            MessageBox.Show("Username already exist");
            //            return;
            //        }
            //    }
            //    User u = new User { name = name.Text, username = username.Text, password = password.Password, };
            //    db.Users.Add(u);
            //    db.SaveChanges();
            //}
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
