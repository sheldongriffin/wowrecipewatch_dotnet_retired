using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WoWJSON
{
    public static class ApiClient
    {
        public static AuctionStub GetAuctionStub(string realm)
        {
            return GetWebObject<AuctionStub>("http://us.battle.net/api/wow/auction/data/" + realm);
        }

        public static AuctionFile GetAuctions(string uri)
        {
            return GetWebObject<AuctionFile>(uri);
        }

        public static Item GetItem(int itemId)
        {
            return GetWebObject<Item>("http://us.battle.net/api/wow/item/" + itemId);
        }

        public static List<Character> GetGuildMembers(string name, string realm)
        {
            var guild = GetWebObject<Guild>("http://us.battle.net/api/wow/guild/" + realm + "/" + name + "?fields=members");
            return guild.Members.Select(a=>a.Character).ToList();
        }

        public static Character GetCharacterSmall(string name, string realm)
        {
            return GetWebObject<Character>("http://us.battle.net/api/wow/character/" + realm + "/" + name + "?fields=guild");
        }

        public static Character GetCharacter(string name, string realm)
        {
            return GetWebObject<Character>("http://us.battle.net/api/wow/character/" + realm + "/" + name + "?fields=professions,companions");
        }

        public static RealmStatusList GetRealms()
        {
            return GetWebObject<RealmStatusList>("http://us.battle.net/api/wow/realm/status");
        }

        private static T GetWebObject<T>(string url)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.Proxy = null;
            request.UseDefaultCredentials = true;

            CookieContainer cc = new CookieContainer();
            request.CookieContainer = cc;

            string result = string.Empty;
            //while (String.IsNullOrWhiteSpace(result))
            //{
                try
                {
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                        //Close and clean up the StreamReader
                        sr.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error retrieving page, trying again in 1 minute");
                    return default(T);
                    //System.Threading.Thread.Sleep(1000*60);
                }
            //}

            File.WriteAllText(url.Replace("/", "").Replace(".", "").Replace(":","").Replace("?","").Replace("=",""), result);


            using (MemoryStream m = new MemoryStream(Encoding.UTF8.GetBytes(result)))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof (T));
                return (T) ser.ReadObject(m);
            }
        }
    }
}
