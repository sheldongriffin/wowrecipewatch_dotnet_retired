using System.Collections.Generic;
using System.Runtime.Serialization;
//using MongoDB.Bson.Serialization.Attributes;

namespace WoWJSON
{
    [DataContract]
    public class Item
    {
        //[BsonId]
        [DataMember(Name = "id")]
        public int ItemId;
        [DataMember(Name = "description")]
        public string Description;
        [DataMember(Name = "name")]
        public string Name;
        [DataMember(Name = "icon")]
        public string Icon;
        [DataMember(Name = "itemBind")]
        public int ItemBind;
        [DataMember(Name = "itemSpells")]
        public List<ItemSpellData> ItemSpells;
        [DataMember(Name = "itemSource")]
        public ItemSourceDescription ItemSource;
        [DataMember(Name = "quality")]
        public int Quality { get; set; }
        [DataMember(Name = "armor")]
        public int Armor;
        [DataMember(Name = "baseArmor")]
        public int BaseArmor;
        [DataMember(Name = "buyPrice")]
        public int BuyPrice;
        [DataMember(Name = "containerSlots")]
        public int ContainerSlots;
        [DataMember(Name = "inventoryType")]
        public int InventoryType;
        [DataMember(Name = "itemClass")]
        public int ItemClass;
        [DataMember(Name = "itemLevel")]
        public int ItemLevel;
        [DataMember(Name = "itemSubClass")]
        public int ItemSubClass;
        [DataMember(Name = "maxCount")]
        public int MaxCount;
        [DataMember(Name = "maxDurability")]
        public int MaxDurability;
        [DataMember(Name = "minFactionId")]
        public int MinFactionId;
        [DataMember(Name = "minReputation")]
        public int MinReputation;
        [DataMember(Name = "requiredLevel")]
        public int RequiredLevel;
        [DataMember(Name = "requiredSkill")]
        public int RequiredSkill;
        [DataMember(Name = "requiredSkillRank")]
        public int RequiredSkillRank;
        [DataMember(Name = "sellPrice")]
        public int SellPrice;
        [DataMember(Name = "stackable")]
        public int Stackable;
        [DataMember(Name = "equippable")]
        public bool Equippable;
        [DataMember(Name = "hasSockets")]
        public bool HasSockets;
        [DataMember(Name = "isAuctionable")]
        public bool IsAuctionable;
        [DataMember(Name = "bonusStats")]
        public List<BonusStat> BonusStats;
    }

    [DataContract]
    public class BonusStat
    {
        [DataMember(Name = "amount")]
        public int Amount;
        [DataMember(Name = "stat")]
        public int Stat;
        [DataMember(Name = "reforged")]
        public bool Reforged;
    }

    [DataContract]
    public class ItemSourceDescription
    {
        [DataMember(Name = "sourceId")]
        public int SourceId;
        [DataMember(Name = "sourceType")]
        public string SourceType;
    }

    [DataContract]
    public class ItemSpellData
    {
        [DataMember(Name = "spellId")]
        public int SpellId;
        [DataMember(Name = "spell")]
        public ItemSpell Spell;
        [DataMember(Name = "nCharges")]
        public int Charges;
        [DataMember(Name = "consumable")]
        public bool Consumable;
        [DataMember(Name = "categoryId")]
        public int CategoryId;
        [DataMember(Name = "trigger")]
        public string Trigger;
    }

    [DataContract]
    public class ItemSpell
    {
        [DataMember(Name = "id")]
        public int Id;
        [DataMember(Name = "name")]
        public string Name;
        [DataMember(Name = "icon")]
        public string Icon;
        [DataMember(Name = "description")]
        public string Description;
        [DataMember(Name = "castTime")]
        public string CastTime;
    }
}
