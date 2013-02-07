using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WoWJSON
{
    [DataContract]
    public class Recipe
    {
        [DataMember(Name = "id")]
        public int RecipeId;
        [DataMember(Name = "name")]
        public string Name;
        [DataMember(Name = "profession")]
        public string Profession;
        [DataMember(Name = "icon")]
        public string Icon;
    }
}
