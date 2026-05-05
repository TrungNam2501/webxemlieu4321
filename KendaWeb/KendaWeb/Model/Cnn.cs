using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KendaWeb.Model
{
    public class Cnn
    {
        public static DataTable ExecuteQuery(string ConnectionString, string Query, object[] parameter = null)
        {
            //string ConnectionString = "Data Source=.; Initial Catalog=test;Integrated Security=True;MultipleActiveResultSets=True;";
            //string ConnectionString = "Data Source=198.1.10.33;Initial Catalog=erp;User ID=kendakv2;Password=kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(Query, conn);
                    if (parameter != null)
                    {
                        string[] listPara = Query.Split(' ');
                        int i = 0;
                        foreach (string item in listPara)
                        {
                            if (item.Contains('?'))
                            {
                                cmd.Parameters.AddWithValue(item, parameter[i]);
                                i++;
                            }
                        }
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
                catch (Exception)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }

        public static bool ExecuteNonQuery(string ConnectionString, string query, Dictionary<string, object> param = null)
        {
            //string ConnectionString = "Data Source=198.1.10.34;Initial Catalog=erp;User ID=kendakv2;Password=kenda123";
            //  string Connect = "Data Source=" + IP + ";Initial Catalog=JianDaMES;User ID=kendakv2;Password=kenda123";
            //string ConnectionString = "Data Source=198.1.10.34;Initial Catalog=erp;User ID=kendakv2;Password=kenda123";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.Text;

                    if (param != null && param.Count > 0)
                        foreach (var item in param)
                            cmd.Parameters.AddWithValue(item.Key, item.Value);

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static DataTable ExecuteQueryBB(string Query, object[] parameter = null)
        {
            string ConnectionString = "Data Source=198.1.10.33;Failover Partner=198.1.10.31;Initial Catalog=BB;User ID=kendaKV2;Password=kenda123";

            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(Query, conn);
                    if (parameter != null)
                    {
                        string[] listPara = Query.Split(' ');
                        int i = 0;
                        foreach (string item in listPara)
                        {
                            if (item.Contains('?'))
                            {
                                cmd.Parameters.AddWithValue(item, parameter[i]);
                                i++;
                            }
                        }
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
    }
    
}