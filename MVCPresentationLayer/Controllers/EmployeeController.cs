using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPresentationLayer.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            EmployeeManager manager = new EmployeeManager();

            List<Employee> employees = manager.returnAllEmployees();

            ViewBag.Title = "Employee List";

            return View(employees);
        }

        // GET: Employee/Details/5
        public ActionResult Details(string id)
        {
            EmployeeManager manager = new EmployeeManager();

            var employee = manager.getEmployeeByEmployeeID(Convert.ToInt32(id));

            ViewBag.Title = "Employee Details";

            ViewBag.BirthDate = employee.BirthDate.ToShortDateString();

            ViewBag.StartDate = employee.StartDate.ToShortDateString();

            ViewBag.EndDate = employee.EndDate.ToShortDateString();

            return View(employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            ViewBag.Roles = new List<string>()
            {
                "MANAGER",
                "SALESMAN",
                "TECHNICIAN",
                "ADMIN"
            };

            ViewBag.Title = "New Employee";

            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(Employee employee, string password)
        {
            try
            {
                // TODO: Add insert logic here
                EmployeeManager manager = new EmployeeManager();

                employee.Password = password.ToCharArray();

                employee.StartDate = DateTime.Now;                

                manager.newEmployee(employee);                

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(string id)
        {
            EmployeeManager manager = new EmployeeManager();            

            Employee employee = manager.getEmployeeByEmployeeID(Convert.ToInt32(id));

            ViewBag.Roles = new List<string>()
            {
                "MANAGER",
                "SALESMAN",
                "TECHNICIAN",
                "ADMIN"
            };

            ViewBag.Title = "Edit Employee";

            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add update logic here
                    EmployeeManager manager = new EmployeeManager();

                    manager.updateEmployee(employee);

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                // TODO: Add delete logic here
                EmployeeManager manager = new EmployeeManager();

                manager.deleteEmployee(Convert.ToInt32(id));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            return View();
        }
    }
}
