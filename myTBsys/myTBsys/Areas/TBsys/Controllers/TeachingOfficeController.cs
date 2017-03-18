using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using myTBsys.Models;
using System.Data.Entity;

namespace myTBsys.Areas.TBsys.Controllers
{
    public class TeachingOfficeController : Controller
    {
        // GET: TBsys/TeachingOffice
        TBsysEntities db = new TBsysEntities();
        public ActionResult Index()                                      //登录后首页
        {
            if (Session["person"] == null || (int)Session["type"] != 6)
            {
                return Redirect("/TBsys/Login/Index");
            }

            return View();
        }
        public ActionResult DataIn()                                      //数据导入
        {
            if (Session["person"] == null || (int)Session["type"] != 6)
            {
                return Redirect("/TBsys/Login/Index");
            }

            return View();
        }
        public ActionResult Time()                                      //时间设置
        {
            if (Session["person"] == null || (int)Session["type"] != 6)
            {
                return Redirect("/TBsys/Login/Index");
            }

            var query = db.T_QT_Other;
            List<T_QT_Other> lst = query.ToList();
            ViewBag.lst = lst;
            return View();
        }

        public ActionResult PersonalInfo()                               //个人信息
        {
            if (Session["person"] == null || (int)Session["type"] != 6)
            {
                return Redirect("/TBsys/Login/Index");
            }

            var query = db.T_SH_Teacher;
            List<T_SH_Teacher> lst = query.ToList();
            ViewBag.lst = lst;
            return View();
        }
        public ActionResult TeachingTask()                                 //教学任务信息
        {
            if (Session["person"] == null || (int)Session["type"] != 6)
            {
                return Redirect("/TBsys/Login/Index");
            }

            var query = db.T_TB_TeachingTask.Include(m => m.T_SH_Teacher);
            var query1 = db.T_SH_Class;
            ViewBag.lst = query.ToList();
            ViewBag.lst1 = query1.ToList();
            return View();
        }
        public ActionResult BookMessage()                                  //教材订购信息
        {
            if (Session["person"] == null || (int)Session["type"] != 6)
            {
                return Redirect("/TBsys/Login/Index");
            }

            var query = db.T_TB_Choose.Include(m => m.T_TB_Books);
            var query1 = db.T_TB_TeachingTask;
            var query2 = db.T_TB_TeaYuding;
            var query3 = db.T_TB_StuYuding;
            var query4 = db.T_TB_StoreTable;
            ViewBag.lst = query.ToList();
            ViewBag.lst1 = query1.ToList();
            ViewBag.lst2 = query2.ToList();
            ViewBag.lst3 = query3.ToList();
            ViewBag.lst4 = query4.ToList();
            return View();
        }

        public ActionResult editSave(String Id, string Name, string TelPhone)    //修改个人信息
        {
            T_SH_Teacher item = db.T_SH_Teacher.Find(Id);
            item.Name = Name;
            item.TelPhone = TelPhone;            
            db.SaveChanges();
            return RedirectToAction("PersonalInfo");
        }
        public ActionResult editPwd(String Id, string newpwd)       //修改个人信息密码
        {
            T_SH_Teacher item = db.T_SH_Teacher.Find(Id);
            item.Password = newpwd;
            db.SaveChanges();
            return RedirectToAction("PersonalInfo");
        }

        public ActionResult editBITime(int Id, DateTime BIStTime, DateTime BIEnTime)       //设置教材录入时间
        {
            T_QT_Other item = db.T_QT_Other.Find(Id);
            item.BIStTime = BIStTime;
            item.BIEnTime = BIEnTime;
            db.SaveChanges();
            return RedirectToAction("Time");
        }
        public ActionResult editCKTime(int Id, DateTime CKStTime, DateTime CKEnTime)       //设置系主任审核时间
        {
            T_QT_Other item = db.T_QT_Other.Find(Id);
            item.CKStTime = CKStTime;
            item.CKEnTime = CKEnTime;
            db.SaveChanges();
            return RedirectToAction("Time");
        }
        public ActionResult editCBTime(int Id, DateTime CBStTime, DateTime CBEnTime)       //设置教材订购时间
        {
            T_QT_Other item = db.T_QT_Other.Find(Id);
            item.CBStTime = CBStTime;
            item.CBEnTime = CBEnTime;
            db.SaveChanges();
            return RedirectToAction("Time");
        }

        public ActionResult TeacherIn(string fileName)                //Excel导入教师信息
        {
            myTBsys.TeacherIn teacherIn = new TeacherIn();
            bool res=teacherIn.Click(fileName);
            db.SaveChanges();
            return RedirectToAction("DataIn");
        }
        public ActionResult StudentIn(string fileName)                //Excel导入学生信息
        {
            myTBsys.StudentIn studentIn = new StudentIn();
            bool res=studentIn.Click(fileName);
            db.SaveChanges();
            return RedirectToAction("DataIn");
        }
        public ActionResult TaskIn(string fileName)                //Excel导入教学信息
        {
            myTBsys.TaskIn taskIn = new TaskIn();
            taskIn.Click(fileName);
            db.SaveChanges();
            return RedirectToAction("DataIn");
        }
    }
}