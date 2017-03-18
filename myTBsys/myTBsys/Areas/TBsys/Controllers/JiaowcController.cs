using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace myTBsys.Areas.TBsys.Controllers
{
    public class JiaowcController : Controller
    {

        myTBsys.Models.TBsysEntities db = new Models.TBsysEntities();
        int id = 0;//T_TB_StoreTable表的ID
        // GET: TBsys/Jiaowc
        public ActionResult Index(string searchstring, int pageIndex = 1, int pageSize = 4)
        {
            IEnumerable<myTBsys.Models.T_TB_Books> query = db.T_TB_Books;
            if (String.IsNullOrEmpty(searchstring))
            {

            }
            else
            {
                query = query.Where(m => m.Name.Contains(searchstring));
            }

            //分页
            int recordCount = query.Count();//总数量
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.recordCount = recordCount;
            ViewBag.searchstring = searchstring;

            //获取数据列表
            List<myTBsys.Models.T_TB_Books> lst = query.ToList();
            ViewBag.lst = lst;

            var query1 = db.T_TB_TeachingTask;
            var query2 = db.T_TB_TeaYuding;
            var query3 = db.T_TB_StuYuding;
            var query4 = db.T_QT_Other;
            ViewBag.lst1 = query1.ToList();
            ViewBag.lst2 = query2.ToList();
            ViewBag.lst3 = query3.ToList();
            ViewBag.lst4 = query4.ToList();


            var query5 = db.T_TB_StoreTable;
            var query6 = db.T_TB_Choose;
            List<myTBsys.Models.T_TB_TeaYuding> lst2 = query2.ToList();
            List<myTBsys.Models.T_TB_StuYuding> lst3 = query3.ToList();
            List<myTBsys.Models.T_TB_Choose> lst6 = query6.ToList();

            //int mn = lst6.Count;
            //ViewBag.mn = mn;

            for (int i = 0; i < lst6.Count; i++)
            {
                myTBsys.Models.T_TB_StoreTable item=new Models.T_TB_StoreTable();
                item.Id = id;
                id=id+1;
                item.TaskId = lst6[i].TeachingTaskId;
                item.BookId = lst6[i].BookId;
                item.ClassId = lst6[i].T_TB_TeachingTask.ClassId;
                int num = 0;//书的数量
                for(int j = 0; j< lst3.Count;j++)
                {
                    if(lst6[i].T_TB_TeachingTask.ClassId == lst3[j].T_SH_Student.ClassId &&
                        lst6[i].BookId == lst3[j].BookId)
                    {
                        num++;
                    }
                }
                item.YudingNum = num;
                db.T_TB_StoreTable.Add(item);
                db.SaveChanges();
            }

            return View();
        }

        public ActionResult Edit(String id)
        {
            
            myTBsys.Models.T_TB_Books item = db.T_TB_Books.Find(id);
            //ViewBag.BooksId = new SelectList(db.T_Shop_ProductCategory, "Id", "Name", item.T_Shop_ProductCategory.Id);
            ViewBag.item = item;
            return View();
        }

        [HttpPost]
        //编辑保存
        public ActionResult EditSave(Models.T_TB_Books book)
        {


            myTBsys.Models.T_TB_Books item = db.T_TB_Books.Find(book.Id);
            item.Name = book.Name;
            item.Edition = book.Edition;
            item.Author = book.Author;
            item.Price = book.Price;
            item.Publisher = book.Publisher;
            //item.Store = book.Store;
            //item.Status = book.Status;
            item.Description = book.Description;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}