using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using svihire.Models;
using svihire.Contexts;

using WebSupergoo.ABCpdf11;
using WebSupergoo.ABCpdf11.Objects;
using WebSupergoo.ABCpdf11.Atoms;
using WebSupergoo.ABCpdf11.Operations;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Web.Script.Serialization;
using System.Globalization;
using System.IO;

namespace svihire.Controllers
{
    public class SurveyController : Controller
    {
        // GET: Survey
        public ActionResult Index(TakeSurveyViewModel model)
        {
            if (model == null)
            {
                return RedirectToAction("../Account/Login");
            }
            else
            {
                return View(model);
            }

        }

        public ActionResult TakeSurvey(Guid? id)
        {
            Invite invite = new Invite();
            using (var dbInvite = new InviteContext())
            {
                invite = dbInvite._Invites.Find(id);
                if (invite != null)
                {
                    var existingData = dbInvite.CheckIfCandidateHasCompletedByID(invite.SurveyId, invite.CandidateId);
                    if (existingData != null && existingData.Count > 0)
                    {
                        //check survey && candidate to see if they have taken it. If they have, send them to a page
                        return View("SurveyOnFile");
                    }
                    else
                    {
                        using (var dbSurvey = new SurveyContext(invite.AccountId))
                        {
                            //build our ViewModel
                            var survey = dbSurvey._Surveys.Find(invite.SurveyId);
                            var model = new TakeSurveyViewModel();
                            //model.surveyItems = dbInvite.LoadSurveyItems(invite.SurveyId);

                            var questions = dbSurvey.Questions
                            .Where(e => e.SurveyId == invite.SurveyId)
                            .Select(q => new SurveyQuestionViewModel()
                            {
                                Id = q.Id,
                                Question = q.QuestionText,
                                PossibleResponses = dbSurvey.QuestionResponses
                                   .OrderBy(e => e.Order)
                                   .Where(e => e.ResponseSetId == q.ResponseSetId)
                                   .Select(r => new SurveyResponseViewModel { Id = r.Id, Text = r.ResponseText })
                                   .ToList()
                            }).ToList();

                            model = new TakeSurveyViewModel()
                            {
                                SurveyTitle = survey.SurveyName,
                                SurveyId = survey.Id,
                                QuestionsViewModel = questions.ToList(),
                                Candidate = dbSurvey.GetCandidateByInviteID(invite.Id)
                            };

                            if (model.Candidate != null && !String.IsNullOrEmpty(model.SurveyTitle) && model.QuestionsViewModel.Count > 0)
                            {
                                return View("Index", model);
                            }
                            else
                            {
                                return View(); //default - say that's not a valid invite
                            }
                        }

                    }

                }
            }
            return View(); //default - say that's not a valid invite
        }

        public ActionResult SurveyReport()
        {
            
            return View();
        }

        [HttpGet]
        public ActionResult GetSurveys()
        {
            List<Survey> surveys = new List<Survey>();
            using (var db = new DashboardContext(GetAccountId()))
            {
                surveys = db.Surveys;
            };

            return Content(new JavaScriptSerializer().Serialize(surveys));
        }

        [HttpGet]
        public ActionResult GetCandidates(string id)
        {
            List<Candidate> candidates = new List<Candidate>();
            using (var db = new SurveyContext(new Guid(GetAccountId())))
            {
                candidates = db.Candidates;
            };

            return Content(new JavaScriptSerializer().Serialize(candidates));
        }

        [HttpGet]
        public ActionResult GetRecords(string id, string candidateId)
        {
            List<Guid> candidates = new List<Guid>();
            using (var db = new ResponseContext(GetAccountId()))
            {
                if (!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(candidateId))
                {
                    candidates = db.ResponseByCandidateIdSurveyId(id, candidateId);
                }
            };

            return Content(new JavaScriptSerializer().Serialize(candidates));
        }



