using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
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

       [AllowAnonymous]
        public ActionResult Reviewers()
        {
            ViewBag.Message = "Your Reviewers page.";
           //NrepServ localService = new NrepServ(NrepServ.ConnString);
          // ReviewersWrapper reviewersWrapper = localService.GetOutComesReviewer(null);
           //ReviewersPageModel reviewersPageModel = new ReviewersPageModel();
           //reviewersPageModel.Outcomes = reviewersWrapper;
           return View();//reviewersPageModel);
        }

        [AllowAnonymous]
       public ActionResult Reviewer(int? id)
       {
           ViewBag.Message = "Your Reviewer page.";
           NrepServ localService = new NrepServ(NrepServ.ConnString);
           ReviewerWrapper reviewerWrapper = localService.GetOutComesReviewer(id);
           ReviewerPageModel reviewerPageModel = new ReviewerPageModel();
           reviewerPageModel.Outcomes = reviewerWrapper;
           return PartialView(reviewerPageModel);
          // return View("Reviewer");
       }

       [AcceptVerbs(HttpVerbs.Post)]
       public JsonResult ReviewersList()
       {
           NrepServ localService = new NrepServ(NrepServ.ConnString);
           List<Reviewer> reviewers = localService.GetReviewers();
           return Json(reviewers);
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