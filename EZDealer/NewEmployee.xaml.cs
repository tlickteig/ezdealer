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
    /// Interaction logic for NewEmployee.xaml
    /// </summary>
    public partial class NewEmployee : Window
    {
        public bool deleteAllowed = false;
        private EmployeeManager _employeeManager;
        public Employee _employee;
        public bool deleteButtonPressed = false;

        public NewEmployee()
        {
            InitializeComponent();
            _employeeManager = new EmployeeManager();
            addEmployeeRoles();
            lblOPassword.Visibility = Visibility.Hidden;
            pwdOPassword.Visibility = Visibility.Hidden;
        }

        public NewEmployee(Employee employee)
        {
            InitializeComponent();
            addEmployeeRoles();
            _employeeManager = new EmployeeManager();
            _employee = employee;
            placeAttributesIntoFields(employee);
            this.Title = "Employee";
        }

        public void editingSelf(bool editingSelf)
        {
            if (editingSelf)
            {
                btnDeleteEmployee.Content = "Log Out";
            }
            else
            {
                btnDeleteEmployee.Content = "Delete Employee";
            }
            cboEmployeeRole.IsEnabled = !editingSelf;
        }

        private void addEmployeeRoles()
        {
            cboEmployeeRole.Items.Add(new ComboBoxItem { Content = "Admin" });
            cboEmployeeRole.Items.Add(new ComboBoxItem { Content = "Salesman" });
            cboEmployeeRole.Items.Add(new ComboBoxItem { Content = "Technician" });
            cboEmployeeRole.Items.Add(new ComboBoxItem { Content = "Manager" });
        }

        private void placeAttributesIntoFields(Employee employee)
        {
            string[] fullName = employee.getEmployeeFirstAndLastName();
            txtFirstName.Text = fullName[0];
            txtLastName.Text = fullName[1];
            dteBirthDate.SelectedDate = employee.getEmployeeBirthDate();

            switch (employee.getRole())
            {
                case "ADMIN":
                    cboEmployeeRole.SelectedValue = cboEmployeeRole.Items[0];
                    break;
                case "SALESMAN":
                    cboEmployeeRole.SelectedValue = cboEmployeeRole.Items[1];
                    break;
                case "TECHNICIAN":
                    cboEmployeeRole.SelectedValue = cboEmployeeRole.Items[2];
                    break;
                case "MANAGER":
                    cboEmployeeRole.SelectedValue = cboEmployeeRole.Items[3];
                    break;
            }
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {            
            string newPassword = pwdPassword.Password;
            string oldPassword = pwdOPassword.Password;
            string confirmPassword = pwdCPassword.Password;

            if (txtFirstName.Text == "")
            {
                MessageBox.Show("First Name field cannot be blank");
            }
            else if (txtLastName.Text == "")
            {
                MessageBox.Show("Last Name field cannot be blank");
            }
            else if (cboEmployeeRole.SelectedIndex == -1)
            {
                MessageBox.Show("Role field cannot be blank");
            }
            else if (dteBirthDate.SelectedDate == null)
            {
                MessageBox.Show("Birth date field cannot be blank");
            }
            else if (pwdOPassword.Password == "" && null != _employee)
            {
                MessageBox.Show("Password field is blank");
            }
            else if (pwdPassword.Equals(pwdCPassword))
            {
                MessageBox.Show("Passwords do not match");
            }
            else if (null != _employee)
            {
                string firstName = txtFirstName.Text;
                string lastName = txtLastName.Text;
                int employeeID = _employee.getEmployeeID();
                string employeeRole = cboEmployeeRole.Text.ToUpper();
                char[] password = (_employeeManager.hashPassword(pwdPassword.Password)).ToCharArray();
                DateTime birthDate = (DateTime)dteBirthDate.SelectedDate;
                DateTime startDate = _employee.getStartDate();
                DateTime endDate = _employee.getEndDate();
                _employee = new Employee(firstName, lastName, employeeID,
                                        employeeRole, birthDate,
                                        startDate, endDate, password);
                this.Close();
            }
            else
            {
                string firstName = txtFirstName.Text;
                string lastName = txtLastName.Text;
                int employeeID = 0;
                string employeeRole = cboEmployeeRole.Text.ToUpper();
                char[] password = (_employeeManager.hashPassword(pwdPassword.Password)).ToCharArray();
                DateTime birthDate = (DateTime)dteBirthDate.SelectedDate;
                DateTime startDate = DateTime.Now;
                DateTime endDate = new DateTime(1, 1, 1);
                _employee = new Employee(firstName, lastName, employeeID,
                                       employeeRole, birthDate,
                                       startDate, endDate, password);
                this.Close();
            }
        }

        private void BtnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            deleteButtonPressed = true;
            this.Close();
        }
    }
}
