using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WoWJSON
{
    [DataContract]
    public class RealmStatusList
    {
        [DataMember(Name = "realms")]
        public List<RealmDescription> Realms;
    }

    [DataContract]
    public class RealmDescription
    {
        [DataMember(Name = "type")]
        public string Type;
        [DataMember(Name = "population")]
        public string Population;
        [DataMember(Name = "queue")]
        public bool Queue;
        [DataMember(Name = "status")]
        public bool Status;
        [DataMember(Name = "name")]
        public string Name;
        [DataMember(Name = "slug")]
        public string Slug;
        [DataMember(Name = "battlegroup")]
        public string Battlegroup;
        [DataMember(Name = "locale")]
        public string Locale;
        [DataMember(Name = "timezone")]
        public string Timezone;
    }
}
/* {
         "type":"pvp",
         "population":"medium",
         "queue":false,
         "wintergrasp":{
            "area":1,
            "controlling-faction":1,
            "status":0,
            "next":1347996700902
         },
         "tol-barad":{
            "area":21,
            "controlling-faction":0,
            "status":0,
            "next":1347995761146
         },
         "status":true,
         "name":"冰風崗哨",
         "slug":"冰風崗哨",
         "battlegroup":"嗜血",
         "locale":"zh_TW",
         "timezone":"Asia/Taipei"
      }*/