using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using myTBsys.Models;
using System.Data.Entity;
using System.Web.UI;
using System.Windows.Forms;

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
            //Session["result"] = 1;
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
            Session["result"] = 1;
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

            var query = db.T_SH_Workers;
            List<T_SH_Workers> lst = query.ToList();
            ViewBag.lst = lst;
            return View();
        }
        public ActionResult TeachingTask(string searchstring, string orderField = "Id desc", int pageIndex = 1)                                 //教学任务信息
        {
            if (Session["person"] == null || (int)Session["type"] != 6)
            {
                return Redirect("/TBsys/Login/Index");
            }

            int pageSize = 10;
            IEnumerable<T_TB_TeachingTask> query = db.T_TB_TeachingTask.Include(m => m.T_SH_Teacher); 
            #region 查询逻辑
            if (String.IsNullOrEmpty(searchstring))
            {

            }
            else
            {
                query = query.Where(m => m.T_SH_Class.Name.Contains(searchstring));
            }
            #endregion
            #region 排序逻辑
            switch (orderField)
            {
                case "className":
                    query = query.OrderBy(m => m.T_SH_Class.Name);
                    break;
                case "Id desc":
                    query = query.OrderByDescending(m => m.Id);
                    break;
                case "StuNum":
                    query = query.OrderByDescending(m => m.StuNum);
                    break;
                case "Semeter":
                    query = query.OrderBy(m => m.Semeter);
                    break;
                case "CourseName":
                    query = query.OrderBy(m => m.CourseName);
                    break;
                case "WeekTime":
                    query = query.OrderBy(m => m.WeekTime);
                    break;
                case "Score":
                    query = query.OrderByDescending(m => m.Score);
                    break;
                case "CourseType":
                    query = query.OrderBy(m => m.CourseType);
                    break;
                case "TotalTime":
                    query = query.OrderBy(m => m.TotalTime);
                    break;
                case "LectureTime":
                    query = query.OrderBy(m => m.LectureTime);
                    break;
                case "MachineTime":
                    query = query.OrderBy(m => m.MachineTime);
                    break;
                case "SEweek":
                    query = query.OrderBy(m => m.SEweek);
                    break;
                case "TeacherName":
                    query = query.OrderBy(m => m.T_SH_Teacher.Name);
                    break;
                case "State":
                    query = query.OrderByDescending(m => m.State);
                    break;
                case "DepartmentName":
                    query = query.OrderBy(m => m.T_SH_Department.Name);
                    break;
                case "AcademyName":
                    query = query.OrderBy(m => m.T_SH_Academy.Name);
                    break;
                default:
                    break;
            }
            #endregion
            int recordCount = query.Count();
            #region 读取指定页数据
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            #endregion
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.recordCount = recordCount;
            List<T_TB_TeachingTask> lst = query.ToList();
            var query1 = db.T_SH_Class;
            ViewBag.lst1 = query1.ToList();
            ViewBag.lst = lst;
            return View();
        }
        public ActionResult BookMessage(string searchstring, string orderField = "Id desc", int pageIndex = 1)                                  //教材订购信息
        {
            if (Session["person"] == null || (int)Session["type"] != 6)
            {
                return Redirect("/TBsys/Login/Index");
            }
            int pageSize = 10;
            IEnumerable<T_TB_Choose> query = db.T_TB_Choose.Include(m => m.T_TB_Books);
            var query1 = db.T_TB_TeachingTask;
            var query2 = db.T_TB_TeaYuding;
            var query3 = db.T_TB_StuYuding;
            IEnumerable<T_TB_StoreTable>  query4 = db.T_TB_StoreTable;

            #region 查询逻辑
            if (String.IsNullOrEmpty(searchstring))
            {

            }
            else
            {
                query = query.Where(m => m.T_TB_TeachingTask.T_SH_Class.Name.Contains(searchstring));
            }
            #endregion
            #region 排序逻辑
            switch (orderField)
            {
                case "ClassName":
                    query = query.OrderBy(m => m.T_TB_TeachingTask.T_SH_Class.Name);
                    break;
                case "Id desc":
                    query = query.OrderByDescending(m => m.Id);
                    break;                
                case "BookName":
                    query = query.OrderBy(m => m.T_TB_Books.Name);
                    break;
                case "CourseName":
                    query = query.OrderBy(m => m.T_TB_TeachingTask.CourseName);
                    break;
                case "Author":
                    query = query.OrderBy(m => m.T_TB_Books.Author);
                    break;
                case "Price":
                    query = query.OrderByDescending(m => m.T_TB_Books.Price);
                    break;
                case "BookNum":
                    query = query.OrderBy(m => m.T_TB_Books.Id);
                    break;
                case "Publisher":
                    query = query.OrderBy(m => m.T_TB_Books.Publisher);
                    break;
                case "Edition":
                    query = query.OrderBy(m => m.T_TB_Books.Edition);
                    break;
                case "Store":
                    query4 = query4.OrderBy(m => m.State);
                    break;                
                case "State":
                    query = query.OrderByDescending(m => m.State);
                    break;               
                default:
                    break;
            }
            #endregion
            int recordCount = query.Count();
            #region 读取指定页数据
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            #endregion
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.recordCount = recordCount;
            List<T_TB_Choose> lst = query.ToList();

            ViewBag.lst = lst;
            ViewBag.lst1 = query1.ToList();
            ViewBag.lst2 = query2.ToList();
            ViewBag.lst3 = query3.ToList();
            ViewBag.lst4 = query4.ToList();
            return View();
         
        }

        public ActionResult Edit(int Id)                                  //编辑教材订购信息
        {
            if (Session["person"] == null || (int)Session["type"] != 6)
            {
                return Redirect("/TBsys/Login/Index");
            }
            T_TB_Choose temp = db.T_TB_Choose.Find(Id);
            ViewBag.item = temp;
            

            return View();
        }
        public ActionResult editBook(int Id, string CourseName, string bookName, string bookId, string Author,decimal Price, string Publisher,int Edition)    //修改订书信息
        {
            T_TB_Choose item = db.T_TB_Choose.Find(Id);
            T_TB_TeachingTask item2 = db.T_TB_TeachingTask.Find(item.TeachingTaskId);
            T_TB_Books item3 = db.T_TB_Books.Find(bookId);
            item2.CourseName = CourseName;
            item3.Id = bookId;
            item3.Name = bookName;
            item3.Author = Author;
            item3.Price = Price;
            item3.Publisher = Publisher;
            item3.Edition = Edition;
            db.SaveChanges();
            return RedirectToAction("BookMessage");
        }
        public ActionResult editSave(string Id, string Des, string TelPhone)    //修改个人信息
        {
            T_SH_Workers item = db.T_SH_Workers.Find(Id);
            item.Des = Des;
            item.TelPhone = TelPhone;            
            db.SaveChanges();

            return RedirectToAction("PersonalInfo");
        }
        public ActionResult editPwd(string Id, string newpwd)       //修改个人信息密码
        {
            T_SH_Workers item = db.T_SH_Workers.Find(Id);
            item.Password = newpwd;
            db.SaveChanges();
            return RedirectToAction("PersonalInfo");
        }

        public ActionResult editBITime(int Id, String BIStTime, String BIEnTime)       //设置教材录入时间
        {
            T_QT_Other item = db.T_QT_Other.Find(Id);
            DateTime BlStTime= Convert.ToDateTime(BIStTime);
            DateTime BlEnTime= Convert.ToDateTime(BIEnTime);
            item.BIStTime = BlStTime;
            item.BIEnTime = BlEnTime;
            db.SaveChanges();
            //Session["result"] = 1;
            return RedirectToAction("Time");
        }
        public ActionResult editCKTime(int Id, String CKStTime, String CKEnTime)       //设置系主任审核时间
        {
            T_QT_Other item = db.T_QT_Other.Find(Id);
            DateTime CkStTime = Convert.ToDateTime(CKStTime);
            DateTime CkEnTime = Convert.ToDateTime(CKEnTime);
            item.CKStTime = CkStTime;
            item.CKEnTime = CkEnTime;
            db.SaveChanges();

            return RedirectToAction("Time");
        }
        public ActionResult editCBTime(int Id, String CBStTime, String CBEnTime)       //设置教材订购时间
        {
            T_QT_Other item = db.T_QT_Other.Find(Id);
            DateTime CbStTime = Convert.ToDateTime(CBStTime);
            DateTime CbEnTime = Convert.ToDateTime(CBEnTime);
            item.CBStTime = CbStTime;
            item.CBEnTime = CbEnTime;
            db.SaveChanges();
            return RedirectToAction("Time");
        }

        public ActionResult TeacherIn(String fileName)                //Excel导入教师信息
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