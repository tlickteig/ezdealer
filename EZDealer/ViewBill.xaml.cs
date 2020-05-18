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
    /// Interaction logic for ViewBill.xaml
    /// </summary>
    public partial class ViewBill : Window
    {
        CustomerManager customerManager = new CustomerManager();

        public ViewBill(Bill bill)
        {
            InitializeComponent();

            Customer customer = customerManager.getCustomerByID(bill.getCustomerID());

            //Set labels
            lblCustomerName.Content = customer.getName();
            lblAmountDue.Content = "Amount Due: " + bill.getAmountDue().ToString("c");
            lblAmountPaid.Content = "Amount Paid: " + bill.getAmountPaid().ToString("c");
            lblDueDate.Content = "Due Date: " + bill.getDueDate().ToString().Substring(0, 8);
            lblIssueDate.Content = "Issue Date: " + bill.getIssueDate().ToString().Substring(0, 9);

            //Add billing line items           
            dgLineItems.ItemsSource = bill.getLineItems();
            //dgLineItems.Columns[0].Header = "Line Item ID";
        }
    }
}
