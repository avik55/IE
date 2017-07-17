using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using IndiaEntertainers.Models;
using IndiaEntertainers.ViewModel;

namespace IndiaEntertainers.Controllers
{
    public class HomeController : Controller
    {
        IEDBContext db = new IEDBContext();
        public ActionResult Index()
        {
            var entertainer = (from i in db.tbl_Entertainer
                               join j in db.tbl_City on i.CityId equals j.CityId
                               join k in db.tbl_EntrImages on i.EntrId equals k.EntrId
                               where k.IsCurProfileImg == true
                               select new TrendingViewModel {
                                   EntrId = i.EntrId,
                                   PerformanceFees = i.Performancefee,
                                   CityName = j.CityName,
                                   ProfilePhoto = k.ImagePath,
                                   Type = i.Type,
                                   FullName = i.FName + " " + i.LName

                               }).ToList();
            ViewBag.Entertainer = entertainer;
            return View();
        }

        [Authorize]
        public ActionResult Entertainer()
        {
            return View();
        }

        [Authorize]
        public ActionResult TalentSeeker()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult LoginSucess()
        {
            var UID = User.Identity.GetUserId();
            if (UID != null)
            {
                if (User.IsInRole("EntertainerBS") || User.IsInRole("EntertainerPRM"))
                    return RedirectToAction("Entertainer");
                else if (User.IsInRole("TalentSeekerBS") || User.IsInRole("TalentSeekerPRM"))
                    return RedirectToAction("TalentSeeker");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Fileupld()
        {
            return View();
        }

        [HttpPost]
        public void upload(System.Web.HttpPostedFileBase aFile)
        {
            string file = aFile.FileName;
            string path = Server.MapPath("../Upload//");
            aFile.SaveAs(path + Guid.NewGuid() + "." + file.Split('.')[1]);
        }
    }
}