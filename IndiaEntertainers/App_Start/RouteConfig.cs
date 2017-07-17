using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IndiaEntertainers
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //routes.MapRoute(
            //      name: "Extra",
            //      url: "controller/action/{p1}/{p2}/{p3}",
            //      defaults: new { controller = "Home", action = "AddSlugs" },
            //      constraints: new { action = "AddSlugs" }
            //  );

            routes.MapRoute(
                 name: "ProfileDetails",
                 url: "Entertainers/{cat}/{id}",
                 defaults: new { controller = "Entertainers", action = "ProfileDetail" },
                 constraints: new { action = "ProfileDetail" }
             );

            routes.MapRoute(
                  name: "IndexSearch",
                  url: "Entertainers/{id}",
                  defaults: new { controller = "Entertainers", action = "Index" },
                  constraints: new { action = "Index" }
              );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
