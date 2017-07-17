using System.Web.Mvc;

namespace IndiaEntertainers.Areas.TalentSeeker
{
    public class TalentSeekerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TalentSeeker";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TalentSeeker_default",
                "TalentSeeker/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}