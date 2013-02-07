using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DownloadApp
{
    public static class JSONGetter
    {
        public static void GetMissingJSON()
        {
            List<TinyObject> list = new List<TinyObject>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TdbConnectionString"].ConnectionString))
            {
                conn.Open();
                //using (SqlCommand cmd = new SqlCommand("select itemid, name, JSON, json_zhtw from Items where (Teaches is not null or ItemId in (select crafteditemid from Recipes) or ItemId in (select itemid from Reagents)) and (JSON_zhtw is null OR datalength(json) = 0 OR json IS NULL or datalength(json_zhtw) = 0)", conn))
                //using (SqlCommand cmd = new SqlCommand("select itemid, name, JSON, json_zhtw from Items where JSON IS NULL AND ItemId > 74600", conn))
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Items i INNER JOIN Recipes r ON i.ItemId = r.ItemId WHERE r.Name LIKE 'vicious%'", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new TinyObject() { Name = reader["Name"].ToString().Trim(), Itemid = (int)reader["ItemId"], JSON = reader["JSON"].ToString().Trim(), JSON_zhtw = reader["JSON_zhtw"].ToString().Trim() });
                        }
                    }
                }
            }

            foreach (var tiny in list)
            {
                Console.WriteLine(tiny.Name);
                if (string.IsNullOrWhiteSpace(tiny.JSON))
                {
                    tiny.JSON = WebHelper.GetHtml("http://us.battle.net/api/wow/item/" + tiny.Itemid);
                }
                //if (string.IsNullOrWhiteSpace(tiny.JSON_zhtw))
                //{
                //    tiny.JSON_zhtw = WebHelper.GetHtml("http://tw.battle.net/api/wow/item/" + tiny.Itemid);
                //}

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TdbConnectionString"].ConnectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd2 = new SqlCommand())
                    {
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection = conn;
                        bool runquery = true;

                        //if (string.IsNullOrWhiteSpace(tiny.JSON.Trim()) && !string.IsNullOrWhiteSpace(tiny.JSON_zhtw.Trim()))
                        //{
                        //    cmd2.CommandText = "UPDATE Items SET JSON_zhtw = @JSON_zhtw WHERE ItemId = @ItemId";
                        //    cmd2.Parameters.AddWithValue("JSON_zhtw", tiny.JSON_zhtw);
                        //}
                        if (!string.IsNullOrWhiteSpace(tiny.JSON.Trim()) && string.IsNullOrWhiteSpace(tiny.JSON_zhtw.Trim()))
                        {
                            cmd2.CommandText = "UPDATE Items SET JSON = @JSON WHERE ItemId = @ItemId";
                            cmd2.Parameters.AddWithValue("JSON", tiny.JSON);
                        }
                        //else if (!string.IsNullOrWhiteSpace(tiny.JSON.Trim()) && !string.IsNullOrWhiteSpace(tiny.JSON_zhtw.Trim()))
                        //{
                        //    cmd2.CommandText = "UPDATE Items SET JSON_zhtw = @JSON_zhtw, JSON = @JSON WHERE ItemId = @ItemId";
                        //    cmd2.Parameters.AddWithValue("JSON_zhtw", tiny.JSON_zhtw);
                        //    cmd2.Parameters.AddWithValue("JSON", tiny.JSON);
                        //}
                        else
                        {
                            runquery = false;
                        }

                        cmd2.Parameters.AddWithValue("ItemId", tiny.Itemid);

                        if (runquery)
                        {
                            try
                            {
                                cmd2.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                    }
                }
            }
        }
    }
}
