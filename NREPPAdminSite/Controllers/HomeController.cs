using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using DataTables.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NREPPAdminSite.Models;
using NREPPAdminSite.Security;

namespace NREPPAdminSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ExtendedUser> _userManager;

        public HomeController()
        {
            MyIdentityDbContext db = new MyIdentityDbContext();

            UserStore<ExtendedUser> userStore = new UserStore<ExtendedUser>(db);
            _userManager = new UserManager<ExtendedUser>(userStore);

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            //NreppUser usr = ReadCookie(Request);
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Programs", "Home");
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult Reviewers()
        {
            return View();
        }

       [Authorize]
       public ActionResult Reviewer(int? id)
       {
           ViewBag.Message = "Your Reviewer page.";
           NrepServ localService = new NrepServ(NrepServ.ConnString);
           var reviewerList = localService.GetOutComesReviewer(id);
           ReviewerPageModel reviewerPageModel = new ReviewerPageModel();
           //reviewerPageModel.Outcomes = reviewerWrapper;
           return PartialView(reviewerPageModel);
           // return View("Reviewer");
       }
       
       [Authorize]
       public JsonResult ReviewersList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
       {
           NrepServ localService = new NrepServ(NrepServ.ConnString);
           ReviewerSearchResult reviewerSearchResult = localService.GetReviewers(requestModel);
           return Json(new DataTablesResponse(requestModel.Draw, reviewerSearchResult.Reviewers, reviewerSearchResult.TotalSearchCount, reviewerSearchResult.TotalSearchCount), JsonRequestBehavior.AllowGet);
       }
         
        [Authorize]
        public ActionResult Programs()
        {
            ViewBag.Message = "Some Message Here";
            var user = _userManager.FindByName(User.Identity.Name);
            var userRoles = _userManager.GetRoles(user.Id);
            
            NrepServ localService = new NrepServ(NrepServ.ConnString);

            List<Intervention> interventionList = localService.GetInterventions(userRoles[0]);


            return View(interventionList);
        }
    }
}