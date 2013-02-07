using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using WoWJSON;

namespace DownloadApp
{
    public static class Scraper
    {
        public static void Scrape()
        {
            Dictionary<string, int> tradeskills = new Dictionary<string, int>()
            {
                {"First Aid", 129},
                {"Blacksmithing",164},
                {"Leatherworking",165},
                {"Alchemy",171},
                {"Herbalism",182},
                {"Cooking",185},
                {"Mining",186},
                {"Tailoring",197},
                {"Engineering",202},
                {"Enchanting",333},
                {"Fishing",356},
                {"Skinning",393},
                {"Jewelcrafting",755},
                {"Inscription",773},
                {"Archaeology",794}
            };

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TdbConnectionString"].ConnectionString))
            {
                conn.Open();
                int itemId;

                using (SqlCommand cmd = new SqlCommand("SELECT MAX(RecipeId) FROM Recipes", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    itemId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                while (true)
                {
                    itemId++;

                    Console.Write("Checking item " + itemId);

                    string html = WebHelper.GetHtml("http://us.battle.net/api/wow/recipe/" + itemId);

                    if (html.IndexOf("Invalid recipe id") > -1 || string.IsNullOrWhiteSpace(html)) { System.Threading.Thread.Sleep(500); continue; }

                    Recipe i = null;

                    using (MemoryStream m = new MemoryStream(Encoding.UTF8.GetBytes(html)))
                    {
                        try
                        {
                            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(WoWJSON.Recipe));
                            i = (Recipe)ser.ReadObject(m);
                        }
                        catch { }
                    }

                    if (i != null)
                    {
                        Console.Write(" found " + i.Name + System.Environment.NewLine);

                        if (!tradeskills.ContainsKey(i.Profession))
                        {
                            Console.Write("Profession " + i.Profession + " not found");
                            System.Threading.Thread.Sleep(500);
                            continue;
                        }

                        using (SqlCommand cmd = new SqlCommand("INSERT INTO Recipes (RecipeId, Name, TradeskillId) VALUES (@RecipeId, @Name, @TradeskillId)", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("RecipeId", i.RecipeId);
                            cmd.Parameters.AddWithValue("Name", i.Name);
                            cmd.Parameters.AddWithValue("TradeskillId", tradeskills[i.Profession]);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        Console.Write(" nothing found\n");
                    }

                    System.Threading.Thread.Sleep(500);
                }
            }
        }
    }
}
