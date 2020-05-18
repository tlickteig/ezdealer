using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPresentationLayer.Controllers
{    
    public class RepairController : Controller
    {
        private PartsManager _partsManager = new PartsManager();

        private RepairManager _repairsManager = new RepairManager();

        private CustomerManager _customerManager = new CustomerManager();

        private BillManager _billingManager = new BillManager();

        private EmployeeManager _employeeManager = new EmployeeManager();

        // GET: Repair
        public ActionResult Index()
        {
            List<PartVM> parts = _partsManager.getAllParts();
            parts = parts.Where(x => x.RepairLineItemID == 0).ToList();

            ViewBag.Title = "Repairs";
            ViewBag.Parts = parts;

            List<Repair> repairs = _repairsManager.getAllRepairs();

            return View(repairs);
        }

        // GET: Repair/Details/5
        public ActionResult Details(string id)
        {
            ViewBag.Title = "Part Details";

            List<PartVM> parts = _partsManager.getAllParts();
            PartVM part = null;

            foreach (PartVM part2 in parts)
            {
                if (part2.SerialNumber == id)
                {
                    part = part2;
                }
            }

            return View(part);
        }

        // GET: Repair/Create
        public ActionResult Create()
        {
            ViewBag.Title = "New Part";
            ViewBag.PartTypes = _partsManager.returnAllPartTypes();

            return View();
        }

        // POST: Repair/Create
        [HttpPost]
        public ActionResult Create(Part part)
        {
            try
            {
                // TODO: Add insert logic here
                _partsManager.addNewPart(part);                

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                String error = ex.Message;

                error = error.Replace('\n', ' ');

                return Redirect("~/Home/Error?error=" + error);
            }
        }

        // POST: Repair/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                // TODO: Add delete logic here
                _partsManager.deletePart(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Redirect("~/Home/Error?error=" + ex.Message);
            }
        }

        public ActionResult CreateRepair()
        {
            ViewBag.Title = "New Repair";
            ViewBag.Customer = _customerManager.returnAllCustomers();
            ViewBag.Parts = _partsManager.getAllParts();

            return View();
        }

        [HttpPost]
        public ActionResult CreateRepair(Repair repair, string customerName, string vin, 
            string isChecked, string firstName, string lastName, 
            string address, string emailAddress, string phoneNumber)
        {
            try
            {
                if (null != isChecked)
                {
                    if (firstName == "")
                    {
                        throw new ApplicationException("First name field cannot be blank");
                    }
                    else if (lastName == "")
                    {
                        throw new ApplicationException("Last name field cannot be blank");
                    }
                    else if (address == "")
                    {
                        throw new ApplicationException("Address field cannot be blank");
                    }
                    else if (emailAddress == "")
                    {
                        throw new ApplicationException("Email address field cannot be blank");
                    }
                    else if (phoneNumber == "")
                    {
                        throw new ApplicationException("Phone number field cannot be blank");
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

                    _customerManager.addNewCustomer(customer);

                    customerName = firstName + " " + lastName;
                }

                //Set the customer ID and employee ID
                repair.EmployeeID = 1000000;
                repair.CustomerID = _customerManager.getCustomerIDByCustomerName(
                    customerName.Split(' ')[0],
                    customerName.Split(' ')[1]
                );                

                //Add a new sale
                repair.Date = DateTime.Now;
                repair.BillingLineItemID = 0;

                //Save the sale and the bill to the current session                
                Session["repair"] = repair;

                //Redirect to the line items screen
                ViewBag.Title = "Line Items";
                ViewBag.Parts = _partsManager.getAllParts();
                return RedirectToAction("CreateLineItems");
            }
            catch (Exception ex)
            {
                return Redirect("~/Home/Error?error=" + ex.Message);
            }
        }

        public ActionResult CreateLineItems()
        {
            List<PartVM> parts = _partsManager.getAllParts().Where(x => x.RepairLineItemID == 0).ToList();

            ViewBag.Title = "Add Line Items";

            return View(parts);
        }

        [HttpGet]
        public ActionResult CreateLineItems2(string data)
        {
            try
            {
                //Declare variables
                Repair repair = (Repair)Session["repair"];
                List<string> serialNumbers = data.Split(',').ToList();
                List<PartVM> parts = _partsManager.getAllParts();
            
                //Add line items to the repair
                foreach (string serialNumber in serialNumbers)
                {
                    Part part = parts.Find(x => x.SerialNumber == serialNumber);
                    repair.LineItems.Add(new RepairLineItem()
                    {
                        SerialNumber = part.SerialNumber,
                        Amount = part.Cost,
                        Type = part.PartType
                    });
                }

                //Create a new bill
                Bill bill = new Bill()
                {
                    CustomerID = repair.CustomerID,
                    AmountDue = repair.getAmount(),
                    AmountPaid = 0M,
                    IssueDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(30),
                    LineItems = new List<BillingLineItem>()
                };
                bill.LineItems.Add(new BillingLineItem()
                {
                    Amount = repair.getAmount()
                });

                //Add everything to the database
                _billingManager.newBill(bill);
                repair.BillingLineItemID = _billingManager.getLastestBillingLineItem();
                _repairsManager.addNewRepair(repair);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Redirect("~/Home/Error?error=" + ex.Message);
            }
        }

        public ActionResult RepairDetails(int id)
        {
            Repair repair = _repairsManager.getAllRepairs().Find(x => x.RepairID == id);

            ViewBag.Title = "Repair Details";
            ViewBag.CustomerName = _customerManager.getCustomerByID(repair.CustomerID).getName();
            ViewBag.EmployeeName = _employeeManager.getEmployeeByEmployeeID(repair.EmployeeID).getEmployeeName();

            return View(repair);
        }
    }
}
