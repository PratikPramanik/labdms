using System;
using System.Text;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using AspNet.StarterKits.Classifieds.Web;
using AspNet.StarterKits.Classifieds.BusinessLogicLayer;


public partial class PostAd_aspx : System.Web.UI.Page
{
    public int PreviousAdId
    {
        get
        {
            if (ViewState["PreviousAdId"] != null)
                return (int)ViewState["PreviousAdId"];
            else
                return 0;
        }
        set
        {
            ViewState["PreviousAdId"] = value;
        }
    }
    public bool IsPreviousAd
    {
        get
        {
            return (ViewState["PreviousAdId"] != null);
        }
    }

    private const string OtherLocationText = "Other...";

    protected void PostAdWizard_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {

        if (Page.IsValid)
        {
            int memberId = Profile.MemberId;
            int categoryId = CategoryPath.CurrentCategoryId;
            string title = Server.HtmlEncode(TitleTextBox.Text);
            string description = Server.HtmlEncode(DescriptionTextBox.Text);
            string code = Server.HtmlEncode(TextBoxCode.Text);
            string url; //= Server.UrlEncode(UrlTextBox.Text);
            Decimal price = 0;//Decimal.Parse(PriceTextBox.Text);
            string syn1 = Server.HtmlEncode(TextBoxSyn1.Text);
            string syn2 = Server.HtmlEncode(TextBoxSyn2.Text);
            string syn3 = Server.HtmlEncode(TextBoxSyn3.Text);
            string syn4 = Server.HtmlEncode(TextBoxSyn4.Text);
            string syn5 = Server.HtmlEncode(TextBoxSyn5.Text);

            string location = "";//Server.HtmlEncode(LocationDropDown.CurrentLocation);

            int numDays = 210000;//Convert.ToInt32(NumDaysList.SelectedValue);

            AdType adType = AdType.ForSale;

            //if (Enum.IsDefined(typeof(AdType), Convert.ToInt32(AdTypeSelection.SelectedValue)))
            //    adType = (AdType)Enum.Parse(typeof(AdType), AdTypeSelection.SelectedValue);

            url = UploadForm(sender, e);
            
            //if (!CheckFileValid())
            //{
            //    e.Cancel = true;
            //}
            if (IsPreviousAd && url != null)
            {
                
                AdsDB.RelistAd(PreviousAdId, 
                        categoryId,
                        title,
                        description,
                        url,
                        price,
                        location,
                        numDays,
                        AdLevel.Unspecified,
                        AdStatus.Unspecified,
                        adType,
                        code, syn1, syn2, syn3, syn4, syn5);

                Response.Redirect("~/MyAds.aspx", true);

            }
            else if (url != null)
            {
                int adId = AdsDB.InsertAd(memberId,
                        categoryId,
                        title,
                        description,
                        url,
                        price,
                        location,
                        numDays,
                        AdLevel.Unspecified,
                        AdStatus.Unspecified,
                        adType,
                        code, syn1, syn2, syn3, syn4, syn5);

                SiteSettings s = SiteSettings.GetSharedSettings();
                //UploadImagesLink.Visible = s.AllowImageUploads;
                //UploadImagesLink.NavigateUrl = "~/ManagePhotos.aspx?id=" + adId.ToString();

            }
            else
            {
                e.Cancel = true;
            }
        }
        else
            e.Cancel = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Limit Description text.
        DescriptionTextBox.Attributes.Add("onkeydown", "textCounter(this,500);");
        DescriptionTextBox.Attributes.Add("onkeyup", "textCounter(this,500);");

        if (!Page.IsPostBack)
        {
            
            PostAdWizard.MoveTo(PostAdWizard.WizardSteps[0]);

            string qsRelistId = Request.QueryString["relist"];
            if (qsRelistId != null)
            {
                int adId;
                if (Int32.TryParse(qsRelistId, out adId))
                {
                    LoadPreviousAd(adId);
                }
            }
        }
    }

