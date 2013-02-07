using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WoWModel;

namespace DownloadApp
{
    public static class PetParser
    {
        public static void Parse()
        {
            List<string> potentialRecipes = new List<string>();
            using (StreamReader r = new StreamReader(@"C:\Users\sheldon.griffin\Downloads\Collectinator\Database\PetDatabase.lua"))
            {
                string line;
                StringBuilder currentToken = new StringBuilder();
                while ((line = r.ReadLine()) != null)
                {
                    if (Regex.Match(line, @"-- .*").Success) // save out current buffer and start on next
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
            }


            Regex nameIdRegex = new Regex(@"-- ([^\n]+) - (\d+)\s+AddPet\(\d+, (\d+)");
            Regex nameIdRegex2 = new Regex(@"-- ([^\n]+) -- (\d+)\s+AddPet\(\d+, (\d+)");
            Regex nameIdRegex3 = new Regex(@"-- ([^\n]+)\s+AddPet\((\d+), (\d+)");
            //  -- Tranquil Mechanical Yeti - 26010
            //AddPet(26010, 21277, R_COMMON, GAME_ORIG)
            using (WoWEntities db = new WoWEntities())
            {
                foreach (var pc in potentialRecipes)
                {
                    Match spellMatch = nameIdRegex.Match(pc);
                    if (!spellMatch.Success)
                    {
                        spellMatch = nameIdRegex2.Match(pc);
                        if (!spellMatch.Success)
                        {
                            spellMatch = nameIdRegex3.Match(pc);
                        }
                    }

                    if (spellMatch.Success)
                    {
                        string name = spellMatch.Groups[1].ToString();
                        Console.WriteLine(name);
                        int spellId = Convert.ToInt32(spellMatch.Groups[2].ToString());
                        int itemId = Convert.ToInt32(spellMatch.Groups[3].ToString());

                        if (!db.Companions.Any(a => a.CompanionId == spellId))
                        {
                            db.Companions.AddObject(new Companion { CompanionId = spellId, Name = name });
                            db.SaveChanges();
                        }
                        else
                        {
                            db.Companions.Single(a => a.CompanionId == spellId).Name = name;
                            db.SaveChanges();
                        }

                        db.Items.Single(a => a.ItemId == itemId).CompanionTaught = db.Companions.Single(a => a.CompanionId == spellId);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
