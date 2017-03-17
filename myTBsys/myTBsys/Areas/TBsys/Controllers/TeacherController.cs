using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace myTBsys.Areas.TBsys.Controllers
{
    public class TeacherController : Controller
    {
        // GET: TBsys/Teacher
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OwnerMessage()
        {
            return View();
        }
        public ActionResult TeachingTask()
        {
            return View();
        }
        public ActionResult BookMessage()
        {
            return View();
        }
    }
}