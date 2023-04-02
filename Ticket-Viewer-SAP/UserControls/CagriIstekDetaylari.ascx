<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CagriIstekDetaylari.ascx.cs" Inherits="UserControls_CagriIstekDetaylari" %>
<dx:ASPxCallbackPanel ID="ProductDetailsCallbackPanel" 
        EnableCallbackAnimation="true" runat="server">
        <PanelCollection>
            <dx:PanelContent>
                <div class="productDetailsHolder">
                    <div class="productDetails">
                        <h5>Şirket Bilgileri</h5>
                        <p class="description">
                            <strong>Şirket Kodu </strong> <dx:ASPxLabel ID="SirketKodu" runat="server" CssClass="descriptionLabel">
                            </dx:ASPxLabel>
                            <strong>Şirket Adı </strong> <dx:ASPxLabel ID="SirketAdi" runat="server" CssClass="descriptionLabel">
                            </dx:ASPxLabel>
                            <strong>Adı, Soyadı </strong> <dx:ASPxLabel ID="AdiSoyadi" runat="server" CssClass="descriptionLabel">
                            </dx:ASPxLabel>
                            <strong>E-Mail Adresi </strong> <dx:ASPxLabel ID="EMailAdresi" runat="server" CssClass="descriptionLabel">
                            </dx:ASPxLabel>
                        </p>
                        <h5>Entegrasyon Bilgileri</h5>
                        <p class="description" style="margin-bottom: 0px;">
                            <strong>Ticket Number </strong> <dx:ASPxLabel ID="TicketNumber" runat="server" CssClass="descriptionLabel">
                            </dx:ASPxLabel>
                            <strong>Alternative Ticket Number </strong> <dx:ASPxLabel ID="AltTicketNumber" runat="server" CssClass="descriptionLabel">
                            </dx:ASPxLabel>
                        </p>
                    </div>
                    <div class="productDetailsChartsHolder2">
                        <h5>Çağrı Bilgileri</h5>
                        <p class="description2" style="margin-bottom: 0px;">
                            <strong>Tarih </strong> <dx:ASPxLabel ID="Tarih" runat="server" CssClass="descriptionLabel2">
                            </dx:ASPxLabel>
                            <strong>Önem Derecesi </strong> <dx:ASPxLabel ID="OnemDerecesi" runat="server" CssClass="descriptionLabel2">
                            </dx:ASPxLabel>
                            <strong>Modül </strong> <dx:ASPxLabel ID="Modul" runat="server" CssClass="descriptionLabel2">
                            </dx:ASPxLabel>
                            <strong>Açıklama </strong> <dx:ASPxLabel ID="Aciklama" runat="server" CssClass="descriptionLabel2">
                            </dx:ASPxLabel>
                        </p>
                    </div>
                    <div class="clear"></div>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>