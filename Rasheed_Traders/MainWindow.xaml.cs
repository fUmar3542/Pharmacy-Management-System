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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rasheed_Traders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isMaximized = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(signup))
            {
                string title = "SignUp";  /*Your Window Instance Name*/
                var existingWindow = Application.Current.Windows.
                Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                if (existingWindow == null)
                {
                    SignUp newWindow = new SignUp(); /* Give Your window Instance */
                    newWindow.Title = title;
                    newWindow.Show();
                }
            }
            else if (sender.Equals(ChangePassword))
            {
                if (userBlock.Text != null)
                {
                    Rasheed_TradersEntities db = new Rasheed_TradersEntities();
                    var doc = from d in db.Users
                              select new
                              {
                                  username = d.username,
                              };
                    bool check = false;
                    foreach(var item in doc)
                    {
                        if(item.username == userBlock.Text)
                        {
                            check = true;
                            string title = "Window1";  /*Your Window Instance Name*/
                            var existingWindow = Application.Current.Windows.
                            Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                            if (existingWindow == null)
                            {
                                Window1 newWindow = new Window1(userBlock.Text); /* Give Your window Instance */
                                newWindow.Title = title;
                                newWindow.Show();
                            }
                        }
                    }
                    if(check == false)
                        MessageBox.Show("Username doesn't exit. Enter valid username or SignUp first");
                }
                else
                    MessageBox.Show("Enter username first");
            }
            else if(sender.Equals(Login))
            {
                if ( userBlock.Text == null || passBlock.Password == null )
                {
                    MessageBox.Show("Please fill out all the fields first");
                    return;
                }
                Rasheed_TradersEntities db = new Rasheed_TradersEntities();
                var doc = from d in db.Users
                          select new
                          {
                              username = d.username,
                              Password = d.password
                          };
                bool check = false;
                foreach (var item in doc)
                {
                    if (item.username == userBlock.Text)
                    {
                        check = true;
                        if (item.Password == passBlock.Password)
                        {                           
                            string title = "HomeWindow";  /*Your Window Instance Name*/
                            var existingWindow = Application.Current.Windows.
                            Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                            if (existingWindow == null)
                            {
                                HomeWindow newWindow = new HomeWindow(); /* Give Your window Instance */
                                newWindow.Title = title;
                                newWindow.Show();
                            }
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Password");
                            return;
                        }
                    }
                }
                if(check == false)
                {
                    MessageBox.Show("Username doesn't exist. Please signup");
                }
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (isMaximized)
            {
                isMaximized = false;
                ChangePassword.Margin = new Thickness(700, 30, 0, 0);
                Main.Margin = new Thickness(0, 0, 0, 150);
            }
            else
            {
                isMaximized = true;
                ChangePassword.Margin = new Thickness(610, 26, 0, 0);
                Main.Margin = new Thickness(0, 0, 0, 0);
            }
        }
    }
}
