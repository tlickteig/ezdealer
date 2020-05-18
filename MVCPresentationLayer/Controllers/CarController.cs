using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPresentationLayer.Controllers
{
    public class CarController : Controller
    {
        private CarManager _manager;

        public CarController()
        {
            _manager = new CarManager();
        }

        // GET: Car
        public ActionResult Index()
        {
            List<Car> cars = _manager.returnAllCars().Where(x => x.SaleID == 0).ToList();

            ViewBag.Title = "Cars";

            return View(cars);
        }

        // GET: Car/Details/5
        public ActionResult Details(string id)
        {
            Car car = _manager.getCarByVIN(id);            

            ViewBag.Title = "Car Details";

            if (car.TradeInID != 0)
            {
                ViewBag.Title = "Trade In Details";
            }

            return View(car);
        }

        // GET: Car/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Add Car";

            ViewBag.Makes = _manager.returnMakes();

            ViewBag.Types = _manager.returnCarTypes();

            return View();
        }

        // POST: Car/Create
        [HttpPost]
        public ActionResult Create(Car car, string shipmentAmount)
        {
            try
            {
                // TODO: Add insert logic here
                _manager.addCar(car, Convert.ToDecimal(shipmentAmount), 
                    1000000, DateTime.Now);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return Redirect("~/Home/Error?error=" + ex.Message);
            }
        }

        // GET: Car/Create
        public ActionResult CreateT()
        {
            ViewBag.Title = "Add Trade In";

            ViewBag.Makes = _manager.returnMakes();

            ViewBag.Types = _manager.returnCarTypes();

            ViewBag.Destinations = new List<string>()
            {
                "WHOLESALE",
                "RESOLD",
                "SCRAPYARD"
            };

            return View();
        }

        // POST: Car/Create
        [HttpPost]
        public ActionResult CreateT(Car car, string tradeInDestination)
        {
            try
            {
                // TODO: Add insert logic here
                _manager.addTradeIn(car, tradeInDestination,
                    1000000, DateTime.Now);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return Redirect("~/Home/Error?error=" + ex.Message);
            }
        }

        /*
        // GET: Car/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Car/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Car/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Car/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        */
    }
}
