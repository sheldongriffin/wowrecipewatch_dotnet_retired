using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WoWJSON;
using WoWModel;
using Auction = WoWJSON.Auction;

namespace RecipeWatch
{
    internal static class AuctionUpdater
    {
        public static void UpdateAuctionValues(string slug)
        {
            WoWEntities db = new WoWEntities();
            Realm realm = db.Realms.Single(a => a.Slug == slug);

            Console.WriteLine(DateTime.Now + " - " + realm.Name + ": Checking " + realm.Name);

            AuctionStub stub = ApiClient.GetAuctionStub(slug);

            Dictionary<string, long> updateTimes = new Dictionary<string, long>();
            Dictionary<int, long> lastValues = new Dictionary<int, long>();

            using (SqlConnection conn = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=WoWModel.WoWDb;user id=sa;password=Password#1;"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT i.ItemId, MIN(ISNULL(a.Buyout, 99999950000)) AS MinBuyout FROM Items i LEFT JOIN Auctions a ON i.ItemId = a.ItemId WHERE i.RecipeId IS NOT NULL AND ISNULL(BindOnPickup, 0) = 0 AND i.RecipeId NOT IN (SELECT RecipeId FROM CharacterRecipes) GROUP BY i.ItemId", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lastValues[(int)reader["ItemId"]] = (long)reader["MinBuyout"];
                            if (lastValues[(int)reader["ItemId"]] == 99999950000) { lastValues[(int)reader["ItemId"]] = long.MaxValue; }
                        }
                    }
                }

                long lastModified;
                using (SqlCommand cmd = new SqlCommand("SELECT MAX(LastModified) FROM Auctions a WHERE RealmId = @RealmId", conn))
                {
                    cmd.Parameters.AddWithValue("RealmId", realm.RealmId);
                    Int64.TryParse(cmd.ExecuteScalar().ToString(), out lastModified);
                }

                Console.WriteLine(DateTime.Now + " - " + realm.Name + ": " + lastModified + " / " + stub.Files[0].LastModified);

                if (lastModified < stub.Files[0].LastModified)
                {
                    Console.WriteLine(DateTime.Now + " - " + realm.Name + ": Found update of: " + stub.Files[0].LastModified + " - last modified " + (updateTimes.ContainsKey(slug) ? updateTimes[slug].ToString() : " NEVER"));

                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("RealmId", typeof(int)));
                    dt.Columns.Add(new DataColumn("Faction", typeof(string)));
                    dt.Columns.Add(new DataColumn("AuctionId", typeof(long)));
                    dt.Columns.Add(new DataColumn("ItemId", typeof(int)));
                    dt.Columns.Add(new DataColumn("Owner", typeof(string)));
                    dt.Columns.Add(new DataColumn("Bid", typeof(long)));
                    dt.Columns.Add(new DataColumn("Buyout", typeof(long)));
                    dt.Columns.Add(new DataColumn("Quantity", typeof(long)));
                    dt.Columns.Add(new DataColumn("TimeLeft", typeof(string)));
                    dt.Columns.Add(new DataColumn("LastModified", typeof(long)));

                    AuctionFile file = ApiClient.GetAuctions(stub.Files[0].Url);
                    //AuctionFile file = ApiClient.GetAuctions("http://localhost/auctions.json");
                    foreach (var a in file.Alliance.Auctions)
                    {
                        AddAuctionRow(stub, a, file, dt, "Alliance", realm);
                        if (lastValues.ContainsKey(a.Item) && lastValues[a.Item] > a.Buyout)
                        {
                            Console.WriteLine(DateTime.Now + " - " + realm.Name + ": New price for " + db.Items.Single(c => c.ItemId == a.Item).Name + " - was " + lastValues[a.Item] + " : now = " + a.Buyout);
                        }
                    }
                    foreach (var a in file.Horde.Auctions)
                    {
                        AddAuctionRow(stub, a, file, dt, "Horde", realm);
                        if (lastValues.ContainsKey(a.Item) && lastValues[a.Item] > a.Buyout)
                        {
                            Console.WriteLine(DateTime.Now + " - " + realm.Name + ": New price for " + db.Items.Single(c => c.ItemId == a.Item).Name + " - was " + lastValues[a.Item] + " : now = " + a.Buyout);
                        }
                    }
                    foreach (var a in file.Neutral.Auctions)
                    {
                        AddAuctionRow(stub, a, file, dt, "Neutral", realm);
                        if (lastValues.ContainsKey(a.Item) && lastValues[a.Item] > a.Buyout)
                        {
                            Console.WriteLine(DateTime.Now + " - " + realm.Name + ": New price for " + db.Items.Single(c => c.ItemId == a.Item).Name + " - was " + lastValues[a.Item] + " : now = " + a.Buyout);
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("DELETE Auctions WHERE RealmId = @RealmId", conn))
                    {
                        cmd.Parameters.AddWithValue("RealmId", realm.RealmId);
                        cmd.ExecuteNonQuery();
                    }

                    SqlBulkCopy copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null) { DestinationTableName = "[Auctions]", BatchSize = dt.Rows.Count };
                    copy.WriteToServer(dt);
                }
            }

            Console.WriteLine(DateTime.Now + " - " + realm.Name + ": Done updating auctions for " + realm.Name);
        }

        private static void AddAuctionRow(AuctionStub stub, Auction a, AuctionFile file, DataTable dt, string faction, Realm realm)
        {
            DataRow dr = dt.NewRow();
            dr["RealmId"] = realm.RealmId;
            dr["Faction"] = faction;
            dr["AuctionId"] = a.Auc;
            dr["ItemId"] = a.Item;
            dr["Owner"] = a.Owner;
            dr["Bid"] = a.Bid;
            dr["Buyout"] = a.Buyout;
            dr["Quantity"] = a.Quantity;
            dr["TimeLeft"] = a.TimeLeft;
            dr["LastModified"] = stub.Files[0].LastModified;
            dt.Rows.Add(dr);
        }
    }
}