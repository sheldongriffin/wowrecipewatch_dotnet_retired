//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WoWModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tradeskill
    {
        public Tradeskill()
        {
            this.Recipes = new HashSet<Recipe>();
            this.Characters = new HashSet<Character>();
        }
    
        public int TradeskillId { get; set; }
        public string Name { get; set; }
        public bool Primary { get; set; }
        public string Icon { get; set; }
    
        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
    }
}
