using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SuperMarket
{
    /// <summary>
    /// 数据库帮组类
    /// </summary>
    class DBHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string constr = "Data Source=LAPTOP-PDKJ1K0Q;Initial Catalog=SuperMarketManager;Persist Security Info=True;User ID=sa;Password=cssl#123";

        /// <summary>
        /// 查询单个值
        /// </summary>
        /// <param name="sql">需要执行的sql语句</param>
        /// <returns>数据结果集中的第一行第一列</returns>
        public static object ExecuteScalar(string sql)
        {
            //数据连接对象
            SqlConnection con = new SqlConnection(constr);
            //数据命令对象
            SqlCommand cmd = new SqlCommand(sql, con);

            try
            {
                con.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine("程序错误，错误信息为："+ex.Message);
            }
            finally
            {
                con.Close();
            }
            return null;
        }

        /// <summary>
        /// 读取一行数据
        /// </summary>
        /// <param name="sql">需要执行的sql语句</param>
        /// <returns>读取器</returns>
        public static SqlDataReader ExecuteReader(string sql)
        {
            //数据连接对象
            SqlConnection con = new SqlConnection(constr);
            //数据命令对象
            SqlCommand cmd = new SqlCommand(sql, con);

            try
            {
                con.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Console.WriteLine("程序错误，错误信息为：" + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 实现数据增删改
        /// </summary>
        /// <param name="sql">需要执行的sql语句</param>
        /// <returns>true：执行成功  false：执行失败</returns>
        public static bool ExecuteNonQuery(string sql)
        {
            //数据连接对象
            SqlConnection con = new SqlConnection(constr);
            //数据命令对象
            SqlCommand cmd = new SqlCommand(sql, con);

            try
            {
                con.Open();
                return cmd.ExecuteNonQuery()>0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("程序错误，错误信息为：" + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return false;
        }
    }
}
