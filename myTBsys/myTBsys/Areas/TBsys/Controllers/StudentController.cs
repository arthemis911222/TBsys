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
        int pageSize = 4;
        // GET: TBsys/Student
        public ActionResult Index()
        {
            if (Session["person"] == null || (int)Session["type"] != 1)
            {
                return Redirect("/TBsys/Login/Index");
            }

            //判定书籍库存
            if (BookChecked())
                ViewBag.flag = 0;
            else
                ViewBag.flag = 1;

            return View();
        }

        //可选教材列表
        public ActionResult Choose(int flag = 1, string orderField = "Id desc", int pageIndex = 1)
        {

            if (Session["person"] == null || (int)Session["type"] != 1)
            {
                return Redirect("/TBsys/Login/Index");
            }

            if(TimeChecked())
            {
                ViewBag.flag = 1;
            }else
            {
                ViewBag.flag = 0;
            }

            string stuId = (string)Session["Id"];
            T_SH_Student stu = db.T_SH_Student.Find(stuId);
            int classId = stu.ClassId;

            var query = db.T_TB_Choose.Where(m => m.T_TB_TeachingTask.ClassId==classId);


            #region 排序逻辑
            // orderField

            switch (orderField)
            {
                case "Id desc":
                    query = query.OrderByDescending(m => m.Id);
                    break;
                default:
                    break;
            }
            #endregion

            #region 分页实现
            int recordCount = query.Count();
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.recordCount = recordCount;
            #endregion

            ViewBag.list = query.ToList();

            return View();
        }

        //已选教材列表
        public ActionResult GetChoose(string orderField = "Id desc", int pageIndex = 1)
        {
            if (Session["person"] == null || (int)Session["type"] != 1)
            {
                return Redirect("/TBsys/Login/Index");
            }

            if (TimeChecked())
            {
                ViewBag.flag = 1;
            }
            else
            {
                ViewBag.flag = 0;
            }

            string stuId = (string)Session["Id"];

            var query = db.T_TB_StuYuding.Where(m => m.StuId==stuId);

            #region 排序逻辑
            // orderField

            switch (orderField)
            {
                case "Id desc":
                    query = query.OrderByDescending(m => m.Id);
                    break;
                default:
                    break;
            }
            #endregion

            #region 分页实现
            int recordCount = query.Count();
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.recordCount = recordCount;
            #endregion

            ViewBag.list = query.ToList();
            return View();
        }

        public JsonResult AddChoose(string allChoose)
        {
            string stuId = (string)Session["Id"];
            int sum = 0;
            //List<int> iList = new List<int>();
            foreach(char c in allChoose)
            {
                if(c == '&')
                {
                    //iList.Add(sum);
                    T_TB_Choose choose = db.T_TB_Choose.Find(sum);

                    var query = db.T_TB_StuYuding.Where(m => m.StuId == stuId && m.TaskId == choose.TeachingTaskId);

                    if(query.Count() == 0)
                    {
                        T_TB_StuYuding item = new T_TB_StuYuding();
                        item.StuId = stuId;
                        item.BookId = choose.BookId;
                        item.TaskId = choose.TeachingTaskId;
                        item.State = 2;

                        db.T_TB_StuYuding.Add(item);
                        db.SaveChanges();
                    }

                    sum = 0;
                }else
                {
                    sum = sum * 10 + c - '0';
                }

                
            }

            return Json(new { code = 1}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int Id)
        {
            T_TB_StuYuding temp = db.T_TB_StuYuding.Find(Id);
            db.T_TB_StuYuding.Remove(temp);
            db.SaveChanges();

            return RedirectToAction("GetChoose");
        }

        public bool TimeChecked()
        {
            //没有到选教材时间
            T_QT_Other ot = db.T_QT_Other.First();
            DateTime stTime = Convert.ToDateTime(ot.CBStTime);
            DateTime enTime = Convert.ToDateTime(ot.CBEnTime);
            DateTime nowTime = DateTime.Now;

            if(nowTime < stTime || nowTime > enTime)
            {
                //return Json(new { code = 0 },JsonRequestBehavior.AllowGet);
                return false;
            }
            else
            {
                //return Json(new { code = 1 }, JsonRequestBehavior.AllowGet);
                return true;
            }

        }

        public bool BookChecked()
        {
            string id = ((string)Session["id"]);

            //1为库存不足
            var query = db.T_TB_StuYuding.Where(m => m.StuId == id && m.State == 0);

            if (query.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public JsonResult UpdatePwd(string oldpwd,string newpwd,string newpwd2)
        {
            string stuId = (string)Session["Id"];
            T_SH_Student person = db.T_SH_Student.Find(stuId);

            if(!person.Password.Equals(oldpwd))
            {
                return Json(new { code = 1 }, JsonRequestBehavior.AllowGet);
            }
            else if(!newpwd.Equals(newpwd2))
            {
                return Json(new { code = 2}, JsonRequestBehavior.AllowGet);
            }else if(newpwd.Equals("") || newpwd2.Equals(""))
            {
                return Json(new { code = 3 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                person.Password = newpwd;
                db.SaveChanges();
                return Json(new { code = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}