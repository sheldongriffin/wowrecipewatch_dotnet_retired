<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecipeList.aspx.cs" Inherits="Web.RecipeList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Recipe List</title>
    <link rel="Stylesheet" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.17/themes/base/jquery-ui.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
    <link href="common.css" rel="Stylesheet" />
    <link href="wow.css" rel="Stylesheet" />
    <link href="profile.css" rel="Stylesheet" />
    <link href="wiki.css" rel="Stylesheet" />
    <link href="profession.css" rel="Stylesheet" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.17/jquery-ui.min.js"></script>
    <link href="wowhead2.css" rel="Stylesheet" />
    <style type="text/css">
        body
        {
            background: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="table">
        <table>
            <thead>
            </thead>
            <tbody>
                <asp:ListView ID="ListView1" runat="server" DataSourceID="SqlDataSource1">
                    <ItemTemplate>
                        <tr class="<%# Container.DisplayIndex % 2 == 0 ? "row1" : "row2" %> <%# Eval("HasRecipe").ToString() == "1" ? "learned" : "unknown" %>">
                            <td>
                                <%# Eval("CharacterName") %>
                            </td>
                            <td>
                                <%# Eval("TradeskillName") %>
                            </td>
                            <td>
                                <a href="" class="item-link color-q<%# Eval("HasRecipe")%>"><%# Eval("RecipeName") %></a>
                            </td>
                            <td>
                                <%# Eval("Source") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
<asp:sqldatasource id="SqlDataSource1" runat="server" connectionstring="<%$ ConnectionStrings:TdbConnectionString %>"
    selectcommand="spGetCharacterRecipeList" selectcommandtype="StoredProcedure"></asp:sqldatasource>