    //adds Code, Name, Synonyms to database in addition to URL of uploaded file
    protected string UploadForm(object sender, EventArgs e)
    {
        //if upload field is empty
        if (IsPostBack)
        {
            if (FormUpload.HasFile == false)
            {
                UploadDetails.Text = "Please first select a file to upload...";
            }
            //otherwise proceed
            else
            {
                    // Save the file
                    string filePath =
                        Server.MapPath("~/Upload/Forms/"
                                        + FormUpload.FileName);
                    FormUpload.SaveAs(filePath);
                    /*END file UPLOAD file code*/
                    return "~/Upload/Forms/" + FormUpload.FileName;
                //}
                //else
                //{
                //    return null;
                //}
            }
        }
        return "no dice!";
    }

    protected bool CheckFileValid()
    {

        //[CHECK if empty]//
        if (FormUpload.FileName == "" || FormUpload.FileName == null)
        {
            return false;
        }
        else
        {
            //[check file extensions]//
            String fileExtension =
                System.IO.Path.GetExtension(FormUpload.FileName).ToLower();
            String[] allowedExtensions = 
                        {	".pdf", ".doc", ".DOC", 
                            ".rtf", ".RTF", ".PDF",
                            ".xls", ".XLS"};	//allowing the usuall suspects for now

            //parse the extension for a match, no match it will fall through at next conditional stmt.
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                { return true; }
            }
        }
        return false;
    }

    protected void LoadPreviousAd(int adId)
    {
        AdsDataComponent.AdsRow ad = AdsDB.GetAdById(adId);
        if (ad != null)
        {
            if (ad.MemberId == Profile.MemberId)
            {
                PreviousAdId = adId;

                SetCurrentCategory(ad.CategoryId);

                //if ((AdType)ad.AdType == AdType.Wanted)
                //    AdTypeSelection.SelectedIndex = 1;
                //else
                //    AdTypeSelection.SelectedIndex = 0;

                TitleTextBox.Text = ad.Title;
                DescriptionTextBox.Text = ad.Description;
                //UrlTextBox.Text = ad.URL;
                //LocationDropDown.CurrentLocation = ad.Location;
            }
        }
    }

    protected void UpdateCategoryDisplay()
    {
        this.CategoryPathLabel.Text = CategoryPath.FullCategoryPath;
    }

    protected void SubcategoriesDS_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        List<CachedCategory> subCategories = e.ReturnValue as List<CachedCategory>;
        if (subCategories == null || subCategories.Count == 0)
        {
            PostAdWizard.MoveTo(PostAdWizard.WizardSteps[1]);
        }
    }

    protected void SetCurrentCategory(int categoryId)
    {
        CategoryPath.CurrentCategoryId = categoryId;
        UpdateCategoryDisplay();
    }

    protected void SubcategoriesList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        int categoryId = Convert.ToInt32(e.CommandArgument);
        SetCurrentCategory(categoryId);
    }

    protected void CategoryPath_CategorySelectionChanged(object sender, CategorySelectionChangedEventArgs e)
    {
        UpdateCategoryDisplay();
    }

    protected void ChangeCategoryButton_Click(object sender, EventArgs e)
    {
        SetCurrentCategory(DefaultValues.CategoryIdMinValue);
        PostAdWizard.MoveTo(PostAdWizard.WizardSteps[0]);
    }
    protected void PostAdWizard_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
    {
        if (e.NextStepIndex == 0)
        {
            SetCurrentCategory(DefaultValues.CategoryIdMinValue);
        }
    }
    /*protected void ValidLocationRequired_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = !LocationDropDown.CurrentLocation.Equals(String.Empty);
    }*/
    /*protected void PriceValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        decimal p = -1;
        if (decimal.TryParse(PriceTextBox.Text, out p))
        {
            args.IsValid = (p >= 0);
        }
        else
            args.IsValid = false;
    }*/
    protected void URLValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = false;
        TextBox URLTextBox = AdDetailsStep.FindControl("UrlTextBox") as TextBox;
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
}
