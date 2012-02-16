<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="ShowAd.aspx.cs"
    Inherits="ShowAd_aspx" Title="Show Form" %>
<%@ Register TagPrefix="uc1" TagName="CategoryDropDown" Src="Controls/CategoryDropDown.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CategoryPath" Src="Controls/CategoryPath.ascx" %>
<%@ Import Namespace="AspNet.StarterKits.Classifieds.Web" %>
<asp:Content ID="SecondBarContent" ContentPlaceHolderID="SecondBar" Runat="server">
    <div id="crumbs_text">
        <uc1:CategoryPath ID="CategoryPath" Runat="server" OnCategorySelectionChanged="CategoryPath_CategorySelectionChanged" /></div>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="search_n_browse">
            <div id="search">
                <fieldset>
                    <legend>Search</legend>
                    <asp:TextBox ID="CommonSearchTextBox" Runat="server" CssClass="search_box" />
                    <uc1:CategoryDropDown ID="CommonCategoryDropDown" Runat="server" SelectOptionVisible="false" />
                    <asp:Button ID="SearchButton" Runat="server" Text="Search" PostBackUrl="~/Search.aspx" />
                    <asp:HyperLink ID="AdvancedSearchLink" Runat="server" NavigateUrl="~/AdvancedSearch.aspx">[go to advanced search]</asp:HyperLink>
                </fieldset>
            </div>
            <div id="browse">
                <fieldset>
                    <legend>Browse Related Forms</legend>
                    <ul>
                        <li>Forms in <strong>
                        <asp:HyperLink ID="CategoryListingsLink" Runat="server"></asp:HyperLink></strong></li><li>
                        <strong class="heading">
                        <asp:HyperLink ID="MemberListingsLink" Runat="server"></asp:HyperLink></strong></li></ul>
                </fieldset>
            </div>
        </div>
        <div id="content_header">
            <a id="content_start"></a>
            <h3>
                Actions for this Form</h3>
            <ul>
                <asp:PlaceHolder ID="AdNotActivePanel" Runat="server" Visible="false">
                    <li><strong>This Form is currently<br />
                    not active.</strong></li></asp:PlaceHolder>
            </ul>
        </div>
        <div id="content">
            <div class="display_left">
                <asp:DataList OnItemCommand="PhotoList_ItemCommand" ID="PhotoList" Runat="Server"
                    DataSourceID="PhotosDataSource" DataKeyField="Id" RepeatLayout="Flow">
                    <ItemTemplate>
                        <p>
                            <asp:LinkButton ID="AdPhotoImageButton" Runat="Server" CommandName="ShowFullSize"
                                CommandArgument='<%# Eval("Id") %>'><img alt="Enlarge Photo" src='<%# Eval("Id", "PhotoDisplay.ashx?photoid={0}&size=medium") %>' style="border:0;" /></asp:LinkButton></p>
                    </ItemTemplate>
                </asp:DataList>
            </div>
            <div class="display_right" id="ad_details">
                <asp:Panel ID="AdDetailsPanel" Runat="server">
                    <h4>
                        Title:&nbsp<asp:Label ID="AdTitleLabel" Runat="server"></asp:Label></h4>
                    <h5>
                        Code:&nbsp<asp:Label ID="AdCodeLabel" Runat="server"></asp:Label></h5>
                    <asp:Panel ID="PhotoPanel" Runat="server" Visible="False">
                    <p class="notice">
                        <asp:LinkButton ID="HidePhotoPanel" Runat="server" OnClick="HidePhotoPanel_Click">Hide again</asp:LinkButton></p>
                    <p>
                        <asp:ImageButton ID="FullSizePhoto" Runat="server" OnClick="FullSizePhoto_Click" AlternateText="Show other ad photos if they exist." /></p>
                </asp:Panel>
                <asp:Panel ID="ResponsePanel" Runat="server" Visible="False">
                    <h5 class="action">
                        Respond to Form:</h5>
                    Your Contact Name:<br />
                    <asp:TextBox ID="ResponseContactNameTextBox" Runat="server" CssClass="user_info"></asp:TextBox>
                    <p>
                        Your Contact Email (used for replies)<br />
                        <asp:TextBox ID="ResponseContactEmailTextBox" Runat="server" CssClass="user_info"></asp:TextBox>
                        <asp:RequiredFieldValidator
                             Runat="server" 
                             ControlToValidate="ResponseContactEmailTextBox" 
                             ErrorMessage="A contact email is required." 
                             ValidationGroup="RespondPanelGroup"
                             ToolTip="A contact email is required."
                             ID="ResponseEmailRequired1"
                             Display="dynamic">
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server"
                             ControlToValidate="ResponseContactEmailTextBox"
                             ValidationExpression=".*@.*\..*"
                             ErrorMessage="A valid email is required."
                             ToolTip="A valid email is required." 
                             ID="ResponseEmailRequired2"    
                             Display="dynamic">
                        </asp:RegularExpressionValidator>
                    </p>
                    <p>
                        Comments or Questions:<br />
                        <asp:TextBox ID="ResponseCommentsTextBox" Runat="server" TextMode="MultiLine" Width="452px"
                            Height="123px"></asp:TextBox></p>
                    <p>
                        <asp:Button ID="ResponseSubmitButton" Runat="server" Text="Submit" OnClick="ResponseSubmitButton_Click" />
                        <asp:Button ID="CancelResponseButton" Runat="server" Text="Cancel" OnClick="CancelResponseButton_Click" />
                    </p>
                </asp:Panel>
                <asp:Panel ID="EmailPanel" Runat="server" Visible="false">
                    <h5 class="action">
                        Send Form as Email:</h5>
                    <p>
                        Your Name:<br />
                        <asp:TextBox ID="EmailSenderNameTextBox" Runat="server" CssClass="user_info"></asp:TextBox>
                    </p>
                    <p>
                        Your Email: (reccomended, optional)<br />
                        <asp:TextBox ID="EmailSenderAddressTextBox" Runat="server" CssClass="user_info"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server"
                             ControlToValidate="EmailSenderAddressTextBox"
                             ValidationExpression=".*@.*\..*"
                             ErrorMessage="A valid email is required."
                             ToolTip="A valid email is required." 
                             ID="EmailSenderAddressTextBoxValidator2"    
                             Display="Dynamic">
                        </asp:RegularExpressionValidator>
                    </p>
                    <p>
                        Send to (Recipient Email):<br />
                        <asp:TextBox ID="EmailRecipientAddressTextBox" Runat="server" CssClass="user_info"></asp:TextBox>
                        <asp:RequiredFieldValidator
                             Runat="server" 
                             ControlToValidate="EmailRecipientAddressTextBox" 
                             ErrorMessage="A recipient email is required."
                             ValidationGroup="EmailPanelGroup"
                             ToolTip="A recipient email is required."
                             ID="EmailRecipientAddressTextBoxValidator1"
                             Display="Dynamic">
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server"
                             ControlToValidate="EmailRecipientAddressTextBox"
                             ValidationExpression=".*@.*\..*"
                             ErrorMessage="A valid email is required."
                             ToolTip="A valid email is required." 
                             ID="EmailRecipientAddressTextBoxValidator2"    
                             Display="Dynamic">
                        </asp:RegularExpressionValidator>
                    </p>
                    <p>
                        Subject:<br />
                        <asp:TextBox ID="EmailSubjectTextBox" Runat="server" Width="452px" CssClass="user_info"></asp:TextBox>
                    </p>
                    <p>
                        Message:<br />
                        <asp:TextBox ID="EmailMessageTextBox" Runat="server" TextMode="MultiLine" Width="452px"
                            Height="123px"></asp:TextBox></p>
                    <p>
                        &nbsp;<asp:Button ID="EmailSubmitButton" Runat="server" Text="Submit" OnClick="EmailSubmitButton_Click" />
                        <asp:Button ID="CancelEmailButton" Runat="server" Text="Cancel" OnClick="CancelEmailButton_Click" />
                    </p>
                </asp:Panel>
                <asp:Panel ID="AdSavedPanel" Runat="server" Visible="False" EnableViewState="False">
                    <p style="text-align:center;" class="notice">
                        <span>The Form has been <a href="MyAds.aspx?saved=1">saved to your list of Bookmarks.</a></span></p>
                </asp:Panel>
                <asp:Panel ID="EmailSentPanel" Runat="server" Visible="False" EnableViewState="False">
                    <p class="notice" style="text-align: center;">
                        <span>The email was sent successfully.</span></p>
                </asp:Panel>
                <asp:Panel ID="EmailNotSentPanel" Runat="server" Visible="False" EnableViewState="False">
                    <p class="notice" style="text-align: center; font-weight:bold;">
                        <span>The email was not sent. Contact the system administrator for further details.</span></p>
                </asp:Panel>
                <asp:Repeater ID="AdDetails" Runat="server" DataSourceID="AdsDataSource" OnItemDataBound="AdDetails_ItemDataBound" OnItemCommand="AdDetails_ItemCommand">
                    <ItemTemplate>
                        <table summary="This table is a list of item details: item category, item type, start date, etc.">
                            <caption class="none">
                                Item details
                            </caption>
                            <colgroup span="2">
                                <col class="col_heading" />
                                <col class="col_detail" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <td class="col_heading">
                                        Title:</td>
                                    <td class="col_detail">
                                        <asp:Label ID="CategoryNameLabel" Runat="server" Text='<%# Eval("Title") %>'></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="col_heading">
                                        Code:</td>
                                    <td class="col_detail">
                                        <asp:Label ID="CodeLabel" Runat="server" Text='<%# Eval("Code") %>'></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="col_heading">
                                        Posted by:</td>
                                    <td class="col_detail">
                                        <asp:HyperLink ID="PostByLink" Runat="server" NavigateUrl='<%# Eval("MemberName", "~/Search.aspx?member={0}") %>'
                                            Text='<%# Eval("MemberName") %>' /></td>
                                </tr>
                                <tr>
                                    <td class="col_heading">
                                        Date Entered:</td>
                                    <td class="col_detail">
                                        <asp:Label ID="DateAddedLabel" Runat="server" Text='<%# Eval("DateCreated", "{0:D}") %>'></asp:Label></td>
                                </tr>
                                                                <tr>
                                    <td class="col_heading">
                                        Synonyms:</td><br />
                                    <td class="col_detail">
                                        <asp:Label ID="LabelSyn1" Runat="server" ></asp:Label><br />
                                        <asp:Label ID="LabelSyn2" Runat="server" ></asp:Label><br />
                                        <asp:Label ID="LabelSyn3" Runat="server" ></asp:Label><br />
                                        <asp:Label ID="LabelSyn4" Runat="server" ></asp:Label><br />
                                        <asp:Label ID="LabelSyn5" Runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr class="ad_description_text">
                                    <td class="col_heading">
                                        Description:</td>
                                    <td class="col_detail">
                                        <p>
                                            <asp:Label ID="DescriptionLabel" Runat="server" Text='<%# Eval("Description") %>'></asp:Label></p>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col_heading">
                                        Full Form:</td>
                                    <td class="col_detail">
                                        <asp:HyperLink ID="URLLink" Runat="server" 
                                        Target="_blank" Text="Click Here to View Entire Form"/></td>
                                </tr>
                            </tbody>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>
                </asp:Panel>
            </div>
        </div>
    </div>
    <asp:ObjectDataSource ID="AdsDataSource" Runat="server" TypeName="AspNet.StarterKits.Classifieds.BusinessLogicLayer.AdsDB"
        SelectMethod="GetAdById" OnSelected="AdsDataSource_Selected">
        <SelectParameters>
            <asp:QueryStringParameter Name="adId" DefaultValue="0" QueryStringField="id" Type="Int32"></asp:QueryStringParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="PhotosDataSource" Runat="server" SelectMethod="GetPhotosByAdId"
        TypeName="AspNet.StarterKits.Classifieds.BusinessLogicLayer.PhotosDB">
        <SelectParameters>
            <asp:QueryStringParameter Name="adId" DefaultValue="0" QueryStringField="id" Type="Int32"></asp:QueryStringParameter>
        </SelectParameters>
    </asp:ObjectDataSource></asp:Content>
