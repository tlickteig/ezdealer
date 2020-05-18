using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPresentationLayer.Controllers
{
    public class BillingController : Controller
    {
        private BillManager billingManager = new BillManager();        

        private CustomerManager customerManager = new CustomerManager();

        // GET: Billing
        public ActionResult Index()
        {
            List<Bill> bills = billingManager.getAllBills();
            List<BillVM> bills2 = new List<BillVM>();

            foreach (Bill bill in bills)
            {
                string customerName = customerManager.getCustomerByID(bill.CustomerID).getName();
                bills2.Add(new BillVM(bill, customerName));                
            }

            ViewBag.Title = "Billing List";

            return View(bills2);
        }

        // GET: Billing/Details/5
        public ActionResult Details(string id)
        {
            Bill bill = billingManager.returnBill(Convert.ToInt32(id));

            string customerName = customerManager.getCustomerByID(bill.CustomerID).getName();

            ViewBag.Title = "Bill Details";

            ViewBag.CustomerName = customerName;

            return View(bill);
        }        
    }
}
