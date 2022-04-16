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
            if (sender.Equals(cancel))
                this.Close();
            else if (sender.Equals(create))
            {
                if (bonusName.Text == null)
                {
                    MessageBox.Show("Enter the bonus name");
                    return;
                }
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                var doc = from d in db.Bonus
                          select d;
                foreach (var item in doc)
                {
                    if (item.name == bonusName.Text)
                    {
                        MessageBox.Show("Bonus name already exist");
                        return;
                    }
                }
                Bonu b = new Bonu { createdAt = DateTime.Now, name = bonusName.Text, description = description.Text, isDeleted = false };
                db.Bonus.Add(b);
                db.SaveChanges();
                MessageBox.Show("Bonus Created successfully");
                loadData();
                bonusName.Text = null;
                description.Text = null;
            }
        }
        private void loadData()
        {
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var doc = from d in db.Bonus
                      where d.isDeleted == false
                      select new
                      {
                          Name = d.name,
                          Date = d.createdAt,
                          Description = d.description
                      };
            tableData.ItemsSource = doc.ToList();
        }
    }
}
