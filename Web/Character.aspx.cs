using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;
using System.Web.UI;
using System.Linq;
using WoWModel;

namespace Web
{
    public partial class CharacterPage : Page
    {
        [WebMethod]
        public static List<string> GetRealms()
        {
            return new WoWEntities().Characters.Select(a => a.Realm).Select(b => b.Slug).Distinct().ToList();
        }

        [WebMethod]
        public static void CheckRealm(string realm, long lastModified)
        {
            if (lastModified > new WoWEntities().Auctions.Where(a => a.Realm.Slug == realm).Max(b => b.LastModified))
            {
                // Get updates!
            }
        }

        private readonly WoWEntities _db = new WoWEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = Char.Name + " - " + Tradeskill.Name;
            lvPrimaryTradeskils.DataSource = _db.Characters.Single(a => a.Name == Char.Name).Recipes.Select(a => a.Tradeskill).Where(a => a.Primary == false).Distinct();
            lvPrimaryTradeskils.DataBind();
            lvSecondaryTradeskills.DataSource = _db.Characters.Single(a => a.Name == Char.Name).Recipes.Select(a => a.Tradeskill).Where(a => a.Primary == true).Distinct();
            lvSecondaryTradeskills.DataBind();
            lvCharacters.DataSource = _db.Characters.Where(a=>a.LastModified > 1);
            lvCharacters.DataBind();
            GetTable();
        }

        protected bool Learned
        {
            get { return string.IsNullOrWhiteSpace(Request.QueryString["Learned"]) ? false : Convert.ToBoolean(Request.QueryString["Learned"]); }
        }

        private Character _char;
        protected Character Char
        {
            get
            {
                if (_char != null)
                {
                    return _char;
                }
                if (string.IsNullOrWhiteSpace(Request.QueryString["Character"]))
                {
                    _char = _db.Characters.First();
                }
                else
                {
                    string name = Request.QueryString["Character"];
                    _char = _db.Characters.Single(a => a.Name == name);
                }
                return _char;
            }
        }

        protected int TradeskillId
        {
            get
            {
                return string.IsNullOrWhiteSpace(Request.QueryString["TradeskillId"]) ? GetCharacterSkill() : Convert.ToInt32(Request.QueryString["TradeskillId"]);
            }
        }

        private Tradeskill _tradeskill;
        protected Tradeskill Tradeskill
        {
            get
            {
                return _tradeskill ?? (_tradeskill = _db.Tradeskills.Single(a => a.TradeskillId == TradeskillId));
            }
        }

        private int GetCharacterSkill()
        {
            return _db.Characters.Single(a => a.Name == Char.Name).Recipes.Where(b => b.Tradeskill.Primary == true).First().TradeskillId;
        }

