﻿using System;
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
                    return Json(new { code = 12, message = "登录成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 3, message = "密码错误" }, JsonRequestBehavior.AllowGet);
                }
            }else
            {
                IEnumerable<T_SH_Workers> workers = db.T_SH_Workers.Where(m => m.WorkId.Equals(Name));

                if (workers.Count() == 0)
                {
                    return Json(new { code = 2, message = "用户名错误" }, JsonRequestBehavior.AllowGet);
                }
                else if (workers.First().Password == password)
                {
                    Session["person"] = workers.First();
                    return Json(new { code = 13, message = "登录成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 3, message = "密码错误" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}