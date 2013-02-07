using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WoWJSON
{
    [DataContract]
    public class Character
    {
        [DataMember(Name = "achievementPoints")]
        public int AchievementPoints { get; set; }
        [DataMember(Name = "battlegroup")]
        public string Battlegroup { get; set; }
        [DataMember(Name = "calcClass")]
        public string CalcClass { get; set; }
        [DataMember(Name = "class")]
        public int ClassId { get; set; }
        [DataMember(Name = "gender")]
        public int Gender { get; set; }
        [DataMember(Name = "lastModified")]
        public long LastModified { get; set; }
        [DataMember(Name = "level")]
        public int Level { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "race")]
        public int Race { get; set; }
        [DataMember(Name = "realm")]
        public string Realm { get; set; }
        [DataMember(Name = "thumbnail")]
        public string Thumbnail { get; set; }
        [DataMember(Name = "professions")]
        public ProfessionList Professions { get; set; }
        [DataMember(Name = "companions")]
        public List<int> Companions { get; set; }
        [DataMember(Name = "guild")]
        public CharacterGuildDescription Guild { get; set; }
    }
}
