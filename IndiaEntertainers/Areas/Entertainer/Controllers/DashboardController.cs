using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using IndiaEntertainers.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using System.IO;

namespace IndiaEntertainers.Areas.Entertainer
{
    public class DashboardController : Controller
    {
        private IEDBContext db = new IEDBContext();

        // GET: Entertainer/Dashboard
        public ActionResult Index()
        {
            return View();
        }

    }
}