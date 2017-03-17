using System.Web.Mvc;

namespace myTBsys.Areas.TBsys
{
    public class TBsysAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TBsys";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TBsys_default",
                "TBsys/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}