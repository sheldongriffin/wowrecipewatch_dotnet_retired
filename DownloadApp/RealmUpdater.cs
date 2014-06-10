using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WoWJSON;
using WoWModel;

namespace DownloadApp
{
    static class RealmUpdater
    {
        public static void Update()
        {
            WoWEntities db = new WoWEntities();

            if (db.Realms.Count() == 0)
            {
                RealmStatusList realms = ApiClient.GetRealms();
                foreach (RealmDescription realm in realms.Realms)
                {
                    db.Realms.Add(new Realm() { Battlegroup = realm.Battlegroup, Name = realm.Name, Population = realm.Population, Queue = realm.Queue, Slug = realm.Slug, Status = realm.Status });
                }
                db.SaveChanges();
            }
        }
    }
}
