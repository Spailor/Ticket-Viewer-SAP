<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Header" %>
<%@ Register Src="~/UserControls/Common/HeaderMenu.ascx" TagPrefix="uc" TagName="HeaderMenu" %>
<div style="float: left;">
    <a href="<%= ResolveClientUrl("~/") %>" class="top-logo"></a>
</div>
<div style="float: right;">

    <uc:HeaderMenu runat="server" ID="HeaderMenu" />

<%--    <asp:LoginView ID="HeadLoginView" runat="server" EnableViewState="false">
        <LoggedInTemplate>
            <asp:LoginStatus ID="HeadLoginStatus" runat="server" CssClass="dx-vam"
                LogoutAction="Redirect" LogoutText="Çıkış Yap" OnLoggedOut="HeadLoginStatus_LoggedOut"
                LogoutPageUrl="~/" />
        </LoggedInTemplate>
    </asp:LoginView>--%>

</div>
<div class="clear"></div>
