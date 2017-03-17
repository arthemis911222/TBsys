using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace myTBsys.Areas.TBsys.Controllers
{
    public class jiaockController : Controller
    {
        myTBsys.Models.textbook_dbaEntities1 db = new Models.textbook_dbaEntities1();
        // GET: TBsys/jiaock
        public ActionResult Index(string searchstring, int pageIndex = 1, int pageSize = 4)
        {
            IEnumerable<myTBsys.Models.T_TB_Fenfa> query = db.T_TB_Fenfa;
            if (String.IsNullOrEmpty(searchstring))
            {

            }
            else
            {
                query = query.Where(m => m.BookId.Contains(searchstring));
            }

            //分页
            int recordCount = query.Count();//总数量
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.recordCount = recordCount;
            ViewBag.searchstring = searchstring;

            //获取数据列表
            List<myTBsys.Models.T_TB_Fenfa> lst = query.ToList();
            ViewBag.lst = lst;

            return View();
        }

        public ActionResult Edit(String id)
        {

            myTBsys.Models.T_TB_Fenfa item = db.T_TB_Fenfa.Find(id);
            //ViewBag.BookId = new SelectList(db.T_TB_Books, "Id", "Name", item.T_TB_Books.Id);
            ViewBag.item = item;
            return View();
        }

        [HttpPost]
        //编辑保存
        public ActionResult EditSave(Models.T_TB_Fenfa book)
        {


            myTBsys.Models.T_TB_Fenfa item = db.T_TB_Fenfa.Find(book.Id);
            item.BookId = book.BookId;
            item.BookNum = book.BookNum;
            item.ShouldPay = book.ShouldPay;
            item.RealPay = book.RealPay;
            item.Time = book.Time;
            item.StuId = book.StuId;

            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Search(string searchstring, int pageIndex = 1, int pageSize = 4)
        {
            IEnumerable<myTBsys.Models.T_TB_StoreTable> query = db.T_TB_StoreTable;
            if (String.IsNullOrEmpty(searchstring))
            {

            }
            else
            {
                query = query.Where(m => m.T_SH_Class.Name.Contains(searchstring));
            }

            //分页
            int recordCount = query.Count();//总数量
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.recordCount = recordCount;
            ViewBag.searchstring = searchstring;

            //获取数据列表
            List<myTBsys.Models.T_TB_StoreTable> lst = query.ToList();
            ViewBag.lst = lst;

            var query1 = db.T_TB_TeachingTask;
            var query2 = db.T_TB_TeaYuding;
            var query3 = db.T_TB_StuYuding;
            ViewBag.lst1 = query1.ToList();
            ViewBag.lst2 = query2.ToList();
            ViewBag.lst3 = query3.ToList();

            return View();
        }
    }
}