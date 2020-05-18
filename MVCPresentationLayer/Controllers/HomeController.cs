using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Welcome to EZDealer!";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "EZDealer Management Software";

            ViewBag.Title = "About";

            return View();
        }

        [HttpGet]
        public ActionResult Error(string error)
        {            
            ViewBag.Error = error;

            return View();
        }
    }
}