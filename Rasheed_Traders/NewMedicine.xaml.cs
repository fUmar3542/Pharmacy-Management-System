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
            loadData();
        }

        public void loadData()
        {
            Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
            var doc = from d in db.Types
                      where d.isDeleted == false
                      select new
                      {
                          name = d.name.ToUpper(),
                      };
            foreach (var item in doc)
                combo.Items.Add(item.name);
            combo.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(Cancel))
                this.Close();
            else if (sender.Equals(createButton))
            {
                Int32 i = 0;
                if (mediName.Text == null || potency.Text == null)
                {
                    MessageBox.Show("Please fill out all the fields first");
                    return;
                }
                Rasheed_TradersEntities1 db = new Rasheed_TradersEntities1();
                var doc = from d in db.Types
                          where d.name == combo.SelectedItem.ToString()
                          select new
                          {
                              id = d.id,
                          };
                foreach (var item in doc)
                {
                    i = item.id;
                }
                var doc1 = from d in db.Medicines
                          select new
                          {
                              name = d.name,
                          };
                foreach (var item in doc1)
                {
                    if (item.name == mediName.Text)
                    {
                        MessageBox.Show("Medicine Name already exist");
                        return;
                    }
                }        
                Medicine u = new Medicine { name = mediName.Text,potency = potency.Text , typeId = i, createdAt = DateTime.Now,isDeleted=false};
                db.Medicines.Add(u);
                db.SaveChanges();
                MessageBox.Show("Medicine added successfully");
                this.Close();
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
