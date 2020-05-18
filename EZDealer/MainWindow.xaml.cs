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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EZDealer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Employee _employee;
        private EmployeeManager _employeeManager;
        private CarManager _carManager;
        private CustomerManager _customerManager;
        private BillManager _billingManager;
        private SaleManager _saleManager;
        private PartsManager _partsManager;
        private RepairManager _repairManager;

        private List<Employee> employees;
        private List<Car> cars;
        private List<PartVM> parts;
        private List<Repair> repairs;
        private List<Bill> bills;

        public MainWindow(Employee employee)
        {
            InitializeComponent();

            //Declare variables
            _employee = employee;
            _employeeManager = new EmployeeManager();
            _carManager = new CarManager();
            _customerManager = new CustomerManager();
            _billingManager = new BillManager();
            _saleManager = new SaleManager();
            _partsManager = new PartsManager();
            _repairManager = new RepairManager();

            lblUser.Content = employee.getEmployeeName();

            //Reset labels and group boxes
            refreshListView();
            hideAllGroupBoxes();
            blankLabels();
            disableButtons();
        }

        private void disableButtons()
        {
            switch (_employee.getRole())
            {
                case "SALESMAN":
                    btnRepairs.IsEnabled = false;
                    btnBilling.IsEnabled = false;
                    btnEmployees.IsEnabled = false;
                    break;
                case "TECHNICIAN":
                    btnSales.IsEnabled = false;
                    btnBilling.IsEnabled = false;
                    btnEmployees.IsEnabled = false;
                    btnCars.IsEnabled = false;
                    break;
                default:                   
                    break;
            }
        }

        private void hideAllGroupBoxes()
        {
            grpBilling.Visibility = Visibility.Hidden;
            grpEmployees.Visibility = Visibility.Hidden;
            grpCars.Visibility = Visibility.Hidden;
            grpRepairs.Visibility = Visibility.Hidden;
            grpSales.Visibility = Visibility.Hidden;
        }

        private void refreshListView()
        {
            refreshEmployeeListView();
            refreshCarListView();
            refreshSaleListView();
            refreshRepairListView();
            refreshBillingListView();

            lblStatusMessage.Content = "Update success";
        }

        private void refreshEmployeeListView()
        {
            try
            {
                //Clear listview and get all employees
                lvwEmployees.Items.Clear();
                employees = _employeeManager.returnAllEmployees();

                //Refresh the employee list
                foreach (Employee emp in employees)
                {
                    lvwEmployees.Items.Add(emp.getEmployeeName());
                }

                lblStatusMessage.Content = "Employee list update success";
            }
            catch (Exception ex)
            {
                lblStatusMessage.Content = ex.Message;
            }
        }

        private void refreshCarListView()
        {
            try
            {
                //Clear listboxes and return all cars
                lvwNewCars.Items.Clear();
                lvwUsedCars.Items.Clear();
                lvwTradeIns.Items.Clear();
                cars = _carManager.returnAllCars();

                //Refresh each car list
                foreach (Car car in cars)
                {
                    if (car.getTradeInID() != 0 && car.getSalesID() == 0)
                    {
                        lvwTradeIns.Items.Add(car.getCarName());
                    }
                    else if (car.isUsed() && car.getSalesID() == 0)
                    {
                        lvwUsedCars.Items.Add(car.getCarName());
                    }
                    else if (car.getSalesID() == 0)
                    {
                        lvwNewCars.Items.Add(car.getCarName());
                    }
                }

                lblStatusMessage.Content = "Car list update success";
            }
            catch (Exception ex)
            {
                lblStatusMessage.Content = ex.Message;
            }
        }

        private void refreshSaleListView()
        {
            try
            {
                //Clear listboxes and return all cars
                lvwSalesLog.Items.Clear();
                cars = _carManager.returnAllCars();

                //Refresh the sales log
                foreach (Car car in cars)
                {
                    if (car.getSalesID() != 0)
                    {
                        lvwSalesLog.Items.Add(car.getCarName());
                    }
                }

                lblStatusMessage.Content = "Sales list update success";
            }
            catch (Exception ex)
            {
                lblStatusMessage.Content = ex.Message;
            }
        }

        private void refreshRepairListView()
        {
            try
            {
                //Clear listboxes and return all parts and repairs
                lvwPartsList.Items.Clear();
                lvwRepairLog.Items.Clear();
                parts = _partsManager.getAllParts();
                repairs = _repairManager.getAllRepairs();

                //Refresh the parts list
                foreach (PartVM part in parts)
                {
                    if (part.RepairLineItemID == 0)
                    {
                        lvwPartsList.Items.Add(part.getPartName());
                    }
                }

                //Refresh the repairs list
                foreach (Repair repair in repairs)
                {
                    lvwRepairLog.Items.Add(repair.getVIN());
                }

                lblStatusMessage.Content = "Repair list update success";
            }
            catch (Exception ex)
            {
                lblStatusMessage.Content = ex.Message;
            }
        }

        private void refreshBillingListView()
        {
            try
            {
                //Clear listboxes and return all bills
                lvwBillingLog.Items.Clear();
                bills = _billingManager.getAllBills();

                //Refresh the billing list
                foreach (Bill bill in bills)
                {
                    lvwBillingLog.Items.Add(
                      _customerManager.getCustomerByID(bill.getCustomerID()).getName());
                }

                lblStatusMessage.Content = "Billing list update success";
            }
            catch (Exception ex)
            {
                lblStatusMessage.Content = ex.Message;
            }
        }

        private void blankLabels()
        {
            //Set every relevant label's content to a blank string
            lblEmployeeName.Content = "";
            lblCarName.Content = "";
            lblCarNameSale.Content = "";
            lblCarType.Content = "";
            lblColor.Content = "";
            lblCostRepairDescription.Content = "";
            lblCustomerNameSale.Content = "";
            lblEmployeeBirthDate.Content = "";
            lblEmployeeEndDate.Content = "";
            lblEmployeeID.Content = "";
            lblEmployeeName.Content = "";
            lblEmployeeNameSale.Content = "";
            lblEmployeeRole.Content = "";
            lblEmployeeStartDate.Content = "";
            lblMSRP.Content = "";
            lblPartQuantityEmployeeName.Content = "";
            lblRepairCustomerName.Content = "";
            lblRepairPartName.Content = "";
            lblSaleAmount.Content = "";
            lblSaleDate.Content = "";
            lblSerialNumberVIN.Content = "";
            lblVIN.Content = "";
        }

        private void refreshCarLabels(Car car)
        {
            lblCarName.Content = car.getCarName();
            lblCarType.Content = car.getCarType();
            lblColor.Content = "Color: " + car.getColor();
            lblVIN.Content = "VIN: " + car.getVIN();
            lblMSRP.Content = "MSRP: " + car.getMSRP().ToString("c");
        }

        private void BtnEmployees_Click(object sender, RoutedEventArgs e)
        {
            hideAllGroupBoxes();
            refreshEmployeeListView();
            blankLabels();
            grpEmployees.Visibility = Visibility.Visible;
        }

        private void BtnCars_Click(object sender, RoutedEventArgs e)
        {
            hideAllGroupBoxes();
            refreshCarListView();
            blankLabels();
            grpCars.Visibility = Visibility.Visible;
        }

        private void BtnSales_Click(object sender, RoutedEventArgs e)
        {
            hideAllGroupBoxes();
            refreshSaleListView();
            blankLabels();
            grpSales.Visibility = Visibility.Visible;
        }

        private void BtnRepairs_Click(object sender, RoutedEventArgs e)
        {
            hideAllGroupBoxes();
            refreshRepairListView();
            blankLabels();
            grpRepairs.Visibility = Visibility.Visible;
        }

        private void BtnBilling_Click(object sender, RoutedEventArgs e)
        {
            hideAllGroupBoxes();
            refreshBillingListView();
            blankLabels();
            grpBilling.Visibility = Visibility.Visible;
        }

        private void LblUser_MouseEnter(object sender, MouseEventArgs e)
        {
            lblUser.Background = Brushes.CornflowerBlue;
        }

        private void LblUser_MouseLeave(object sender, MouseEventArgs e)
        {
            lblUser.Background = Brushes.White;
        }

        private void BtnNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            //Make a new employee form and return the resulting employee
            NewEmployee empForm = new NewEmployee();
            empForm.editingSelf(false);
            empForm.ShowDialog();
            Employee emp = empForm._employee;

            //Try to add the employee to the database
            try
            {
                _employeeManager.newEmployee(emp);
                lblStatusMessage.Content = "Update Success";
            }
            catch (Exception ex)
            {
                lblStatusMessage.Content = ex.Message;
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void LblUser_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Make a new employee form and give it the employee to be edited
            NewEmployee employeeForm = new NewEmployee(_employee);
            employeeForm.editingSelf(true);
            employeeForm.ShowDialog();

            //Logout or update employee information
            if (employeeForm.deleteButtonPressed)
            {
                this.Close();
            }
            else
            {
                //Try to send the update to the database
                Employee temp = employeeForm._employee;
                try
                {
                    _employeeManager.updateEmployee(temp);
                    _employee = temp;
                    lblStatusMessage.Content = "Update Success";
                    lblUser.Content = _employee.getEmployeeName();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void LvwEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Grab the selected employee
            Employee emp = employees[lvwEmployees.SelectedIndex];

            //Setup variables
            DateTime birthDate = emp.getBirthDate();
            DateTime startDate = emp.getStartDate();
            DateTime endDate = emp.getEndDate();

            //Set end date label
            int year = endDate.Year;
            if (year == 1)
            {
                lblEmployeeEndDate.Content = "End Date: N/A";
            }
            else
            {
                lblEmployeeEndDate.Content = "End Date: " + endDate.ToString().Substring(0, endDate.ToString().IndexOf(" "));
            }

            //Set employee labels
            lblEmployeeName.Content = emp.getEmployeeName();
            lblEmployeeBirthDate.Content = "DOB: " + birthDate.ToString().Substring(0, birthDate.ToString().IndexOf(" "));
            lblEmployeeStartDate.Content = "Start: " + startDate.ToString().Substring(0, startDate.ToString().IndexOf(" "));
            lblEmployeeRole.Content = emp.getRole();
            lblEmployeeID.Content = emp.getEmployeeID().ToString();
        }

        private void LvwEmployees_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Grab the selected employee
            Employee emp = employees[lvwEmployees.SelectedIndex];            

            //Make an employee form and give it the current employee
            NewEmployee newEmployee = new NewEmployee(emp);
            newEmployee.editingSelf(false);
            newEmployee.ShowDialog();

            //Delete or update employee information
            Employee temp = newEmployee._employee;
            if (!newEmployee.deleteButtonPressed)
            {
                //Update the employee
                try
                {
                    _employeeManager.updateEmployee(temp);
                    lblStatusMessage.Content = "Update success";
                }
                catch (Exception ex)
                {
                    lblStatusMessage.Content = ex.Message;
                }
            }
            else
            {
                //Delete the employee
                try
                {
                    _employeeManager.deleteEmployee(temp.getEmployeeID());
                    lblStatusMessage.Content = "Update success";
                }
                catch (Exception ex)
                {
                    lblStatusMessage.Content = ex.Message;
                }
            }
        }

        private void BtnNewCar_Click(object sender, RoutedEventArgs e)
        {
            //Make a car form
            NewCar carForm = new NewCar(false);
            carForm.isTradeIn(false);
            carForm.ShowDialog();

            //Get the shipment amount
            Decimal shipmentAmount = Convert.ToDecimal
                (carForm.shipmentAmountOrTradeInDestination);

            //Try to add the car in the database
            if (carForm.saved)
            {
                Car car = carForm.currentCar;
                try
                {
                    _carManager.addCar(car, shipmentAmount, _employee.getEmployeeID(), DateTime.Now);
                }
                catch (Exception ex)
                {
                    lblStatusMessage.Content = ex.Message;
                }
            }
        }

        private void BtnUsedCar_Click(object sender, RoutedEventArgs e)
        {
            //Make a new form
            NewCar carForm = new NewCar(true);
            carForm.isTradeIn(false);
            carForm.ShowDialog();

            //Get the shipment amount
            Decimal shipmentAmount = Convert.ToDecimal(
                carForm.shipmentAmountOrTradeInDestination);

            //Try to add to the database
            if (carForm.saved)
            {
                Car car = carForm.currentCar;
                try
                {
                    _carManager.addCar(car, shipmentAmount, _employee.getEmployeeID(), DateTime.Now);
                }
                catch (Exception ex)
                {
                    lblStatusMessage.Content = ex.Message;
                }
            }
        }

        private void BtnTradeIn_Click(object sender, RoutedEventArgs e)
        {
            //Make a new car form
            NewCar carForm = new NewCar(true);
            carForm.isTradeIn(true);
            carForm.ShowDialog();

            //Get the trade in destination
            string destination = carForm.shipmentAmountOrTradeInDestination;

            //Try to add to the database
            if (carForm.saved)
            {
                Car car = carForm.currentCar;
                try
                {
                    _carManager.addTradeIn(car, destination, _employee.getEmployeeID(), DateTime.Now);
                }
                catch (Exception ex)
                {
                    lblStatusMessage.Content = ex.Message;
                }
            }
        }

        private void LvwNewCars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Make a new list of cars and loop through to see which ones are used
            List<Car> newCars = new List<Car>();
            foreach (Car car in cars)
            {
                if (!car.isUsed())
                {
                    newCars.Add(car);
                }
            }

            //Send the selected car to the label updater method
            refreshCarLabels(newCars[lvwNewCars.SelectedIndex]);
        }

        private void LvwUsedCars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Make a list of used cars and populate it
            List<Car> usedCars = new List<Car>();
            foreach (Car car in cars)
            {
                if (car.isUsed() && (car.getTradeInID() == 0))
                {
                    usedCars.Add(car);
                }
            }

            //Send the list to the label updater method
            refreshCarLabels(usedCars[lvwUsedCars.SelectedIndex]);
        }

        private void LvwTradeIns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Make a list of trade-ins and populate it
            List<Car> tradeIns = new List<Car>();
            foreach (Car car in cars)
            {
                if ((car.getTradeInID() != 0) && (car.isUsed()))
                {
                    tradeIns.Add(car);
                }
            }

            //Send the list to the updater method
            refreshCarLabels(tradeIns[lvwTradeIns.SelectedIndex]);
        }

        private void LvwNewCars_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Make a list of new cars and add new cars to it
            List<Car> newCars = new List<Car>();            
            foreach (Car car in cars)
            {
                if (!car.isUsed())
                {
                    newCars.Add(car);
                }
            }

            //Get the selected car object
            Car car2;
            if (lvwNewCars.SelectedIndex != -1)
            {
                car2 = newCars[lvwNewCars.SelectedIndex];
            }
            else
            {
                car2 = newCars[0];
            }

            //Send the car to a new form
            Car updatedCar;
            NewCar carForm = new NewCar(car2);
            carForm.ShowDialog();
            updatedCar = carForm.currentCar;
        }

        private void LvwUsedCars_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Make a list of new cars and add used cars to it
            List<Car> usedCars = new List<Car>();
            foreach (Car car in cars)
            {
                if (car.isUsed())
                {
                    usedCars.Add(car);
                }
            }

            //Get the selected car object
            Car car2;
            if (lvwUsedCars.SelectedIndex != -1)
            {
                car2 = usedCars[lvwUsedCars.SelectedIndex];
            }
            else {
                car2 = usedCars[0];
            }

            //Send the car to a new form
            Car updatedCar;
            NewCar carForm = new NewCar(car2);
            carForm.ShowDialog();
            updatedCar = carForm.currentCar;
        }

        private void LvwTradeIns_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Make a list and add trade-ins to it
            List<Car> usedCars = new List<Car>();
            foreach (Car car in cars)
            {
                if (car.isUsed() && (car.getTradeInID() != 0))
                {
                    usedCars.Add(car);
                }
            }

            //Get the selected car object
            Car car2;
            if (lvwTradeIns.SelectedIndex != -1)
            {
                car2 = usedCars[lvwTradeIns.SelectedIndex];
            }
            else
            {
                car2 = usedCars[0];
            }

            //Pass the car to a new form
            Car updatedCar;
            NewCar carForm = new NewCar(car2);
            carForm.ShowDialog();
            updatedCar = carForm.currentCar;
        }

        private void BtnNewSale_Click(object sender, RoutedEventArgs e)
        {
            NewSale saleForm = new NewSale();
            saleForm.ShowDialog();
            if (saleForm.saved)
            {
                string firstName;
                string lastName;

                try
                {
                    //Add a customer
                    if ((bool)saleForm.chkExisting.IsChecked)
                    {
                        _customerManager.addNewCustomer(saleForm.cust);
                    }

                    //New Bill
                    List<BillingLineItem> lineItems = new List<BillingLineItem>();
                    firstName = saleForm.cust.getFirstName();
                    lastName = saleForm.cust.getLastName();
                    int c = _customerManager.getCustomerIDByCustomerName(firstName, lastName);

                    //New billing line item
                    lineItems.Add(new BillingLineItem(0, saleForm.sale.getSaleAmount()));
                    _billingManager.newBill(new Bill(0, c,                       
                        saleForm.sale.getSaleAmount(), 0, DateTime.Now, DateTime.Now.AddDays(30), lineItems));

                    //New sale
                    firstName = saleForm.cust.getFirstName();
                    lastName = saleForm.cust.getLastName();
                    string vin = saleForm.availableCars[saleForm.cboCar.SelectedIndex].getVIN();
                    saleForm.sale.addEmployee(_employee.getEmployeeID());
                    saleForm.sale.setBillingLineItemID(_billingManager.getLastestBillingLineItem());
                    saleForm.sale.setCustomerID(_customerManager.getCustomerIDByCustomerName(firstName, lastName));
                    _saleManager.addNewSale(saleForm.sale, vin);

                    lblStatusMessage.Content = "Update success";
                }
                catch (Exception ex)
                {                    
                    lblStatusMessage.Content = ex.Message;
                }
            }
        }

        private void LvwSalesLog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Create a list of sold cars
            try
            {
                //Get a list of sold cars
                List<Car> soldCars = new List<Car>();
                foreach (Car car in cars)
                {
                    if (car.getSalesID() != 0)
                    {
                        soldCars.Add(car);
                    }
                }

                //Get necessary objects
                Car currentCar = soldCars[lvwSalesLog.SelectedIndex];
                Sale sale = _saleManager.returnSaleByID(currentCar.getSalesID());
                Customer customer = _customerManager.getCustomerByID(sale.getCustomerID());
                Employee emp = _employeeManager.getEmployeeByEmployeeID(sale.getEmployeeID());

                //Change labels
                lblCarNameSale.Content = currentCar.getCarName();
                lblSaleAmount.Content = sale.getSaleAmount().ToString("c");
                lblSaleDate.Content = sale.getSaleDate();
                lblCustomerNameSale.Content = customer.getName();
                lblEmployeeNameSale.Content = emp.getEmployeeName();
            }
            catch (Exception ex)
            {
                lblStatusMessage.Content = ex.Message;
            }
        }

        private void LvwSalesLog_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Get a list of sales
            List<Sale> sales = _saleManager.returnAllSales();
            Sale sale;

            //Get the selected item
            if (lvwSalesLog.SelectedIndex != -1)
            {
                sale = sales[lvwSalesLog.SelectedIndex];
            }
            else
            {
                sale = sales[0];
            }

            //Pass the sale to a new form
            NewSale saleForm = new NewSale(sale);
            saleForm.ShowDialog();
        }

        private void BtnNewPart_Click(object sender, RoutedEventArgs e)
        {
            //Create the dialog
            NewPart partForm = new NewPart();
            partForm.ShowDialog();

            //Add the part to the database
            try
            {
                Part part = partForm.part;
                _partsManager.addNewPart(part);
                lblStatusMessage.Content = "Update success";
            }
            catch (Exception ex)
            {
                lblStatusMessage.Content = ex.Message;
            }
        }

        private void LvwPartsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Make a list of parts not associated with a repair
            List<PartVM> partsVM = new List<PartVM>();
            foreach (PartVM part in parts)
            {
                if (part.RepairLineItemID == 0)
                {
                    partsVM.Add(part);
                }
            }

            //String formatting
            Part partVM = partsVM[lvwPartsList.SelectedIndex];
            char[] partType = partVM.getPartType().ToLower().Replace("_", " ").ToCharArray();
            partType[0] = Char.ToUpper(partType[0]);

            //Update the labels
            lblRepairPartName.Content = new String(partType);
            lblSerialNumberVIN.Content = partVM.getSerialNumber();
            lblCostRepairDescription.Content = partVM.getPartCost().ToString("c");
            lblPartQuantityEmployeeName.Content = "";
            lblRepairCustomerName.Content = "";
        }

        private void LvwPartsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            //Make a list of parts not associated with a repair
            List<PartVM> partsVM = new List<PartVM>();
            foreach (PartVM part2 in parts)
            {
                if (part2.RepairLineItemID == 0)
                {
                    partsVM.Add(part2);
                }
            }

            //Get the selected part
            Part part;
            if (lvwPartsList.SelectedIndex != -1)
            {
                part = partsVM[lvwPartsList.SelectedIndex];
            }
            else
            {
                part = parts[0];
            }

            //Pass the part to a new form
            NewPart partForm = new NewPart(part);
            partForm.ShowDialog();

            //Delete the part if neccessary
            if (partForm.deleted)
            {
                try
                {
                    _partsManager.deletePart(part.getSerialNumber());
                }
                catch (Exception ex)
                {
                    lblStatusMessage.Content = ex.Message;
                }
            }
        }

        private void BtnNewRepair_Click(object sender, RoutedEventArgs e)
        {
            //Create a new form
            NewRepair repairForm = new NewRepair();
            repairForm.ShowDialog();

            try
            {
                //New customer
                if (repairForm.chkNewCustomer.IsChecked == true)
                {
                    _customerManager.addNewCustomer(repairForm.customer);
                }

                //New Bill
                string firstName = repairForm.customer.getFirstName();
                string lastName = repairForm.customer.getLastName();
                int c = _customerManager.getCustomerIDByCustomerName(firstName, lastName);

                //New line item
                List<BillingLineItem> lineItems = new List<BillingLineItem>();
                lineItems.Add(new BillingLineItem(0, repairForm.repair.getAmount()));
                _billingManager.newBill(new Bill(0, c,
                    repairForm.repair.getAmount(), 0, DateTime.Now, DateTime.Now.AddDays(30), lineItems));

                //Add repair
                repairForm.repair.setEmployeeID(_employee.getEmployeeID());
                repairForm.repair.setCustomerID(_customerManager.getCustomerIDByCustomerName(
                    repairForm.customer.getFirstName(), repairForm.customer.getLastName()));
                repairForm.repair.setBillingLineItemID(_billingManager.getLastestBillingLineItem());
                _repairManager.addNewRepair(repairForm.repair);
            }
            catch (Exception ex)
            {
                //throw ex;
                lblStatusMessage.Content = ex.Message;
            }
        }

        private void LvwRepairLog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Get the selected repair
            Repair repair = repairs[lvwRepairLog.SelectedIndex];

            //Update labels
            lblRepairPartName.Content = "Repair";
            lblSerialNumberVIN.Content = repair.getVIN();
            lblCostRepairDescription.Content = repair.getRepairDescription();
            lblPartQuantityEmployeeName.Content = 
                _employeeManager.getEmployeeByEmployeeID(repair.getEmployeeID()).getEmployeeName();
            lblRepairCustomerName.Content = 
                _customerManager.getCustomerByID(repair.getCustomerID()).getName();
        }

        private void LvwRepairLog_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Get the selected item
            Repair repair;
            if (lvwRepairLog.SelectedIndex != -1)
            {
                repair = repairs[lvwRepairLog.SelectedIndex];
            }
            else
            {
                repair = repairs[0];
            }

            //Pass the repair to a new form
            NewRepair repairWindow = new NewRepair(repair);
            repairWindow.ShowDialog();
        }

        private void LvwBillingLog_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Get the selected item
            Bill bill;
            if (lvwBillingLog.SelectedIndex != -1)
            {
                bill = bills[lvwBillingLog.SelectedIndex];
            }
            else
            {
                bill = bills[0];
            }

            //Pass the bill to a new form
            ViewBill billForm = new ViewBill(bill);
            billForm.ShowDialog();
        }
    }
}
