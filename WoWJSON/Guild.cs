using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WoWJSON
{
    [DataContract]
    public class GuildBase
    {
        [DataMember(Name = "achievementPoints")]
        public int AchievementPoints { get; set; }
        [DataMember(Name = "level")]
        public int Level { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    [DataContract]
    public class CharacterGuildDescription : GuildBase
    {
        [DataMember(Name = "members")]
        public int MemberCount { get; set; }
    }

    [DataContract]
    public class Guild : GuildBase
    {
        [DataMember(Name = "members")]
        public List<GuildMember> Members { get; set; }
    }

    [DataContract]
    public class GuildMember
    {
        [DataMember(Name = "character")]
        public Character Character { get; set; }
        [DataMember(Name = "rank")]
        public int Rank { get; set; }
    }
}
