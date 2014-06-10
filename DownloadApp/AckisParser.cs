using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace RecipeWatch
{
    internal static class AckisParser
    {
        public static void Parse()
        {
            Dictionary<string, string> vendors = new Dictionary<string, string>() {
                {"49701","Chef's Award"},
                {"31032","Dalaran Cooking Award"},
                {"32514","Dream Shards"},
                {"16722","Heavenly Shards"},
                {"16748","Heavy Savage Leather"},
                {"32515","Heavy Borean Leather"},
                {"46359","Elementium Bar"},
                {"15909","Lunar Festival"},
                {"57922","Illustrious Jewelcrafter's Token"},
                {"56925","Illustrious Jewelcrafter's Token"},
                {"52584","Illustrious Jewelcrafter's Token"},
                {"19065","Dalaran Jewelcrafter's Token"},
                {"28721","Dalaran Jewelcrafter's Token"},
                {"30489","Honor Points"}
            };
            Dictionary<int, string> tradeskills = new Dictionary<int, string>() { { 755, "Jewelcrafting" }, { 197, "Tailoring" }, { 164, "Blacksmithing" }, { 165, "Leatherworking" }, { 171, "Alchemy" }, { 202, "Engineering" }, { 333, "Enchanting" }, { 773, "Inscription" }, { 129, "FirstAid" }, { 185, "Cooking" } };

            List<AckisRecipe> recipes = new List<AckisRecipe>();

            using (SqlConnection conn = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=WoWModel.WoWDb;Persist Security Info=True;User ID=sa;Password=Password#1"))
            {
                conn.Open();

                foreach (var tradeskillId in tradeskills.Keys)
                {
                    #region Split File Into AckisRecipe Chunks

                    List<string> potentialRecipes = new List<string>();
                    using (StreamReader r = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\AckisRecipeList\Database\Recipes\" + tradeskills[tradeskillId] + ".lua"))
                    {
                        string line;
                        StringBuilder currentToken = new StringBuilder();
                        while ((line = r.ReadLine()) != null)
                        {
                            if (Regex.Match(line, @"-- .* -- \d+$").Success) // save out current buffer and start on next
                            {
                                potentialRecipes.Add(currentToken.ToString());
                                currentToken = new StringBuilder();
                                currentToken.Append(line);
                            }
                            else
                            {
                                currentToken.Append(line);
                            }
                        }

                        potentialRecipes.Add(currentToken.ToString());
                    }

                    #endregion

                    Regex trainerRegex = new Regex("recipe:AddTrainer");
                    Regex nameIdRegex = new Regex(@"-- ([^\n]+) -- (\d+)");
                    Regex worldDropRegex = new Regex(@"recipe:AddWorldDrop");
                    Regex repRegex = new Regex(@"recipe:AddRepVendor\(FAC.([^,]+), REP.([^,]+), \d+");
                    Regex discoveryRegex = new Regex("recipe:AddDiscovery");
                    Regex mobDropRegex = new Regex("recipe:AddMobDrop");
                    Regex customRegex = new Regex(@"recipe:AddCustom\(""([^\)]+)""\)");
                    Regex vendorRegex = new Regex(@"recipe:AddVendor\(([^\)]+)\)");
                    Regex limitedvendorRegex = new Regex("recipe:AddLimitedVendor");
                    Regex seasonalRegex = new Regex("recipe:AddSeason");
                    Regex retiredRegex = new Regex("F.RETIRED");
                    Regex questRegex = new Regex("recipe:AddQuest");

                    foreach (string potentialRecipe in potentialRecipes)
                    {
                        Match spellMatch = nameIdRegex.Match(potentialRecipe);
                        if (!spellMatch.Success)
                        {
                            //Console.WriteLine(potentialRecipe);
                            continue;
                        }

                        string name = spellMatch.Groups[1].ToString();
                        int spellId = Convert.ToInt32(spellMatch.Groups[2].ToString());

                        AckisRecipe r = new AckisRecipe { Name = name, SpellId = spellId, TradeskillId = tradeskillId };

                        int craftedItemId;
                        Int32.TryParse(GetSingleMatch(@"recipe:SetCraftedItem\((\d+),", potentialRecipe), out craftedItemId);
                        if (craftedItemId > 0)
                        {
                            r.CraftedItemId = craftedItemId;
                        }

                        int recipeItemId;
                        Int32.TryParse(GetSingleMatch(@"recipe:SetRecipeItem\((\d+),", potentialRecipe), out recipeItemId);
                        if (recipeItemId > 0)
                        {
                            r.RecipeItemId = recipeItemId;
                        }

                        if (trainerRegex.Match(potentialRecipe).Success)
                        {
                            r.Source = "Trained";
                        }
                        else if (retiredRegex.Match(potentialRecipe).Success)
                        {
                            r.Source = "Retired";
                        }
                        else if (worldDropRegex.Match(potentialRecipe).Success)
                        {
                            r.Source = "World Drop";
                        }
                        else if (repRegex.Match(potentialRecipe).Success)
                        {
                            Match m = repRegex.Match(potentialRecipe);
                            r.Source = m.Groups[1] + " - " + m.Groups[2];
                        }
                        else if (discoveryRegex.Match(potentialRecipe).Success)
                        {
                            r.Source = "Discovery";
                        }
                        else if (mobDropRegex.Match(potentialRecipe).Success)
                        {
                            r.Source = "Mob Drop";
                        }
                        else if (customRegex.Match(potentialRecipe).Success)
                        {
                            Match m = customRegex.Match(potentialRecipe);
                            r.Source = m.Groups[1].ToString();
                        }
                        else if (vendorRegex.Match(potentialRecipe).Success || limitedvendorRegex.Match(potentialRecipe).Success)
                        {
                            r.Source = "Vendor";
                            Match m = vendorRegex.Match(potentialRecipe);
                            if (m.Success)
                            {
                                foreach (string vendorId in vendors.Keys)
                                {
                                    if (m.Groups[1].ToString().IndexOf(vendorId) > -1)
                                    {
                                        r.Vendor = vendors[vendorId];
                                        break;
                                    }
                                }
                                r.Vendor = "Vendor";
                                //if (string.IsNullOrWhiteSpace(r.Vendor))
                                //{
                                //    r.VendorId = m.Groups[1].ToString();
                                //}
                            }
                            else if (limitedvendorRegex.Match(potentialRecipe).Success)
                            {
                                r.Vendor = "Limited";
                            }
                            else
                            {
                                r.Vendor = "Vendor";
                            }
                        }
                        else if (seasonalRegex.Match(potentialRecipe).Success)
                        {
                            r.Source = "Seasonal";
                        }
                        else if (questRegex.Match(potentialRecipe).Success)
                        {
                            r.Source = "Quest";
                        }
                        else
                        {
                            Console.WriteLine(potentialRecipe);
                        }

                        recipes.Add(r);

                        using (SqlCommand cmd = new SqlCommand("SELECT Name FROM Recipes WHERE RecipeId = @RecipeId", conn))
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.Parameters.AddWithValue("@RecipeId", spellId);
                            string c = (string)cmd.ExecuteScalar();
                            if (c != r.Name)
                            {
                                using (SqlConnection conn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["TdbConnectionString"].ConnectionString))
                                {
                                    conn2.Open();
                                    using (SqlCommand cmd2 = new SqlCommand("UPDATE Recipes SET Name = @Name WHERE RecipeId = @RecipeId", conn2))
                                    {
                                        cmd2.CommandType = System.Data.CommandType.Text;
                                        cmd2.Parameters.AddWithValue("@RecipeId", spellId);
                                        cmd2.Parameters.AddWithValue("@Name", r.Name);
                                        cmd2.ExecuteNonQuery();
                                    }
                                }
                                Console.WriteLine(r.Name + " - " + r.Source + " - " + r.Vendor);
                            }
                        }

                    }
                }
            }
        }

        private static string GetSingleMatch(string matchString, string source)
        {
            Match match = Regex.Match(source, matchString);
            return match.Success ? match.Groups[1].ToString() : string.Empty;
        }
    }

    internal class AckisRecipe
    {
        public string Name { get; set; }

        public int SpellId { get; set; }

        public int TradeskillId { get; set; }

        public int CraftedItemId { get; set; }

        public int RecipeItemId { get; set; }

        public string Source { get; set; }

        public string Vendor { get; set; }

        public string VendorId { get; set; }
    }
}