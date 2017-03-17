using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace myTBsys
{
    public class TeacherIn
    {
        bool result ;
        public bool Click(String fileName)
        {
                DataTable dt = new DataTable();//创建一个DataTable表，用于存储从excel表中读取的数据              
                bind(dt, fileName);//excel表中数据导入到DataTable中过程函数 
                return result;            

        }
        private void bind(DataTable table, string fileName)
        {
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                "Data Source=" + fileName + ";" +
                "Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";

            String excelStr = String.Format("select * from [教师信息$]");//这里是sheet表名  
            OleDbDataAdapter da = new OleDbDataAdapter(excelStr, strConn);
            DataSet ds = new DataSet();//创建数据集，数据集这里你可以看作包含多个数据表  
            try
            {
                da.Fill(ds);//将excel表中的数据保存到DataTable中。  
                table = ds.Tables[0];
                //this.dataGridView1.DataSource = dt;  
            }
            catch (Exception err)
            {
                result = false;//
            }

            foreach (DataRow dr in table.Rows)
            {
                insertToSql(dr);//此函数是将数据表table中的数据一行行的插入到sqlserver中。  
            }
            result = true;
        }

        private void insertToSql(DataRow dataRow)
        {
            string connString = @"server=121.42.152.172\SQLEXPRESS;uid=wjj;pwd=wujiajie;database=textbook_dba";
            SqlConnection conn = new SqlConnection(connString);//创建连接  
            conn.Open();//打开连接
            String sqlstring = "insert into T_SH_Teacher(Id,Name,Password,TelPhone,IsHead,DepartmentId,MajorId) "
                                 + "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
            try
            {
                 sqlstring = String.Format(sqlstring,
                 Convert.ToString(dataRow["Id"]),
                                            Convert.ToString(dataRow["Name"]),
                                            Convert.ToString(dataRow["Password"]),
                                          Convert.ToString(dataRow["TelPhone"]),
                                          Convert.ToInt16(dataRow["IsHead"]),
                                          Convert.ToInt16(dataRow["DepartmentId"]),
                                          Convert.ToInt16(dataRow["MajorId"]));
                SqlCommand command = new SqlCommand(sqlstring, conn);
                command.ExecuteNonQuery();//执行sql语句
               
                
            }
            catch(Exception ex)
            {
                result = false;
            }
            
             
        }
    }
}