using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WoWJSON
{
    [DataContract]
    public class AuctionFile
    {
        [DataMember(Name = "realm")]
        public RealmDescription Realm;
        [DataMember(Name = "alliance")]
        public AuctionList Alliance;
        [DataMember(Name = "neutral")]
        public AuctionList Neutral;
        [DataMember(Name = "horde")]
        public AuctionList Horde;
    }

    [DataContract]
    public class AuctionList
    {
        [DataMember(Name = "auctions")]
        public List<Auction> Auctions; 
    }

    [DataContract]
    public class Auction
    {
        [DataMember(Name = "auc")]
        public long Auc;
        [DataMember(Name = "item")]
        public int Item;
        [DataMember(Name = "owner")]
        public string Owner;
        [DataMember(Name = "bid")]
        public long Bid;
        [DataMember(Name = "buyout")]
        public long Buyout;
        [DataMember(Name = "quantity")]
        public long Quantity;
        [DataMember(Name = "timeLeft")]
        public string TimeLeft;
    }
}
