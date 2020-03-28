<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBase.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Account_Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitlePartPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HelpMenuDescriptionPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
            <div class="accountHeader">
    <h2>
        Create a New Account</h2>
    <p>Use the form below to create a new account.</p>
    <p>Passwords are required to be a minimum of <%= Membership.MinRequiredPasswordLength %> characters in length.</p>
</div>
    <dx:ASPxLabel ID="lblUserName" runat="server" AssociatedControlID="tbUserName" Text="User Name:" />
    <div class="form-field">
        <dx:ASPxTextBox ID="tbUserName" runat="server" Width="200px">
            <ValidationSettings ValidationGroup="RegisterUserValidationGroup">
                <RequiredField ErrorText="User Name is required." IsRequired="true" />
            </ValidationSettings>
        </dx:ASPxTextBox>
    </div>
    <dx:ASPxLabel ID="lblEmail" runat="server" AssociatedControlID="tbEmail" Text="E-mail:" />
    <div class="form-field">
        <dx:ASPxTextBox ID="tbEmail" runat="server" Width="200px">
            <ValidationSettings ValidationGroup="RegisterUserValidationGroup">
                <RequiredField ErrorText="E-mail is required." IsRequired="true" />
                <RegularExpression ErrorText="Email validation failed" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
            </ValidationSettings>
        </dx:ASPxTextBox>
    </div>
    <dx:ASPxLabel ID="lblPassword" runat="server" AssociatedControlID="tbPassword" Text="Password:" />
    <div class="form-field">
        <dx:ASPxTextBox ID="tbPassword" ClientInstanceName="Password" Password="true" runat="server"
            Width="200px">
            <ValidationSettings ValidationGroup="RegisterUserValidationGroup">
                <RequiredField ErrorText="Password is required." IsRequired="true" />
            </ValidationSettings>
        </dx:ASPxTextBox>
    </div>
    <dx:ASPxLabel ID="lblConfirmPassword" runat="server" AssociatedControlID="tbConfirmPassword"
        Text="Confirm password:" />
    <div class="form-field">
        <dx:ASPxTextBox ID="tbConfirmPassword" Password="true" runat="server" Width="200px">
            <ValidationSettings ValidationGroup="RegisterUserValidationGroup">
                <RequiredField ErrorText="Confirm Password is required." IsRequired="true" />
            </ValidationSettings>
            <ClientSideEvents Validation="function(s, e) {
                var originalPasswd = Password.GetText();
                var currentPasswd = s.GetText();
                e.isValid = (originalPasswd  == currentPasswd );
                e.errorText = 'The Password and Confirmation Password must match.';
            }" />
        </dx:ASPxTextBox>
    </div>
    <br />
    <dx:ASPxButton ID="btnCreateUser" runat="server" Text="Create User" ValidationGroup="RegisterUserValidationGroup"
        OnClick="btnCreateUser_Click">
    </dx:ASPxButton>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContentPlaceHolder" Runat="Server">

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterRangeControlPlaceHolder" Runat="Server">
</asp:Content>

