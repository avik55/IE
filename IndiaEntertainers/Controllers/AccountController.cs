using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using IndiaEntertainers.Models;
using System.Collections.Generic;
using System.IO;
using System.Data.Entity;
using IndiaEntertainers.HTMLHelper;
using System.Configuration;

namespace IndiaEntertainers.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private EmailServicess objEmailServicess = new EmailServicess();
        private IEDBContext db = new IEDBContext();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return RedirectToAction("Index", "Home", new { area = "" });
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                var message = "Invalid login attempt.";
                return Json(message, JsonRequestBehavior.AllowGet);
                //return View(model);
            }

            SignInStatus result = new SignInStatus();
            var user = await UserManager.FindAsync(model.UserName, model.Password);
            if (user != null)
            {
                if (user.EmailConfirmed == true)
                {
                    result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
                }
                else
                {
                    ViewBag.UserId = user.Id;
                    var message = "Please Confirm Your Email Address.";
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var message = "Invalid login attempt.";
                return Json(message, JsonRequestBehavior.AllowGet);
                //ModelState.AddModelError("", "Invalid login attempt.");
                //return View(model);
            }

            switch (result)
            {
                case SignInStatus.Success:
                    var message = "Success";
                    return Json(message, JsonRequestBehavior.AllowGet);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    //  ModelState.AddModelError("", "Invalid login attempt.");
                    return Json("Invalid login attempt.", JsonRequestBehavior.AllowGet);
                    //  return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        #region Old code for Email Verification(Link verification)
        //// POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        ////[ValidateAntiForgeryToken]
        //public async Task<JsonResult> Register(RegisterViewModel model)
        //{
        //    var user = new ApplicationUser();
        //    IdentityResult result = new IdentityResult();
        //    List<string> results = new List<string>();

        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            user = new ApplicationUser { UserName = model.UserName, Email = model.Email, PhoneNumber = model.Mobile };
        //            result = await UserManager.CreateAsync(user, model.Password);
        //            var callbackUrl = "";

        //            if (result.Succeeded)
        //            {
        //                if (model.UserType == "Entertainer")
        //                    UserManager.AddToRole(user.Id, "EntertainerBS");
        //                else if (model.UserType == "Talent Seeker")
        //                    UserManager.AddToRole(user.Id, "TalentSeekerBS");
        //                var token = UserManager.GenerateEmailConfirmationToken(user.Id);
        //                callbackUrl = this.Url.Action("ConfirmEmail", "Account", new { area = "", userId = user.Id, code = token }, protocol: Request.Url.Scheme);
        //                EmailMessageModel msg = new EmailMessageModel();
        //                msg.Destination = model.Email;
        //                var filee = Server.MapPath("~/Content/VerificationEmailTemplate.html");
        //                string text = System.IO.File.ReadAllText(filee);
        //                text = text.Replace("#$", callbackUrl);
        //                // msg.Body = "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\"> Click here!</a> <br /> Or Paste the following link in the Url  <br />" + callbackUrl.Trim();
        //                msg.Body = text;
        //                msg.Subject = user.UserName + ", Verification mail from indiaentertainers.com";
        //                var fff = objEmailServicess.SendByGmail(msg);
        //                ViewBag.UserId = user.Id;
        //                results.Add("A confirmation mail has been sent to your Email ID. Please verify the same.");
        //                return Json(results, JsonRequestBehavior.AllowGet);
        //            }
        //        }

        //        foreach (var item in result.Errors)
        //            results.Add(item);

        //        if (result.Errors.Count() <= 0)
        //        {
        //            foreach (var item in ModelState.Values)
        //            {
        //                if (item.Errors.Count > 0)
        //                {
        //                    foreach (var item1 in item.Errors)
        //                        results.Add(item1.ErrorMessage);
        //                }
        //            }
        //        }
        //        return Json(results, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        await UserManager.DeleteAsync(user);
        //        throw;
        //    }

        //}

        //
        // GET: /Account/ConfirmEmail
        #endregion

        #region New code for Email Verification(OTP verification)
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> Register(RegisterViewModel model)
        {
            var user = new ApplicationUser();
            IdentityResult result = new IdentityResult();
            List<string> results = new List<string>();

            try
            {
                if (ModelState.IsValid)
                {
                    user = new ApplicationUser { UserName = model.UserName, Email = model.Email, PhoneNumber = model.Mobile };
                    result = await UserManager.CreateAsync(user, model.Password);
                    var RandomNumber = "";

                    if (result.Succeeded)
                    {
                        if (model.UserType == "Entertainer")
                            UserManager.AddToRole(user.Id, "EntertainerBS");
                        else if (model.UserType == "Talent Seeker")
                            UserManager.AddToRole(user.Id, "TalentSeekerBS");
                        var token = UserManager.GenerateEmailConfirmationToken(user.Id);

                        TempData["UserToken"] = token;

                        Random rnd = new Random();
                        int number1 = rnd.Next(1, 13);
                        int number2 = rnd.Next(1, 7);
                        int number3 = rnd.Next(52);
                        int number4 = rnd.Next(25, 90);
                        RandomNumber = number1.ToString() + number2.ToString() + number3.ToString() + number4.ToString();
                        EmailMessageModel msg = new EmailMessageModel();
                        msg.Destination = model.Email;
                        var filee = Server.MapPath("~/Content/VerificationOTPTemplate.html");
                        string text = System.IO.File.ReadAllText(filee);
                        text = text.Replace("#$", RandomNumber);
                        msg.Body = text;
                        msg.Subject = user.UserName + ", Verification mail from indiaentertainers.com";
                        var emailSent = objEmailServicess.SendByGmail(msg);
                        ViewBag.UserId = user.Id;
                        results.Add(RandomNumber);
                        results.Add(user.Id);
                        results.Add(emailSent.ToString());

                        if (model.UserType == "Talent Seeker")
                        {
                            results.Add(model.fname);
                            results.Add(model.lname);
                            results.Add(model.companyname);
                            results.Add(model.Email);
                            results.Add(model.CityId.ToString());
                        }

                        return Json(results, JsonRequestBehavior.AllowGet);
                    }
                }

                foreach (var item in result.Errors)
                    results.Add(item);

                if (result.Errors.Count() <= 0)
                {
                    foreach (var item in ModelState.Values)
                    {
                        if (item.Errors.Count > 0)
                        {
                            foreach (var item1 in item.Errors)
                                results.Add(item1.ErrorMessage);
                        }
                    }
                }
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                await UserManager.DeleteAsync(user);
                throw;
            }

        }
        #endregion

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> CheckUserNameAlreadyExist(string username)
        {
            var result = await UserManager.FindByNameAsync(username);
            if (result != null)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }

        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                var userr = await UserManager.FindByIdAsync(userId);
                await SignInManager.SignInAsync(userr, false, false);
                return RedirectToAction("LoginSucess", "Home", new { area = "" });
            }
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> ConfirmOTP(string userId)
        {
            try
            {
                if (!String.IsNullOrEmpty(userId))
                {
                    string code = TempData["UserToken"].ToString();
                    var result = await UserManager.ConfirmEmailAsync(userId, code);

                    if (result.Succeeded)
                    {
                        var userr = await UserManager.FindByIdAsync(userId);
                        await SignInManager.SignInAsync(userr, false, false);

                        var uid = (User.Identity.GetUserId() == null ? userId : User.Identity.GetUserId());
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

                        var entertinerID = Entertainer.EntrId;
                        if (entertinerID != 0)
                        {
                            return Json(entertinerID, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json("Error", JsonRequestBehavior.AllowGet);

            }
            catch { return Json("Error", JsonRequestBehavior.AllowGet); }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> ConfirmOTPTalentSeekar(string userId, string firstname, string lastname, string companyname, string cityid, string email)
        {
            try
            {
                if (!String.IsNullOrEmpty(userId))
                {
                    string code = TempData["UserToken"].ToString();
                    var result = await UserManager.ConfirmEmailAsync(userId, code);

                    if (result.Succeeded)
                    {
                        var userr = await UserManager.FindByIdAsync(userId);
                        await SignInManager.SignInAsync(userr, false, false);

                        var uid = (User.Identity.GetUserId() == null ? userId : User.Identity.GetUserId());
                        tbl_TalentSeeker TalentSeekar = new tbl_TalentSeeker();
                        var Userr = db.AspNetUsers.Where(d => d.Id == uid).FirstOrDefault();
                        TalentSeekar.UserId = userId;
                        TalentSeekar.FName = firstname;
                        TalentSeekar.LName = lastname;
                        TalentSeekar.CompEmail = Userr.Email;
                        TalentSeekar.Email = Userr.Email;
                        TalentSeekar.Mobile = Userr.PhoneNumber;
                        TalentSeekar.CompanyName = companyname;
                        TalentSeekar.StateId = 0;
                        TalentSeekar.IsActive = false;
                        TalentSeekar.IsPremium = false;
                        TalentSeekar.RegDate = DateTime.Now;
                        int cityID = (!String.IsNullOrEmpty(cityid)) ? Convert.ToInt32(cityid) : 0;
                        TalentSeekar.CityId = cityID;
                        db.tbl_TalentSeeker.Add(TalentSeekar);
                        db.SaveChanges();
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Error", JsonRequestBehavior.AllowGet);

            }
            catch { return Json("Error", JsonRequestBehavior.AllowGet); }
        }

        [HttpPost]
        [AllowAnonymous]
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

        [HttpPost]
        [AllowAnonymous]
        public JsonResult ChangeCover(ImageModel img)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var UID = User.Identity.GetUserId();
                    if (!String.IsNullOrEmpty(UID))
                    {
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
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
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
                else
                {
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
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
                    string VideoImg = CustomHelper.GetYouTubeVideoImage(vd.Videolink);
                    return Json(VideoImg, JsonRequestBehavior.AllowGet);
                }
                ViewBag.EntrId = vd.EntrId;
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            { return Json("Fail", JsonRequestBehavior.AllowGet); }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DeleteRegisterImages(string ImageTitle)
        {
            try
            {
                if (!String.IsNullOrEmpty(ImageTitle))
                {
                    var imageList = db.tbl_EntrImages.Where(x => x.Title == ImageTitle).ToList();

                    if (imageList != null && imageList.Count > 0)
                    {
                        if (imageList.Count > 1)
                        {
                            var UserID = User.Identity.GetUserId();
                            var Entertiner = db.tbl_Entertainer.FirstOrDefault(d => d.UserId == UserID);

                            if (Entertiner != null)
                            {
                                var imgResult = db.tbl_EntrImages.FirstOrDefault(x => x.Title == ImageTitle && x.EntrId == Entertiner.EntrId);
                                if (imgResult != null)
                                {
                                    db.tbl_EntrImages.Remove(imgResult);
                                    db.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            db.tbl_EntrImages.Remove(imageList.FirstOrDefault());
                            db.SaveChanges();
                        }
                    }
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("Fail", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult DeleteRegisterVideo(string VideoTitle)
        {
            try
            {
                if (!String.IsNullOrEmpty(VideoTitle))
                {
                    var videoList = db.tbl_EntrVideos.Where(x => x.Title == VideoTitle).ToList();

                    if (videoList != null && videoList.Count > 0)
                    {
                        if (videoList.Count > 1)
                        {
                            var UserID = User.Identity.GetUserId();
                            var Entertiner = db.tbl_Entertainer.FirstOrDefault(d => d.UserId == UserID);

                            if (Entertiner != null)
                            {
                                var videoResult = db.tbl_EntrVideos.FirstOrDefault(x => x.Title == VideoTitle && x.EntrId == Entertiner.EntrId);
                                if (videoResult != null)
                                {
                                    db.tbl_EntrVideos.Remove(videoResult);
                                    db.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            db.tbl_EntrVideos.Remove(videoList.FirstOrDefault());
                            db.SaveChanges();
                        }
                    }
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("Fail", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }


        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> ForgotPassword(string UserEmail, string UserName)
        {
            try
            {
                List<string> results = new List<string>();
                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindByNameAsync(UserName);
                    if (user == null || user.Email != UserEmail)
                    {
                        results.Add("User with this emailid and username is not available.");
                        return Json(results, JsonRequestBehavior.AllowGet);
                    }

                    var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var callbackUrl = this.Url.Action("ResetPassword", "Account", new { UserId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    EmailMessageModel msg = new EmailMessageModel();
                    msg.Destination = user.Email;
                    msg.Subject = "Reset Password";
                    var msgg = "Please reset your password by : <a href=\"" + callbackUrl + "\">clicking here</a>";
                    var filee = Server.MapPath("~/Content/MailTemplate.html");
                    string text = System.IO.File.ReadAllText(filee);
                    text = text.Replace("MailMessage", msgg);
                    msg.Body = text;
                    objEmailServicess.SendByGmail(msg);
                    results.Add("Email sent on your mail id to reset password.");
                    return Json(results, JsonRequestBehavior.AllowGet);
                }

                // If we got this far, something failed, redisplay form
                results.Add("An Error has occured while processing your request!");
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/LogOff
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        //GET: Account/GenerateEmailConfirmToken
        [AllowAnonymous]
        public ActionResult GenerateEmailConfirmToken(string userId)
        {
            try
            {
                var User = UserManager.FindById(userId);
                var token = UserManager.GenerateEmailConfirmationToken(userId);
                var callbackUrl = this.Url.Action("ConfirmEmail", "Account", new { area = "", userId = User.Id, code = token }, protocol: Request.Url.Scheme);
                EmailMessageModel msg = new EmailMessageModel();
                msg.Destination = User.Email;
                msg.Body = "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">Clik here!</a> </br> Or Paste the following link in the Url  </br>" + callbackUrl.Trim();
                msg.Subject = "India Entertainers, Confirm your account";
                var fff = objEmailServicess.SendByGmail(msg);
                ViewBag.UserId = User.Id;
                var ff = objEmailServicess.SendByGmail(msg);
                return View("ConfirmMailSend");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                IdentityResult result = new IdentityResult();
                List<string> results = new List<string>();
                if (ModelState.IsValid)
                {
                    var Psschgresult = UserManager.ChangePassword(User.Identity.GetUserId(), model.OldPassword, model.Password);
                    if (Psschgresult.Succeeded)
                    {
                        results.Add("Your password change successfully!");
                        return Json(results, JsonRequestBehavior.AllowGet);
                    }
                    foreach (var item in Psschgresult.Errors)
                        results.Add(item);
                    return Json(results, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    results.Add("An error occured during process your request, Please try after sometime.");
                    return Json(results, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult Blog()
        {
            try
            {
                var url = ConfigurationManager.AppSettings["blogURL"];
                HttpCookie cookie = new HttpCookie("BlogValue");
                cookie.Value = string.Empty;
                cookie.Expires = DateTime.Now.AddMinutes(1);
                Response.Cookies.Add(cookie);
                return Redirect(url);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}