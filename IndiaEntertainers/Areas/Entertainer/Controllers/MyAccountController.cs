using IndiaEntertainers.Areas.Entertainer.Models;
using IndiaEntertainers.Models;
using IndiaEntertainers.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IndiaEntertainers.Areas.Entertainer.Controllers
{
    [Authorize(Roles = "")]
    public class MyAccountController : Controller
    {
        private IEDBContext db = new IEDBContext();

        #region Profile Details
        // GET: Entertainer/MyAccount/MyPofile
        [Authorize]
        public ActionResult MyPofile()
        {
            var uid = User.Identity.GetUserId();
            var entnr = db.tbl_Entertainer.Where(d => d.UserId == uid).FirstOrDefault();
            if (entnr == null)
                return RedirectToAction("Register");
            return View(entnr);
        }

        // GET: Entertainer/MyAccount/Profile
        [Authorize]
        public ActionResult ProfileDetail()
        {
            var uid = User.Identity.GetUserId();
            var entertainer = db.tbl_Entertainer.FirstOrDefault(d => d.UserId == uid);
            if (entertainer == null)
            {                
                tbl_Entertainer Entertainer = new tbl_Entertainer();
                var Userr = db.AspNetUsers.Where(d => d.Id == uid).FirstOrDefault();
                Entertainer.FName = "-";
                Entertainer.UserId = uid;
                Entertainer.YearsOfPerforming = "-";
                Entertainer.Gender = "-";
                Entertainer.Email = Userr.Email;
                Entertainer.IntgramPageLink = "#";
                Entertainer.Mobile = Userr.PhoneNumber;
                Entertainer.StateId = 0;
                Entertainer.CityId = 0;
                Entertainer.RegDate = DateTime.Now.Date;
                Entertainer.FBPageLink = "#";
                Entertainer.Gender = "-";
                Entertainer.IsActive = true;
                Entertainer.IsPremium = false;
                Entertainer.Nationality = "-";
                Entertainer.NoofShow = 0;
                Entertainer.PerfLanguage = "-";
                Entertainer.PerfLength = 0;
                Entertainer.Performancefee = 0;
                Entertainer.PERFORMINGMEMBERS = "-";
                Entertainer.Profile = "-";
                Entertainer.ProfileTitle = "-";
                Entertainer.TwiterPageLink = "#";
                Entertainer.Type = "-";
                Entertainer.IsProfileCompleted = false;

                db.tbl_Entertainer.Add(Entertainer);
                db.SaveChanges();

                entertainer = db.tbl_Entertainer.FirstOrDefault(d => d.UserId == uid);
                //return RedirectToAction("Register");
            }

            ViewBag.photos = db.tbl_EntrImages.Where(d => d.EntrId == entertainer.EntrId).OrderByDescending(d => d.ImageId).ToList();
            var videos = db.tbl_EntrVideos.Where(d => d.EntrId == entertainer.EntrId).OrderByDescending(d => d.Vid).ToList();

            foreach (var v in videos)
            {
                v.Videolink = GetYoutubeParameter(v.Videolink);
            }

            ViewBag.videos = videos;
            return View(entertainer);
        }
        #endregion

        #region Create Section
        // GET: Entertainer/MyAccount/Create
        [Authorize]
        public ActionResult Create()
        {
            var uid = User.Identity.GetUserId();
            ViewBag.uid = uid;
            return View();
        }

        // GET: Entertainer/MyAccount/Create
        [HttpPost]
        public async Task<ActionResult> Create(tbl_Entertainer Entertainer, int? maincatid, HttpPostedFileBase coverimg, HttpPostedFileBase profileimg)
        {
            var user = new ApplicationUser();
            try
            {
                if (ModelState.IsValid)
                {
                    Entertainer.IsActive = false;
                    Entertainer.IsPremium = false;
                    Entertainer.RegDate = DateTime.Now.Date;
                    db.tbl_Entertainer.Add(Entertainer);
                    await db.SaveChangesAsync();

                    //Insert Main Category
                    if (maincatid != null)
                    {
                        tbl_EntertainerCats maincat = new tbl_EntertainerCats();
                        maincat.CatId = maincatid;
                        maincat.EntrId = Entertainer.EntrId;
                        db.tbl_EntertainerCats.Add(maincat);
                        await db.SaveChangesAsync();
                    }

                    //insert User image and Cover Image
                    if (coverimg != null && coverimg.ContentLength > 0)
                    {
                        tbl_EntrImages cimg = new tbl_EntrImages();
                        cimg.EntrId = Entertainer.EntrId;
                        cimg.IsCurCoverImg = true;
                        cimg.IsCurProfileImg = false;
                        cimg.Name = "Cover Image of " + Entertainer.FName + " " + Entertainer.Slug;
                        cimg.Orderby = 1;
                        cimg.Title = Entertainer.FName + " " + Entertainer.Slug;
                        cimg.Type = "CoverImage";
                        var fileExtention = Path.GetExtension(coverimg.FileName);
                        cimg.ImagePath = string.Concat("Cover-" + Entertainer.FName + "-" + Entertainer.EntrId).Replace(" ", "-") + fileExtention;
                        var pan = Path.Combine(Server.MapPath("~/Images/UserImages"), cimg.ImagePath);
                        coverimg.SaveAs(pan);
                        db.tbl_EntrImages.Add(cimg);
                        await db.SaveChangesAsync();
                    }

                    //insert User image and Profile Image
                    if (profileimg != null && profileimg.ContentLength > 0)
                    {
                        tbl_EntrImages pimg = new tbl_EntrImages();
                        pimg.EntrId = Entertainer.EntrId;
                        pimg.IsCurCoverImg = false;
                        pimg.IsCurProfileImg = true;
                        pimg.Name = "Profile Image of " + Entertainer.FName;
                        pimg.Orderby = 1;
                        pimg.Title = Entertainer.FName + " " + Entertainer.Slug;
                        pimg.Type = "ProfileImage";
                        var fileExtention = System.IO.Path.GetExtension(profileimg.FileName);
                        pimg.ImagePath = string.Concat("Profile-" + Entertainer.FName + "-" + Entertainer.EntrId).Replace(" ", "-") + fileExtention;
                        var pan = Path.Combine(Server.MapPath("~/images/UserImages"), pimg.ImagePath);
                        profileimg.SaveAs(pan);
                        db.tbl_EntrImages.Add(pimg);
                        await db.SaveChangesAsync();
                    }

                    return RedirectToAction("MyPofile");
                }
                // If we got this far, something failed, redisplay form
                ViewBag.uid = User.Identity.GetUserId(); ;
                return View(Entertainer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Update Section
        // GET: Entertainer/MyAccount/Update
        [Authorize]
        public ActionResult Update()
        {
            var uid = User.Identity.GetUserId();
            var entnr = db.tbl_Entertainer.Where(d => d.UserId == uid).FirstOrDefault();
            if (entnr == null)
                return RedirectToAction("Register");
            return View(entnr);
        }

        [Authorize]
        // POST: Entertainer/MyAccount/Update
        [HttpPost]
        public async Task<ActionResult> Update(tbl_Entertainer Entertainer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(Entertainer).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("MyPofile");
                }
                // If we got this far, something failed, redisplay form
                ViewBag.uid = User.Identity.GetUserId(); ;
                return View(Entertainer);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Images Section
        // POST: Entertainer/MyAccount/MyVideos
        [Authorize]
        public ActionResult MyImages()
        {
            try
            {
                var uid = User.Identity.GetUserId();
                var entnr = db.tbl_Entertainer.Where(d => d.UserId == uid).FirstOrDefault();
                if (entnr == null)
                    return RedirectToAction("Register");
                var images = db.tbl_EntrImages.Where(d => d.EntrId == entnr.EntrId).ToList();
                return View(images);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Entertainer/MyAccount/UploadImages
        public ActionResult UploadImage()
        {
            var uid = User.Identity.GetUserId();
            var entnr = db.tbl_Entertainer.Where(d => d.UserId == uid).FirstOrDefault();
            if (entnr == null)
                return RedirectToAction("Register");
            ViewBag.EntrId = entnr.EntrId;
            return View();
        }

        // POST: Entertainer/MyAccount/UploadImages
        [HttpPost]
        public ActionResult UploadImage(tbl_EntrImages Images, HttpPostedFileBase Img)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var imagescount = db.tbl_EntrImages.Where(d => d.IsCurProfileImg != true && d.IsCurCoverImg != true && d.EntrId == Images.EntrId).Count();
                    if (imagescount >= 8)
                    {
                        ModelState.AddModelError("", "You Can add Max 8 Photos.");
                        ViewBag.EntrId = Images.EntrId;
                        return View(Images);
                    }

                    var uid = User.Identity.GetUserId();
                    ViewBag.uid = uid;
                    Images.IsCurCoverImg = false;
                    Images.IsCurProfileImg = false;
                    Images.Orderby = 1;
                    Images.Type = "Image";
                    Images.UploadDate = DateTime.Now.Date;
                    db.tbl_EntrImages.Add(Images);
                    db.SaveChanges();

                    //insert User image and Cover Image
                    if (Img != null && Img.ContentLength > 0)
                    {
                        var fileExtention = Path.GetExtension(Img.FileName);
                        Images.ImagePath = string.Concat("Image-" + Images.ImageId).Replace(" ", "-") + fileExtention;
                        var pan = Path.Combine(Server.MapPath("~/Images/UserImages"), Images.ImagePath);
                        Img.SaveAs(pan);
                        db.Entry(Images).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return RedirectToAction("MyImages");
                }

                ViewBag.EntrId = Images.EntrId;
                return View(Images);
            }
            catch (Exception)
            {
                ViewBag.EntrId = Images.EntrId;
                throw;
            }

        }

        // GET: Entertainer/MyAccount/UpdateImg
        public ActionResult UpdateImg(int id)
        {
            var img = db.tbl_EntrImages.Find(id);
            if (img == null)
                return HttpNotFound();
            return View(img);
        }

        // POST: Entertainer/MyAccount/UpdateImg
        [HttpPost]
        public ActionResult UpdateImg(tbl_EntrImages Images, HttpPostedFileBase Img)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var uid = User.Identity.GetUserId();
                    ViewBag.uid = uid;
                    Images.IsCurCoverImg = false;
                    Images.IsCurProfileImg = false;
                    Images.Orderby = 1;
                    Images.Type = "Image";
                    Images.UploadDate = DateTime.Now.Date;
                    //insert User image and Cover Image
                    if (Img != null && Img.ContentLength > 0)
                    {
                        string fullPath = Request.MapPath("~/Images/UserImages/" + Images.ImagePath);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        var fileExtention = Path.GetExtension(Img.FileName);
                        Images.ImagePath = string.Concat("Image-" + Images.ImageId).Replace(" ", "-") + fileExtention;
                        var pan = Path.Combine(Server.MapPath("~/Images/UserImages"), Images.ImagePath);
                        Img.SaveAs(pan);
                    }

                    db.Entry(Images).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("MyImages");
                }

                ViewBag.EntrId = Images.EntrId;
                return View(Images);
            }
            catch (Exception)
            {
                ViewBag.EntrId = Images.EntrId;
                throw;
            }

        }

        // GET: Entertainer/MyAccount/DeleteImg
        public ActionResult DeleteImg(int id)
        {
            var img = db.tbl_EntrImages.Find(id);
            if (img == null)
                return HttpNotFound();
            string fullPath = Request.MapPath("~/Images/UserImages/" + img.ImagePath);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            db.tbl_EntrImages.Remove(img);
            db.SaveChanges();
            return RedirectToAction("MyImages");

        }
        #endregion

        #region Videos Section
        // POST: Entertainer/MyAccount/MyVideos
        [Authorize]
        public ActionResult MyVideos()
        {
            try
            {
                var uid = User.Identity.GetUserId();
                var entnr = db.tbl_Entertainer.Where(d => d.UserId == uid).FirstOrDefault();
                if (entnr == null)
                    return RedirectToAction("Register");
                var videos = db.tbl_EntrVideos.Where(d => d.EntrId == entnr.EntrId).ToList();
                return View(videos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Entertainer/MyAccount/UploadVideo
        public ActionResult UploadVideo()
        {
            var uid = User.Identity.GetUserId();
            var entnr = db.tbl_Entertainer.Where(d => d.UserId == uid).FirstOrDefault();
            if (entnr == null)
                return RedirectToAction("Register");
            ViewBag.EntrId = entnr.EntrId;
            return View();
        }

        // POST: Entertainer/MyAccount/UploadVideo
        [HttpPost]
        public ActionResult UploadVideo(tbl_EntrVideos vd)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    vd.UploadDate = DateTime.Now.Date;
                    db.tbl_EntrVideos.Add(vd);
                    db.SaveChanges();
                    return RedirectToAction("MyVideos");
                }
                ViewBag.EntrId = vd.EntrId;
                return View(vd);

            }
            catch (Exception)
            {
                ViewBag.EntrId = vd.EntrId;
                throw;
            }

        }

        // GET: Entertainer/MyAccount/UpdateVd
        public ActionResult UpdateVd(int id)
        {
            var vid = db.tbl_EntrVideos.Find(id);
            if (vid == null)
                return HttpNotFound();
            return View(vid);
        }

        // POST: Entertainer/MyAccount/UpdateVd
        [HttpPost]
        public ActionResult UpdateVd(tbl_EntrVideos vd)
        {
            try
            {
                vd.UploadDate = DateTime.Now.Date;
                db.Entry(vd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyVideos");
            }
            catch (Exception)
            {
                throw;
            }

        }

        // GET: Entertainer/MyAccount/DeleteVd
        public ActionResult DeleteVd(int id)
        {
            var vid = db.tbl_EntrVideos.Find(id);
            if (vid == null)
                return HttpNotFound();
            db.tbl_EntrVideos.Remove(vid);
            db.SaveChanges();
            return RedirectToAction("MyVideos");

        }
        #endregion


        //GET: Entertainer/MyAccount/Register
        public ActionResult Register()
        {
            var uid = User.Identity.GetUserId();
            var entr = db.tbl_Entertainer.Where(d => d.UserId == uid).FirstOrDefault();
            if (entr != null) return RedirectToAction("ProfileDetail");
            ViewBag.Cities = db.tbl_City.Where(d => d.StateId == 0).Select(x => new SelectListItem { Text = x.CityName, Value = x.CityId.ToString() });
            ViewBag.uid = uid; ViewBag.StateId = "0"; ViewBag.CityId = "0"; ViewBag.CatId = "0";
            ViewBag.PerfMembers = ""; ViewBag.PerfLanguage = ""; ViewBag.Gender = ""; ViewBag.YearsOfPerforming = ""; ViewBag.NoofShow = ""; ViewBag.PerfLength = "0";
            return View();
        }

        //POST: Entertainer/MyAccount/Register
        [HttpPost]
        public ActionResult Register(ViewModel.RegisterViewModel RVMEntr)
        {
            try
            {
                tbl_Entertainer Entr = new tbl_Entertainer();
                if (ModelState.IsValid)
                {
                    Entr.IsItIndian = RVMEntr.IsItIndian;
                    if (RVMEntr.IsItIndian)
                        Entr.Nationality = "Indian";
                    else
                        Entr.Nationality = "Non-Indian";
                    var uid = User.Identity.GetUserId();
                    Entr.IsActive = true; Entr.IsPremium = false;
                    Entr.RegDate = DateTime.Now.Date;
                    var Userr = db.AspNetUsers.Where(d => d.Id == uid).FirstOrDefault();
                    Entr.Email = Userr.Email;
                    Entr.PERFORMINGMEMBERS = RVMEntr.PerfMembers; Entr.NoofShow = RVMEntr.NoofShow;
                    Entr.CityId = RVMEntr.CityId; Entr.FBPageLink = RVMEntr.FBPageLink;
                    Entr.FName = RVMEntr.FName; Entr.Gender = RVMEntr.Gender;
                    Entr.IntgramPageLink = RVMEntr.IntgramPageLink; Entr.IsItTrending = false;
                    Entr.IsProfileCompleted = true; Entr.Members = 0; Entr.Mobile = RVMEntr.Mobile;
                    Entr.PerfLanguage = RVMEntr.PerfLanguage;
                    Entr.PerfLength = RVMEntr.PerfLength; Entr.Performancefee = RVMEntr.Performancefee;
                    Entr.Profile = RVMEntr.Profile; Entr.ProfileTitle = RVMEntr.ProfileTitle;
                    Entr.Showfee = false; Entr.Star = 0; Entr.StateId = RVMEntr.StateId;
                    Entr.TwiterPageLink = RVMEntr.TwiterPageLink; Entr.Type = RVMEntr.Type;
                    Entr.YearsOfPerforming = RVMEntr.YearsOfPerforming;
                    Entr.UserId = User.Identity.GetUserId();
                    //Add Slug
                    Regex re = new Regex("[;\\\\/:*?\"<>.,|&']");
                    if (Entr.FName != "-" || Entr.FName != null)
                    {
                        string slug = Entr.FName.Replace(" ", "-").ToLower(); slug = re.Replace(slug, "");
                        var chkduplicate = db.tbl_Entertainer.Where(d => d.Slug == slug).FirstOrDefault();
                        if (chkduplicate != null)
                        {
                            bool flag = true; int i = 1; slug = slug + "-";
                            while (flag)
                            {
                                string slugg = slug + i.ToString();
                                var chkduplicate1 = db.tbl_Entertainer.Where(d => d.Slug == slugg).FirstOrDefault();
                                if (chkduplicate1 == null)
                                { flag = false; slug = slugg; }
                                else i++;
                            }
                        }
                        Entr.Slug = slug;
                    }

                    db.tbl_Entertainer.Add(Entr);
                    db.SaveChanges();

                    //Insert Main Category
                    if (RVMEntr.CatId != null)
                    {
                        tbl_EntertainerCats maincat = new tbl_EntertainerCats();
                        maincat.CatId = RVMEntr.CatId;
                        maincat.EntrId = Entr.EntrId;
                        db.tbl_EntertainerCats.Add(maincat);
                        db.SaveChanges();
                    }

                    //insert User image and Cover Image
                    if (RVMEntr.coverimg != null && RVMEntr.coverimg.ContentLength > 0)
                    {
                        tbl_EntrImages cimg = new tbl_EntrImages();
                        cimg.EntrId = Entr.EntrId; cimg.IsCurCoverImg = true;
                        cimg.IsCurProfileImg = false; cimg.Name = "Cover Image of " + Entr.FName;
                        cimg.Orderby = 1; cimg.Title = Entr.FName;
                        cimg.Type = "CoverImage";
                        var fileExtention = Path.GetExtension(RVMEntr.coverimg.FileName);
                        cimg.ImagePath = string.Concat("Cover-" + Entr.FName + "-" + Entr.EntrId).Replace(" ", "-") + fileExtention;
                        var pan = Path.Combine(Server.MapPath("~/Images/UserImages"), cimg.ImagePath);
                        RVMEntr.coverimg.SaveAs(pan);
                        db.tbl_EntrImages.Add(cimg);
                        db.SaveChanges();
                    }

                    //insert User image and Profile Image
                    if (RVMEntr.profileimg != null && RVMEntr.profileimg.ContentLength > 0)
                    {
                        tbl_EntrImages pimg = new tbl_EntrImages();
                        pimg.EntrId = Entr.EntrId; pimg.IsCurCoverImg = false;
                        pimg.IsCurProfileImg = true; pimg.Name = "Profile Image of " + Entr.FName;
                        pimg.Orderby = 1; pimg.Title = Entr.FName;
                        pimg.Type = "ProfileImage";
                        var fileExtention = System.IO.Path.GetExtension(RVMEntr.profileimg.FileName);
                        pimg.ImagePath = string.Concat("Profile-" + Entr.FName + "-" + Entr.EntrId).Replace(" ", "-") + fileExtention;
                        var pan = Path.Combine(Server.MapPath("~/images/UserImages"), pimg.ImagePath);
                        RVMEntr.profileimg.SaveAs(pan);
                        db.tbl_EntrImages.Add(pimg); db.SaveChanges();
                    }
                    ViewBag.uid = Entr.UserId;
                    return RedirectToAction("ProfileDetail");
                }
                ViewBag.PerfMembers = RVMEntr.PerfMembers; ViewBag.Gender = RVMEntr.Gender; ViewBag.uid = RVMEntr.UserId; ViewBag.StateId = Convert.ToString(RVMEntr.StateId); ViewBag.CityId = Convert.ToString(RVMEntr.CityId); ViewBag.CatId = Convert.ToString(RVMEntr.CatId);
                ViewBag.Gender = RVMEntr.Gender; @ViewBag.PerfLanguage = RVMEntr.PerfLanguage; ViewBag.YearsOfPerforming = RVMEntr.YearsOfPerforming;
                ViewBag.NoofShow = RVMEntr.NoofShow == null ? "0" : Convert.ToString(RVMEntr.NoofShow);
                ViewBag.PerfLength = RVMEntr.PerfLength;
                if (RVMEntr.StateId != null && RVMEntr.StateId != 0 && RVMEntr.CityId != null && RVMEntr.CityId != 0)
                    ViewBag.Cities = db.tbl_City.Where(d => d.StateId == RVMEntr.StateId).Select(x => new SelectListItem { Text = x.CityName, Value = x.CityId.ToString() });
                else
                    ViewBag.Cities = db.tbl_City.Where(d => d.StateId == 0).Select(x => new SelectListItem { Text = x.CityName, Value = x.CityId.ToString() });
                return View(RVMEntr);
            }
            catch (Exception)
            { throw; }
        }

        // GET: Entertainer/MyAccount/ChangeCover
        public JsonResult ChangeCover(ImageModel img)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var UID = User.Identity.GetUserId();
                    var entrn = db.tbl_Entertainer.FirstOrDefault(d => d.UserId == UID);
                    //insert User image and Cover Image
                    if (img.Photo != null && img.Photo.ContentLength > 0)
                    {
                        var uimg = db.tbl_EntrImages.Where(d => d.EntrId == entrn.EntrId && d.IsCurCoverImg == true).FirstOrDefault();
                        if (uimg != null)
                        {
                            uimg.Orderby = uimg.Orderby == null ? 1 : uimg.Orderby++;
                            if (uimg.Orderby > 100)
                                uimg.Orderby = 1;
                            uimg.IsCurCoverImg = true;
                            uimg.Title = img.Title;
                            uimg.UploadDate = DateTime.Now.Date;
                            string fullPath1 = Path.Combine(Server.MapPath("~/Images/UserImages"), uimg.ImagePath);
                            if (System.IO.File.Exists(fullPath1))
                            {
                                System.IO.File.Delete(fullPath1);
                            }

                            var fileExtention = Path.GetExtension(img.Photo.FileName);
                            uimg.ImagePath = string.Concat("Cover-" + entrn.FName + "-" + entrn.EntrId + "-" + uimg.Orderby).Replace(" ", "-") + fileExtention;
                            var pan1 = Path.Combine(Server.MapPath("~/Images/UserImages"), uimg.ImagePath);
                            img.Photo.SaveAs(pan1);
                            db.Entry(uimg).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            tbl_EntrImages cimg = new tbl_EntrImages();
                            cimg.EntrId = entrn.EntrId;
                            cimg.IsCurCoverImg = true;
                            cimg.IsCurProfileImg = false;
                            cimg.Title = img.Title;
                            cimg.UploadDate = DateTime.Now.Date;
                            cimg.Type = "CoverImage";
                            cimg.Name = "Cover Image of " + entrn.FName;
                            var fileExtention = Path.GetExtension(img.Photo.FileName);
                            cimg.ImagePath = string.Concat("Cover-" + entrn.FName + "-" + entrn.EntrId).Replace(" ", "-") + fileExtention;
                            string fullPath1 = Path.Combine(Server.MapPath("~/Images/UserImages"), cimg.ImagePath);
                            if (System.IO.File.Exists(fullPath1))
                            {
                                System.IO.File.Delete(fullPath1);
                            }
                            cimg.Orderby = 1;
                            var pan = Path.Combine(Server.MapPath("~/Images/UserImages"), cimg.ImagePath);
                            img.Photo.SaveAs(pan);
                            db.tbl_EntrImages.Add(cimg);
                            db.SaveChanges();
                        }

                    }

                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Entertainer/MyAccount/ChangeProfileImg
        public JsonResult ChangeProfileImg(ImageModel img)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var UID = User.Identity.GetUserId();
                    var entrn = db.tbl_Entertainer.FirstOrDefault(d => d.UserId == UID);
                    //insert User image and Cover Image
                    if (img.Photo != null && img.Photo.ContentLength > 0)
                    {
                        var uimg = db.tbl_EntrImages.Where(d => d.EntrId == entrn.EntrId && d.IsCurProfileImg == true).FirstOrDefault();
                        if (uimg != null)
                        {

                            uimg.Orderby = uimg.Orderby == null ? 1 : uimg.Orderby++;
                            if (uimg.Orderby > 100)
                                uimg.Orderby = 1;
                            uimg.IsCurProfileImg = true;
                            uimg.Title = img.Title;
                            uimg.UploadDate = DateTime.Now.Date;
                            string fullPath1 = Path.Combine(Server.MapPath("~/Images/UserImages"), uimg.ImagePath);
                            if (System.IO.File.Exists(fullPath1))
                            {
                                System.IO.File.Delete(fullPath1);
                            }

                            var fileExtention = Path.GetExtension(img.Photo.FileName);
                            uimg.ImagePath = string.Concat("Profile-" + entrn.FName + "-" + entrn.EntrId + "-" + uimg.Orderby).Replace(" ", "-") + fileExtention;
                            var pan1 = Path.Combine(Server.MapPath("~/Images/UserImages"), uimg.ImagePath);
                            img.Photo.SaveAs(pan1);
                            db.Entry(uimg).State = EntityState.Modified;
                            db.SaveChanges();

                            if (entrn.IsProfileCompleted == null || entrn.IsProfileCompleted == false)
                            {
                                var propic = db.tbl_EntrImages.Where(d => d.EntrId == entrn.EntrId && d.IsCurProfileImg == true).FirstOrDefault();
                                if (propic != null)
                                {
                                    if ((entrn.CityId != 0 && entrn.CityId != null) && (entrn.FName != "-" && entrn.CityId != null) && (entrn.Gender != "-" && entrn.Gender != null) && (entrn.Nationality != "-" && entrn.Nationality != null) && (entrn.NoofShow != 0 && entrn.NoofShow != null) && (entrn.PerfLanguage != "-" && entrn.PerfLanguage != null) && (entrn.PerfLength != 0) && (entrn.Performancefee != null && entrn.Performancefee != null) && (entrn.PERFORMINGMEMBERS != "-" && entrn.PERFORMINGMEMBERS != null) && (entrn.ProfileTitle != "-" && entrn.ProfileTitle != null))
                                    {
                                        entrn.IsProfileCompleted = true;
                                        db.Entry(entrn).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                        else
                        {
                            tbl_EntrImages cimg = new tbl_EntrImages();
                            cimg.EntrId = entrn.EntrId;
                            cimg.IsCurCoverImg = false;
                            cimg.IsCurProfileImg = true;
                            cimg.Title = img.Title;
                            cimg.UploadDate = DateTime.Now.Date;
                            cimg.Type = "ProfileImage";
                            cimg.Name = "Profile Image of" + entrn.FName;
                            var fileExtention = Path.GetExtension(img.Photo.FileName);
                            cimg.ImagePath = string.Concat("Profile-" + entrn.FName + "-" + entrn.EntrId).Replace(" ", "-") + fileExtention;
                            string fullPath1 = Path.Combine(Server.MapPath("~/Images/UserImages"), cimg.ImagePath);
                            if (System.IO.File.Exists(fullPath1))
                            {
                                System.IO.File.Delete(fullPath1);
                            }
                            cimg.Orderby = 1;
                            var pan = Path.Combine(Server.MapPath("~/Images/UserImages"), cimg.ImagePath);
                            img.Photo.SaveAs(pan);
                            db.tbl_EntrImages.Add(cimg);
                            db.SaveChanges();

                            if (entrn.IsProfileCompleted == null || entrn.IsProfileCompleted == false)
                            {
                                var propic = db.tbl_EntrImages.Where(d => d.EntrId == entrn.EntrId && d.IsCurProfileImg == true).FirstOrDefault();
                                if (propic != null)
                                {
                                    if ((entrn.CityId != 0 && entrn.CityId != null) && (entrn.FName != "-" && entrn.CityId != null) && (entrn.Gender != "-" && entrn.Gender != null) && (entrn.Nationality != "-" && entrn.Nationality != null) && (entrn.NoofShow != 0 && entrn.NoofShow != null) && (entrn.PerfLanguage != "-" && entrn.PerfLanguage != null) && (entrn.PerfLength != 0 && entrn.PerfLength != null) && (entrn.Performancefee != 0 && entrn.Performancefee != null) && (entrn.PERFORMINGMEMBERS != "-" && entrn.PERFORMINGMEMBERS != null) && (entrn.ProfileTitle != "-" && entrn.ProfileTitle != null))
                                    {
                                        entrn.IsProfileCompleted = true;
                                        db.Entry(entrn).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Entertainer/MyAccount/AddNewPhoto
        public JsonResult AddNewPhoto(ImageModel img)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var UID = User.Identity.GetUserId();
                    var entrn = db.tbl_Entertainer.FirstOrDefault(d => d.UserId == UID);

                    var imagescount = db.tbl_EntrImages.Where(d => d.IsCurProfileImg != true && d.IsCurCoverImg != true && d.EntrId == entrn.EntrId).Count();
                    if (imagescount >= 8)
                    {
                        return Json("You Can add Max 8 Photos.", JsonRequestBehavior.AllowGet);
                    }

                    //insert User image and Cover Image
                    if (img.Photo != null && img.Photo.ContentLength > 0)
                    {
                        tbl_EntrImages nimg = new tbl_EntrImages();
                        nimg.EntrId = entrn.EntrId;
                        nimg.IsCurCoverImg = false;
                        nimg.IsCurProfileImg = false;
                        nimg.Title = img.Title;
                        nimg.UploadDate = DateTime.Now.Date;
                        nimg.Type = "Image";
                        nimg.Name = "Image of" + entrn.FName;
                        nimg.ImagePath = "";

                        var fileExtention = Path.GetExtension(img.Photo.FileName);
                        db.tbl_EntrImages.Add(nimg);
                        db.SaveChanges();

                        nimg.ImagePath = string.Concat("Image-" + entrn.FName + "-" + nimg.ImageId).Replace(" ", "-") + fileExtention;
                        var pan = Path.Combine(Server.MapPath("~/Images/UserImages"), nimg.ImagePath);
                        img.Photo.SaveAs(pan);
                        db.Entry(nimg).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Entertainer/MyAccount/DeleteImage
        public JsonResult DeleteImage(int id)
        {
            try
            {
                var UID = User.Identity.GetUserId();
                var entrn = db.tbl_Entertainer.FirstOrDefault(d => d.UserId == UID);
                var image = db.tbl_EntrImages.Where(d => d.ImageId == id && d.EntrId == entrn.EntrId).FirstOrDefault();
                if (image != null)
                {
                    db.tbl_EntrImages.Remove(image); db.SaveChanges();
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Entertainer/MyAccount/AddVideo
        public JsonResult AddVideo(tbl_EntrVideos vd)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var UID = User.Identity.GetUserId();
                    var entrn = db.tbl_Entertainer.FirstOrDefault(d => d.UserId == UID);
                    vd.EntrId = entrn.EntrId;
                    vd.Title = vd.Title;
                    vd.UploadDate = DateTime.Now.Date;
                    db.tbl_EntrVideos.Add(vd);
                    db.SaveChanges();
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                ViewBag.EntrId = vd.EntrId;
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            { return Json("Fail", JsonRequestBehavior.AllowGet); }
        }

        // POST: Entertainer/MyAccount/DeleteVideo
        public JsonResult DeleteVideo(int id)
        {
            try
            {
                var UID = User.Identity.GetUserId();
                var entrn = db.tbl_Entertainer.FirstOrDefault(d => d.UserId == UID);
                var vidd = db.tbl_EntrVideos.Where(d => d.Vid == id && d.EntrId == entrn.EntrId).FirstOrDefault();
                if (vidd != null)
                {
                    db.tbl_EntrVideos.Remove(vidd);
                    db.SaveChanges();
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Entertainer/MyAccount/GetMessage
        public JsonResult GetMessage()
        {
            try
            {
                var UID = User.Identity.GetUserId();
                var entrn = db.tbl_Entertainer.FirstOrDefault(d => d.UserId == UID);
                if (entrn == null)
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                else
                {
                    return Json(entrn.Profile, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
                throw;
            }
        }

        // POST: Entertainer/MyAccount/UpdateProfile
        public JsonResult UpdateProfile(ProfileViewModel profiled)
        {
            try
            {
                var UID = User.Identity.GetUserId();
                var entrn = db.tbl_Entertainer.FirstOrDefault(d => d.UserId == UID);
                if (entrn == null)
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                //if (profiled.ProfTitle.Trim().Length > 1500)
                //    return Json("Max Length 1500 Characters", JsonRequestBehavior.AllowGet);
                //else
                //{
                entrn.Profile = profiled.ProfTitle;
                db.Entry(entrn).State = EntityState.Modified;
                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);

                // }
            }
            catch (Exception)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
                throw;
            }
        }

        // POST: Entertainer/MyAccount/GetMessage
        public JsonResult GetDetails()
        {
            try
            {
                var UID = User.Identity.GetUserId();
                var entrn = db.tbl_Entertainer.FirstOrDefault(d => d.UserId == UID);
                if (entrn == null)
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                else
                {
                    ProfileViewModel US = new ProfileViewModel();
                    US.FbLink = entrn.FBPageLink == "#" ? null : entrn.FBPageLink;
                    US.EntrId = entrn.EntrId;
                    US.Mobile = entrn.Mobile;
                    US.Gender = entrn.Gender == "-" ? null : entrn.Gender;
                    US.InstLink = entrn.IntgramPageLink == "#" ? null : entrn.IntgramPageLink;
                    US.Nationality = entrn.Nationality == "-" ? null : entrn.Nationality;
                    US.NoOfShow = entrn.NoofShow;
                    US.PerfLength = entrn.PerfLength;
                    US.PerfMembers = entrn.PERFORMINGMEMBERS == "-" ? null : entrn.PERFORMINGMEMBERS;
                    US.ProfTitle = entrn.ProfileTitle == "-" ? null : entrn.ProfileTitle;
                    US.ProfDetail = entrn.Profile == "-" ? null : entrn.Profile;
                    US.TwtLink = entrn.TwiterPageLink == "#" ? null : entrn.TwiterPageLink;
                    //Double pf = 0;
                    US.PerfFee = entrn.Performancefee;
                    if (entrn.Showfee)
                    {
                        US.Showfee = false;
                    }
                    if (!entrn.Showfee)
                    {
                        US.Showfee = true;
                    }

                    US.PerfLang = entrn.PerfLanguage;

                    //  US.PerfLangTitle = entrn.PerfLanguage == "-" ? null : entrn.PerfLanguage;
                    //var splitperflang = entrn.PerfLanguage.Split(',');
                    //foreach (var item in splitperflang)
                    //{
                    //    if (item == "English")
                    //        US.PerfLang = true;
                    //    if (item == "Hindi")
                    //        US.PerfLang1 = true;
                    //    if (item == "Multilingual")
                    //        US.PerfLang2 = true;
                    //}

                    US.CityId = entrn.CityId == 0 ? null : entrn.CityId;
                    if (entrn.CityId != null && entrn.CityId != 0)
                    {
                        var city = db.tbl_City.Find(entrn.CityId);
                        US.City = city.CityName;
                    }
                    else
                    {
                        US.City = "";
                    }


                    US.FName = entrn.FName == "-" ? null : entrn.FName;
                    US.CatId = entrn.tbl_EntertainerCats.Count() == 0 ? null : entrn.tbl_EntertainerCats.FirstOrDefault().CatId;
                    return Json(US, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
                throw;
            }
        }

        // POST: Entertainer/MyAccount/UpdateProfileDetails
        public JsonResult UpdateProfileDetails(ProfileViewModel profiled)
        {
            try
            {
                if (profiled != null)
                {
                    var entrn = db.tbl_Entertainer.Find(profiled.EntrId);
                    if (entrn == null)
                    {
                        var UID = User.Identity.GetUserId();
                        entrn = db.tbl_Entertainer.FirstOrDefault(d => d.UserId == UID);

                        if (entrn == null)
                        {
                            return Json("Fail", JsonRequestBehavior.AllowGet);
                        }
                    }

                    if (profiled.CatId != null && profiled.CatId == 11)
                    {
                        if (!String.IsNullOrEmpty(profiled.FName))
                        {
                            if (!profiled.FName.ToLower().Contains("dj"))
                            {
                                profiled.FName = "DJ " + profiled.FName;
                            }
                        }
                    }

                    entrn.FName = profiled.FName;
                    entrn.FBPageLink = profiled.FbLink;
                    entrn.Gender = profiled.Gender;
                    entrn.IntgramPageLink = profiled.InstLink;
                    entrn.Nationality = profiled.Nationality;
                    if (entrn.Nationality == "Indian") entrn.IsItIndian = true;
                    else entrn.IsItIndian = false;
                    entrn.NoofShow = profiled.NoOfShow;
                    entrn.PerfLength = profiled.PerfLength;
                    entrn.PERFORMINGMEMBERS = profiled.PerfMembers;
                    entrn.ProfileTitle = profiled.ProfTitle;
                    entrn.Profile = profiled.ProfDetail;
                    entrn.TwiterPageLink = profiled.TwtLink;
                    entrn.Performancefee = profiled.PerfFee;
                    entrn.CityId = profiled.CityId;
                    entrn.Showfee = profiled.DonotShowfee;
                    entrn.PerfLanguage = profiled.PerfLang;
                    entrn.Mobile = profiled.Mobile;


                    //Change Main Category of the Artist
                    if (profiled.CatId != null && profiled.CatId != 0)
                    {
                        var Artistcat = db.tbl_EntertainerCats.Where(d => d.EntrId == entrn.EntrId).FirstOrDefault();
                        if (Artistcat != null)
                        {
                            if (Artistcat.CatId != profiled.CatId)
                            {
                                db.tbl_EntertainerCats.Remove(Artistcat);
                                db.SaveChanges();
                                tbl_EntertainerCats maincat = new tbl_EntertainerCats();
                                maincat.CatId = profiled.CatId;
                                maincat.EntrId = entrn.EntrId;
                                db.tbl_EntertainerCats.Add(maincat);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            try
                            {
                                tbl_EntertainerCats maincat = new tbl_EntertainerCats();
                                maincat.CatId = profiled.CatId;
                                maincat.EntrId = entrn.EntrId;
                                db.tbl_EntertainerCats.Add(maincat);
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }

                    if (entrn.IsProfileCompleted == null || entrn.IsProfileCompleted == false)
                    {
                        var propic = db.tbl_EntrImages.Where(d => d.EntrId == entrn.EntrId && d.IsCurProfileImg == true).FirstOrDefault();
                        if (propic != null)
                        {
                            if ((entrn.CityId != null && entrn.CityId != 0) && (entrn.FName != null && entrn.FName != "-") && (entrn.Gender != null && entrn.Gender != "-") && (entrn.Nationality != null && entrn.Nationality != "-") && (entrn.NoofShow != null && entrn.NoofShow != 0) && (entrn.PerfLanguage != null && entrn.PerfLanguage != "-") && (entrn.PerfLength != 0) && (entrn.Performancefee != 0) && (entrn.PERFORMINGMEMBERS != null && entrn.PERFORMINGMEMBERS != "-") && (entrn.ProfileTitle != null && entrn.ProfileTitle != "-"))
                                entrn.IsProfileCompleted = true;
                        }
                    }

                    //Add Slug
                    Regex re = new Regex("[;\\\\/:*?\"<>.,|&']");
                    if (entrn.FName != "-" || entrn.FName != null)
                    {
                        string slug = entrn.FName.Replace(" ", "-").ToLower(); slug = re.Replace(slug, "");
                        var chkduplicate = db.tbl_Entertainer.Where(d => d.Slug == slug && d.EntrId != entrn.EntrId).FirstOrDefault();
                        if (chkduplicate != null)
                        {
                            bool flag = true; int i = 1; slug = slug + "-";
                            while (flag)
                            {
                                string slugg = slug + i.ToString();
                                var chkduplicate1 = db.tbl_Entertainer.Where(d => d.Slug == slugg && d.EntrId != entrn.EntrId).FirstOrDefault();
                                if (chkduplicate1 == null)
                                { flag = false; slug = slugg; }
                                else i++;
                            }
                        }
                        entrn.Slug = slug;
                    }

                    db.Entry(entrn).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
                throw;
            }
        }

        public string GetYoutubeParameter(string url)
        {
            const string pattern = @"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/)([\w\-]+))(?:[^\s?]+)?)";
            const string replacement = "https://www.youtube.com/embed/$1";

            var rgx = new Regex(pattern);
            var result = rgx.Replace(url, replacement);
            return result;
        }

        //Testing
        //GET: Entertainer/MyAccount/TestingRegister
        public ActionResult TestingRegister()
        {
            var uid = User.Identity.GetUserId();
            ViewBag.uid = uid;
            ViewBag.EmailId = db.AspNetUsers.Where(d => d.Id == uid);
            return View();
        }

        //POST: Entertainer/MyAccount/TestingRegister
        [HttpPost]
        public ActionResult TestingRegister(RegisterEntrnViewModel REntr, HttpPostedFileBase coverimg, HttpPostedFileBase profileimg)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tbl_Entertainer Entr = new tbl_Entertainer();
                    Entr.CityId = REntr.CityId; //Entr.Email = REntr.e;
                    Entr.IsActive = false;
                    Entr.IsPremium = false;
                    Entr.RegDate = DateTime.Now.Date;
                    db.tbl_Entertainer.Add(Entr);
                    db.SaveChanges();

                    //Insert Main Category
                    if (REntr.CatId != null)
                    {
                        tbl_EntertainerCats maincat = new tbl_EntertainerCats();
                        // maincat.CatId = int.Parse(CatId);
                        maincat.EntrId = Entr.EntrId;
                        db.tbl_EntertainerCats.Add(maincat);
                        db.SaveChanges();
                    }

                    //insert User image and Cover Image
                    if (coverimg != null && coverimg.ContentLength > 0)
                    {
                        tbl_EntrImages cimg = new tbl_EntrImages();
                        cimg.EntrId = Entr.EntrId;
                        cimg.IsCurCoverImg = true;
                        cimg.IsCurProfileImg = false;
                        cimg.Name = "Cover Image of " + Entr.FName + " " + Entr.Slug;
                        cimg.Orderby = 1;
                        cimg.Title = Entr.FName;
                        cimg.Type = "CoverImage";
                        var fileExtention = Path.GetExtension(coverimg.FileName);
                        cimg.ImagePath = string.Concat("Cover-" + Entr.FName + "-" + Entr.EntrId).Replace(" ", "-") + fileExtention;
                        var pan = Path.Combine(Server.MapPath("~/Images/UserImages"), cimg.ImagePath);
                        coverimg.SaveAs(pan);
                        db.tbl_EntrImages.Add(cimg);
                        db.SaveChanges();
                    }

                    //insert User image and Profile Image
                    if (profileimg != null && profileimg.ContentLength > 0)
                    {
                        tbl_EntrImages pimg = new tbl_EntrImages();
                        pimg.EntrId = Entr.EntrId;
                        pimg.IsCurCoverImg = false;
                        pimg.IsCurProfileImg = true;
                        pimg.Name = "Profile Image of " + Entr.FName;
                        pimg.Orderby = 1;
                        pimg.Title = Entr.FName;
                        pimg.Type = "ProfileImage";
                        var fileExtention = System.IO.Path.GetExtension(profileimg.FileName);
                        pimg.ImagePath = string.Concat("Profile-" + Entr.FName + "-" + Entr.EntrId).Replace(" ", "-") + fileExtention;
                        var pan = Path.Combine(Server.MapPath("~/images/UserImages"), pimg.ImagePath);
                        profileimg.SaveAs(pan);
                        db.tbl_EntrImages.Add(pimg);
                        db.SaveChanges();
                    }

                    return RedirectToAction("ProfileDetail");

                }
                //  ViewBag.uid = Entr.UserId;
                return View(REntr);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}