using DataObjects;
using System;
using LogicLayer;
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
    /// Interaction logic for NewCar.xaml
    /// </summary>
    public partial class NewCar : Window
    {
        public Car currentCar = new Car();
        public CarManager manager = new CarManager();
        public bool saved = false;
        public bool shipment = false;

        public string shipmentAmountOrTradeInDestination;

        public NewCar(bool used)
        {
            InitializeComponent();
            populateComboBoxes();
            currentCar.setSaleID(0);
            currentCar.setTradeInID(0);
            currentCar.setShipmentID(0);
            currentCar.setUsed(used);            
            this.Title = !used ? this.Title : "Add Used Car";
        }

        public NewCar(Car car)
        {
            InitializeComponent();
            populateComboBoxes();
            this.currentCar = car;

            txtVIN.Text = car.getVIN();
            cboMakes.SelectedItem = car.getMake();
            txtModel.Text = car.getModel();
            cboYears.SelectedItem = car.getYear();
            txtColor.Text = car.getColor();
            txtMSRP.Text = car.getMSRP().ToString("c");                     
            lblTradeInDestination.Content = "Amount: ";
            txtVIN.IsEnabled = false;
            cboMakes.IsEnabled = false;
            txtModel.IsEnabled = false;
            cboYears.IsEnabled = false;
            txtColor.IsEnabled = false;
            txtMSRP.IsEnabled = false;
            txtShipmentAmount.IsEnabled = false;
            cboCarType.IsEnabled = false;
            cboTradeInDestination.IsEnabled = false;
            btnSubmit.Visibility = Visibility.Hidden;
            this.Title = "Car";

            if (car.getTradeInID() != 0)
            {
                txtShipmentAmount.Visibility = Visibility.Hidden;
                cboTradeInDestination.Visibility = Visibility.Visible;
            }
            else
            {
                txtShipmentAmount.Visibility = Visibility.Visible;
                cboTradeInDestination.Visibility = Visibility.Hidden;
            }
            
            try
            {
                if (car.getTradeInID() == 0)
                {
                    Decimal amount = 
                        manager.returnShipmentAmount(car.getShipmentID());
                    txtShipmentAmount.Text = amount.ToString("c");
                }
                else
                {
                    cboTradeInDestination.SelectedItem = manager.
                        returnTradeInDestination(car.getTradeInID());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void isTradeIn(bool test)
        {
            if (test)
            {
                lblTradeInDestination.Content = "Destination: ";
                txtShipmentAmount.Visibility = Visibility.Hidden;
                this.Title = "Add Trade-In";
            }
            else
            {
                lblTradeInDestination.Content = "Amount: ";
                txtShipmentAmount.Visibility = Visibility.Visible;
            }

            shipment = test;
        }

        private void populateComboBoxes()
        {
            addYears();
            addMakes();
            addCarTypes();
            addTradeInDestinations();
        }

        private void addMakes()
        {
            List<string> makes = manager.returnMakes();
            makes.Sort();

            foreach (string make in makes)
            {
                cboMakes.Items.Add(make);
            }
            cboMakes.SelectedItem = cboMakes.Items[0];
        }

        private void addCarTypes()
        {
            List<string> types = manager.returnCarTypes();
            types.Sort();

            foreach (string type in types)
            {
                cboCarType.Items.Add(type);
            }
            cboCarType.SelectedItem = cboCarType.Items[0];
        }

        private void addTradeInDestinations()
        {
            cboTradeInDestination.Items.Add("WHOLESALE");
            cboTradeInDestination.Items.Add("RESOLD");
            cboTradeInDestination.Items.Add("SCRAPYARD");
            cboTradeInDestination.SelectedIndex = 0;
        }

        private void addYears()
        {
            int minYear = 1900;
            int maxYear = DateTime.Now.Year + 1;

            for (int i = minYear; i <= maxYear; i++)
            {
                cboYears.Items.Add(i.ToString());
            }
            cboYears.SelectedItem = cboYears.Items[0];
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //Validate user input
            try
            {
                Decimal test = Convert.ToDecimal(txtMSRP.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Current entry is not a number");
                return;
            }

            if (!shipment)
            {
                try
                {
                    Decimal test = Convert.ToDecimal(txtShipmentAmount.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Current entry is not a number");
                    return;
                }
            }

            //Confirm label values are valid
            if (txtVIN.Text == "")
            {
                MessageBox.Show("VIN field cannot be blank");
            }
            else if (txtModel.Text == "")
            {
                MessageBox.Show("Makes field cannot be blank");
            }
            else if (txtColor.Text == "")
            {
                MessageBox.Show("Color field cannot be blank");
            }
            else if (txtMSRP.Text == "")
            {
                MessageBox.Show("MSRP field cannot be blank");
            }
            else if (txtShipmentAmount.Text == "" && (!shipment))
            {
                MessageBox.Show("Shipment amount field cannot be blank");
            }
            else
            {                

                //Declare attributes
                string vin = txtVIN.Text;
                string make = cboMakes.SelectedItem.ToString();
                string model = txtModel.Text;
                int year = Convert.ToInt32(cboYears.SelectedItem.ToString());
                string color = txtColor.Text;
                Decimal msrp = Convert.ToDecimal(txtMSRP.Text);
                string carType = cboCarType.SelectedItem.ToString();

                if (!shipment)
                {
                    shipmentAmountOrTradeInDestination = txtShipmentAmount.Text;
                }
                else
                {
                    shipmentAmountOrTradeInDestination = cboTradeInDestination.SelectedItem.ToString();
                }

                //Create object and return
                currentCar = new Car(vin, make, model, year, color, msrp,
                                    currentCar.getSalesID(),
                                    currentCar.getShipmentID(),
                                    carType, currentCar.isUsed(),
                                    currentCar.getTradeInID());
                saved = true;
                this.Close();
            }
        }
    }
}
