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
    /// Interaction logic for NewSale.xaml
    /// </summary>
    public partial class NewSale : Window
    {
        public Customer cust;
        public Sale sale;
        public bool saved = false;

        private CarManager carManager = new CarManager();
        private CustomerManager customerManager = new CustomerManager();

        public List<Car> cars;
        public List<Car> availableCars = new List<Car>();
        public List<Customer> customers;

        public NewSale()
        {
            InitializeComponent();
            refreshComboBoxes();
            chkExisting.IsChecked = true;
            cboCustomer.IsEnabled = false;
        }

        public NewSale(Sale sale)
        {
            InitializeComponent();
            refreshComboBoxes();
            this.Title = "Sale";

            //Disable everything
            txtAmount.IsEnabled = false;
            txtFirstName.IsEnabled = false;
            txtLastName.IsEnabled = false;
            txtAddress.IsEnabled = false;
            txtEmail.IsEnabled = false;
            txtPhoneNumber.IsEnabled = false;
            btnSave.Visibility = Visibility.Hidden;
            chkExisting.IsEnabled = false;
            cboCar.IsEnabled = false;
            cboCustomer.IsEnabled = false;

            //Create Objects
            Car car = new Car();
            Customer customer = customerManager.getCustomerByID(sale.getCustomerID());

            //Find the proper car object            
            List<Car> cars = new List<Car>();
            cars = carManager.returnAllCars();
            foreach (Car car2 in cars)
            {
                if (car2.getSalesID() == sale.getID())
                {
                    car = car2;
                }
            }            
           
            //Set labels
            txtAmount.Text = sale.getSaleAmount().ToString("c");
            txtFirstName.Text = customer.getFirstName();
            txtLastName.Text = customer.getLastName();
            txtAddress.Text = customer.getAddress();
            txtEmail.Text = customer.getEmailAddress();
            txtPhoneNumber.Text = customer.getPhoneNumber();

            //Set combo boxes
            cboCar.Items.Clear();
            cboCar.Items.Add(car.getCarName());
            cboCar.SelectedIndex = 0;
            cboCustomer.SelectedItem = customer.getName();
        }

        private void refreshComboBoxes()
        {
            try
            {
                //Get customers and cars
                cars = carManager.returnAllCars();
                customers = customerManager.returnAllCustomers();

                foreach (Car car in cars)
                {
                    if (car.getSalesID() == 0 && car.getTradeInID() == 0)
                    {
                        availableCars.Add(car);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //Refresh the car cbo
            cboCar.Items.Clear();
            foreach (Car car in availableCars)
            {
                cboCar.Items.Add(car.getCarName());
            }
            cboCar.SelectedIndex = 0;

            //Refresh the customers cbo
            cboCustomer.Items.Clear();
            foreach (Customer customer in customers)
            {
                cboCustomer.Items.Add(customer.getName());
            }
            cboCustomer.SelectedIndex = 0;
        }

        private void ChkExisting_Click(object sender, RoutedEventArgs e)
        {
            if (!(bool)chkExisting.IsChecked)
            {                
                txtFirstName.IsEnabled = false;
                txtLastName.IsEnabled = false;
                txtAddress.IsEnabled = false;
                txtEmail.IsEnabled = false;
                txtPhoneNumber.IsEnabled = false;
                cboCustomer.IsEnabled = true;
            }
            else
            {                
                txtFirstName.IsEnabled = true;
                txtLastName.IsEnabled = true;
                txtAddress.IsEnabled = true;
                txtEmail.IsEnabled = true;
                txtPhoneNumber.IsEnabled = true;
                cboCustomer.IsEnabled = false;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Decimal amount = 0;

            string carName = cars[cboCar.SelectedIndex].getCarName();
            string firstName = "";
            string lastName = "";
            string address = "";
            string emailAddress = "";
            string phoneNumber = "";

            try
            {
                amount = Convert.ToDecimal(txtAmount.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid entry for sale amount");
            }

            if (((bool)chkExisting.IsChecked) && (txtFirstName.Text == ""))
            {
                MessageBox.Show("First name field cannot be blank");
            }
            else if (((bool)chkExisting.IsChecked) && (txtLastName.Text == ""))
            {
                MessageBox.Show("Last name field cannot be blank");
            }
            else if (((bool)chkExisting.IsChecked) && (txtAddress.Text == ""))
            {
                MessageBox.Show("Address field cannot be blank");
            }
            else if (((bool)chkExisting.IsChecked) && (txtEmail.Text == ""))
            {
                MessageBox.Show("Email address field cannot be blank");
            }
            else if (((bool)chkExisting.IsChecked) && (txtPhoneNumber.Text == ""))
            {
                MessageBox.Show("Phone number field cannot be blank");
            }
            else if (((bool)chkExisting.IsChecked) && (amount != 0))
            {
                firstName = txtFirstName.Text;
                lastName = txtLastName.Text;
                emailAddress = txtEmail.Text;
                address = txtAddress.Text;
                phoneNumber = txtPhoneNumber.Text;
                cust = new Customer(firstName, lastName, address, 0, emailAddress, phoneNumber);
                sale = new Sale(0, amount, DateTime.Now, 0, 0, 0);
                saved = true;
                this.Close();
            }
            else if ((!(bool)chkExisting.IsChecked) && (amount != 0))
            {
                cust = customers[cboCustomer.SelectedIndex];
                sale = new Sale(0, amount, DateTime.Now, 0, 0, 0);
                saved = true;
                this.Close();
            }
        }
    }
}
