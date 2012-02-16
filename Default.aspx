<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="Default.aspx.cs"
    Inherits="Default_aspx" Title="Lab Document Management System" %>
<%@ Register TagPrefix="uc1" TagName="FeaturedAd" Src="Controls/FeaturedAd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CategoryBrowse" Src="Controls/CategoryBrowse.ascx" %>
<asp:Content ID="SecondBarContent" ContentPlaceHolderID="SecondBar" Runat="server">
    <div id="crumbs_search">
        <asp:Panel ID="panel1" runat="server" DefaultButton="CommonSearchButton">
        <p>
            Search:
        </p>
        <p>
        
            <asp:TextBox ID="CommonSearchTextBox" Runat="server" CssClass="search_box" AccessKey="s"
                TabIndex="1"></asp:TextBox>
            <asp:Button ID="CommonSearchButton" Runat="server" Text="Search" CssClass="submit"
                PostBackUrl="~/Search.aspx" />
        </p></asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="col_main_left">
            <div id="announcements">
            <asp:Panel ID="panel2" runat="server" DefaultButton="T2Button">
                <ul>
                    <li>Administrators can upload forms to the server <a href="PostAd.aspx">Upload a New Form (Login Required)</a></li>
                    <li>Try a <b>Broad Search</b> for a wider range of search results:
                    <asp:TextBox ID="Type2SearchBox" Runat="server" CssClass="search_box" AccessKey="s"
                        TabIndex="1" Height="14px" Width="160px"></asp:TextBox>
                    <asp:Button ID="T2Button" Runat="server" Text=" Broad Search" CssClass="submit"
                        OnClick="T2_Click" Font-Size="Smaller" Height="22px" Width="76px" /></li>
                </ul></asp:Panel>
            </div>
        </div>
        <div id="col_main_right" align="center">
            <h3 class="section">
                Test Forms, Procedures and Other Lab Documents</h3>
            <div class="content_right">
                <uc1:CategoryBrowse ID="CategoryBrowser" Runat="server" AutoNavigate="True">
                </uc1:CategoryBrowse>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
            </div>
        </div>
    </div>
</asp:Content>
