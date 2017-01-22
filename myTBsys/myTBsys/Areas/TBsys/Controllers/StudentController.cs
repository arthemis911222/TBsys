using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using myTBsys.Models;

namespace myTBsys.Areas.TBsys.Controllers
{
    public class StudentController : Controller
    {
        TBsysEntities db = new TBsysEntities();
        // GET: TBsys/Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Choose()
        {
            

            return View();
        }
    }
}