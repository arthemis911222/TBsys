using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using myTBsys.Models;

namespace myTBsys.Areas.TBsys.Controllers
{
    public class DeanController : Controller
    {
        TBsysEntities db = new TBsysEntities();
        int pageSize = 4;
        // GET: TBsys/Dean

        public ActionResult Index()
        {
            //判定书籍库存
            if (BookChecked())
                ViewBag.flag = 0;
            else
                ViewBag.flag = 1;

            return View();
        }

        public ActionResult DeanIndex(string searchName,string orderField = "State desc", int pageIndex = 1)
        {
            if (Session["type"] == null || (int)Session["type"] != 3)
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

            T_SH_Teacher teacher = (T_SH_Teacher)Session["person"];
            IEnumerable<T_TB_Choose> query = db.T_TB_Choose.Where(m => m.T_TB_TeachingTask.DepartmentId == teacher.DepartmentId);

            if (string.IsNullOrEmpty(searchName))
            {
                
            }
            else
            {
                query = query.Where(m => m.T_TB_TeachingTask.CourseName.Contains(searchName));
            }

            #region 排序逻辑
            // orderField

            switch (orderField)
            {
                case "Id desc":
                    query = query.OrderByDescending(m => m.Id);
                    break;
                case "State desc":
                    query = query.OrderBy(m => m.State);
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

        public ActionResult CheckChoose(int choose,int id)
        {
            T_TB_Choose temp = db.T_TB_Choose.Find(id);
            temp.State = choose;
            temp.StateReason = "";
            db.SaveChanges();

            return RedirectToAction("DeanIndex");
        }

        public JsonResult CheckChoose2(int choose, int id, string reason)
        {
            T_TB_Choose temp = db.T_TB_Choose.Find(id);

            if(temp == null)
                return Json(new { code = 0 }, JsonRequestBehavior.AllowGet);

            temp.State = choose;
            temp.StateReason = reason;
            db.SaveChanges();

            return Json(new{ code = 1 },JsonRequestBehavior.AllowGet);
        }

        public ActionResult NoBookList(string orderField = "Index desc", int pageIndex = 1)
        {
            if (Session["type"] == null || (int)Session["type"] != 3)
            {
                return Redirect("/TBsys/Login/Index");
            }

            T_SH_Teacher teacher = (T_SH_Teacher)Session["person"];
            var query = db.T_TB_TeachingTask.Where(m => m.DepartmentId == teacher.DepartmentId && m.State != 4);

            #region 排序逻辑
            // orderField

            switch (orderField)
            {
                case "Index desc":
                    query = query.OrderBy(m => m.TeacherId);
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

        public bool TimeChecked()
        {
            //没有到选教材时间
            T_QT_Other ot = db.T_QT_Other.First();
            DateTime stTime = Convert.ToDateTime(ot.CKStTime);
            DateTime enTime = Convert.ToDateTime(ot.CKEnTime);
            DateTime nowTime = DateTime.Now;

            if (nowTime < stTime || nowTime > enTime)
            {
                return false;
            }else
            {
                return true;
            }

        }

        public bool BookChecked()
        {
            int id = ((T_SH_Teacher)Session["Person"]).DepartmentId;

            //1为库存不足
            var query = db.T_TB_Choose.Where(m => m.T_TB_TeachingTask.DepartmentId == id && (m.State == 0 || m.State == 1));

            if (query.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}