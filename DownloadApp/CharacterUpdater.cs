using System;
using System.Collections.Generic;
using System.Linq;
using WoWJSON;
using WoWModel;

namespace DownloadApp
{
    public static class CharacterUpdater
    {
        public static void RefreshCharacters()
        {
            UpdateCharacter("Doraie", "Aerie Peak");
            UpdateCharacter("Iraie", "Aerie Peak");
            UpdateCharacter("Taalisa", "Aerie Peak");
            UpdateCharacter("Cyrai", "Aerie Peak");
            UpdateCharacter("Joraa", "Aerie Peak");
            UpdateCharacter("Cionaie", "Aerie Peak");
            //UpdateCharacter("Mavraie", "Cairne");
        }

        private static void UpdateCharacter(string name, string realm)
        {
            Console.WriteLine("Fetching " + name);

            using (WoWEntities db = new WoWEntities())
            {
                var character = ApiClient.GetCharacter(name, realm);
                WoWModel.Character dbcharacter = db.Characters.SingleOrDefault(a => a.Name == name);
                if (dbcharacter == null)
                {
                    dbcharacter = new WoWModel.Character();
                    db.Characters.Add(new WoWModel.Character { 
                        Name = character.Name, 
                        AchievementPoints = character.AchievementPoints, 
                        ClassId = character.ClassId, 
                        Gender = character.Gender, 
                        LastModified = character.LastModified, 
                        Level = character.Level, 
                        RaceId = character.Race,
                        Realm = db.Realms.Single(a => a.Name == realm), Thumbnail = character.Thumbnail });
                    db.SaveChanges();
                    dbcharacter = db.Characters.Single(a => a.Name == name);
                }
                dbcharacter.AchievementPoints = character.AchievementPoints;
                dbcharacter.Class = db.Classes.Single(a => a.ClassId == character.ClassId);
                dbcharacter.Gender = character.Gender;
                dbcharacter.LastModified = character.LastModified;
                dbcharacter.Level = character.Level;
                dbcharacter.Name = character.Name;
                dbcharacter.Race = db.Races.Single(a => a.RaceId == character.Race);
                dbcharacter.Realm = db.Realms.Single(a => a.Name == character.Realm);
                dbcharacter.Thumbnail = character.Thumbnail;

                List<int> knownTradeskills = dbcharacter.Tradeskills.Select(a => a.TradeskillId).ToList();
                foreach (var tradeskillId in character.Professions.Primary.Select(a => a.Id).Where(b => !knownTradeskills.Contains(b)))
                {
                    dbcharacter.Tradeskills.Add(db.Tradeskills.Single(a => a.TradeskillId == tradeskillId));
                }
                foreach (var tradeskillId in character.Professions.Secondary.Select(a => a.Id).Where(b => !knownTradeskills.Contains(b)))
                {
                    dbcharacter.Tradeskills.Add(db.Tradeskills.Single(a => a.TradeskillId == tradeskillId));
                }

                List<int> knownRecipes = dbcharacter.Recipes.Select(a => a.RecipeId).ToList();

                foreach (Profession profession in character.Professions.Primary.Concat(character.Professions.Secondary))
                {
                    foreach (int recipeId in profession.Recipes)
                    {
                        if (!knownRecipes.Contains(recipeId))
                        {
                            WoWModel.Recipe recipe = db.Recipes.SingleOrDefault(a => a.RecipeId == recipeId);
                            if (recipe == null)
                            {
                                Console.WriteLine("Error: Recipe " + recipeId + " not found");
                            }
                            else
                            {
                                dbcharacter.Recipes.Add(recipe);
                            }
                        }
                    }
                }
                //foreach (var recipeid in character.Professions.Primary.SelectMany(profession => profession.Recipes).Where(recipeid => !knownRecipes.Contains(recipeid)))
                //{
                //    dbcharacter.Recipes.Add(db.Recipes.Single(a => a.RecipeId == recipeid));
                //}

                //foreach (var recipeid in character.Professions.Secondary.SelectMany(profession => profession.Recipes).Where(recipeid => !knownRecipes.Contains(recipeid)))
                //{
                //    dbcharacter.Recipes.Add(db.Recipes.Single(a => a.RecipeId == recipeid));
                //}

                //List<int> knownPets = dbcharacter.Companions.Select(a => a.CompanionId).ToList();

                //foreach (var companionId in character.Companions.Where(companionId => !knownPets.Contains(companionId)))
                //{
                //    if (!db.Companions.Any(a => a.CompanionId == companionId))
                //    {
                //        db.Companions.AddObject(new Companion() { CompanionId = companionId, Name = "Companion" + companionId });
                //        db.SaveChanges();
                //    }
                //    dbcharacter.Companions.Add(db.Companions.Single(b => b.CompanionId == companionId));
                //}

                db.SaveChanges();
            }
        }
    }
}
