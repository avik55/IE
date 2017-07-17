using IndiaEntertainers.Models;
using IndiaEntertainers.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace IndiaEntertainers.Controllers
{
    public class EntertainersController : Controller
    {
        IEDBContext db = new IEDBContext();
        EmailServicess objEmailServicess = new EmailServicess();

        // GET: Entertainer
        public ActionResult Index(string q)
        {
            ViewBag.SearhBy = q;
            if (!string.IsNullOrEmpty(q))
            {
                var entertainer = (from i in db.tbl_Entertainer
                                   join j in db.tbl_City on i.CityId equals j.CityId
                                   join k in db.tbl_EntrImages on i.EntrId equals k.EntrId
                                   join m in db.tbl_EntertainerCats on i.EntrId equals m.EntrId
                                   join n in db.tbl_Category on m.CatId equals n.CatId
                                   where k.IsCurProfileImg == true && n.Title == q
                                   select new TrendingViewModel
                                   {
                                       EntrId = i.EntrId,
                                       PerformanceFees = i.Performancefee,
                                       CityName = j.CityName,
                                       ProfilePhoto = k.ImagePath,
                                       Type = i.Type,
                                       FullName = i.FName + " " + i.LName,
                                       CategoryName = n.Title

                                   }).ToList();
                ViewBag.TotalRows = entertainer.Count();
                ViewBag.Entertainer = entertainer;
                return View(entertainer);
            }
            else
            {
                var entertainer = (from i in db.tbl_Entertainer
                                   join j in db.tbl_City on i.CityId equals j.CityId
                                   join k in db.tbl_EntrImages on i.EntrId equals k.EntrId
                                   join m in db.tbl_EntertainerCats on i.EntrId equals m.EntrId
                                   join n in db.tbl_Category on m.CatId equals n.CatId
                                   where k.IsCurProfileImg == true
                                   select new TrendingViewModel
                                   {
                                       EntrId = i.EntrId,
                                       PerformanceFees = i.Performancefee,
                                       CityName = j.CityName,
                                       ProfilePhoto = k.ImagePath,
                                       Type = i.Type,
                                       FullName = i.FName + " " + i.LName,
                                       CategoryName = n.Title

                                   }).OrderBy(x => (new Guid())).ToList();
                ViewBag.TotalRows = entertainer.Count();
                ViewBag.Entertainer = entertainer;
                ViewBag.SearhBy = "Entertainers";
                return View(entertainer);
            }
           
        }

        // GET: Entertainer/ProfileDetail
        public ActionResult ProfileDetail(int id)
        {
            var entertainer = db.tbl_Entertainer.FirstOrDefault(d => d.EntrId == id);
            ViewBag.photos = db.tbl_EntrImages.Where(d => d.EntrId == id).ToList();
            ViewBag.videos = db.tbl_EntrVideos.Where(d => d.EntrId == id).ToList();
            return View(entertainer);
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
                    ESMS.SenderUserId = User.Identity.GetUserId() == null ? null : User.Identity.GetUserId();
                    ESMS.ReceivedDateTime = DateTime.Now;
                    db.tbl_EntnMessages.Add(ESMS);
                    db.SaveChanges();

                    var entr = db.tbl_Entertainer.Find(ESMS.EntrId);
                    EmailMessageModel msg = new EmailMessageModel();
                    msg.Destination = entr.Email;
                    msg.Body = "Name : " + ESMS.Name + "<br /> Email : " + ESMS.EmailID + "<br /> Contact No. : " + ESMS.ContactNo + "<br /> Message : " + ESMS.Message;
                    msg.Subject = ESMS.Name + " Contacted you from India Entertainers.";
                    var fff = objEmailServicess.SendByGmail(msg);
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }

        }
    }
}