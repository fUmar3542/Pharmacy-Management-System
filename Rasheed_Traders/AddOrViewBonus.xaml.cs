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
    /// Interaction logic for AddOrViewBonus.xaml
    /// </summary>
    public partial class AddOrViewBonus : Window
    {
        public AddOrViewBonus()
        {
            InitializeComponent();
            loadData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void loadData()
        {
            List<Bonus> a = new List<Bonus>();
            a.Add(new Bonus() { ID = 1, Type = "Overall", Name = "Normal", Percentage = "10%" });
            a.Add(new Bonus() { ID = 2, Type = "Particular", Name = "Advance", Percentage = "55%" });
            a.Add(new Bonus() { ID = 3, Type = "Overall", Name = "Executive", Percentage = "80%" });
            table.ItemsSource = a;
        }
    }

    public class Bonus
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Percentage { get; set; }
    }
}
