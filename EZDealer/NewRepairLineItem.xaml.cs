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
    /// Interaction logic for NewRepairLineItem.xaml
    /// </summary>
    public partial class NewRepairLineItem : Window
    {
        public RepairLineItem lineItem;
        public bool saved = false;

        private PartsManager _partsManager = new PartsManager();

        public NewRepairLineItem()
        {
            InitializeComponent();
            populateComboBox();
        }

        private void populateComboBox()
        {
            List<string> partTypes = _partsManager.returnAllPartTypes();
            cboPartType.Items.Clear();
            foreach (string partType in partTypes)
            {
                cboPartType.Items.Add(partType);
            }
            cboPartType.SelectedIndex = 0;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Decimal amount;
            string serialNumber;
            string type;

            try
            {
                amount = Convert.ToDecimal(txtAmount.Text);

                if (txtSerialNumber.Text == "")
                {
                    MessageBox.Show("Serial Number field cannot be blank");
                }
                else
                {
                    serialNumber = txtSerialNumber.Text;
                    type = cboPartType.SelectedItem.ToString();

                    if (_partsManager.isPartAvailable(serialNumber, type))
                    {
                        lineItem = new RepairLineItem(0, 0, serialNumber, amount, type);
                        saved = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Part not found");
                    }
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Amount field in incorrect format");
            }            
        }
    }
}
