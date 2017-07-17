using System.Web.Mvc;

namespace IndiaEntertainers.Areas.Entertainer
{
    public class EntertainerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Entertainer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Entertainer_default",
                "Entertainer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}