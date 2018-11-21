using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSupergoo.ABCpdf11;
using WebSupergoo.ABCpdf11.Objects;
using WebSupergoo.ABCpdf11.Atoms;
using WebSupergoo.ABCpdf11.Operations;

namespace svihire.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            string accountID = null;
            try
            {
                accountID = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId()).AccountId;
            }
            catch (Exception ex)
            {
                return RedirectToAction("../Account/Login");
            }

            return RedirectToAction("../Dashboard/Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
    }
}
