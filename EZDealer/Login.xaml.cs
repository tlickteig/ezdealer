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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private EmployeeManager _employeeManager;

        public Window1()
        {
            InitializeComponent();
            _employeeManager = new EmployeeManager();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            //Declare variables
            Employee emp = null;
            string employeeID = txtUsername.Text;
            string password = pwdPassword.Password;

            //Try to login
            try
            {
                //Login
                int IDNumber = Int32.Parse(employeeID);
                string hashedPassword = _employeeManager.hashPassword(password);
                emp = _employeeManager.getAnEmployee(IDNumber, password);
                if (null == emp)
                {
                    throw new Exception("Incorrect username or password");
                }
                MainWindow main = new MainWindow(emp);
                this.Visibility = Visibility.Hidden;
                main.ShowDialog();
                txtUsername.Text = "";
                pwdPassword.Password = "";

                //Logout when main window is done
                main = null;
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
    }
}
