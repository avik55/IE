using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using IndiaEntertainers.Models;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IndiaEntertainers.Areas.TalentSeeker.Controllers
{
    public class DashboardController : Controller
    {
        private IEDBContext db = new IEDBContext();

        // GET: TalentSeeker/Dashboard
        public ActionResult Index()
        {
            return View();
        }

        // GET: TalentSeeker/Dashboard/MyPofile
        [Authorize]
        public ActionResult MyPofile()
        {
            var uid = User.Identity.GetUserId();
            var tltskr = db.tbl_TalentSeeker.Where(d => d.UserId == uid).FirstOrDefault();
            if (tltskr == null)
                return RedirectToAction("Create");
            return View(tltskr);
        }

        // GET: TalentSeeker/Dashboard/MyPofile
        [Authorize]
        public ActionResult Create()
        {
            var uid = User.Identity.GetUserId();
            ViewBag.uid = uid;
            var Userr = db.AspNetUsers.Where(d => d.Id == uid).FirstOrDefault();
            @ViewBag.Email = Userr.Email;
            return View();
        }

        // POST: TalentSeeker/Dashboard/MyPofile
        [HttpPost]
        public async Task<ActionResult> Create(tbl_TalentSeeker TalentSeeker)
        {
            var user = new ApplicationUser();
            try
            {
                if (ModelState.IsValid)
                {
                    TalentSeeker.IsActive = false;
                    TalentSeeker.IsPremium = false;
                    TalentSeeker.RegDate = DateTime.Now.Date;
                    db.tbl_TalentSeeker.Add(TalentSeeker);
                    await db.SaveChangesAsync();
                    return RedirectToAction("MyPofile");
                }
                // If we got this far, something failed, redisplay form
                ViewBag.uid = User.Identity.GetUserId();
                return View(TalentSeeker);
            }
            catch (Exception)
            {
                throw;
            }

        }

        // GET: TalentSeeker/Dashboard/MyPofile
        [Authorize]
        public ActionResult Update()
        {
            var uid = User.Identity.GetUserId();
            var tltskr = db.tbl_TalentSeeker.Where(d => d.UserId == uid).FirstOrDefault();
            if (tltskr == null)
                return RedirectToAction("Create");
            return View(tltskr);
        }

        // POST: TalentSeeker/Dashboard/MyPofile
        [HttpPost]
        public async Task<ActionResult> Update(tbl_TalentSeeker TalentSeeker)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(TalentSeeker).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("MyPofile");
                }
                // If we got this far, something failed, redisplay form
                ViewBag.uid = User.Identity.GetUserId(); ;
                return View(TalentSeeker);
            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}