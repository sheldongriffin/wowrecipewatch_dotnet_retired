using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WoWJSON
{
    [DataContract]
    public class ProfessionList
    {
        [DataMember(Name = "primary")]
        public List<Profession> Primary { get; set; }
        [DataMember(Name = "secondary")]
        public List<Profession> Secondary { get; set; }
    }

    [DataContract]
    public class Profession
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "icon")]
        public string Icon { get; set; }
        [DataMember(Name = "recipes")]
        public List<int> Recipes { get; set; }
    }
}
