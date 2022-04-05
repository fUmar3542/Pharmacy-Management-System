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
                string title = "Window1";  /*Your Window Instance Name*/
                var existingWindow = Application.Current.Windows.
                Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                if (existingWindow == null)
                {
                    Window1 newWindow = new Window1(); /* Give Your window Instance */
                    newWindow.Title = title;
                    newWindow.Show();
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
