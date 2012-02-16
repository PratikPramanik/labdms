<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="Login.aspx.cs" 
    Inherits="Login_aspx" Title="Login" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="col_main_left">
            <div id="user_assistance">
                <a id="content_start"></a>
                <h3>
                    Login Page</h3>
                <p>
                    Please Login. If you require additional access beyond that of a regular account,
                    please contact the webmaster.</p>
            </div>
        </div>
        <div id="col_main_right">
            <h2 class="section">
                Login</h2>
            <div class="content_right">
                <asp:Panel ID="AccessNoticePanel" Runat="server" EnableViewState="False" Visible="False">
                    <b>You have accessed a page or feature that requires login information.</b><br />
                    Use the form below to log in using your account information.<br />
                    <br />
                </asp:Panel>
                <asp:Login ID="LoginConrol" Runat="server" TitleText="" CssClass="login_box">
                <TextBoxStyle CssClass="text"></TextBoxStyle>                
                </asp:Login>
                <p>
                </p>
                <p>
                    <asp:LinkButton ID="ForgotPasswordButton" Runat="server" OnClick="ForgotPasswordButton_Click">Forgot Password?</asp:LinkButton>
                </p>
                <asp:PasswordRecovery ID="PasswordRecovery" Runat="server" Visible="False"
                     UserNameTitleText="" 
                     QuestionTitleText="Step 2: Identity Confirmation." 
                     UserNameInstructionText="Step 1: Enter your User Name." Width="280px" OnInit="PasswordRecovery_Init" OnSendMailError="PasswordRecovery_SendMailError">
                    <TitleTextStyle Font-Bold="True"></TitleTextStyle>
                    <InstructionTextStyle Font-Bold="True"></InstructionTextStyle>
                    <LabelStyle Wrap="False" />
                </asp:PasswordRecovery>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
            </div>
        </div>
    </div>
</asp:Content>
