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
    
    public partial class Reagent
    {
        public int RecipeId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
