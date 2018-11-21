using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using svihire.Contexts;
using svihire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace svihire.Controllers
{
    public class DashboardController : Controller
    {
        public string GetManagerID()
        {
            try
            {
                return User.Identity.GetUserId();
            }
            catch (Exception ex)
            {
                RedirectToAction("../Account/Login");
                return null;
            }
        }

        public string GetAccountId()
        {
            try
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId()).AccountId;
            }
            catch (Exception ex)
            {
                RedirectToAction("../Account/Login");
                return null;
            }
        }

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

            PrepareFilters();
            var model = GetDashboardViewModel();
            return View(model);
        }

        public JsonResult AddCandidateAsync()
        {
            return new JsonResult();
        }

        public JsonResult CheckIfCandidateHasData()
        {
            return new JsonResult();
        }

        private DashboardViewModel GetDashboardViewModel()
        {
            var model = new DashboardViewModel();
            var whatsUp = new List<InviteViewModel>();
            using (var db = new DashboardContext(GetAccountId()))
            {
                model.Positions = db.Surveys;
                model.Managers = db.Managers;
                //model.Invites = db.Invites;
                model.User = db.GetUser(User.Identity.GetUserId());
                model.Invites = db.GetAccountInvites();
            }
            return model;
        }

        public async Task<ActionResult> CheckCandidateEmail(string surveyID, string email)
        {
            var existingInvites = new List<ExistingSurveyData>();
            JavaScriptSerializer TheSerializer = new JavaScriptSerializer();

            using (var db = new DashboardContext(GetAccountId()))
            {
                if(!String.IsNullOrEmpty(email))
                {
                    existingInvites = db.CheckIfCandidateHasCompletedByEmail(surveyID, email); 
                    if(existingInvites != null && existingInvites.Count > 0)
                    {
                        return PartialView("_ExistingData", existingInvites);
                    }
                }           
            }

            var noData = new EmptyResult();
            return noData;
        }

        private void PrepareFilters()
        {
            //async all the get stuff
        }

        private void GetCandidates()
        {
            var managerID = GetManagerID();
            //get candidates from thing

        }

        private void GetHiringManagers()
        {
            //get all managers in this account
        }

        private void GetPositions()
        {
            //get all surveys for this account
        }

        private void GetStatuses()
        {
            //enum of all available status? 
        }



    }
}