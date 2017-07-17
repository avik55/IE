using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using IndiaEntertainers.Models;
using IndiaEntertainers.ViewModel;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Configuration;

namespace IndiaEntertainers.Controllers
{
    public class HomeController : Controller
    {
        IEDBContext db = new IEDBContext();
        private EmailServicess objEmailServicess = new EmailServicess();

        public ActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                var entertainer = (from i in db.tbl_Entertainer
                                   join j in db.tbl_City on i.CityId equals j.CityId
                                   join k in db.tbl_EntrImages on i.EntrId equals k.EntrId
                                   where k.IsCurProfileImg == true && i.IsItTrending == true && i.IsActive == true
                                   select new TrendingViewModel
                                   {
                                       EntrId = i.EntrId,
                                       PerformanceFees = i.Performancefee,
                                       CityName = j.CityName,
                                       ProfilePhoto = k.ImagePath,
                                       Type = i.Type,
                                       Showfee = i.Showfee,
                                       FullName = i.FName,
                                       Slug = i.Slug
                                   }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                ViewBag.Entertainer = entertainer;
                ViewBag.TrendingVideos = db.tbl_EntrVideos.Where(d => d.Name == "HomeTrending").OrderBy(d => new Guid()).Take(8).ToList();
                ViewBag.IsLoggedIn = TempData["IsLoggedIn"];
                return View();
            }
            else
            {
                return RedirectToAction("Filters", "Entertainers", new { area = "", search });
            }


        }

