using IndiaEntertainers.Models;
using IndiaEntertainers.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace IndiaEntertainers.Controllers
{
    public class EntrCustomController : Controller
    {
        IEDBContext db = new IEDBContext();
        EmailServicess objEmailServicess = new EmailServicess();

        public string GetYoutubeParameter(string url)
        {
            const string pattern = @"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/)([\w\-]+))(?:[^\s?]+)?)";
            const string replacement = "https://www.youtube.com/embed/$1";

            var rgx = new Regex(pattern);
            var result = rgx.Replace(url, replacement);
            return result;
        }

        public ActionResult ChckUserIsLogin()
        {
            try
            {
                var user = User.Identity.GetUserId();
                if (user != null)
                    return Json("true", JsonRequestBehavior.AllowGet);
                else
                    return Json("false", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult SendMessagetoEntrn(tbl_EntnMessages ESMS)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (var ip in host.AddressList)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                            ESMS.IPAddress = ip.ToString();
                    }

                    ESMS.IsRead = false;
                    ESMS.Status = "New";
                    ESMS.ReceivedDateTime = DateTime.Now;
                    var userid = User.Identity.GetUserId();
                    var AspUser = db.AspNetUsers.Where(d => d.Id == userid).FirstOrDefault();
                    if (AspUser != null)
                    {
                        if (User.IsInRole("EntertainerBS"))
                        {
                            var entrnnn = db.tbl_Entertainer.Where(d => d.UserId == userid).FirstOrDefault();
                            ESMS.ContactNo = AspUser.PhoneNumber == null ? "0" : AspUser.PhoneNumber; //  entrnnn.Mobile;
                            ESMS.EmailID = entrnnn.Email;
                            ESMS.Name = entrnnn.FName;
                            ESMS.SenderUserId = userid;
                        }
                        else if (User.IsInRole("TalentSeekerBS"))
                        {
                            var ts = db.tbl_TalentSeeker.Where(d => d.UserId == userid).FirstOrDefault();
                            ESMS.ContactNo = AspUser.PhoneNumber == null ? "0" : AspUser.PhoneNumber; ;
                            ESMS.EmailID = ts.Email;
                            ESMS.Name = ts.FName;
                            ESMS.SenderUserId = userid;
                        }

                        ESMS.SenderUserId = userid;
                    }

                    db.tbl_EntnMessages.Add(ESMS);
                    db.SaveChanges();
                    var entr = db.tbl_Entertainer.Find(ESMS.EntrId);

                    EmailMessageModel msg = new EmailMessageModel();
                    msg.Destination = entr.Email;

                    var filee = Server.MapPath("~/Content/MailTemplate.html");
                    string text = System.IO.File.ReadAllText(filee);
                    var msgg = "Entertainer Name: " + entr.FName + "<br /> Name : " + ESMS.Name + "<br /> Email : " + ESMS.EmailID + "<br /> Contact No. : " + ESMS.ContactNo + "<br /> Message : " + ESMS.Message;
                    text = text.Replace("MailMessage", msgg);
                    msg.Body = text;
                    msg.Subject = ESMS.Name + " Contacted you from India Entertainers.";
                    var fff = objEmailServicess.SendByGmail(msg);
                    return Json("Success-" + entr.FName, JsonRequestBehavior.AllowGet);
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
                //throw e;
            }

        }

        // GET: Entertainer/Filters
        public ActionResult Filters(string search)
        {
            ViewBag.SearhBy = "Search Result for " + search;
            if (!string.IsNullOrEmpty(search))
            {
                ViewBag.cats = db.tbl_Category.Where(d => d.Title.ToLower().Contains(search.ToLower())).OrderBy(d => d.Title).ToList();
                var entertainer = (from i in db.tbl_Entertainer
                                   join j in db.tbl_City on i.CityId equals j.CityId
                                   join k in db.tbl_EntrImages on i.EntrId equals k.EntrId
                                   join m in db.tbl_EntertainerCats on i.EntrId equals m.EntrId
                                   join n in db.tbl_Category on m.CatId equals n.CatId
                                   where k.IsCurProfileImg == true && i.IsActive == true && i.IsProfileCompleted == true && i.FName.Contains(search)
                                   orderby i.Star descending
                                   select new TrendingViewModel
                                   {
                                       EntrId = i.EntrId,
                                       PerformanceFees = i.Performancefee,
                                       CityName = j.CityName,
                                       ProfilePhoto = k.ImagePath,
                                       Type = i.Type,
                                       FullName = i.FName,
                                       CategoryName = n.Title

                                   }).ToList();
                ViewBag.TotalRows = entertainer.Count();
                ViewBag.Entertainer = entertainer;
                return View(entertainer);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "", search });
            }

        }

        //Filters result in json
        public JsonResult InnerFilter(FilterModel FilterModel)
        {
            try
            {
                int PageSize = 20;
                int Skip = FilterModel.PageNo == 0 ? 0 : PageSize * FilterModel.PageNo;
                List<SearchModel> Entertainers = new List<SearchModel>();
                Entertainers = (from en in db.tbl_Entertainer
                                join cat in db.tbl_EntertainerCats on en.EntrId equals cat.EntrId
                                join ct in db.tbl_City on en.CityId equals ct.CityId
                                join im in db.tbl_EntrImages on en.EntrId equals im.EntrId
                                where im.IsCurProfileImg == true && en.IsProfileCompleted == true && en.IsActive == true
                                orderby en.Star descending
                                select new SearchModel
                                {
                                    Name = en.FName,
                                    Category = cat.tbl_Category.Title.Substring(0, (cat.tbl_Category.Title.Length - 1)),
                                    City = ct.CityName,
                                    EntrId = en.EntrId,
                                    Id = en.EntrId,
                                    IsItCat = false,
                                    Showfee = en.Showfee,
                                    fee = en.Performancefee,
                                    CityId = ct.CityId,
                                    CategoryId = (int)cat.CatId,
                                    GenderId = en.Gender == "Male" ? 1 : (en.Gender == "Female" ? 2 : (en.Gender == "All Male Group" ? 3 : (en.Gender == "All Female Group" ? 4 : (en.Gender == "Mixed Group" ? 5 : 0)))),
                                    NationalityId = en.IsItIndian == true ? 1 : 2,
                                    PerfLangId = en.PerfLanguage == "Hindi" ? 1 : (en.PerfLanguage == "English" ? 2 : (en.PerfLanguage == "Multilingual" ? 3 : 0)),
                                    URL = "/Entertainers/" + cat.tbl_Category.Title + "/" + en.Slug,
                                    imgurl = "/images/UserImages/" + im.ImagePath
                                }
                               ).ToList();

                //Filter Cities
                if (FilterModel.FlCity != null && FilterModel.FlCity.Count() > 0)
                {
                    FilterModel.FlCity.RemoveAll(d => d == null);
                    FilterModel.FlCity.RemoveAll(d => d.CityId == 0);
                    if (FilterModel.FlCity.Count() > 0)
                    {
                        List<int> CIdList = new List<int>();
                        CIdList = FilterModel.FlCity.Select(d => d.CityId).ToList();
                        Entertainers = Entertainers.Where(d => CIdList.Contains(d.CityId)).ToList();
                    }
                }

                //Filter Category
                if (FilterModel.FlCategory != null && FilterModel.FlCategory.Count() > 0)
                {
                    FilterModel.FlCategory.RemoveAll(d => d == 0);
                    if (FilterModel.FlCategory.Count() > 0)
                        Entertainers = Entertainers.Where(d => FilterModel.FlCategory.Contains(d.CategoryId)).ToList();
                }

                //Filter Gender
                if (FilterModel.FlGender != null && FilterModel.FlGender.Count() > 0)
                {
                    FilterModel.FlGender.RemoveAll(d => d == 0);
                    if (FilterModel.FlGender.Count() > 0)
                        Entertainers = Entertainers.Where(d => FilterModel.FlGender.Contains(d.GenderId)).ToList();
                }

                //Filter Nationality
                if (FilterModel.FlNationality != null && FilterModel.FlNationality.Count() > 0)
                {
                    FilterModel.FlNationality.RemoveAll(d => d == 0);
                    if (FilterModel.FlNationality.Count() > 0)
                        Entertainers = Entertainers.Where(d => FilterModel.FlNationality.Contains(d.NationalityId)).ToList();
                }

                //Filter Language
                if (FilterModel.FlPerfLanguage != null && FilterModel.FlPerfLanguage.Count() > 0)
                {
                    FilterModel.FlPerfLanguage.RemoveAll(d => d == 0);
                    if (FilterModel.FlPerfLanguage.Count() > 0)
                        Entertainers = Entertainers.Where(d => FilterModel.FlPerfLanguage.Contains(d.PerfLangId)).ToList();
                }

                // Performance Fee
                if (FilterModel.FlbudgetMin != 0 || FilterModel.FlbudgetMax != 0)
                {
                    Entertainers = Entertainers.Where(d => d.fee >= FilterModel.FlbudgetMin && d.fee <= FilterModel.FlbudgetMax).ToList();
                }
                Entertainers = Entertainers.Skip(Skip).Take(PageSize).ToList();
                return Json(Entertainers, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            { throw; }
        }

        //Filters Get Cities
        public JsonResult GetCities(string Qry)
        {
            try
            {
                if (String.IsNullOrEmpty(Qry))
                    return Json("-", JsonRequestBehavior.AllowGet);
                var Cities = (from en in db.tbl_City.Where(d => d.CityName.ToLower().Contains(Qry.ToLower()))
                              select new CityFltrModel
                              {
                                  Name = en.CityName,
                                  CityId = en.CityId,
                                  Id = en.CityId
                              }
                        ).ToList();
                return Json(Cities, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        //Filters Get Categories
        public JsonResult GetCategories(string Qry)
        {
            try
            {
                if (String.IsNullOrEmpty(Qry))
                    return Json("-", JsonRequestBehavior.AllowGet);
                int i = 0;
                var Categories = (from en in db.tbl_Category.Where(d => d.Title.ToLower().Contains(Qry.ToLower()))
                                  select new CategoryFltrModel
                                  {
                                      Name = en.Title,
                                      CatId = en.CatId,
                                      Id = en.CatId,

                                  }
                        ).ToList();
                return Json(Categories, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}