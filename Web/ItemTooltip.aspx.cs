using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WoWJSON;

namespace Web
{
    public partial class ItemTooltip : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = Request.QueryString["url"];
            string returnVal = string.Empty;

            if (url.IndexOf("spell=") == -1)
            {
                int itemId = Convert.ToInt32(url.Replace("http://www.wowhead.com/item=", string.Empty));

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TdbConnectionString"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT JSON FROM Items WHERE ItemId = @ItemId", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("ItemId", itemId);
                        string json = cmd.ExecuteScalar().ToString();
                        if (!string.IsNullOrWhiteSpace(json))
                        {
                            returnVal = json.Replace("\"id\"", "\"cached\":true,\"id\"");
                        }
                    }

                    if (string.IsNullOrWhiteSpace(returnVal))
                    {
                        string html = WebHelper.GetHtml("http://us.battle.net/api/wow/item/" + itemId);

                        using (SqlCommand cmd = new SqlCommand("UPDATE Items SET JSON = @Json WHERE ItemId = @ItemId", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("Json", html);
                            cmd.Parameters.AddWithValue("ItemId", itemId);
                            cmd.ExecuteNonQuery();
                        }

                        using (MemoryStream m = new MemoryStream(Encoding.UTF8.GetBytes(html)))
                        {
                            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Item));
                            Item i = (Item)ser.ReadObject(m);

                            using (SqlCommand cmd = new SqlCommand("UPDATE Items SET BindOnPickup = @BindOnPickup, Icon = @Icon, Quality = @Quality, SourceId = @SourceId, SourceType = @SourceType WHERE ItemId = @ItemId", conn))
                            {
                                cmd.Parameters.AddWithValue("BindOnPickup", i.ItemBind);
                                cmd.Parameters.AddWithValue("Icon", i.ItemSpells.Count > 0 ? i.ItemSpells.Last().Spell.Icon : i.Icon);
                                cmd.Parameters.AddWithValue("Quality", i.Quality);
                                cmd.Parameters.AddWithValue("SourceId", i.ItemSource.SourceId);
                                cmd.Parameters.AddWithValue("SourceType", i.ItemSource.SourceType);
                                cmd.Parameters.AddWithValue("ItemId", i.ItemId);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        returnVal = html;
                    }
                }
            }

            Response.ContentType = "application/json";
            Response.Write(returnVal);
        }
    }
}