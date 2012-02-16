using System;

public partial class Default_aspx : System.Web.UI.Page
{

    protected void T2_Click(object sender, EventArgs e)
    {
        //Redirect to search page
        Response.Redirect("SearchT2.aspx?T2="
                          +Server.UrlEncode(Type2SearchBox.Text));
    }
}
