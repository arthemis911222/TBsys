using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace myTBsys.Areas.TBsys.Controllers
{

    public class jiaockController : Controller
    {
        myTBsys.Models.TBsysEntities db = new Models.TBsysEntities();
        int id_ST = 0;//T_TB_StoreTable表的ID
        int id_FF = 0;//T_TB_FenFa表的ID
        // GET: TBsys/jiaock
        public ActionResult Index(string searchAC, string searchClass, int pageIndex = 1, int pageSize = 4)
        {
            if (Session["person"] == null || (int)Session["type"] != 4)
            {
                return Redirect("/TBsys/Login/Index");
            }

            IEnumerable<myTBsys.Models.T_TB_Fenfa> query = db.T_TB_Fenfa;
            if (String.IsNullOrEmpty(searchAC))
            {

            }
            else
            {
                //query = query.Where(m => m.BookId.Contains(searchstring));
                query = query.Where(m => m.T_SH_Class.T_SH_Major.T_SH_Department.T_SH_Academy.Name.Equals(searchAC));
                if (String.IsNullOrEmpty(searchClass))
                {

                }
                else
                {
                    query = query.Where(m => m.T_SH_Class.Name.Equals(searchClass));
                }
            }

            //分页
            int recordCount = query.Count();//总数量
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.recordCount = recordCount;
            // ViewBag.searchstring = searchstring;
            ViewBag.searchAC = searchAC;
            ViewBag.searchClass = searchClass;


            //获取数据列表
            List<myTBsys.Models.T_TB_Fenfa> lst = query.ToList();
            ViewBag.lst = lst;


            var query1 = db.T_TB_TeachingTask;
            var query2 = db.T_TB_TeaYuding;
            var query3 = db.T_TB_StuYuding;
            var query4 = db.T_QT_Other;
            var query5 = db.T_TB_StoreTable;
            var query6 = db.T_TB_Choose;
            var query7 = db.T_TB_Fenfa;
            var query8 = db.T_SH_Academy;
            var query9 = db.T_SH_Class;
            List<myTBsys.Models.T_TB_TeaYuding> lst2 = query2.ToList();
            List<myTBsys.Models.T_TB_StuYuding> lst3 = query3.ToList();
            List<myTBsys.Models.T_QT_Other> lst4 = query4.ToList();
            List<myTBsys.Models.T_TB_StoreTable> lst5 = query5.ToList();
            List<myTBsys.Models.T_TB_Choose> lst6 = query6.ToList();
            List<myTBsys.Models.T_TB_Fenfa> lst7 = query7.ToList();
            List<myTBsys.Models.T_SH_Academy> lst8 = query8.ToList();

            if (lst5.Count == 0)//判断StoreTable表是否已生成
            {
                for (int i = 0; i < lst6.Count; i++)
                {
                    myTBsys.Models.T_TB_StoreTable item = new Models.T_TB_StoreTable();
                    item.Id = id_ST;
                    id_ST = id_ST + 1;
                    item.TaskId = lst6[i].TeachingTaskId;
                    item.BookId = lst6[i].BookId;
                    item.ClassId = lst6[i].T_TB_TeachingTask.ClassId;
                    item.State = 2;
                    int num = 0;//书的数量
                    for (int j = 0; j < lst3.Count; j++)
                    {
                        if (lst6[i].T_TB_TeachingTask.ClassId == lst3[j].T_SH_Student.ClassId &&
                            lst6[i].BookId == lst3[j].BookId)
                        {
                            num++;
                        }
                    }
                    item.YudingNum = num;
                    db.T_TB_StoreTable.Add(item);
                    db.SaveChanges();
                }

            }

            if(lst7.Count == 0)//判断分发表是否已生成
            {
                for (int i = 0; i < lst5.Count; i++)
                {
                    myTBsys.Models.T_TB_Fenfa item = new Models.T_TB_Fenfa();
                    //item.Id = id_FF;
                    //id_FF=id_FF+1;
                    item.BookId = lst5[i].BookId;
                    item.ClassId = (int)lst5[i].ClassId;
                    item.BookNum = lst5[i].YudingNum;
                    item.RealPay = lst5[i].YudingNum * (double)lst5[i].T_TB_Books.Price * (double)lst4[0].Discount;
                    item.State = 2;
                    item.StuId = "14211160237";
                    db.T_TB_Fenfa.Add(item);
                    db.SaveChanges();
                }

            }

            ViewBag.lst1 = query1.ToList();
            ViewBag.lst2 = query2.ToList();
            ViewBag.lst3 = query3.ToList();
            ViewBag.lst4 = query4.ToList();
            ViewBag.lst8 = query8.ToList();
            ViewBag.lst9 = query9.ToList();

            return View();
        }

        public ActionResult Edit(int id)
        {
            if (Session["person"] == null || (int)Session["type"] != 4)
            {
                return Redirect("/TBsys/Login/Index");
            }

            myTBsys.Models.T_TB_Fenfa item = db.T_TB_Fenfa.Find(id);
            //ViewBag.BookId = new SelectList(db.T_TB_Books, "Id", "Name", item.T_TB_Books.Id);
            ViewBag.item = item;
            return View();
        }

        [HttpPost]
        //编辑保存
        public ActionResult EditSave(Models.T_TB_Fenfa book,string CState)
        {


            myTBsys.Models.T_TB_Fenfa item = db.T_TB_Fenfa.Find(book.Id);
            //item.BookId = book.BookId;
            //item.BookNum = book.BookNum;
            //item.RealPay = book.RealPay;
            //item.Time = book.Time;
            
            int State = 2;
            if(CState == "否")
            {
                State = 2;
            }
            if (CState == "是")
            {
                State = 4;
            }
            item.StuId = book.StuId;
            item.State = State;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Search(string searchAC, string searchClass, int pageIndex = 1, int pageSize = 4)
        {
            if (Session["person"] == null || (int)Session["type"] != 4)
            {
                return Redirect("/TBsys/Login/Index");
            }

            IEnumerable<myTBsys.Models.T_TB_StoreTable> query = db.T_TB_StoreTable;
            if (String.IsNullOrEmpty(searchAC))
            {

            }
            else
            {
                query = query.Where(m => m.T_SH_Class.T_SH_Major.T_SH_Department.T_SH_Academy.Name.Equals(searchAC));
                if (String.IsNullOrEmpty(searchClass))
                {

                }
                else
                {
                    query = query.Where(m => m.T_SH_Class.Name.Equals(searchClass));
                }
            }

            //分页
            int recordCount = query.Count();//总数量
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.recordCount = recordCount;
            ViewBag.searchAC = searchAC;
            ViewBag.searchClass = searchClass;
            //获取数据列表
            List<myTBsys.Models.T_TB_StoreTable> lst = query.ToList();
            ViewBag.lst = lst;

            var query1 = db.T_TB_TeachingTask;
            var query2 = db.T_TB_TeaYuding;
            var query3 = db.T_TB_StuYuding;
            var query4 = db.T_QT_Other;
            var query8 = db.T_SH_Academy;
            var query9 = db.T_SH_Class;
            ViewBag.lst1 = query1.ToList();
            ViewBag.lst2 = query2.ToList();
            ViewBag.lst3 = query3.ToList();
            ViewBag.lst4 = query4.ToList();
            ViewBag.lst8 = query8.ToList();
            ViewBag.lst9 = query9.ToList();
            return View();
        }

        public ActionResult TeaSearch(string searchAC, string searchDEP, int pageIndex = 1, int pageSize = 4)
        {
            if (Session["person"] == null || (int)Session["type"] != 4)
            {
                return Redirect("/TBsys/Login/Index");
            }

            IEnumerable<myTBsys.Models.T_TB_TeaYuding> query2 = db.T_TB_TeaYuding;
            if (String.IsNullOrEmpty(searchAC))
            {

            }
            else
            {
                query2 = query2.Where(m => m.T_SH_Teacher.T_SH_Department.T_SH_Academy.Name.Equals(searchAC));
                if (String.IsNullOrEmpty(searchDEP))
                {

                }
                else
                {
                    query2 = query2.Where(m => m.T_SH_Teacher.T_SH_Department.Name.Equals(searchDEP));
                }
            }

            //分页
            int recordCount = query2.Count();//总数量
            query2 = query2.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.recordCount = recordCount;
            ViewBag.searchAC = searchAC;
            ViewBag.searchDEP = searchDEP;
            //获取数据列表
            List<myTBsys.Models.T_TB_TeaYuding> lst2 = query2.ToList();
            ViewBag.lst2 = lst2;

            var query1 = db.T_TB_TeachingTask;
            var query3 = db.T_TB_StuYuding;
            var query4 = db.T_QT_Other;
            var query8 = db.T_SH_Academy;
            var query10 = db.T_SH_Department;
            ViewBag.lst1 = query1.ToList();
            ViewBag.lst3 = query3.ToList();
            ViewBag.lst4 = query4.ToList();
            ViewBag.lst8 = query8.ToList();
            ViewBag.lst10 = query10.ToList();
            return View();
        }
    }
}