using DataObjects;
using LogicLayer;
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

namespace EZDealer
{
    /// <summary>
    /// Interaction logic for NewPart.xaml
    /// </summary>
    public partial class NewPart : Window
    {
        private PartsManager _partsManager = new PartsManager();
        private bool viewMode = false;
        public bool deleted = false;
        public Part part = new Part();

        public NewPart()
        {
            InitializeComponent();
            refreshListBoxes();
        }

        public NewPart(Part part)
        {
            InitializeComponent();
            refreshListBoxes();

            viewMode = true;
            this.Title = "Part";

            txtCost.Text = part.getPartCost().ToString("c");
            txtSerialNumber.Text = Convert.ToString(part.getSerialNumber());
            cboPartTypes.SelectedItem = part.getPartType();

            txtCost.IsEnabled = false;
            txtSerialNumber.IsEnabled = false;
            cboPartTypes.IsEnabled = false;
            btnSave.Content = "Remove Part";
        }

        private void refreshListBoxes()
        {
            List<string> partTypes = _partsManager.returnAllPartTypes();
            cboPartTypes.Items.Clear();
            foreach (string partType in partTypes)
            {
                cboPartTypes.Items.Add(partType);
            }
            cboPartTypes.SelectedIndex = 0;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtCost.Text == "")
            {
                MessageBox.Show("Cost field cannot be blank");
            }
            else if (txtSerialNumber.Text == "")
            {
                MessageBox.Show("Serial number field cannot be blank");
            }
            else
            {
                try
                {
                    Decimal cost = Convert.ToDecimal(txtCost.Text);                    
                    part = new Part(cboPartTypes.SelectedItem.ToString(), txtSerialNumber.Text, cost);
                    if (viewMode)
                    {
                        deleted = true;
                    }
                    this.Close();
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Cost field in incorrect format");
                }
            }
            
        }
    }
}
