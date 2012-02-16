<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Indexer.aspx.cs" 
    Inherits="Admin_Indexer" Title="Admin >> LabDMS Document Indexer"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:HyperLink ID="HyperLink1" runat="server" 
            NavigateUrl="~/Admin/Default.aspx" style="font-size: small">&lt;&lt;Back</asp:HyperLink>
        <b>
        <br />
        LabDMS Document Indexer</b><br />
        <asp:Button ID="Button1" runat="server" Text="Index All Documents" OnClick="IndexDocs"/>
        <br />
        Status:<br />
        <asp:TextBox ID="IndexStatus" runat="server" BackColor="Black" 
            Font-Names="Tahoma" Font-Size="X-Small" ForeColor="White" Height="300px" 
            ReadOnly="True" TextMode="MultiLine" Width="600px">Click &quot;Index All Documents&quot; to begin indexing.</asp:TextBox>
    
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\CLASSIFIEDSDB.mdf;Integrated Security=True;User Instance=True" 
        ProviderName="System.Data.SqlClient" 
        SelectCommand="SELECT [Id], [Title], [Description], [AdStatus], [Code], [Syn1], [Syn2], [Syn3], [Syn4], [Syn5] FROM [Ads]"
        DataSourceMode="DataSet"
        >
    </asp:SqlDataSource>
    </form>
</body>
</html>