        private void GetTable()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TdbConnectionString"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("spGetCharacterRecipes", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("CharacterName", Char.Name);
                    cmd.Parameters.AddWithValue("TradeskillId", TradeskillId);
                    cmd.Parameters.AddWithValue("HasRecipe", Learned);
                    using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                    {
                        ad.Fill(ds);
                        ds.Relations.Add("Reagents", ds.Tables[0].Columns["SpellId"], ds.Tables[1].Columns["RecipeId"], false);
                        ds.Relations.Add("Auctions", ds.Tables[0].Columns["SpellId"], ds.Tables[2].Columns["RecipeId"], false);
                    }
                }
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow drRecipe = ds.Tables[0].Rows[i];

                object craftedItemId = drRecipe["SkillName"].ToString().IndexOf("Enchant ") == 0 ? drRecipe["RecipeItemId"] : drRecipe["CraftedItemId"];

                sb.AppendFormat("<tr class=\"unknown {0}\">", i % 2 == 0 ? "row1" : "row2");
                sb.AppendFormat("<td>");
                sb.AppendFormat("<a href=\"http://www.wowhead.com/item={0}\" target=\"_blank\" class=\"itemLink\">", craftedItemId);
                sb.AppendFormat("<span class=\"icon-frame frame-18\" style='background-image: url(\"handler.ashx?thumb={0}.jpg\");'></span>", drRecipe["Icon"]);
                sb.AppendFormat("</a>");
                sb.AppendFormat("<a href=\"http://www.wowhead.com/spell={0}\" class=\"item-link color-q{1}\" target=\"_blank\">", drRecipe["SpellId"], drRecipe["Quality"]);
                sb.AppendFormat("<strong>{0}</strong>", drRecipe["SkillName"]);
                sb.AppendFormat("</a>");
                sb.AppendFormat("</td>");

                sb.Append("<td>");
                if (!string.IsNullOrWhiteSpace(drRecipe["RecipeItemId"].ToString()))
                {
                    sb.AppendFormat("<a href=\"http://www.wowhead.com/item={0}\" target=\"_blank\">Recipe</a>", drRecipe["RecipeItemId"]);
                }
                sb.Append("</td>");

                sb.Append("<td>");

                //DataRow[] childRows = drRecipe.GetChildRows("Reagents");
                //if (childRows.Length > 0)
                //{
                //    sb.Append("<tr class=\"reagents\"><td>" + string.Join(", ", childRows.Select(dr => dr["ItemId"].ToString())) + "</td></tr>");
                //}
                //      <td class="reagents" data-raw="2">
                //    <div class="reagent-list">
                //        <a href="/wow/en/item/23564" class="item-link reagent"><span class="icon-frame frame-18 " style="background-image: url(&quot;http://us.media.blizzard.com/wow/icons/18/inv_chest_chain_17.jpg&quot;);"> 1 </span><span style="display: none">Twisting Nether Chain Shirt</span> </a>
                //        <a href="/wow/en/item/30183" class="item-link reagent"><span class="icon-frame frame-18 " style="background-image: url(&quot;http://us.media.blizzard.com/wow/icons/18/inv_elemental_mote_nether.jpg&quot;);"> 2 </span><span style="display: none">Nether Vortex</span> </a>
                //        <span class="clear"></span>
                //    </div>
                //</td>--%>
                //if (drRecipe["RecipeItemId"] == DBNull.Value)
                //{
                    sb.Append(drRecipe["Source"]);
                //}
                //else
                //{
                //    sb.AppendFormat("<a href='http://www.wowhead.com/item={0}' class='itemLink color-q{2}'>{1}</a>", drRecipe["RecipeItemId"], drRecipe["Source"], drRecipe["Quality"]);
                //}
                sb.AppendFormat("</td>");
                sb.AppendFormat("<td>{0}</td>", GetWoWMoney(drRecipe["ServerwideAverage"]));

                if (drRecipe["Source"].ToString() != "Vendor" && !Learned)
                {
                    DataRow[] auctionRows = drRecipe.GetChildRows("Auctions");
                    if (auctionRows.Length > 0)
                    {
                        foreach (DataRow dr in auctionRows)
                        {
                            sb.AppendFormat("<tr><td>{0} - {1}</td><td>{2}</td><td>{3}</td></tr>", "", dr["Faction"], FromEpochTime(dr["LastModified"]), GetWoWMoney(dr["Buyout"]));
                        }
                    }
                }

                sb.Append("</tr>");
            }

            RecipeTable.Text = sb.ToString();
        }

        protected string FromEpochTime(object lastModified)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime addSeconds = epoch.AddSeconds((Convert.ToInt64(lastModified.ToString()) / 1000));
            return addSeconds.ToLocalTime().ToString();
        }

        protected string GetWoWMoney(object val)
        {
            if (val == DBNull.Value) { return string.Empty; }
            var m = Convert.ToInt64(val);
            var copper = m % 100;
            m = (m - copper) / 100;
            var silver = m % 100;
            var gold = (m - silver) / 100;

            return string.Format("<span class='price'><span class='icon-gold'>{0}</span><span class='icon-silver'>{1}</span><span class='icon-copper'>{2}</span></span>", gold, silver, copper);
        }
    }
}
