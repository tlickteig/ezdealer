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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class NewRepair : Window
    {
        public Repair repair;
        public Customer customer;
        public bool saved;

        private CustomerManager _customerManager = new CustomerManager();
        private RepairManager _repairManager = new RepairManager();
        private List<RepairLineItem> _lineItems = new List<RepairLineItem>();
        private List<Customer> _customers = new List<Customer>();

        public NewRepair()
        {
            InitializeComponent();
            populateCustomerList();
            chkNewCustomer.IsChecked = true;
            cboCustomers.IsEnabled = false;
        }

        public NewRepair(Repair repair)
        {
            InitializeComponent();
            this.Title = "Repair";

            //Get repair information
            string repairDescription = repair.getRepairDescription();
            string vin = repair.getVIN();
            List<RepairLineItem> lineItems = repair.GetLineItems();

            //Get customer information
            Customer customer = _customerManager.getCustomerByID(repair.getCustomerID());
            string firstName = customer.getFirstName();
            string lastName = customer.getLastName();
            string address = customer.getAddress();
            string email = customer.getEmailAddress();
            string phoneNumber = customer.getPhoneNumber();

            //Set everything to disabled
            btnAddLineItem.Visibility = Visibility.Hidden;
            btnRemoveLineItem.Visibility = Visibility.Hidden;
            txtVIN.IsEnabled = false;
            cboCustomers.IsEnabled = false;
            chkNewCustomer.IsEnabled = false;
            txtFirstName.IsEnabled = false;
            txtLastName.IsEnabled = false;
            txtAddress.IsEnabled = false;
            txtEmailAddress.IsEnabled = false;
            txtPhoneNumber.IsEnabled = false;
            txtDescription.IsEnabled = false;
            btnSaveRepair.Visibility = Visibility.Hidden;

            //Set the text of everything
            txtVIN.Text = repair.getVIN();
            txtDescription.Text = repair.getRepairDescription();
            txtFirstName.Text = customer.getFirstName();
            txtLastName.Text = customer.getLastName();
            txtAddress.Text = customer.getAddress();
            txtEmailAddress.Text = customer.getEmailAddress();
            txtPhoneNumber.Text = customer.getPhoneNumber();

            //Set the combo box item
            cboCustomers.Items.Clear();
            cboCustomers.Items.Add(customer.getName());
            cboCustomers.SelectedIndex = 0;

            //Add items to the datagrid
            _lineItems = repair.GetLineItems();
            dgLineItemList.ItemsSource = _lineItems;
            //dgLineItemList.Columns.RemoveAt(0);
            //dgLineItemList.Columns.RemoveAt(1);
        }

        private void populateCustomerList()
        {
            try
            {
                _customers = _customerManager.returnAllCustomers();
                cboCustomers.Items.Clear();
                foreach (Customer customer in _customers)
                {
                    cboCustomers.Items.Add(customer.getName());
                }
                cboCustomers.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChkNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (!(bool)chkNewCustomer.IsChecked)
            {
                txtFirstName.IsEnabled = false;
                txtLastName.IsEnabled = false;
                txtAddress.IsEnabled = false;
                txtEmailAddress.IsEnabled = false;
                txtPhoneNumber.IsEnabled = false;
                cboCustomers.IsEnabled = true;
            }
            else
            {
                txtFirstName.IsEnabled = true;
                txtLastName.IsEnabled = true;
                txtAddress.IsEnabled = true;
                txtEmailAddress.IsEnabled = true;
                txtPhoneNumber.IsEnabled = true;
                cboCustomers.IsEnabled = false;
            }
        }

        private void BtnAddLineItem_Click(object sender, RoutedEventArgs e)
        {
            NewRepairLineItem lineItemWindow = new NewRepairLineItem();
            lineItemWindow.ShowDialog();
            if (lineItemWindow.saved)
            {              
                RepairLineItem lineItem = lineItemWindow.lineItem;
                _lineItems.Add(lineItem);
                dgLineItemList.ItemsSource = new List<RepairLineItem>();
                dgLineItemList.ItemsSource = _lineItems;
                dgLineItemList.Columns.RemoveAt(0);
                dgLineItemList.Columns.RemoveAt(1);
            }
        }

        private void BtnSaveRepair_Click(object sender, RoutedEventArgs e)
        {
            if (txtVIN.Text == "")
            {
                MessageBox.Show("VIN Field cannot be blank");
            }
            else if ((chkNewCustomer.IsChecked == true) && (txtFirstName.Text == ""))
            {
                MessageBox.Show("First Name field cannot be blank");
            }
            else if ((chkNewCustomer.IsChecked == true) && (txtLastName.Text == ""))
            {
                MessageBox.Show("Last Name field cannot be blank");
            }
            else if ((chkNewCustomer.IsChecked == true) && (txtAddress.Text == ""))
            {
                MessageBox.Show("Address field cannot be blank");
            }
            else if ((chkNewCustomer.IsChecked == true) && (txtEmailAddress.Text == ""))
            {
                MessageBox.Show("Email Address field cannot be blank");
            }
            else if ((chkNewCustomer.IsChecked == true) && (txtPhoneNumber.Text == ""))
            {
                MessageBox.Show("Phone Number field cannot be blank");
            }
            else if (txtDescription.Text == "")
            {
                MessageBox.Show("Description field cannot be blank");
            }
            else
            {
                repair = new Repair(0, txtDescription.Text, 0, 0, DateTime.Now, 0, txtVIN.Text, _lineItems);

                if (chkNewCustomer.IsChecked == true)
                {
                    customer = new Customer(txtFirstName.Text, txtLastName.Text,
                        txtAddress.Text, 0, txtEmailAddress.Text, txtPhoneNumber.Text);
                }
                else
                {
                    customer = _customers[cboCustomers.SelectedIndex];
                }

                saved = true;
                this.Close();
            }
        }

        private void BtnRemoveLineItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgLineItemList.SelectedIndex != -1)
            {
                RepairLineItem lineItem = _lineItems[dgLineItemList.SelectedIndex];
                _lineItems.Remove(lineItem);
                dgLineItemList.ItemsSource = new List<RepairLineItem>();
                dgLineItemList.ItemsSource = _lineItems;
                dgLineItemList.Columns.RemoveAt(0);
                dgLineItemList.Columns.RemoveAt(1);
            }
        }
    }
}
