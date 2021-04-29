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
    /// 超市管理类
    /// </summary>
    class Manager
    {
        /// <summary>
        /// 主菜单
        /// </summary>
        public void zcd()
        {
            Console.WriteLine("-----------------------欢迎使用超市购物管理系统----------------------- ");
            Console.WriteLine("\n\t\t\t1. 显示所有商品 ");
            Console.WriteLine("\n\t\t\t2. 显示所有商品分类 ");
            Console.WriteLine("\n\t\t\t3. 新增商品 ");
            Console.WriteLine("\n\t\t\t4. 修改商品售价 ");
            Console.WriteLine("\n\t\t\t5. 删除商品信息");
            Console.WriteLine("\n\t\t\t6. 在线购物");
            Console.WriteLine("\n\t\t\t7. 订单管理 ");
            Console.WriteLine("\n\t\t\t0. 退出 ");
            Console.WriteLine("\n----------------------------------------------------------------------- ");
            Console.Write("\n\t\t\t请输入您的操作: ");
            int cz = Convert.ToInt32(Console.ReadLine());
            switch (cz)
            {
                case 1:
                    Xs();
                    break;
                case 2:
                    Xsfl();
                    break;
                case 3:
                    Xz();
                    break;
                case 4:
                    Xg();
                    break;
                case 5:
                    Sc();
                    break;
                case 6:
                    Gw();
                    break;
                case 7:
                    Gl();
                    break;
                case 0:
                    Console.WriteLine("\n\t\t\t谢谢您的使用，欢迎您的下次光临！");
                    return;
                default:
                    Console.WriteLine("\n\t\t\t选项错误，请稍后再试！");
                    zcd();
                    break;
            }
            Console.Write("\n\n\t\t\t输入任意键返回主菜单：");
            string sr = Console.ReadLine();
            zcd();
        }

        /// <summary>
        /// 显示所有商品 
        /// </summary>
        private void Xs()
        {
            Console.WriteLine("\n\n-----------------------------------------------------------------------");
            Console.WriteLine("\n\t商品编号\t商品名称\t单价\t库存\t所属分类");
            Console.WriteLine("\n----------------------------------------------------------------------- ");
            string sql = string.Format("SELECT G.GID,G.Name,G.Price,G.Stock,T.TypeName FROM GOODS G INNER JOIN GoodsType T ON G.GTID = T.GTID");
            SqlDataReader s = DBHelper.ExecuteReader(sql);
            while (s!=null && s.Read() && s.HasRows)
            {
                string bh = s["GID"].ToString();
                string mc = s["Name"].ToString();
                string dj = s["Price"].ToString();
                string kc = s["Stock"].ToString();
                string fl = s["TypeName"].ToString();
                Console.WriteLine("\n\t\t{0}\t{1}\t{2}\t{3}\t{4}",bh,mc,dj,kc,fl);
            }
            Console.WriteLine("\n\n-----------------------------------------------------------------------");
            if (s != null)
            {
                s.Close();
            }
        }

        /// <summary>
        /// 显示所有商品分类
        /// </summary>
        private void Xsfl()
        {
            Console.WriteLine("\n\n-----------------------------------------------------------------------");
            Console.WriteLine("\n\t分类编号\t商品分类名称");
            Console.WriteLine("\n----------------------------------------------------------------------- ");
            string sql = string.Format("SELECT GTID,TypeName FROM GoodsType");
            SqlDataReader s = DBHelper.ExecuteReader(sql);
            while (s != null && s.Read() && s.HasRows)
            {
                string bh = s["GTID"].ToString();
                string mc = s["TypeName"].ToString();

                Console.WriteLine("\n\t\t{0}\t{1}", bh, mc);
            }
            if (s != null)
            {
                s.Close();
            }
        }

        /// <summary>
        /// 新增商品
        /// </summary>
        private void Xz()
        {
            Console.Write("\n\t\t\t请输入商品名称：");
            string name = Console.ReadLine();
            string sql = string.Format("SELECT COUNT(0) FROM Goods WHERE NAME='{0}'",name);
            int res = Convert.ToInt32(DBHelper.ExecuteScalar(sql));
            if (res > 0)
            {
                Console.WriteLine("\n\t\t\t此商品已存在！请您稍后再试");
            }
            else
            {
                Console.Write("\n\t\t\t请输入商品售价:");
                double sj = Convert.ToDouble(Console.ReadLine());
                Console.Write("\n\t\t\t请输入商品入库内存：");
                int kc = Convert.ToInt32(Console.ReadLine());
                Console.Write("\n\t\t\t请输入商品所属分类：");
                int fl = Convert.ToInt32(Console.ReadLine());
                string sql2 = string.Format("INSERT INTO [dbo].[Goods] (Name,GTID,STOCK,Price) VALUES('{0}',{1},{2},'{3}')",name,fl,kc,sj);
                bool res2 = DBHelper.ExecuteNonQuery(sql2);
                if (res2)
                {
                    Console.WriteLine("\n\t\t\t商品已上架！");
                }
                else
                {
                    Console.WriteLine("\n\t\t\t商品上架失败，请稍后再试!！");
                }
            }
        }

        /// <summary>
        /// 修改商品售价
        /// </summary>
        private void Xg()
        {
            Console.Write("\n\t\t\t请输入需要修改售价的商品编号：");
            int bh =Convert.ToInt32(Console.ReadLine());
            string sql = string.Format("SELECT COUNT(0) FROM Goods WHERE GID={0} ", bh);
            int res = Convert.ToInt32(DBHelper.ExecuteScalar(sql));
            if (res > 0)
            {
                Console.Write("\n\t\t\t请输入商品的最新售价：");
                double sj = Convert.ToDouble(Console.ReadLine());
                string sql1 = string.Format("SELECT Name,Price FROM Goods WHERE GID={0}",bh);
                SqlDataReader s = DBHelper.ExecuteReader(sql1);
                if(s!=null && s.Read() && s.HasRows)
                {
                    string name = s["Name"].ToString();
                    double ysj = Convert.ToDouble(s["Price"]);
                    Console.WriteLine("\n\t\t\t您将要修改“{0}”的商品售价，该商品原售价为：{1}元，现在的售价为：{2}元",name,ysj,sj);
                    string yzm1 = yzm();
                    Console.Write("\n\t\t\t请输入验证码“{0}”继续您的操作：",yzm1);
                    string yzm2 = Console.ReadLine();
                    if (yzm2.Equals(yzm1))
                    {
                        string sql3 = string.Format("UPDATE Goods SET Price={0} WHERE GID = {1}",sj,bh);
                        bool res1 = DBHelper.ExecuteNonQuery(sql3);
                        if (res1)
                        {
                            Console.WriteLine("\n\t\t\t修改商品售价成功！");
                        }
                        else
                        {
                            Console.WriteLine("\n\t\t\t修改商品售价失败！请稍后再试！");
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("\n\t\t\t验证码输入有误，用户自行取消业务!");
                    }
                }
                if (s != null)
                {
                    s.Close();
                }
            }
            else
            {
                Console.WriteLine("\n\t\t\t该商品编号不存在！请您稍后再试");
            }
        }

        /// <summary>
        /// 删除商品信息
        /// </summary>
        private void Sc()
        {
            Console.Write("\n\t\t\t请输入需要下架的商品编号：");
            int bh = Convert.ToInt32(Console.ReadLine());
            string sql = string.Format("SELECT COUNT(0) FROM Goods WHERE GID={0} ", bh);
            int res = Convert.ToInt32(DBHelper.ExecuteScalar(sql));
            if (res > 0)
            {
                string sql1 = string.Format("SELECT Name,Price,Stock  FROM Goods WHERE GID={0}", bh);
                SqlDataReader s = DBHelper.ExecuteReader(sql1);
                if (s != null && s.Read() && s.HasRows)
                {
                    string name = s["Name"].ToString();
                    double ysj = Convert.ToDouble(s["Price"]);
                    int kc = Convert.ToInt32(s["Stock"]);
                    Console.WriteLine("\n\t\t\t您将要下架的商品名称为“{0}”，商品售价：{1}， 商品库存：{2}", name, ysj,kc);
                    string yzm1 = yzm();
                    Console.Write("\n\t\t\t请输入验证码“{0}”继续您的操作：", yzm1);
                    string yzm2 = Console.ReadLine();
                    if (yzm2.Equals(yzm1))
                    {
                        string sql3 = string.Format("SELECT COUNT(0) FROM OrderDetails WHERE GID ={0}",bh);
                        int res3 = Convert.ToInt32(DBHelper.ExecuteScalar(sql3));
                        if (res3==0)
                        {
                            string sql4 = string.Format("DELETE FROM Goods WHERE GID={0}",bh);
                            bool res4 = DBHelper.ExecuteNonQuery(sql4);
                            if (res4)
                            {
                                Console.WriteLine("\n\t\t\t下架商品成功！");
                            }
                            else
                            {
                                Console.WriteLine("\n\t\t\t下架商品失败！请稍后再试！");
                            }
                            
                        }
                        else
                        {
                            Console.WriteLine("\n\t\t\t商品已被客户购买过！暂时无法进行删除操作！");
                        }

                    }
                    else
                    {
                        Console.WriteLine("\n\t\t\t验证码输入有误，用户自行取消业务!");
                    }
                }
                if (s != null)
                {
                    s.Close();
                }
            }
            else
            {
                Console.WriteLine("\n\t\t\t该商品编号不存在！请您稍后再试");
            }
        }

        /// <summary>
        /// 在线购物
        /// </summary>
        private void Gw()
        {
            //获取当前订单号
            string orderIDSql = "SELECT CONVERT(VARCHAR(4),DATEPART(YY,GETDATE()))+'0'+CONVERT(VARCHAR(2),DATEPART(MM,GETDATE()))+CONVERT(VARCHAR(2),DATEPART(DD,GETDATE()))+RIGHT(RAND(),6)";
            string s = DBHelper.ExecuteScalar(orderIDSql).ToString();
            //获取当前下单时间
            string nowTimeSql = "SELECT GETDATE()";
            string date2 = DBHelper.ExecuteScalar(nowTimeSql).ToString();
            string sql = string.Format("INSERT INTO [dbo].[Order](OID,OrderDate) VALUES('{0}','{1}')",s,date2);
            bool res = DBHelper.ExecuteNonQuery(sql);
            if (res)
            {
                Console.WriteLine("\n\t\t\t请选择您需要购买的商品编号：");
                Xs();
                //初始化商品总价格和do while的循环变量
                double zj = 0;
                string pd = "1";
                do
                {
                    Console.Write("\n\t\t\t请选择商品编号：");
                    int bh = Convert.ToInt32(Console.ReadLine());
                    Console.Write("\n\t\t\t请选择购买数量：");
                    int sl = Convert.ToInt32(Console.ReadLine());
                    string sql1 = string.Format("SELECT Stock FROM Goods WHERE GID='{0}'",bh);
                    int kc = Convert.ToInt32(DBHelper.ExecuteScalar(sql1));
                    if (sl > kc)
                    {
                        Console.WriteLine("库存不足，请根据具体仓库库存进行购买！");
                    }
                    else
                    {
                        string s1= string.Format("SELECT Price FROM Goods WHERE GID = {0}",bh);
                        Double dj = Convert.ToDouble(DBHelper.ExecuteScalar(s1));
                        double xj = dj * sl; 
                        string sql2 = string.Format("INSERT INTO [dbo].[OrderDetails](OID,GID,Number,Subtotal) VALUES('{0}',{1},{2},'{3}')",s,bh,sl,xj);
                        bool res2 = DBHelper.ExecuteNonQuery(sql2);
                        if (res2)
                        {
                            zj += xj;
                            Console.Write("\n\t\t\t输入N结束购物，按任意键继续你的购物：");
                                pd = Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("系统繁忙，请重试！");
                        }
                    }
                } while (!pd.ToLower().Equals("n"));
                Console.WriteLine("\n\t\t\t购物结束，请确认您的订单信息：");
                Console.Write("\n\t\t\t请输入您的姓名：");
                string name = Console.ReadLine();
                Console.Write("\n\t\t\t请输入您的联系电话：");
                string dh = Console.ReadLine();
                Console.Write("\n\t\t\t请输入您的配送地址：");
                string dz = Console.ReadLine();
                string sql3 = string.Format("UPDATE  [dbo].[Order] SET TotalPrice='{0}',CustomerName='{1}',Phone='{2}',Address='{3}'WHERE OID='{4}'",zj,name,dh,dz,s);
                bool res1 = DBHelper.ExecuteNonQuery(sql3);
                if (res1)
                {
                    Console.WriteLine("\n\t\t\t请稍后,系统正在结算,结算后,我们将为您打印购物小票,请稍后… ");
                    
                    Console.WriteLine("\n\n-----------------------------------------------------------------------");
                    Console.WriteLine("\n\t\t商品名称   单价   购买数量   小计");
                    Console.WriteLine("\n----------------------------------------------------------------------- ");
                    string sql4 = string.Format("SELECT G.Name,G.Price,O.Number,O.Subtotal FROM Goods G INNER JOIN OrderDetails O ON G.GID=O.GID WHERE O.OID='{0}'", s);
                    SqlDataReader s4 = DBHelper.ExecuteReader(sql4);
                    while (s4 != null && s4.Read() && s4.HasRows)
                    {
                        string mz1 = s4["Name"].ToString();
                        double dj1 = Convert.ToDouble(s4["Price"]);
                        double xj1 = Convert.ToDouble(s4["Subtotal"]);
                        int sl1 = Convert.ToInt32(s4["Number"]);
                        Console.WriteLine("\n\t\t{0}\t{1}\t{2}\t{3}",mz1,dj1,sl1,xj1);
                    }
                    Console.WriteLine("\n\n-----------------------------------------------------------------------");
                    Console.WriteLine("\n\t总价：{0}元",zj);
                    Console.WriteLine("\n\t感谢您对本店的支持,欢迎下次再来,祝您生活愉快!");
                    Console.WriteLine("\n\t*******************************************************************");


                    if (s != null)
                    {
                        s4.Close();
                    }

                }
                else
                {
                    Console.WriteLine("在线购物系统繁忙！请稍后再试");
                }
            }
            else
            {
                Console.WriteLine("在线购物系统繁忙！请稍后再试");
            }
        }

        /// <summary>
        /// 订单管理
        /// </summary>
        private void Gl()
        {
            Console.Write("\n\t\t\t请输入订单编号：");
            string bh = Console.ReadLine();
            string sql = string.Format("SELECT COUNT(0) FROM [dbo].[Order] WHERE OID= '{0}'",bh);
            int res = Convert.ToInt32(DBHelper.ExecuteScalar(sql));
            if (res > 0)
            {
                Console.WriteLine("\n\t\t\t为保证您的数据安全,现在对您的信息进行查验,请输入该订单的联系人电话:");
                string dhhm = Console.ReadLine();
                string sql5 = string.Format("SELECT COUNT(0) FROM [dbo].[Order] WHERE OID= '{0}' AND Phone={1}", bh,dhhm);
                int res5 = Convert.ToInt32(DBHelper.ExecuteScalar(sql5));
                if (res5>0)
                {
                    Console.WriteLine("\n\t\t\t我们根据您提供的订单号与手机号码.查询到如下单信息,请您查阅: ");
                    string sql1 = string.Format("SELECT OrderDate,TotalPrice,CustomerName,Phone,Address FROM [dbo].[Order] WHERE OID='{0}'", bh);
                    SqlDataReader s = DBHelper.ExecuteReader(sql1);
                    if (s != null && s.Read() && s.HasRows)
                    {
                        string sj = s["OrderDate"].ToString();
                        double jg = Convert.ToDouble(s["TotalPrice"]);
                        string yhm = s["CustomerName"].ToString();
                        string sj0 = s["Phone"].ToString();
                        string dz0 = s["Address"].ToString();
                        Console.WriteLine("\n\t\t\t订单编号：" + bh);
                        Console.WriteLine("\n\t\t\t下单时间：" + sj);
                        Console.WriteLine("\n\t\t\t客户姓名：{0}\t客户联系电话：{1}", yhm, sj0);
                        Console.WriteLine("\n\t\t\t客户联系地址：" + dz0);

                        Console.WriteLine("\n\n-----------------------------------------------------------------------");
                        Console.WriteLine("\n\t\t商品名称   单价   购买数量   小计");
                        Console.WriteLine("\n----------------------------------------------------------------------- ");
                        string sql4 = string.Format("SELECT G.Name,G.Price,O.Number,O.Subtotal FROM Goods G INNER JOIN OrderDetails O ON G.GID=O.GID WHERE O.OID='{0}'", s);
                        SqlDataReader s4 = DBHelper.ExecuteReader(sql4);
                        while (s4 != null && s4.Read() && s4.HasRows)
                        {
                            string mz1 = s4["Name"].ToString();
                            double dj1 = Convert.ToDouble(s4["Price"]);
                            double xj1 = Convert.ToDouble(s4["Subtotal"]);
                            int sl1 = Convert.ToInt32(s4["Number"]);
                            Console.WriteLine("\n\t\t{0}\t{1}\t{2}\t{3}", mz1, dj1, sl1, xj1);
                        }
                        Console.WriteLine("\n\n-----------------------------------------------------------------------");
                        Console.WriteLine("\n\t总价：{0}元", jg);
                        Console.WriteLine("\n\t*******************************************************************");
                    }
                }
                else
                {
                    Console.WriteLine("您输入的电话与订单不匹配,查询信息失败!");
                }
            }
            else
            {
                Console.WriteLine("\n\t\t\t很抱歉,没有查询到相关的订单信息,请稍后再试!");
                Console.WriteLine("\n\t\t\t******************************************************************");
                

            }
        }

        /// <summary>
        /// 生成4个随机数
        /// </summary>
        /// <returns>4位随机数</returns>
        private string yzm()
        {
            return new Random().Next(1000, 9999).ToString();
        }
    }
}
