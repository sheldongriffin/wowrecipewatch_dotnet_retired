using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WoWModel;

namespace DownloadApp
{
    public static class AveragePriceUpdater
    {
        public static void Update()
        {
            Dictionary<int, List<long>> averages = new Dictionary<int, List<long>>();

            string allLines = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\TheUndermineJournalGE\MarketData.lua");
            foreach (Match match in Regex.Matches(allLines, @"addonTable.marketdata\[(\d+)\] = {\s*\[""marketmedian""\] = (\d+)[^}]+}"))
            {
                int itemid = Convert.ToInt32(match.Groups[1].Value);
                long medianprice = Convert.ToInt64(match.Groups[2].Value);
                if (!averages.ContainsKey(itemid))
                {
                    averages[itemid] = new List<long>() { medianprice };
                }
                else
                {
                    averages[itemid].Add(medianprice);
                }
            }


            using (SqlConnection conn = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=WoWModel.WoWDb;user id=sa;password=Password#1;"))
            {
                conn.Open();

                foreach (var itemid in averages.Keys)
                {
                    Console.WriteLine(itemid);
                    long avg = Convert.ToInt64(averages[itemid].Average());

                    using (SqlCommand cmd = new SqlCommand("UPDATE Items SET ServerwideAverage = @ServerwideAverage WHERE ItemId = @ItemId", conn))
                    {
                        cmd.Parameters.AddWithValue("ItemId", itemid);
                        cmd.Parameters.AddWithValue("ServerwideAverage", avg);
                        cmd.ExecuteNonQuery();
                    } 
                    
                    using (SqlCommand cmd = new SqlCommand("UPDATE Recipes SET ServerwideAverage = @ServerwideAverage WHERE RecipeId = (SELECT RecipeId FROM Items WHERE ItemId = @ItemId)", conn))
                    {
                        cmd.Parameters.AddWithValue("ItemId", itemid);
                        cmd.Parameters.AddWithValue("ServerwideAverage", avg);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
