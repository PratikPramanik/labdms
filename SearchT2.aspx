<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SearchT2.aspx.cs" Inherits="SearchT2" Title="LabDMS - Broad Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="SecondBar" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
    <div id="body">
        Query:<asp:TextBox ID="CommonSearchTextBox" Runat="server" Width="390px" 
             ontextchanged="change"></asp:TextBox>
        <script runat="server">
            void change(Object sender, EventArgs e)
            {
                Label1.Text = ((TextBox)sender).Text;
            }
            void Type1_Button(Object sender, EventArgs e)
            {
                Response.Redirect("Search.aspx?T2="
                          + Server.UrlEncode(TextBoxQuery.Text));
        </script>
        <asp:Button ID="SearchButton" runat="server" Text="Search" 
            Width="137px" OnClick="T2_Click" />
        &nbsp;<asp:Button ID="Button1" runat="server" Text="Use Regular Search &gt;&gt;" 
            Width="120px" OnClick="Type1_Button"/><br />
        <asp:Label ID="Label1" runat="server" Text="Query Entered:" Visible="True"></asp:Label>
        <br /><br />
        <b>Results:</b><br />
        <asp:Label ID="Results" runat="server"
            TextMode="MultiLine" Width="760px" BackColor="#F7F6F2" 
            BorderColor="#1740B8" ForeColor="Black">Results will be posted here!</asp:Label>
        <br />
        <asp:HyperLink ID="Home_Bottom" runat="server" Font-Size="Small" 
            NavigateUrl="~/Default.aspx">&lt;&lt; Back to Home</asp:HyperLink> 
            |<asp:HyperLink ID="HyperLink1" runat="server" Font-Size="Small" 
            NavigateUrl="~/Search.aspx">Regular Search &gt;&gt;</asp:HyperLink>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetAdById" 
            TypeName="AdsDataComponentTableAdapters.AdsDataAdapter">
            <SelectParameters>
                <asp:ControlParameter ControlID="Label1" DefaultValue="" Name="Id" 
                    PropertyName="Text" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>