        [Authorize]
        public ActionResult Entertainer()
        {
            var uid = User.Identity.GetUserId();
            var Entr = db.tbl_Entertainer.Where(d => d.UserId == uid).FirstOrDefault();
            if (Entr != null)
            {
                if (Entr.FName == "-" || Entr.Gender == "-" || Entr.Nationality == "-")
                    return RedirectToAction("VerifictnConfm", "Home");
                // return RedirectToAction("ProfileDetail", "MyAccount", new { area = "Entertainer" });
                else
                    return RedirectToAction("Index");
            }
            if (Entr == null)
            {
                // return RedirectToAction("Register", "MyAccount", new { area = "Entertainer" });
                try
                {
                    tbl_Entertainer Entertainer = new tbl_Entertainer();
                    var Userr = db.AspNetUsers.Where(d => d.Id == uid).FirstOrDefault();
                    Entertainer.FName = "-"; Entertainer.UserId = uid;
                    Entertainer.YearsOfPerforming = "-"; Entertainer.Gender = "-";
                    Entertainer.Email = Userr.Email; Entertainer.IntgramPageLink = "#";
                    Entertainer.Mobile = Userr.PhoneNumber;
                    Entertainer.StateId = 0;
                    Entertainer.CityId = 0; Entertainer.RegDate = DateTime.Now.Date;
                    Entertainer.FBPageLink = "#"; Entertainer.Gender = "-";
                    Entertainer.IsActive = true; Entertainer.IsPremium = false;
                    Entertainer.Nationality = "-"; Entertainer.NoofShow = 0;
                    Entertainer.PerfLanguage = "-"; Entertainer.PerfLength = 0;
                    Entertainer.Performancefee = 0; Entertainer.PERFORMINGMEMBERS = "-";
                    Entertainer.Profile = "-"; Entertainer.ProfileTitle = "-";
                    Entertainer.TwiterPageLink = "#"; Entertainer.Type = "-";
                    Entertainer.IsProfileCompleted = false;

                    db.tbl_Entertainer.Add(Entertainer);
                    db.SaveChanges();
                    //return RedirectToAction("ProfileDetail", "MyAccount", new { area = "Entertainer" });
                    return RedirectToAction("VerifictnConfm", "Home");
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            return View();
        }

        [Authorize]
        public ActionResult TalentSeeker()
        { return View(); }

        public ActionResult AboutUs()
        { return View(); }

        public ActionResult FAQ()
        { return View(); }

        public ActionResult LoginSucess()
        {
            var UID = User.Identity.GetUserId();
            if (UID != null)
            {
                if (User.IsInRole("EntertainerBS") || User.IsInRole("EntertainerPRM"))
                    return RedirectToAction("Entertainer");
                else if (User.IsInRole("TalentSeekerBS") || User.IsInRole("TalentSeekerPRM"))
                    return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //Search on Home Page
        public JsonResult Search(string Qry)
        {
            try
            {
                if (String.IsNullOrEmpty(Qry))
                    return Json("-", JsonRequestBehavior.AllowGet);
                var Cats = db.tbl_Category.Where(d => d.Title.ToLower().Contains(Qry.ToLower())).ToList();
                if (Cats != null && Cats.Count() > 0)
                {
                    var catsss = (from ct in Cats
                                  select new SearchModel
                                  {
                                      Name = ct.Title,
                                      EntrId = ct.CatId,
                                      Id = ct.CatId,
                                      IsItCat = true,
                                      URL = "/Entertainers/" + ct.Title
                                  }
                                ).ToList();

                    if (catsss.Count() == 1)
                    {
                        long CID = catsss.FirstOrDefault().Id;
                        var Entetainers = (from en in db.tbl_Entertainer
                                           join cat in db.tbl_EntertainerCats
                                           on en.EntrId equals cat.EntrId
                                           join ct in db.tbl_City on en.CityId equals ct.CityId
                                           where cat.CatId == CID && en.IsActive == true && en.IsProfileCompleted == true
                                           select new SearchModel
                                           {
                                               Name = en.FName + " (" + cat.tbl_Category.Title + "/" + ct.CityName + ")",
                                               EntrId = en.EntrId,
                                               Id = en.EntrId,
                                               IsItCat = false,
                                               URL = "/Entertainers/" + cat.tbl_Category.Title + "/" + en.Slug
                                           }
                                ).ToList();
                        catsss.AddRange(Entetainers);
                    }
                    return Json(catsss, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Entetainers = (from en in db.tbl_Entertainer.Where(d => d.FName.ToLower().Contains(Qry.ToLower()))
                                       join cat in db.tbl_EntertainerCats on en.EntrId equals cat.EntrId
                                       join ct in db.tbl_City on en.CityId equals ct.CityId
                                       where en.IsActive == true && en.IsProfileCompleted == true
                                       select new SearchModel
                                       {
                                           Name = en.FName + " (" + cat.tbl_Category.Title + "/" + ct.CityName + ")",
                                           EntrId = en.EntrId,
                                           Id = en.EntrId,
                                           IsItCat = false,
                                           URL = "/Entertainers/" + cat.tbl_Category.Title + "/" + en.Slug
                                       }
                                 ).ToList();
                    return Json(Entetainers, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { throw; }
        }

        public JsonResult SendMessagetoIE(tbl_EntnMessages ESMS)
        {
            try
            {
                EmailServicess objEmailServicess = new EmailServicess();
                EmailMessageModel msg = new EmailMessageModel();
                msg.Destination = "info@indiaentertainers.com";
                tbl_ContactUs css = new tbl_ContactUs();
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                        css.IPAddress = ip.ToString();
                }
                css.IsRead = false; css.Message = ESMS.Message;
                css.ReceivedDateTime = DateTime.Now.Date; css.Status = "New";
                var userid = User.Identity.GetUserId();
                if (userid != null)
                {
                    css.SenderUserId = userid;
                    if (User.IsInRole("EntertainerBS"))
                    {
                        var entrnnn = db.tbl_Entertainer.Where(d => d.UserId == userid).FirstOrDefault();
                        css.ContactNo = entrnnn.Mobile; css.EmailID = entrnnn.Email; css.Name = entrnnn.FName;
                        var msgg = "Name : " + entrnnn.FName + "<br /> Email : " + entrnnn.Email + "<br /> Contact No. : " + entrnnn.Mobile + "<br /> Message : " + ESMS.Message;
                        var filee = Server.MapPath("~/Content/MailTemplate.html");
                        string text = System.IO.File.ReadAllText(filee);
                        text = text.Replace("MailMessage", msgg);
                        msg.Body = text;
                        msg.Subject = entrnnn.FName + " Contacted to IndiaEntertainers.com";
                    }
                    else if (User.IsInRole("TalentSeekerBS"))
                    {
                        var ts = db.tbl_TalentSeeker.Where(d => d.UserId == userid).FirstOrDefault();
                        css.ContactNo = ts.Mobile;
                        css.EmailID = ts.Email;
                        css.Name = ts.FName;
                        var msgg = "Name : " + ts.FName + "<br /> Email : " + ts.Email + "<br /> Contact No. : " + ts.Mobile + "<br /> Message : " + ESMS.Message;
                        var filee = Server.MapPath("~/Content/MailTemplate.html");
                        string text = System.IO.File.ReadAllText(filee);
                        text = text.Replace("MailMessage", msgg);
                        msg.Body = text;
                        msg.Subject = ts.FName + " Contacted to IndiaEntertainers.com";
                    }
                    var fff = objEmailServicess.SendByGmail(msg);
                    db.tbl_ContactUs.Add(css);
                    db.SaveChanges();
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        tbl_ContactUs cs = new tbl_ContactUs();
                        var host1 = Dns.GetHostEntry(Dns.GetHostName());
                        foreach (var ip in host1.AddressList)
                        {
                            if (ip.AddressFamily == AddressFamily.InterNetwork)
                                cs.IPAddress = ip.ToString();
                        }
                        cs.ContactNo = ESMS.ContactNo;
                        cs.EmailID = ESMS.EmailID;
                        cs.IsRead = false;
                        cs.Message = ESMS.Message;
                        cs.Name = ESMS.Name;
                        cs.ReceivedDateTime = DateTime.Now.Date;
                        cs.SenderUserId = "";
                        cs.Status = "New";
                        var msgg = "Name : " + ESMS.Name + "<br /> Email : " + ESMS.EmailID + "<br /> Contact No. : " + ESMS.ContactNo + "<br /> Message : " + ESMS.Message;
                        var filee = Server.MapPath("~/Content/MailTemplate.html");
                        string text = System.IO.File.ReadAllText(filee);
                        text = text.Replace("MailMessage", msgg);
                        msg.Body = text;
                        msg.Subject = ESMS.Name + " Contacted to IndiaEntertainers.com";
                        var fff = objEmailServicess.SendByGmail(msg);
                        db.tbl_ContactUs.Add(cs);
                        db.SaveChanges();
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult MyTypeHeadExNew()
        { return View(); }

        public JsonResult Subscribe(string EmailId)
        {
            try
            {
                if (!String.IsNullOrEmpty(EmailId))
                {
                    var chkduplicate = db.tbl_Subscribers.Where(d => d.EmailId == EmailId.Trim()).FirstOrDefault();
                    if (chkduplicate == null)
                    {
                        tbl_Subscribers NewSubcriber = new tbl_Subscribers();
                        var host = Dns.GetHostEntry(Dns.GetHostName());
                        foreach (var ip in host.AddressList)
                        {
                            if (ip.AddressFamily == AddressFamily.InterNetwork)
                                NewSubcriber.IPAddress = ip.ToString();
                        }
                        NewSubcriber.EmailId = EmailId.Trim();
                        NewSubcriber.IsActive = true;
                        NewSubcriber.Subscribedate = DateTime.Now.Date;
                        db.tbl_Subscribers.Add(NewSubcriber);
                        db.SaveChanges();

                        ///Send Email about subscriber
                        EmailMessageModel msg = new EmailMessageModel();
                        var receiverEmailID = ConfigurationManager.AppSettings["subscribeInfoReceiverEmailID"];
                        msg.Destination = receiverEmailID;
                        var filee = Server.MapPath("~/Content/SubscriberInfoTemplate.html");
                        string text = System.IO.File.ReadAllText(filee);
                        text = text.Replace("#$", EmailId.Trim());
                        msg.Body = text;
                        msg.Subject = "Subscriber Info.";
                        var emailSent = objEmailServicess.SendByGmail(msg);


                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                    return Json("Duplicate", JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("Empty", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }


        //Fetch asyn trending performances
        //Filters result in json
        public JsonResult GetEntertaines()
        {
            try
            {
                List<SearchModel> Entertainers = new List<SearchModel>();
                Entertainers = (from en in db.tbl_Entertainer
                                join ct in db.tbl_City on en.CityId equals ct.CityId
                                join k in db.tbl_EntrImages on en.EntrId equals k.EntrId
                                join cat in db.tbl_EntertainerCats on en.EntrId equals cat.EntrId
                                where k.IsCurProfileImg == true && en.IsItTrending == true
                                orderby en.Star descending
                                select new SearchModel
                                {
                                    Category = cat.tbl_Category.Title.Substring(0, (cat.tbl_Category.Title.Length - 1)),
                                    Name = en.FName,
                                    City = ct.CityName,
                                    EntrId = en.EntrId,
                                    Id = en.EntrId,
                                    Showfee = en.Showfee,
                                    fee = en.Performancefee,
                                    URL = "/Entertainers/ProfileDetail/" + en.EntrId,
                                    imgurl = "../images/UserImages/" + k.ImagePath
                                }).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                return Json(Entertainers, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            { throw; }
        }

        public ActionResult HCYP()
        { return View(); }

        //Verification Complete
        public ActionResult VerifictnConfm()
        { return View(); }

        public ActionResult TnC()
        { return View(); }


        public ActionResult GC()
        {
            EmailServicess ES = new EmailServicess();
            EmailMessageModel EMM = new EmailMessageModel();

            EMM.Body = "<h1>Pal</h1>";
            EMM.Subject = "Testing";
            EMM.Destination = "contact@mokshasolutions.com";
            var fff = ES.SendByGmail(EMM);

            return View();
        }

        public ActionResult Register()
        {
            var uid = User.Identity.GetUserId();
            var entr = db.tbl_Entertainer.Where(d => d.UserId == uid).FirstOrDefault();
            if (entr != null)
            {
                TempData["IsLoggedIn"] = true;
                return RedirectToAction("Index");
            }
            ViewBag.Cities = db.tbl_City.Where(d => d.StateId == 0).Select(x => new SelectListItem { Text = x.CityName, Value = x.CityId.ToString() });
            ViewBag.uid = uid; ViewBag.StateId = "0"; ViewBag.CityId = "0"; ViewBag.CatId = "0";
            ViewBag.PerfMembers = ""; ViewBag.PerfLanguage = ""; ViewBag.Gender = ""; ViewBag.YearsOfPerforming = ""; ViewBag.NoofShow = ""; ViewBag.PerfLength = "0";
            return View();
        }

        public ActionResult RegisterTalentSeekar()
        {
            var uid = User.Identity.GetUserId();
            var entr = db.tbl_Entertainer.Where(d => d.UserId == uid).FirstOrDefault();
            //if (entr != null) return RedirectToAction("ProfileDetail");
            ViewBag.Cities = db.tbl_City.Where(d => d.StateId == 0).Select(x => new SelectListItem { Text = x.CityName, Value = x.CityId.ToString() });
            ViewBag.uid = uid; ViewBag.StateId = "0"; ViewBag.CityId = "0"; ViewBag.CatId = "0";
            ViewBag.PerfMembers = ""; ViewBag.PerfLanguage = ""; ViewBag.Gender = ""; ViewBag.YearsOfPerforming = ""; ViewBag.NoofShow = ""; ViewBag.PerfLength = "0";
            return View();
        }

    }
}

//Temporary Function for add slug to the Entertainer table
//public ActionResult AddSlugs()
//{
//    try
//    {
//        Regex re = new Regex("[;\\\\/:*?\"<>|&']");
//        var entertainers = db.tbl_Entertainer.Where(d => d.EntrId == 27).ToList();
//        foreach (var item in entertainers)
//        {
//            if (item.FName == "-" || item.FName == null) continue;
//            string slug = item.FName.Replace(" ", "-").ToLower(); slug = re.Replace(slug, "");
//            var chkduplicate = db.tbl_Entertainer.Where(d => d.Slug == slug).FirstOrDefault();
//            if (chkduplicate != null)
//            {
//                bool flag = true; int i = 1; slug = slug + "-";
//                while (flag)
//                {
//                    string slugg = slug + i.ToString();
//                    var chkduplicate1 = db.tbl_Entertainer.Where(d => d.Slug == slugg).FirstOrDefault();
//                    if (chkduplicate1 == null) { flag = false; slug = slugg; }
//                }
//            }
//            item.Slug = slug; db.SaveChanges();
//        }
//        return View();
//    }
//    catch (Exception){throw;}
//}