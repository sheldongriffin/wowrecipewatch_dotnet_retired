<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Character.aspx.cs" Inherits="Web.CharacterPage" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title></title>
    <link rel="Stylesheet" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.17/themes/base/jquery-ui.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
    <link href="common.css" rel="Stylesheet" />
    <link href="wow.css" rel="Stylesheet" />
    <link href="profile.css" rel="Stylesheet" />
    <link href="wiki.css" rel="Stylesheet" />
    <link href="profession.css" rel="Stylesheet" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.17/jquery-ui.min.js"></script>
    <link href="wowhead2.css" rel="Stylesheet" />
    <script type="text/javascript">
        var tradeskill = { 129: 'First Aid', 164: 'Blacksmithing', 165: 'Leatherworking', 171: 'Alchemy', 185: 'Cooking', 186: 'Mining', 197: 'Tailoring', 202: 'Engineering', 333: 'Enchanting', 356: 'Fishing', 393: 'Skinning', 755: 'Jewelcrafting', 773: 'Inscription', 794: 'Archaeology' };

        $(document).ready(function () {
            //$.ajax({
            //    type: "POST",
            //    url: "Character.aspx/GetRealms",
            //    data: "{}",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (msg) {
            //        for (var i = 0; i < msg.d.length; i++) {
            //            $('#foo').append(msg.d[i]);
            //        }
            //    }
            //});
            $('a.itemLink, a.item-link').mouseover(function (evt) {
                $.getJSON("ItemTooltip.aspx", { url: $(this).attr('href') }, function (data) {
                    if (data == null) { return; }
                    $('div.wowhead-tooltip').show().css({ 'top': evt.pageY + 10, 'left': evt.pageX + 20 });
                    var html = '<b class="q' + data.quality + '">' + data.name + (typeof data.cached != "undefined" ? "(C)" : "") + '</b>';
                    if (data.itemBind == 1) {
                        html += '<br>Binds when picked up';
                    }
                    html += '<br>Requires <a href="/skill=' + data.requiredSkill + '" class="q1">' + tradeskill[data.requiredSkill] + '</a> (' + data.requiredSkillRank + ')';
                    html += '<br>Item Level ' + data.itemLevel;
                    if (data.itemSource != null) {
                        html += '<br>' + data.itemSource.sourceType + ' (' + data.itemSource.sourceId + ')';
                    }
                    $('#itemHeader').html(html);
                });
            }).mouseout(function (evt) {
                $('div.wowhead-tooltip').hide();
            });
        });
    </script>
