using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Data.SqlClient;
using System.Configuration;
using WoWJSON;

namespace DownloadApp
{
    public static class DbTweak
    {
        public static void Tweak()
        {
            List<Item> items = new List<Item>();
            HashSet<int> recipes = new HashSet<int>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TdbConnectionString"].ConnectionString))
            {
                conn.Open();

                using (SqlDataReader reader = new SqlCommand("SELECT * FROM Items WHERE JSON IS NOT NULL", conn).ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader["Name"].ToString());
                        using (MemoryStream m = new MemoryStream(Encoding.UTF8.GetBytes(reader["JSON"].ToString())))
                        {
                            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Item));
                            items.Add((Item)ser.ReadObject(m));
                        }
                    }
                }

                using (SqlDataReader reader = new SqlCommand("SELECT RecipeId FROM Recipes", conn).ExecuteReader())
                {
                    while (reader.Read())
                    {
                        recipes.Add((int)reader["RecipeId"]);
                    }
                }
            }

            using (SqlConnection conn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["TdbConnectionString"].ConnectionString))
            {
                conn2.Open();
                foreach (Item i in items)
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Items SET Name = @Name, BindOnPickup = @BindOnPickup, Icon = @Icon, Quality = @Quality, SourceId = @SourceId, SourceType = @SourceType, RecipeId = @RecipeId WHERE ItemId = @ItemId", conn2))
                    {
                        cmd.Parameters.AddWithValue("Name", i.Name);
                        cmd.Parameters.AddWithValue("BindOnPickup", i.ItemBind);
                        cmd.Parameters.AddWithValue("Icon", i.ItemSpells.Count > 0 ? (object)i.ItemSpells.Last().Spell.Icon : !string.IsNullOrWhiteSpace(i.Icon) ? (object)i.Icon : DBNull.Value);
                        cmd.Parameters.AddWithValue("Quality", i.Quality);
                        cmd.Parameters.AddWithValue("SourceId", i.ItemSource.SourceId);
                        cmd.Parameters.AddWithValue("SourceType", i.ItemSource.SourceType);
                        cmd.Parameters.AddWithValue("RecipeId", i.ItemSpells.Any(a => a.Trigger == "ON_LEARN") && recipes.Contains(i.ItemSpells.First(b => b.Trigger == "ON_LEARN").SpellId) ? (object)i.ItemSpells.First(b => b.Trigger == "ON_LEARN").SpellId : DBNull.Value);
                        cmd.Parameters.AddWithValue("ItemId", i.ItemId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
