using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using myTBsys.Models;

namespace myTBsys.Areas.TBsys.Controllers
{
    public class LoginController : Controller
    {
        TBsysEntities db = new TBsysEntities();
        // GET: TBsys/Login
        public ActionResult Index()
        {
            Session["person"] = null;
            Session["type"] = null;
            Session["name"] = null;
            Session["Id"] = null;
            return View();
        }

        public JsonResult LoginCheck(string Name, string password,string checkres)
        {
            if(checkres == "学生")
            {
                IEnumerable<T_SH_Student> students = db.T_SH_Student.Where(m => m.Id.Equals(Name));

                if (students.Count() == 0)
                {
                    return Json(new { code = 2, message = "用户名错误" }, JsonRequestBehavior.AllowGet);
                }
                else if (students.First().Password == password)
                {
                    Session["person"] = students.First();
                    Session["type"] = 1;
                    Session["name"] = students.First().Name;
                    Session["Id"] = students.First().Id;
                    return Json(new { code = 11, message = "登录成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 3, message = "密码错误" }, JsonRequestBehavior.AllowGet);
                }
            }else if(checkres == "老师")
            {
                IEnumerable<T_SH_Teacher> teachers = db.T_SH_Teacher.Where(m => m.Id.Equals(Name));

                if (teachers.Count() == 0)
                {
                    return Json(new { code = 2, message = "用户名错误" }, JsonRequestBehavior.AllowGet);
                }
                else if (teachers.First().Password == password)
                {
                    Session["person"] = teachers.First();
                    Session["name"] = teachers.First().Name;
                    Session["Id"] = teachers.First().Id;

                    if (teachers.First().IsHead == 0)
                    {
                        Session["type"] = 2;
                        return Json(new { code = 12, message = "登录成功" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        Session["type"] = 3;
                        return Json(new { code = 13, message = "登录成功" }, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    return Json(new { code = 3, message = "密码错误" }, JsonRequestBehavior.AllowGet);
                }
            }else
            {
                IEnumerable<T_SH_Workers> workers = db.T_SH_Workers.Where(m => m.Id.Equals(Name));

                if (workers.Count() == 0)
                {
                    return Json(new { code = 2, message = "用户名错误" }, JsonRequestBehavior.AllowGet);
                }
                else if (workers.First().Password == password)
                {
                    Session["person"] = workers.First();
                    Session["name"] = workers.First().Des;
                    if (workers.First().Identity == 1)
                    {
                        Session["type"] = 4;
                        return Json(new { code = 14, message = "登录成功" }, JsonRequestBehavior.AllowGet);

                    }
                    else if(workers.First().Identity == 2)
                    {
                        Session["type"] = 5;
                        return Json(new { code = 15, message = "登录成功" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        Session["type"] = 6;
                        return Json(new { code = 16, message = "登录成功" }, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    return Json(new { code = 3, message = "密码错误" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}