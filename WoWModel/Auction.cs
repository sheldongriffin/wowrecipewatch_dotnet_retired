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
    
    public partial class Auction
    {
        public int RealmId { get; set; }
        public string Faction { get; set; }
        public long AuctionId { get; set; }
        public int ItemId { get; set; }
        public string Owner { get; set; }
        public Nullable<long> Bid { get; set; }
        public Nullable<long> Buyout { get; set; }
        public long Quantity { get; set; }
        public string TimeLeft { get; set; }
        public long LastModified { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual Realm Realm { get; set; }
    }
}
