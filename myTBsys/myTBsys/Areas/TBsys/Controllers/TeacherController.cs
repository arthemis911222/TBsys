using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using myTBsys.Models;
using System.Data.Entity;

namespace myTBsys.Areas.TBsys.Controllers
{
    public class TeacherController : Controller
    {
        TBsysEntities db = new TBsysEntities();
        int pageSize = 4;
        // GET: TBsys/Teacher
        public ActionResult Index()
        {
            if (Session["person"] == null || ((int)Session["type"] != 2 && (int)Session["type"] != 3))
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

        public ActionResult Choose(string orderField = "Id desc",int pageIndex = 1)
        {
            if (Session["person"] == null || ((int)Session["type"] != 2 && (int)Session["type"] != 3))
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

            string id = (string)Session["Id"];
            var query = db.T_TB_TeachingTask.Where(m => m.TeacherId == id && m.State == 2);

            #region 排序逻辑
            // orderField

            switch (orderField)
            {
                case "class":
                    query = query.OrderBy(m => m.ClassId);
                    break;
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

        public ActionResult Add(int cId,string bookName,string bookId,string bookAuthor,string Price,string Publisher,int Edition,string Reason,int bookCheck=0)
        {
            //确定书籍书否存在，不存在则添加
            T_TB_Books book = db.T_TB_Books.Find(bookId);
            if(bookId != null && book ==null)
            {
                book = new T_TB_Books();
                book.Id = bookId;
                book.Name = bookName;
                book.Author = bookAuthor;
                book.Price = Convert.ToDecimal(Price);
                book.Publisher = Publisher;
                book.Edition = Edition;

                db.T_TB_Books.Add(book);
            }

            //向教材选定表添加
            T_TB_Choose choose = new T_TB_Choose() ;
            choose.BookId = bookId;
            choose.TeachingTaskId = cId;
            choose.Reason = Reason;
            choose.State = 2;
            db.T_TB_Choose.Add(choose);

            //修改教学任务状态为已经填写
            T_TB_TeachingTask task = db.T_TB_TeachingTask.Find(cId);
            task.State = 4;

            //添加老师预定书
            if(bookCheck == 1)
            {
                T_TB_TeaYuding temp = new T_TB_TeaYuding();
                temp.TeaId = (string)Session["Id"];
                temp.BookId = bookId;
                temp.TaskId = cId;
                db.T_TB_TeaYuding.Add(temp);
            }

            db.SaveChanges();

            return RedirectToAction("GetLIst");
        }

        public ActionResult GetLIst(string orderField = "State desc", int pageIndex = 1)
        {

            if (Session["person"] == null || ((int)Session["type"] != 2 && (int)Session["type"] != 3))
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

            string id = (string)Session["Id"];
            var query = db.T_TB_Choose.Where(m => m.T_TB_TeachingTask.TeacherId == id);

            #region 排序逻辑
            // orderField

            switch (orderField)
            {
                case "class":
                    query = query.OrderBy(m => m.T_TB_TeachingTask.ClassId);
                    break;
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

        public ActionResult SaveEdit(int cId, string bookName, string bookId, string bookAuthor, string Price, string Publisher, int Edition, string Reason, int bookCheck = 0)
        {
            //确定书籍书否存在，不存在则添加,存在则修改信息
            T_TB_Books book = db.T_TB_Books.Find(bookId);
            if (book == null)
            {
                book = new T_TB_Books();
                book.Id = bookId;
                book.Name = bookName;
                book.Author = bookAuthor;
                book.Price = Convert.ToDecimal(Price);
                book.Publisher = Publisher;
                book.Edition = Edition;

                db.T_TB_Books.Add(book);
            }
            else
            {
                book.Name = bookName;
                book.Author = bookAuthor;
                book.Price = Convert.ToDecimal(Price);
                book.Publisher = Publisher;
                book.Edition = Edition;
            }

            //向教材选定表修改
            T_TB_Choose choose = db.T_TB_Choose.Where(m => m.TeachingTaskId == cId).First();
            choose.BookId = bookId;
            choose.Reason = Reason;

            //无库存、不通过状态编辑后,状态改变
            choose.State = 2;

            //添加老师预定书修改
            if (bookCheck == 0)
            {
                string teaId = (string)Session["Id"];
                var query = db.T_TB_TeaYuding.Where(m => m.TeaId == teaId && m.TaskId == cId);
                if (query.Count() != 0)
                {
                    db.T_TB_TeaYuding.Remove(query.First());
                }
            }
            else if (bookCheck == 1)
            {
                string teaId = (string)Session["Id"];
                var query = db.T_TB_TeaYuding.Where(m => m.TeaId == teaId && m.TaskId == cId);

                if (query.Count() == 0)
                {
                    T_TB_TeaYuding temp = new T_TB_TeaYuding();
                    temp.TeaId = (string)Session["Id"];
                    temp.BookId = bookId;
                    temp.TaskId = cId;
                    db.T_TB_TeaYuding.Add(temp);
                }
                else
                {
                    T_TB_TeaYuding temp = query.First();
                    temp.BookId = bookId;
                }
            }

            db.SaveChanges();

            return RedirectToAction("GetLIst");
        }

        public JsonResult YudingCheck(int cId)
        {
            string teaId = (string)Session["Id"];
            var query = db.T_TB_TeaYuding.Where(m => m.TeaId == teaId && m.TaskId == cId);

            if (query.Count() == 0)
            {
                return Json(new { code = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = 1 }, JsonRequestBehavior.AllowGet);
            }

        }

        public bool TimeChecked()
        {
            //没有到选教材时间
            T_QT_Other ot = db.T_QT_Other.First();
            DateTime stTime = Convert.ToDateTime(ot.BIStTime);
            DateTime enTime = Convert.ToDateTime(ot.BIEnTime);
            DateTime nowTime = DateTime.Now;

            if (nowTime < stTime || nowTime > enTime)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public bool BookChecked()
        {
            string id = (string)Session["Id"];

            //1为库存不足
            var query = db.T_TB_Choose.Where(m => m.T_TB_TeachingTask.TeacherId == id && (m.State == 0 || m.State == 1));

            if (query.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

            //foreach(T_TB_StoreTable item in query)
            //{
            //    T_TB_TeachingTask ntask = db.T_TB_TeachingTask.Find(item.TaskId);
            //    //T_TB_Choose nchoose = db.T_TB_Choose.Where(m => m.TeachingTaskId == item.TaskId).First();
            //    //db.T_TB_Choose.Remove(nchoose);
            //    ntask.State = 0;

            //    db.SaveChanges();
            //}
        }

        public JsonResult UpdatePwd(string oldpwd, string newpwd, string newpwd2)
        {
            string stuId = (string)Session["Id"];
            T_SH_Teacher person = db.T_SH_Teacher.Find(stuId);

            if (!person.Password.Equals(oldpwd))
            {
                return Json(new { code = 1 }, JsonRequestBehavior.AllowGet);
            }
            else if (!newpwd.Equals(newpwd2))
            {
                return Json(new { code = 2 }, JsonRequestBehavior.AllowGet);
            }
            else if (newpwd.Equals("") || newpwd2.Equals(""))
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