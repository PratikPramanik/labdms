using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;

public partial class EditAd_aspx : System.Web.UI.Page
{
    public int NumPostBacks
    {
        get
        {
            if (ViewState["NumPostBacks"] != null)
                return (int)ViewState["NumPostBacks"];
            else
            {
                ViewState["NumPostBacks"] = 0;
                return 0;
            }
        }
        set
        {
            ViewState["NumPostBacks"] = value;
        }
    }
    private string UrlReferrer
    {
        get
        {
            return ViewState["UrlReferrer"] as string;
        }
        set
        {
            ViewState["UrlReferrer"] = value;
        }

    }

    protected void CancelButton_Click(object sender, EventArgs e)
    {
        ReturnToPreviousPage();
    }
    protected void AdDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ReturnToPreviousPage();
    }
    protected void ChangeCategoryButton_Click(object sender, EventArgs e)
    {
        ChangeCategoryPanel.Visible = true;
        AdFormView.Enabled = false;
    }

    protected void ChangeCategoryCancelButton_Click(object sender, EventArgs e)
    {
        ChangeCategoryPanel.Visible = false;
        AdFormView.Enabled = true;
    }
    protected void ChangeCategoryOkButton_Click(object sender, EventArgs e)
    {
        if (CategoryDropDown.SelectedCategoryId != DefaultValues.CategoryIdMinValue)
        {
            int adId = Convert.ToInt32(this.AdFormView.DataKey.Value);
            AdsDB.UpdateAdCategory(adId, CategoryDropDown.SelectedCategoryId);
            AdFormView.DataBind();
        }

        ChangeCategoryPanel.Visible = false;
        AdFormView.Enabled = true;
    }
    protected void AdDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        bool adValid = false;
        AdsDataComponent.AdsRow ad = e.ReturnValue as AdsDataComponent.AdsRow;
        if (ad != null)
        {
            if (User.IsInRole(DefaultValues.AdministratorRole) || ad.MemberId == Profile.MemberId)
            {
                adValid = true;
                string adIdString = ad.Id.ToString();
                CategoryDropDown.CurrentCategoryId = ad.CategoryId;
                ManagePhotosLink.NavigateUrl = "~/ManagePhotos.aspx?id=" + adIdString;
                ShowAdLink.NavigateUrl = "~/ShowAd.aspx?id=" + adIdString;
            }
        }

        if (!adValid)
        {
            ReturnToPreviousPage();
        }
    }

    protected void ReturnToPreviousPage()
    {
        string referrer = UrlReferrer;
        if (referrer != null)
            Response.Redirect(referrer);
        else
            Response.Redirect("~/MyAds.aspx", true);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Check if the URL querystring contains a valid ad.
        int adId = DefaultValues.IdNullValue;
        string adIdQs = Request.QueryString["id"];
        if (adIdQs == null || !Int32.TryParse(adIdQs, out adId))
        {
            Response.Redirect("~/MyAds.aspx");
        }

        // Limit Description text.
        TextBox DescriptionTextBox = AdFormView.FindControl("DescriptionTextBox") as TextBox;
        DescriptionTextBox.Attributes.Add("onkeydown", "textCounter(this,500);");
        DescriptionTextBox.Attributes.Add("onkeyup", "textCounter(this,500);");

        if (!Page.IsPostBack)
        {
            ManagePhotosLinkPanel.Visible = SiteSettings.GetSharedSettings().AllowImageUploads;
            if (Request.UrlReferrer != null)
                this.UrlReferrer = Request.UrlReferrer.ToString();
        }
        else
            NumPostBacks++;

        int goBackSteps = NumPostBacks + 1;
        BackLink.NavigateUrl = String.Format("javascript:history.go(-{0});", goBackSteps);
    }

    protected void ValidLocationRequired_ServerValidate(object source, ServerValidateEventArgs args)
    {
        LocationDropDown_ascx LocationDropDown = AdFormView.FindControl("LocationDropDown") as LocationDropDown_ascx;
        args.IsValid = LocationDropDown != null && !LocationDropDown.CurrentLocation.Equals(String.Empty);
    }

    protected void PriceValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        decimal p = -1;
        TextBox PriceTextBox = AdFormView.FindControl("PriceTextBox") as TextBox;
        if (decimal.TryParse(PriceTextBox.Text, out p))
        {
            args.IsValid = (p >= 0);
        }
        else
            args.IsValid = false;
    }
    protected void URLValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = false;
        TextBox URLTextBox = AdFormView.FindControl("URLTextBox") as TextBox;
        if (URLTextBox != null && !URLTextBox.Text.Equals(String.Empty))
        {
            try
            {
                Uri uri = new Uri(URLTextBox.Text);
                if (uri.IsWellFormedOriginalString() &
                    (uri.Scheme == "http" | uri.Scheme == "https"))
                {
                    args.IsValid = true;
                }
            }
            catch
            {
            }
        }
        else
        {
            // Empty URL is okay.
            args.IsValid = true;
        }
   } 
    protected void AdFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        if (!Page.IsValid)
            e.Cancel = true;
    }
    
    //occurs when upload button is clicked
    protected void ChangeFile(object sender, EventArgs e)
    {
        string oldfilepath;
        string newfilepath;
        string emailmessage;
        TextBox URLTextBox = AdFormView.FindControl("URLTextBox") as TextBox;
        TextBox NewFilePath = AdFormView.FindControl("NewFilePath") as TextBox;
        FileUpload FileUpload = AdFormView.FindControl("FileUploadToChangeURLTextBox") as FileUpload;
        Label UploadMsg = AdFormView.FindControl("UploadMsg") as Label;

        oldfilepath = URLTextBox.Text;
        emailmessage = "Form " + ((TextBox)AdFormView.FindControl("TitleTextBox")).Text + " (" +
                       ((TextBox)AdFormView.FindControl("TextBoxCode")).Text + " has had it's file replaced";

        //if user has opted to Upload new file
        if (IsPostBack)
        {
            if (FileUpload.HasFile)
            {
                try
                {
                    //has to fill in the root with the server path
                    newfilepath = Server.MapPath("~/Upload/Forms/" +
                         FileUpload.FileName);
                    
                    FileUpload.SaveAs(newfilepath);

                    UploadMsg.Text = "File name: " +
                          FileUpload.PostedFile.FileName + "<br>" +
                          FileUpload.PostedFile.ContentLength + " kb<br>" +
                          "Content type: " +
                          FileUpload.PostedFile.ContentType;

                    //NEW!!! send webmaster email.
                    //email says file is uploaded... 
                    //may or may not be associated with database (send title as well)
                    emailmessage += "/nFile: [" + oldfilepath +
                                    "] has been replaced with [" + newfilepath + "].";

                    //update file path in textbox to update DB when user clicks apply changes
                    NewFilePath.Text = newfilepath;  //vestigal, used in testting only, dont delete yet
                    URLTextBox.Text = newfilepath;
                }
                catch (Exception ex)
                {
                    UploadMsg.Text = "ERROR: " + ex.Message.ToString();
                }
            }
            else
            {
                UploadMsg.Text += "No File Chosen.";
            }
        }
    }
}