        [HttpGet]
        public ActionResult Preview(string id)
        {
            ViewBag.Message = "Your PDF page.";
            var model = new SurveyReportViewModel();

            if (!string.IsNullOrWhiteSpace(id))
            {
                
                var accountId = GetAccountId();
                // id = "00000000-0000-0000-0000-000000000000";
                Guid guidAccount = new Guid(accountId);
                Guid recordId = new Guid(id);

                using (var dbRes = new ResponseContext(accountId))
                {
                    //build our ViewModel
                    // var survey = dbSurvey._Surveys.Find(invite.SurveyId);
                    var responses = new List<Response>();
                    //model.surveyItems = dbInvite.LoadSurveyItems(invite.SurveyId);

                    responses = dbRes.ResponseByRecordId(recordId);

                    if (responses != null && responses.Count > 0)
                    {
                        using (var dbSurvey = new SurveyContext(guidAccount))
                        {
                            var serveyId = responses[0].SurveyId;
                            var questions = dbSurvey.Questions
                            .Where(e => e.SurveyId == serveyId)
                            .Select(q => new ResponseQuestionViewModel()
                            {
                                Id = q.Id,
                                Question = q.QuestionText,
                                PossibleResponses = dbSurvey.QuestionResponses
                                    .OrderBy(e => e.Order)
                                    .Where(e => e.ResponseSetId == q.ResponseSetId)
                                    .Select(r => new SurveyResponseViewModel { Id = r.Id, Text = r.ResponseText, ResponseScore = r.ResponseScore })
                                    .ToList(),
                                QuestionRange =
                                ((dbSurvey.QuestionRenges
                                    .Where(e => e.QuestionId == q.Id).ToList().Count > 0) ?
                                     dbSurvey.QuestionRenges
                                    .Where(e => e.QuestionId == q.Id)
                                    .Select(r => new QuestionRangesViewModel
                                    {
                                        QuestionId = r.Id,
                                        RangeMax = r.RangeMax
                                    ,
                                        RangeMin = r.RangeMin
                                    }).FirstOrDefault()
                                     : null)


                            }).ToList();

                            foreach (var item in questions)
                            {
                                item.QuestionResponse =
                              ((responses.Where(w => w.QuestionId == item.Id).Count() > 0) ?
                             responses.Where(w => w.QuestionId == item.Id).First()
                              : null);
                            }

                            model.DateOfCompletion = responses[0].ModifiedDate;
                            model.Candidate = dbSurvey.GetCandidateByID(responses[0].CandidateId);
                            model.categories = dbSurvey.Categories.Where(w => w.AccountId == guidAccount).Select(q => new ResponseCategoryViewModel()
                            {
                                Id = q.Id,
                                CategoryName = q.CategoryName,
                                AccountId = q.AccountId,
                                CategoryQuestions = dbSurvey.CategoryQuestions.Where(e => e.CategoryId == q.Id).Select(x => new ResponseCategoryQuestionsViewModel()
                                {
                                    Id = x.Id,
                                    CategoryId = x.CategoryId,
                                    AccountId = x.AccountId,
                                    QuestionId = x.QuestionId
                                }).ToList()


                            }).ToList();

                            var totalCountWithinRange = 0;
                            var totalCatQuestion = 0;
                            foreach (var item in model.categories)
                            {
                                foreach (var q in item.CategoryQuestions)
                                {
                                    var range = questions.Where(w => w.Id == q.QuestionId).FirstOrDefault();
                                    var res = responses.Where(z => z.QuestionId == q.QuestionId).FirstOrDefault();
                                    if (range != null && range.QuestionRange != null)
                                    {
                                        q.isInRange = res != null &&
                                        range.QuestionRange.RangeMin < res.ResponseScore &&
                                        range.QuestionRange.RangeMax > res.ResponseScore;
                                    }
                                    else
                                    {
                                        q.isInRange = true;
                                    }

                                }

                                item.CountWithInRange = item.CategoryQuestions.Where(w => w.isInRange).Count();
                                totalCountWithinRange += item.CountWithInRange;
                                totalCatQuestion += item.CategoryQuestions.Count();
                            }
                            model.percentMatch = totalCountWithinRange * 100 / totalCatQuestion;
                            model.QuestionsViewModel = questions;
                            model.recordId = recordId;
                            model.SurveyId = responses[0].SurveyId;
                            model.SurveyTitle = ((dbSurvey.Surveys.Where(w => w.Id == responses[0].SurveyId).Count() > 0) ?
                                dbSurvey.Surveys.Where(w => w.Id == responses[0].SurveyId).First().SurveyName : null);

                        }
                    }
                }
              
            }
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult CreatePDF(string id, string data)
        {
            Doc doc = new Doc();
            doc.HtmlOptions.Paged = true;
            doc.HtmlOptions.Timeout = 1000000;
            doc.HtmlOptions.DoMarkup = true;
            doc.HtmlOptions.Engine = EngineType.Chrome;
            doc.HtmlOptions.UseScript = true; // enable JavaScript
            // doc.HtmlOptions.Media = MediaType.Print; // Or Screen for a more screen oriented output
            //doc.HtmlOptions.InitialWidth = 1024; // In case we have a responsive site which is non-specific on good widths
            //doc.HtmlOptions.BrowserWidth = 1024;
            //doc.Width = 1024;
            //// Add html to Doc
            doc.HtmlOptions.PageLoadMethod =
            PageLoadMethodType.WebBrowserNavigate;
            int theID = doc.AddImageHtml(id);

            // Loop through document to create multi-page PDF
            while (true)
            {
                if (!doc.Chainable(theID))
                    break;
                doc.Page = doc.AddPage();
                theID = doc.AddImageToChain(theID);
            }

            // Flatten the PDF
            for (int i = 1; i <= doc.PageCount; i++)
            {
                doc.PageNumber = i;
                doc.Flatten();
            }
            //Response.Clear();

            //Response.Cache.SetCacheability(HttpCacheability.Private);
            //Response.AddHeader("Content-Disposition", "attachment; filename=GeneratedPDF_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf");
            //Response.ContentType = "application/octet-stream";

            //    doc.Save(Response.OutputStream);
            string name = "pagedhtml_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
            string x = System.Web.HttpContext.Current.Server.MapPath("~/pdf/" + name);
            doc.Save(x);
            //Response.Flush();
            return Json(new { fileName = name, errorMessage = "" });
        }

        [DeleteFileAttribute]
        public FileResult GetPDF(string id)
        {
            return File(System.Web.HttpContext.Current.Server.MapPath("~/pdf/" + id), System.Net.Mime.MediaTypeNames.Application.Pdf);
        }

        public string GetAccountId()
        {
            try
            {
                //return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId()).AccountId;
                return "17C89406-1B21-43A3-A49D-F07F3666B317";
            }
            catch (Exception ex)
            {
                RedirectToAction("../Account/Login");
                return null;
            }
        }
    }


    public static class Utilities
    {
        public static string getClsssBasedOnScore(int score)
        {
            string responseClass = "";
            switch (score)
            {
                case 1:
                    responseClass = " never text-left ";
                    break;
                case 2:
                    responseClass = " rarely text-center ";
                    break;

                case 3:
                    responseClass = "sometimes text-center ";
                    break;
                case 4:
                    responseClass = "often text-center ";
                    break;
                case 5:
                    responseClass = "always text-right";
                    break;
                default:
                    responseClass = "text-left";
                    break;
            }
            return responseClass;
        }


        public static string getCalcWidthForHighlight(decimal minRange, decimal maxRange)
        {
            decimal endGap = 5 - maxRange;

            
            return "calc(100% - "+ ((minRange + endGap - 1) * 24).ToString()  + "%)";
        }

        public static string FormatDate(string date)
        {
            DateTime dt = DateTime.Parse(date);

            return dt.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
        }
    }
    public class DeleteFileAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Flush();
            string filePath = (filterContext.Result as FilePathResult).FileName;
            File.Delete(filePath);
        }
    }
}