</head>
<body>
    <div id="foo"></div>
    <form id="form1" runat="server">
    <div id="wrapper">
        <div id="header">
        </div>
        <div id="content">
            <div class="content-top">
                <div class="content-trail">
                    <asp:ListView ID="lvCharacters" runat="server">
                        <LayoutTemplate>
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <a href="Character.aspx?Character=<%#Eval("Name") %>"><%#Eval("Name") %></a>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
                <div class="content-bot">
                    <div id="profile-wrapper" class="profile-wrapper profile-wrapper-alliance">
                        <div class="profile-sidebar-anchor">
                            <div class="profile-sidebar-outer">
                                <div class="profile-sidebar-inner">
                                    <div class="profile-sidebar-contents">
                                        <div class="profile-sidebar-crest">
                                            <a href="Character.aspx?Character=<%= Char.Name %>/" class="profile-sidebar-character-model" style="background-image: url(http://us.battle.net/static-render/us/<%= Char.Thumbnail.Replace("avatar","inset") %>?alt=/wow/static/images/2d/inset/11-1.jpg);">
                                                <span class="hover"></span><span class="fade"></span>
                                            </a>
                                            <div class="profile-sidebar-info">
                                                <div class="name"><a href="Character.aspx?Character=<%= Char.Name %>/"><%= Char.Name %></a></div>
                                                <div class="under-name color-c<%= Char.ClassId %>">
                                                    <span class="level"><strong><%= Char.Level %></strong></span> <a href="http://us.battle.net/wow/en/game/race/draenei" class="race">Draenei</a> <a href="http://us.battle.net/wow/en/game/class/1" class="class">1</a>
                                                </div>
                                                <div class="guild">
                                                    <a href="/wow/en/guild/<%= Char.Realm.Slug %>/Sabi/?character=<%= Char.Name %>">Sabi</a>
                                                </div>
                                                <div class="realm">
                                                    <span id="profile-info-realm" class="tip"><%= Char.Realm.Name %></span>
                                                </div>
                                            </div>
                                        </div>
                                        <ul class="profile-sidebar-menu" id="profile-sidebar-menu">
                                            <li><a href="/wow/en/character/aerie-peak/<%= Char.Name %>/" class="back-to"><span class="arrow"><span class="icon">Character Summary</span></span></a> </li>
                                            <li class=""><span class="divider">Primary Professions</span> </li>
                                            <asp:ListView ID="lvPrimaryTradeskils" runat="server">
                                                <LayoutTemplate>
                                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <li class="<%# (int)Eval("TradeskillId") == Tradeskill.TradeskillId ? "active" : "" %>">
                                                        <a href="Character.aspx?Character=<%= Char.Name %>&amp;TradeskillId=<%# Eval("TradeskillId") %>">
                                                            <span class="arrow"><span class="icon"><span class="icon-frame frame-14 ">
                                                                <img src="handler.ashx?thumb=<%# Eval("Icon") %>" alt="" width="14" height="14" />
                                                            </span><%# Eval("Name") %></span></span>
                                                        </a>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:ListView>
                                            <li class=""><span class="divider">Secondary Skills</span> </li>
                                            <asp:ListView ID="lvSecondaryTradeskills" runat="server">
                                                <LayoutTemplate>
                                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <li>
                                                        <a href="Character.aspx?Character=<%= Char.Name %>&amp;TradeskillId=<%# Eval("TradeskillId") %>">
                                                            <span class="arrow"><span class="icon"><span class="icon-frame frame-14 ">
                                                                <img src="handler.ashx?thumb=<%# Eval("Icon") %>" alt="" width="14" height="14" />
                                                            </span><%# Eval("Name") %></span></span>
                                                        </a>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="profile-contents">
                            <div class="profile-section-header">
                                <div class="profession-rank">
                                    <div class="profile-progress border-3 completed">
                                        <div class="bar border-3 hover" style="width: 100%"></div>
                                        <div class="bar-contents">
                                            <a class="profession-details" href="/wow/en/profession/<%= Tradeskill.Name %>"><span class="name">Illustrious</span> <span class="value">525 / 525</span> </a>
                                        </div>
                                    </div>
                                </div>
                                <h3 class="category ">
                                    <a href="/wow/en/profession/<%= Tradeskill.Name %>">
                                        <span class="icon-frame frame-18 " style='background-image: url("handler.ashx?thumb=<%= Tradeskill.Icon%>");'></span>
                                        <%=Tradeskill.Name %>
                                    </a>
                                </h3>
                            </div>
                            <div class="profile-section">
                                <div class="profile-filters" id="profession-filters">
                                    <div class="keyword">
                                        <span class="view" style="display: none;"></span><span class="reset" style=""></span>
                                        <input class="input active" id="filter-keyword" type="text" value="Filter..." />
                                    </div>
                                    <div class="tabs">
                                        <a href="Character.aspx?Character=<%= Char.Name %>&amp;TradeskillId=<%= Tradeskill.TradeskillId %>&amp;Learned=True" class="<%= Learned ? "tab-active" : "" %>">Learned (397) </a>
                                        <a href="Character.aspx?Character=<%= Char.Name %>&amp;TradeskillId=<%= Tradeskill.TradeskillId %>&amp;Learned=False" class="<%= !Learned ? "tab-active" : "" %>">Not Yet Learned (199) </a>
                                    </div>
                                </div>
                                <div id="professions" style="display: block;">
                                    <div class="table-options data-options table-top">
                                        <div class="option">
                                            <ul class="ui-pagination"></ul>
                                        </div>
                                        Showing <strong class="results-start">1</strong>–<strong class="results-end">6</strong>
                                        of <strong class="results-total">6</strong> results <span class="clear"></span>
                                    </div>
                                    <div class="data-container type-table">
                                        <div class="table ">
                                          <table id="recipes">
                                            <thead>
                                                <tr>
                                                    <th class=" first-child"><a href="javascript:;" class="sort-link default"><span class="arrow down">Name </span></a></th>
                                                    <th><a href="javascript:;" class="sort-link numeric"><span class="arrow">Source </span></a></th>
                                                    <th><a class="sort-link numeric last-child"><span class="arrow">Avg </span></a></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Literal ID="RecipeTable" runat="server"></asp:Literal>
                                            </tbody>
                                        </table>
                                        </div>
                                    </div>
                                    <div class="table-options data-options table-bottom">
                                        <div class="option">
                                            <ul class="ui-pagination"></ul>
                                        </div>
                                        Showing <strong class="results-start">1</strong>–<strong class="results-end">6</strong>
                                        of <strong class="results-total">6</strong> results <span class="clear"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="footer"></div>
            <div id="service"></div>
        </div>
    </div>

    <asp:SqlDataSource FilterExpression="HasRecipe = {0}" ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:TdbConnectionString %>" SelectCommand="
    select *, r.Name as SkillName, i.ItemId AS RecipeItemId, '' AS Icon,
 (select COUNT(recipeid) from characterrecipes cr INNER JOIN Characters c ON cr.CharacterId = c.Id where cr.RecipeId = r.SpellId and 
 c.Name = @CharacterName) as hasrecipe
 from recipes r 
    left join items i on r.spellid = i.teaches
    where TradeskillId = @TradeskillId
    ORDER BY (SELECT ISNULL(MIN(buyout), 99736510100) FROM auctions a WHERE a.ItemId = i.ItemId) , Source">
        <FilterParameters>
            <asp:QueryStringParameter DefaultValue="False" Name="Learned" QueryStringField="Learned" Type="Boolean" />
        </FilterParameters>
        <SelectParameters>
            <asp:Parameter Name="CharacterName" Type="String" />
            <asp:Parameter Name="TradeskillId" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    </form>
    <div class="wowhead-tooltip" style="position: absolute; width: 320px; display: none;">
        <p style="background-image: none; visibility: hidden;"><div></div></p>
        <table class="">
            <tbody>
                <tr>
                    <td>
                        <table style="white-space: nowrap; width: 100%;">
                            <tbody>
                                <tr>
                                    <td id="itemHeader">
                                        <b class="q2">Plans: Adamantite Weapon Chain</b><br />
                                        Requires <a href="/skill=164" class="q1"><%= Tradeskill.Name %></a> (335)<br />
                                        Item Level 63
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table style="width: 100%;">
                            <tbody>
                                <tr>
                                    <td></td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                    <th style="background-position: 100% 0%;"></th>
                </tr>
                <tr>
                    <th style="background-position: 0% 100%;"></th>
                    <th style="background-position: 100% 100%;"></th>
                </tr>
            </tbody>
        </table>
        <div class="wowhead-tooltip-powered" style="display: none;">
        </div>
    </div>
</body>
</html>
