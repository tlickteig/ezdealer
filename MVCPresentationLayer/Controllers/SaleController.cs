using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPresentationLayer.Controllers
{
    public class SaleController : Controller
    {
        private SaleManager manager = new SaleManager();

        private CustomerManager customerManager = new CustomerManager();

        private EmployeeManager employeeManager = new EmployeeManager();

        private CarManager carManager = new CarManager();

        private BillManager billingManager = new BillManager();

        // GET: Sale
        public ActionResult Index()
        {
            List<Sale> sales = manager.returnAllSales();
            List<SaleVM> saleVMs = new List<SaleVM>();

            foreach (Sale sale in sales)
            {
                string employeeName = employeeManager.getEmployeeByEmployeeID(sale.EmployeeID).getEmployeeName();
                string customerName = customerManager.getCustomerByID(sale.CustomerID).getName();

                SaleVM sale2 = new SaleVM(sale, employeeName, customerName);
                saleVMs.Add(sale2);
            }

            ViewBag.Title = "Sales List";

            return View(saleVMs);
        }

        // GET: Sale/Details/5
        public ActionResult Details(int id)
        {
            Sale sale = manager.returnSaleByID(id);

            string employeeName = employeeManager.getEmployeeByEmployeeID(sale.EmployeeID).getEmployeeName();
            string customerName = customerManager.getCustomerByID(sale.CustomerID).getName();
            SaleVM sale2 = new SaleVM(sale, employeeName, customerName);

            ViewBag.Title = "Sale Details";

            return View(sale2);
        }

        // GET: Sale/Create
        public ActionResult Create()
        {
            ViewBag.Customers = customerManager.returnAllCustomers();            
            ViewBag.Title = "New Sale";

            return View();
        }

        // POST: Sale/Create
        [HttpPost]
        public ActionResult Create(Sale sale, string customerName, string vin, 
            string isChecked, string firstName, string lastName, 
            string address, string emailAddress, string phoneNumber)
        {
            try
            {
                if (vin == "")
                {
                    throw new ApplicationException("VIN field cannot be blank");
                }

                if (null != isChecked)
                {
                    if (firstName == "")
                    {
                        throw new ApplicationException("First name cannot be blank");
                    }
                    else if (lastName == "")
                    {
                        throw new ApplicationException("Last name cannot be blank");
                    }
                    else if (address == "")
                    {
                        throw new ApplicationException("Address cannot be blank");
                    }
                    else if (emailAddress == "")
                    {
                        throw new ApplicationException("Email address cannot be blank");
                    }
                    else if (phoneNumber == "")
                    {
                        throw new ApplicationException("Phone number cannot be blank");
                    }
                }

                //Choose whether to add a new customer
                if (null != isChecked)
                {
                    Customer customer = new Customer()
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        EmailAddress = emailAddress,
                        PhoneNumber = phoneNumber
                    };

                    customerManager.addNewCustomer(customer);

                    customerName = firstName + " " + lastName;
                }

                //Set the customer ID and employee ID
                sale.EmployeeID = 1000000;
                sale.CustomerID = customerManager.getCustomerIDByCustomerName(
                    customerName.Split(' ')[0],
                    customerName.Split(' ')[1]
                    );

                //Add a new bill to the database
                Bill bill = new Bill()
                {
                    CustomerID = sale.CustomerID,
                    AmountDue = sale.SaleAmount,
                    AmountPaid = 0M,
                    IssueDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(30),
                    LineItems = new List<BillingLineItem>()
                };
                bill.LineItems.Add(new BillingLineItem()
                {
                    Amount = sale.SaleAmount
                });
                billingManager.newBill(bill);

                //Add the sale to the database
                sale.SaleDate = DateTime.Now;
                sale.BIllingLineItemID = billingManager.getLastestBillingLineItem();
                manager.addNewSale(sale, vin);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Redirect("~/Home/Error?error=" + ex.Message);
            }
        }        
    }
}
