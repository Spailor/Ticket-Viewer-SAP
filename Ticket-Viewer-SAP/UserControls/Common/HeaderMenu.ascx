﻿<%@ Control Language="C#" AutoEventWireup="true" 
    CodeFile="HeaderMenu.ascx.cs" Inherits="HeaderMenu" %>

<div class="mainMenu">

    <dx:ASPxMenu ID="mainMenu" runat="server" Theme="Moderno" 
        DataSourceID="siteMapDataSource" SelectParentItem="true">
        <RootItemSubMenuOffset FirstItemY="1" />
        <ClientSideEvents ItemClick="OnMenuItemClick" />
    </dx:ASPxMenu>

</div>

<dx:ASPxSiteMapDataSource ID="siteMapDataSource" runat="server" 
    >
</dx:ASPxSiteMapDataSource>
