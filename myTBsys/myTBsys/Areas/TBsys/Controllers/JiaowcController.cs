using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using System.IO;
using NPOI.DDF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;


namespace myTBsys.Areas.TBsys.Controllers
{
    public class JiaowcController : Controller
    {

        myTBsys.Models.TBsysEntities db = new Models.TBsysEntities();
        int id = 0;//T_TB_StoreTable表的ID
        // GET: TBsys/Jiaowc
        public ActionResult Index(string searchstring, int pageIndex = 1, int pageSize = 4)
        {
            if (Session["person"] == null || (int)Session["type"] != 5)
            {
                return Redirect("/TBsys/Login/Index");
            }

            IEnumerable<myTBsys.Models.T_TB_Books> queryall = db.T_TB_Books;//总返回全部的数据
            IEnumerable<myTBsys.Models.T_TB_Books> query = db.T_TB_Books;//可返回删改后的数据

            var query1 = db.T_TB_TeachingTask;
            var query2 = db.T_TB_TeaYuding;
            var query3 = db.T_TB_StuYuding;
            var query4 = db.T_QT_Other;
            var query5 = db.T_TB_StoreTable;
            var query6 = db.T_TB_Choose;
            List<myTBsys.Models.T_TB_TeaYuding> lst2 = query2.ToList();
            List<myTBsys.Models.T_TB_StuYuding> lst3 = query3.ToList();
            List<myTBsys.Models.T_TB_StoreTable> lst5 = query5.ToList();
            List<myTBsys.Models.T_TB_Choose> lst6 = query6.ToList();

            
            if (String.IsNullOrEmpty(searchstring))
            {

            }
            else
            {
                //query = query.Where(m => m.Name.Contains(searchstring));
                query = query.Where(m => m.Publisher.Equals(searchstring));
            }
            List<myTBsys.Models.T_TB_Books> lst = query.ToList();
            //删除book表中有记录但是在choose表中不存在的课本
            for(int i = 0; i < lst.Count; i++)
            {
                int exist = 0;
                for(int j=0;j< lst6.Count; j++)
                {
                    if(lst[i].Id == lst6[j].BookId)
                    {
                        exist = 1;
                        break;
                    }
                }
                if (exist == 0)
                {
                    lst.Remove(lst[i]);
                    i = i - 1;
                }
            }
            int recordCount = lst.Count;
            //分页
            //int recordCount = query.Count();//总数量
            //query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.recordCount = recordCount;
            ViewBag.searchstring = searchstring;

            //返回给select处理显示
            List<myTBsys.Models.T_TB_Books> lstall = queryall.ToList();
            ViewBag.lstall = lstall;
            //获取数据列表
            //List<myTBsys.Models.T_TB_Books> lst = query.ToList();
            ViewBag.lst = lst;

            


            if (lst5.Count == 0)//判断表是否已生成
            {
                for (int i = 0; i < lst6.Count; i++)
                {
                    myTBsys.Models.T_TB_StoreTable item = new Models.T_TB_StoreTable();
                    item.Id = id;
                    id = id + 1;
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
            ViewBag.lst1 = query1.ToList();
            ViewBag.lst2 = query2.ToList();
            ViewBag.lst3 = query3.ToList();
            ViewBag.lst4 = query4.ToList();
            ViewBag.lst5 = query5.ToList();
            ViewBag.lst6 = query6.ToList();

            return View();
        }

        public ActionResult Edit(String id)
        {
            if (Session["person"] == null || (int)Session["type"] != 5)
            {
                return Redirect("/TBsys/Login/Index");
            }

            var query5 = db.T_TB_StoreTable;
            ViewBag.lst5 = query5.ToList();

            myTBsys.Models.T_TB_Books item = db.T_TB_Books.Find(id);
            //ViewBag.BooksId = new SelectList(db.T_Shop_ProductCategory, "Id", "Name", item.T_Shop_ProductCategory.Id);
            ViewBag.item = item;
            return View();
        }

        [HttpPost]
        //编辑保存
        public ActionResult EditSave(Models.T_TB_Books book, /*int State*/string CState)
        {


            myTBsys.Models.T_TB_Books item = db.T_TB_Books.Find(book.Id);

            int State=0;
            if (CState == "失败")
            {
                State = 0;
            }
            if (CState == "成功")
            {
                State = 4;
            }
            if (CState == "初始")
            {
                State = 2;
            }

            //修改Stortable中书状态为无库存
            var query5 = db.T_TB_StoreTable;
            List<myTBsys.Models.T_TB_StoreTable> lst5 = query5.ToList();
            for(int i=0; i < lst5.Count; i++)
            {
                if(lst5[i].BookId == book.Id)
                {
                    lst5[i].State = State;
                }
            }

            //修改StuYuding中学生选书失败
            var query3 = db.T_TB_StuYuding;
            List<myTBsys.Models.T_TB_StuYuding> lst3 = query3.ToList();
            for (int i = 0; i < lst3.Count; i++)
            {
                if (lst3[i].BookId == book.Id)
                {
                    lst3[i].State = State;
                }
            }


            //修改TeaYuding中教师选书失败
            var query2 = db.T_TB_TeaYuding;
            List<myTBsys.Models.T_TB_TeaYuding> lst2 = query2.ToList();
            for (int i = 0; i < lst2.Count; i++)
            {
                if (lst2[i].BookId == book.Id)
                {
                    lst2[i].State = State;
                }
            }

            var query8 = db.T_TB_Choose;
            List<myTBsys.Models.T_TB_Choose> lst8 = query8.ToList();
            for (int i = 0; i < lst8.Count; i++)
            {
                if (lst8[i].BookId == book.Id)
                {
                    if (State == 0)//修改choose中书为无库存
                    {
                        lst8[i].State = 1;
                    }
                    else
                    {
                        lst8[i].State = State;
                    }
                }
            }

            //item.Store = book.Store;
            //item.Status = book.Status;
            //item.Description = book.Description;

            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult ExportExcel(string searchstring/*HttpContext context*/)
        {
            try
            {
                // 1.获取数据集合
                var query5 = db.T_TB_StoreTable;
                var query3 = db.T_TB_StuYuding;
                List<myTBsys.Models.T_TB_StoreTable> lst5 = query5.ToList();

                IEnumerable<myTBsys.Models.T_TB_Books> query = db.T_TB_Books;
                if (String.IsNullOrEmpty(searchstring))
                {

                }
                else
                {
                    //query = query.Where(m => m.Name.Contains(searchstring));
                    query = query.Where(m => m.Publisher.Equals(searchstring));
                }

                List<myTBsys.Models.T_TB_Books> lst = query.ToList();
                List<myTBsys.Models.T_TB_StuYuding> lst3 = query3.ToList();

                List<BookMsg> lstb = new List<BookMsg>();
                var query6 = db.T_TB_Choose;
                List<myTBsys.Models.T_TB_Choose> lst6 = query6.ToList();

                //删除book表中有记录但是在choose表中不存在的课本
                for (int i = 0; i < lst.Count; i++)
                {
                    int exist = 0;
                    for (int j = 0; j < lst6.Count; j++)
                    {
                        if (lst[i].Id == lst6[j].BookId)
                        {
                            exist = 1;
                            break;
                        }
                    }
                    if (exist == 0)
                    {
                        lst.Remove(lst[i]);
                        i = i - 1;
                    }
                }

                for (int i = 0; i < lst.Count; i++)
                {
                    int num = 0;
                    for (int j = 0; j < lst3.Count; j++)
                    {
                        if (lst[i].Id == lst3[j].BookId)
                        {
                            num++;
                        }
                    }
                    lstb.Add(new BookMsg { Name = lst[i].Name, Id = lst[i].Id, Publisher = lst[i].Publisher, Edition = (int)lst[i].Edition, Price = (double)lst[i].Price, Num = num, Total = (double)(num * lst[i].Price) });
                }


                // 2.设置单元格抬头
                // key：实体对象属性名称，可通过反射获取值
                // value：Excel列的名称
                Dictionary<string, string> cellheader = new Dictionary<string, string> {
            { "Name", "书名" },
            { "Id", "书号" },
            { "Publisher", "出版社" },
            { "Edition", "版本" },
            { "Price", "单价" },
            { "Num", "数量" },
            { "Total", "总价" },

            };
                
                // 3.进行Excel转换操作，并返回转换的文件下载链接
                string urlPath = EntityListToExcel2003(cellheader, lstb, searchstring);
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                //context.Response.ContentType = "text/plain";
                //context.Response.Write(js.Serialize(urlPath)); // 返回Json格式的内容
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return RedirectToAction("Index");
        }


        public string EntityListToExcel2003(Dictionary<string, string> cellHeard, List<BookMsg> enList, string sheetName)
        {
            try
            {
                string fileName = sheetName + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls"; // 文件名称
                //string urlPath = "/D:/" + fileName; // 文件下载的URL地址，供给前台下载
                //string filePath = System.Web.HttpContext.Current.Server.MapPath("\\" + urlPath); // 文件路径
                string filePath = "D:/" + fileName;
                // 1.检测是否存在文件夹，若不存在就建立个文件夹
                string directoryName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                // 2.解析单元格头部，设置单元头的中文名称
                HSSFWorkbook workbook = new HSSFWorkbook(); // 工作簿
                ISheet sheet = workbook.CreateSheet(sheetName); // 工作表
                IRow row = sheet.CreateRow(0);
                List<string> keys = cellHeard.Keys.ToList();
                for (int i = 0; i < keys.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(cellHeard[keys[i]]); // 列名为Key的值
                }

                // 3.List对象的值赋值到Excel的单元格里
                int rowIndex = 1; // 从第二行开始赋值(第一行已设置为单元头)
                foreach (var en in enList)
                {
                    IRow rowTmp = sheet.CreateRow(rowIndex);
                    for (int i = 0; i < keys.Count; i++) // 根据指定的属性名称，获取对象指定属性的值
                    {
                        string cellValue = ""; // 单元格的值
                        object properotyValue = null; // 属性的值
                        System.Reflection.PropertyInfo properotyInfo = null; // 属性的信息

                        // 3.1 若属性头的名称包含'.',就表示是子类里的属性，那么就要遍历子类，eg：UserEn.UserName
                        if (keys[i].IndexOf(".") >= 0)
                        {
                            // 3.1.1 解析子类属性(这里只解析1层子类，多层子类未处理)
                            string[] properotyArray = keys[i].Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                            string subClassName = properotyArray[0]; // '.'前面的为子类的名称
                            string subClassProperotyName = properotyArray[1]; // '.'后面的为子类的属性名称
                            System.Reflection.PropertyInfo subClassInfo = en.GetType().GetProperty(subClassName); // 获取子类的类型
                            if (subClassInfo != null)
                            {
                                // 3.1.2 获取子类的实例
                                var subClassEn = en.GetType().GetProperty(subClassName).GetValue(en, null);
                                // 3.1.3 根据属性名称获取子类里的属性类型
                                properotyInfo = subClassInfo.PropertyType.GetProperty(subClassProperotyName);
                                if (properotyInfo != null)
                                {
                                    properotyValue = properotyInfo.GetValue(subClassEn, null); // 获取子类属性的值
                                }
                            }
                        }
                        else
                        {
                            // 3.2 若不是子类的属性，直接根据属性名称获取对象对应的属性
                            properotyInfo = en.GetType().GetProperty(keys[i]);
                           
                            if (properotyInfo != null)
                            {
                                properotyValue = properotyInfo.GetValue(en, null);
                            }
                        }

                        // 3.3 属性值经过转换赋值给单元格值
                        if (properotyValue != null)
                        {
                            cellValue = properotyValue.ToString();
                            // 3.3.1 对时间初始值赋值为空
                            if (cellValue.Trim() == "0001/1/1 0:00:00" || cellValue.Trim() == "0001/1/1 23:59:59")
                            {
                                cellValue = "";
                            }
                        }

                        // 3.4 填充到Excel的单元格里
                        rowTmp.CreateCell(i).SetCellValue(cellValue);
                    }
                    rowIndex++;
                }

                // 4.生成文件
                FileStream file = new FileStream(filePath, FileMode.Create);
                workbook.Write(file);
                file.Close();

                // 5.返回下载路径
                return filePath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

    public class BookMsg
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public int Num { get; set; }
        public int Edition { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        
    }
}