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
    /// Interaction logic for AddOrViewPurchase.xaml
    /// </summary>
    public partial class AddOrViewPurchase : Window
    {
        public AddOrViewPurchase()
        {
            InitializeComponent();
            loadData();
        }

        private void loadData()
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(createPurchase))
            {
                string title = "CreatePurchase";  /*Your Window Instance Name*/
                var existingWindow = Application.Current.Windows.
                Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
                if (existingWindow == null)
                {
                    CreatePurchase newWindow = new CreatePurchase(); /* Give Your window Instance */
                    newWindow.Title = title;
                    newWindow.Show();
                }
            }
            else if (sender.Equals(searchButton))
            {

            }
        }
    }
}
