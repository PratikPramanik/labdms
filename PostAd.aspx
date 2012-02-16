<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="PostAd.aspx.cs"
    Inherits="PostAd_aspx" Title="Upload Form" %>
<%@ Register TagPrefix="uc1" TagName="LocationDropDown" Src="Controls/LocationDropDown.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CategoryPath" Src="Controls/CategoryPath.ascx" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="col_main_left">
            <div id="user_assistance">
                <a id="content_start"></a>
                <h3>
                    Step 1: Select Category</h3>
                <p>
                    Select the Category your Form type is in.</p>
                <h3>
                    Step 2: Give Form Information and Upload</h3>
                <p>
                    Fill out the necessary information for the form. 
                    Upload the form. If you have not
                    filled in all the necesary fields you will be notified. If uploading a TIS, fill
                    out each synonym separately (any extras can be added at a later date) and in "Description"
                    put in "Sample" or "Purpose" information. Make sure to copy and paste line-by-line to ensure
                    all data is entered.</p>
                <h3>
                    Step 3: Get Administrator to Activate the Form</h3>
                <p>
                    Forms must be activated by an administrator in order 
                    for the form to be viewable.</p>
            </div>
        </div>
        <div id="col_main_right">
            <asp:Wizard ID="PostAdWizard" Runat="server" OnFinishButtonClick="PostAdWizard_FinishButtonClick"
                DisplaySideBar="False" CssClass="wizard" StepStyle-CssClass="wizard-step" ActiveStepIndex="0"
                OnPreviousButtonClick="PostAdWizard_PreviousButtonClick" NavigationStyle-HorizontalAlign="Left" >
                <WizardSteps>
                    <asp:WizardStep ID="WizardStep1" Runat="server" Title="Category Selection">
                        <h2 class="section">
                            Upload Form: Category Selection</h2>
                        <div class="content_right">
                            <fieldset>
                                <legend class="select_category">Please select a Category for the Form:</legend>
                                <p>
                                    <uc1:CategoryPath ID="CategoryPath" Runat="server" OnCategorySelectionChanged="CategoryPath_CategorySelectionChanged" />
                                </p>
                                <div>
                                    <asp:DataList Runat="server" ID="SubcategoriesList" DataSourceID="SubcategoriesDS"
                                        OnItemCommand="SubcategoriesList_ItemCommand" CellSpacing="10" RepeatColumns="2">
                                        <ItemTemplate>
                                            <asp:LinkButton Runat="Server" ID="CategoryButton" CommandArgument='<%# Eval("Id") %>'
                                                Text='<%# Eval("Name") %>' Font-Size="12px" Font-Bold="True" />
                                        </ItemTemplate>
                                    </asp:DataList>
                                    <asp:ObjectDataSource ID="SubcategoriesDS" Runat="server" TypeName="AspNet.StarterKits.Classifieds.Web.CategoryCache"
                                        SelectMethod="GetCategoriesByParentId" OnSelected="SubcategoriesDS_Selected">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="CategoryPath" PropertyName="CurrentCategoryId"
                                                Type="Int32" DefaultValue="0" Name="parentCategoryId" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </div>
                            </fieldset>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="AdDetailsStep" Runat="server" Title="Enter Form Details">
                        <h2 class="section">
                            Post an Form: Details</h2>
                        <div class="content_right">
                            <fieldset>
                                <legend>
                                    Your Selected Category:</legend>
                                    <asp:Label Runat="server" ID="CategoryPathLabel"></asp:Label>
                                    |
                                    <asp:LinkButton Runat="server" ID="ChangeCategoryButton" OnClick="ChangeCategoryButton_Click"
                                        ValidationGroup="ChangeCategory">Change</asp:LinkButton>
                                <p>
                                    <asp:ValidationSummary Runat="server" ID="ValidationSummary1" HeaderText="Please correct the following:" />
                                </p>
                                <p></p>
                                
                                
                            <legend>Title: <span class="small_text">(100 characters max)</span></legend><span>
                            <asp:TextBox Text='<%# Bind("Title") %>' Runat="server" ID="TitleTextBox" CssClass="post_title" MaxLength="100"></asp:TextBox></span>
                            <asp:RequiredFieldValidator Runat="server" ErrorMessage="A Title for the form is required."
                                ID="RequiredTitle" ControlToValidate="TitleTextBox">
                                *</asp:RequiredFieldValidator>
                            <p>
                            </p>
                            
                            <legend>Code/Acronym: <span class="small_text">(20 characters max)</span></legend>
                            <span><asp:TextBox Text='<%# Bind("Code") %>' Runat="server" ID="TextBoxCode" 
                            CssClass="post_title" MaxLength="20"></asp:TextBox></span>
                            <asp:RequiredFieldValidator Runat="server" ErrorMessage="A Code for the form is required."
                                ID="RequiredCode" ControlToValidate="TextBoxCode">
                                *</asp:RequiredFieldValidator>
                            <p></p>
                            <legend>Synonyms/Subtitles: <span class="small_text">(50 characters max each, 5 Synonyms max)</span></legend>
                            <span><asp:TextBox Text='<%# Bind("Syn1") %>' Runat="server" ID="TextBoxSyn1" 
                            CssClass="post_title" MaxLength="50"></asp:TextBox></span><br />
                            <span><asp:TextBox Text='<%# Bind("Syn2") %>' Runat="server" ID="TextBoxSyn2" 
                            CssClass="post_title" MaxLength="50"></asp:TextBox></span><br />
                            <span><asp:TextBox Text='<%# Bind("Syn3") %>' Runat="server" ID="TextBoxSyn3" 
                            CssClass="post_title" MaxLength="50"></asp:TextBox></span><br />
                            <span><asp:TextBox Text='<%# Bind("Syn4") %>' Runat="server" ID="TextBoxSyn4" 
                            CssClass="post_title" MaxLength="50"></asp:TextBox></span><br />
                            <span><asp:TextBox Text='<%# Bind("Syn5") %>' Runat="server" ID="TextBoxSyn5" 
                            CssClass="post_title" MaxLength="50"></asp:TextBox></span><br />
                            
                            <legend>Description: <span class="small_text">(500 characters max)</span></legend><span>
                            <asp:TextBox Text='<%# Bind("Description") %>' Runat="server" ID="DescriptionTextBox"
                                TextMode="MultiLine" Columns="80" Rows="16" CssClass="post_description"></asp:TextBox>
                            </span>
                            <asp:RequiredFieldValidator Runat="server" ErrorMessage="A Description is required."
                                ID="RequiredDescription" ControlToValidate="DescriptionTextBox">
                                *</asp:RequiredFieldValidator>
                                
                                   
                            <legend>Pick a file to upload:</legend>
                            <asp:FileUpload ID="FormUpload" runat="server"/>
                            <asp:RequiredFieldValidator Runat="server" ErrorMessage="An Upload is required."
                                ID="UploadValidator" ControlToValidate="FormUpload">
                                *</asp:RequiredFieldValidator>
                            <asp:Label ID="UploadDetails" runat="server"></asp:Label>
                            <p></p><br />
                            
                            </fieldset>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep2" Runat="server" StepType="Complete" Title="Done">
                        <h2 class="section">
                            Done!</h2>
                        <div class="content_right">
                            <p>
                                Your Form was successfully submitted.&nbsp;</p> 
                                <asp:Label ID="Label1" runat="server"></asp:Label><br />
                            <p style="text-align: center">
                                <br />
                                    <asp:HyperLink Runat="server" ID="MyAdsLink" 
                                    Font-Bold="True" NavigateUrl="~/MyAds.aspx">Go to My Forms page</asp:HyperLink>
                            </p>
                        </div>
                    </asp:WizardStep>
                </WizardSteps>
                <StepStyle CssClass="wizard-step" />
                <NavigationStyle HorizontalAlign="Left" />
            </asp:Wizard>
        </div>
    </div>
<script type="text/javascript">
function textCounter(elem, maxLimit) 
{
    if (elem.value.length > maxLimit)
    {
       elem.value = elem.value.substring(0, maxLimit);
    }
} 

</script>    
</asp:Content>
