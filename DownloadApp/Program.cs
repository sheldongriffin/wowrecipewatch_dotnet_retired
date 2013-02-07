using System;
using System.Linq;
using RecipeWatch;
using WoWModel;

namespace DownloadApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("a: Auction Update");
            Console.WriteLine("c: Characters");
            Console.WriteLine("d: Db Tweak");
            Console.WriteLine("p: Pet Update");
            Console.WriteLine("s: Scrape");
            Console.WriteLine("j: Get JSON");
            Console.WriteLine("k: Ackis Parser");
            ConsoleKeyInfo ck = Console.ReadKey();
            switch (ck.KeyChar) {
                case 'a':
                    using (WoWEntities ctx = new WoWEntities())
                    {
                        foreach (string realm in ctx.Characters.Select(a => a.Realm).Select(b => b.Slug).Distinct())
                        {
                            AuctionUpdater.UpdateAuctionValues(realm);
                        }
                    }
                    break;
                case 'c':
                    CharacterUpdater.RefreshCharacters();
                    break;
                case 'd':
                    DbTweak.Tweak();
                    break;
                case 'p':
                    PetParser.Parse();
                    break;
                case 's':
                    Scraper.Scrape();
                    break;
                case 'j':
                    JSONGetter.GetMissingJSON();
                    break;
                case 'k':
                    AckisParser.Parse();
                    break;
            }

            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}
