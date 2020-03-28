<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductsOnayBekleyen.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Products" %>

<%--<%@ Register Src="~/UserControls/ProductDetails.ascx" TagPrefix="uc1" TagName="ProductDetails" %>
<%@ Register Src="~/UserControls/CagriIstekDetaylari.ascx" TagPrefix="uc2" TagName="CagriIstekDetaylari" %>--%>

<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Çağrı İstekleri</asp:Content>

<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">

    <script type="text/javascript">
        function OnGridFocusedRowChanged() {
            Aciklama.SetText("Loading...");
            ProductsGridView.GetRowValues(ProductsGridView.GetFocusedRowIndex(), 'ID;IstekTarihiSaati;OnemDerecesiAdi;IstekSahibiAdiSoyadi;IstekSirketAdi;IstekAciklama;IstekSirketKodu;IstekSahibiMailAdresi;ExternalTicketNumber;AlternativeTicketNumber;Moduller;DosyaEkleri;SonucAciklama', OnGetRowValues);
        }
        function OnGetRowValues(values) {
            //var evrakadi = callbackEvrak.PerformCallback(values[0]);
            //ASPxLabel1.SetText(evrakadi);

            if (values[6] == "")
                SirketKodu.SetText("--");
            else
                SirketKodu.SetText(values[6]);

            SirketAdi.SetText(values[4]);
            AdiSoyadi.SetText(values[3]);

            if (values[7] == "")
                EMailAdresi.SetText("--");
            else
                EMailAdresi.SetText(values[7]);

            if (values[8] == "")
                TicketNumber.SetText("--");
            else
                TicketNumber.SetText(values[8]);

            if (values[9] == "")
                AltTicketNumber.SetText("--");
            else
                AltTicketNumber.SetText(values[9]);

            var d = new Date(values[1]);
            var dd = d.getDate();
            var mm = d.getMonth() + 1; //January is 0!
            var yyyy = d.getFullYear();
            if (dd < 10) {
                dd = '0' + dd
            }

            if (mm < 10) {
                mm = '0' + mm
            }
            today = dd + '-' + mm + '-' + yyyy;
            Tarih.SetText(today);

            OnemDerecesi.SetText(values[2]);
            Modul.SetText(values[10]);
            Aciklama.SetText(values[5]);
            CozumAciklama.SetText(values[12]);
           
            document.getElementById('dosya1').href = ''; 
            document.getElementById('dosya1').text = '';
            document.getElementById('dosya2').href = '';
            document.getElementById('dosya2').text = '';
            document.getElementById('dosya3').href = '';
            document.getElementById('dosya3').text = '';
            document.getElementById('dosya4').href = '';
            document.getElementById('dosya4').text = '';
            document.getElementById('dosya5').href = '';
            document.getElementById('dosya5').text = '';
            if (values[11] != "")
            {
                var res = values[11].split(",");
                for (var i = 0; i < res.length; i++) {
                    var arti = i + 1;
                    var dosya = 'dosya' + arti.toString();
                    document.getElementById(dosya).href = 'UploadControl\\' + res[i];
                    document.getElementById(dosya).text = res[i];
                }
            }
            
            
        }
        function OnCustomButtonClick(s, e) {
            if (e.buttonID == 'btnOnayla') {
                s.GetRowValues(e.visibleIndex, "ID;IstekSahibiAdiSoyadi", OnGetRowValues);
            }
            function OnGetRowValues(Value) {
                callbackOnayla.PerformCallback(Value[0]);
                PopupOnayla.Show();
            }

            if (e.buttonID == 'btnReddet') {
                s.GetRowValues(e.visibleIndex, "ID;IstekSahibiAdiSoyadi", OnGetRowValuesReddet);
            }
            function OnGetRowValuesReddet(Value) {
                callbackReddet.PerformCallback(Value[0]);
                PopupReddet.Show();
            }
        }
        function OnEkDosyaListesi() {
            ProductsGridView.GetRowValues(ProductsGridView.GetFocusedRowIndex(), 'ID;IstekSahibiAdiSoyadi', OnGetRowIDValues);
        }
        function OnGetRowIDValues(ValueID) {
            var id = callbackEkDosyalar.PerformCallback(ValueID[0]);
            ASPxLabel2.SetText = id;
            ASPxPopupControl1.Show();
        }
        
    </script>
    <dx:ASPxCallback ID="callbackOnayla"
        ClientInstanceName="callbackOnayla"
        OnCallback="callbackOnayla_Callback" runat="server">
    </dx:ASPxCallback>
    <dx:ASPxCallback ID="callbackReddet"
        ClientInstanceName="callbackReddet"
        OnCallback="callbackReddet_Callback" runat="server">
    </dx:ASPxCallback>
<%--    <dx:ASPxCallback ID="callbackEvrak"
        ClientInstanceName="callbackEvrak"
        OnCallback="callbackEvrak_Callback" runat="server">
    </dx:ASPxCallback>--%>
    <dx:ASPxPopupControl ID="PopupOnayla" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopupOnayla"
    HeaderText="Onayla" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" Theme="Moderno">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPanel ID="ASPxPanel4" runat="server" DefaultButton="btOK">
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <table style="width:300px;" >
                                <tr>
                                    <td style="text-align:center; padding:20px 0 20px 0;" >
                                        Seçmiş olduğunuz çağrı'yı onaylamak istediğinizden eminmisiniz?
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:center" >
                                        <dx:ASPxButton ID="ASPxButton7" runat="server" Text="Evet" Width="80px"
                                            AutoPostBack="False" >
                                            <ClientSideEvents Click="function(s, e) { PopupOnayla.Hide(); callbackPopupEvet.PerformCallback(); ProductsGridView.Refresh(); }" />
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="ASPxButton8" runat="server" Text="Vazgeç" Width="80px" AutoPostBack="False" >
                                            <ClientSideEvents Click="function(s, e) { PopupOnayla.Hide(); }" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
    </dx:ASPxPopupControl>
    <dx:ASPxCallback ID="callbackPopupEvet"
        ClientInstanceName="callbackPopupEvet"
        OnCallback="callbackPopupEvet_Callback" runat="server">
    </dx:ASPxCallback>
    <dx:ASPxPopupControl ID="PopupReddet" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopupReddet"
    HeaderText="Reddet" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" Theme="Moderno">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPanel ID="ASPxPanel1" runat="server" DefaultButton="btOK">
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <table style="width:300px;" >
                                <tr>
                                    <td style="text-align:center; padding:20px 0 20px 0;" >
                                        Seçmiş olduğunuz çağrı'yı reddetmek istediğinizden eminmisiniz?
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:center; padding:20px 0 20px 0;" >
                                        <span>Reddetme nedeninizi belirtin:</span><br />
                                        <dx:ASPxMemo ID="TxtMemo" runat="server" Width="100%" Height="100" MaxLength="250"></dx:ASPxMemo>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:center" >
                                        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Evet" Width="80px"
                                            AutoPostBack="False" >
                                            <ClientSideEvents Click="function(s, e) { PopupReddet.Hide(); callbackPopupReddetEvet.PerformCallback(); ProductsGridView.Refresh(); }" />
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Vazgeç" Width="80px" AutoPostBack="False" >
                                            <ClientSideEvents Click="function(s, e) { PopupReddet.Hide(); }" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
    </dx:ASPxPopupControl>
    <dx:ASPxCallback ID="callbackPopupReddetEvet"
        ClientInstanceName="callbackPopupReddetEvet"
        OnCallback="callbackPopupReddetEvet_Callback" runat="server">
    </dx:ASPxCallback>
    <h1>Test Onayı Bekleyen Çağrılar</h1>
    <dx:ASPxGridView ID="ProductsGridView" runat="server" CssClass="gridView"
        AutoGenerateColumns="False" ClientInstanceName="ProductsGridView"
        KeyFieldName="ID" Width="100%" KeyboardSupport="true"
        OnCustomColumnDisplayText="ProductsGridView_CustomColumnDisplayText"
        OnHtmlDataCellPrepared="ProductsGridView_HtmlDataCellPrepared">
        <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused"
            RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow" />
        <Columns>
            <dx:GridViewCommandColumn VisibleIndex="0" Width="70px" MinWidth="60" ButtonType="Image" Caption=" ">
                <CellStyle VerticalAlign="Middle" />
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnOnayla">
                        <Styles>
                            <Style CssClass="InlineButton"></Style>
                        </Styles>
                        <Image ToolTip="Onayla" Url="Content/Images/Apply_24x24.png" Height="24px" Width="24px" />
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="btnReddet">
                        <Styles>
                            <Style CssClass="InlineButton"></Style>
                        </Styles>
                        <Image ToolTip="Reddet" Url="Content/Images/Close_24x24-2.jpg" Height="24px" Width="24px" />
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="InternalTicketNumber" Caption="Ticket ID" Width="10%" CellStyle-HorizontalAlign="Left" VisibleIndex="0" />
            <dx:GridViewDataTextColumn FieldName="ID" Caption="İstek ID" Width="10%" CellStyle-HorizontalAlign="Left" VisibleIndex="0" />
            <dx:GridViewDataTextColumn FieldName="IstekTarihiSaati" Caption="İstek Tarihi" Width="10%" VisibleIndex="1">
                <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="OnemDerecesiAdi" Caption="Önem Derecesi" Width="10%" VisibleIndex="2" Visible="false" />
            <dx:GridViewDataTextColumn FieldName="IstekSahibiAdiSoyadi" Caption="Adı Soyadı" Width="15%" VisibleIndex="3" />
            <dx:GridViewDataTextColumn FieldName="IstekSirketAdi" Caption="Şirket Adı" Width="20%" VisibleIndex="4" />
            <dx:GridViewDataTextColumn FieldName="IstekAciklama" Caption="Açıklama" Width="30%" VisibleIndex="5" />

            <dx:GridViewDataTextColumn FieldName="IstekSirketKodu" Visible="false" VisibleIndex="6" />
            <dx:GridViewDataTextColumn FieldName="IstekSahibiMailAdresi" Visible="false" VisibleIndex="7" />
            <dx:GridViewDataTextColumn FieldName="ExternalTicketNumber" Visible="false" VisibleIndex="8" />
            <dx:GridViewDataTextColumn FieldName="AlternativeTicketNumber" Visible="false" VisibleIndex="9" />
            <dx:GridViewDataTextColumn FieldName="Moduller" Visible="false" VisibleIndex="10" />
            <dx:GridViewDataTextColumn FieldName="DosyaEkleri" Visible="false" VisibleIndex="11" />

            <dx:GridViewDataTextColumn FieldName="SonucAciklama" Caption="Çözüm Açıklaması" Width="35%" VisibleIndex="12" />


        </Columns>
        <SettingsBehavior AllowFocusedRow="True" />
        <SettingsPager PageSize="6">
            <NextPageButton Visible="true">
            </NextPageButton>
            <PrevPageButton Visible="true">
            </PrevPageButton>
            <Summary Visible="true" />
            <PageSizeItemSettings Visible="true">
            </PageSizeItemSettings>
        </SettingsPager>
        <Settings ShowGroupPanel="False" GridLines="None" ShowFilterRow="true" ShowFilterRowMenu="true" />
        <ClientSideEvents FocusedRowChanged="function(s, e) { OnGridFocusedRowChanged(); }" CustomButtonClick="OnCustomButtonClick" />
    </dx:ASPxGridView>
</asp:Content>
<asp:Content ID="BottomContent" runat="server" ContentPlaceHolderID="BottomContentPlaceHolder">
  <%--  <dx:ASPxCallback ID="callbackEkDosyalar"
        ClientInstanceName="callbackEkDosyalar"
        OnCallback="callbackEkDosyalar_Callback" runat="server">
    </dx:ASPxCallback>--%>
    <%--<uc1:ProductDetails runat="server" ID="ProductDetails" />--%>
    <%--<uc2:CagriIstekDetaylari runat="server" ID="CagriIstekDetaylari" />--%>
    <div class="productDetailsHolder">
        <div class="productDetails">
            <h5>Şirket Bilgileri</h5>
            <p class="description">
                <strong>Şirket Kodu </strong>
                <dx:ASPxLabel ID="SirketKodu" ClientInstanceName="SirketKodu" runat="server" CssClass="descriptionLabel">
                </dx:ASPxLabel>
                <strong>Şirket Adı </strong>
                <dx:ASPxLabel ID="SirketAdi" ClientInstanceName="SirketAdi" runat="server" CssClass="descriptionLabel">
                </dx:ASPxLabel>
                <strong>Adı, Soyadı </strong>
                <dx:ASPxLabel ID="AdiSoyadi" ClientInstanceName="AdiSoyadi" runat="server" CssClass="descriptionLabel">
                </dx:ASPxLabel>
                <strong>E-Mail Adresi </strong>
                <dx:ASPxLabel ID="EMailAdresi" ClientInstanceName="EMailAdresi" runat="server" CssClass="descriptionLabel">
                </dx:ASPxLabel>
            </p>
            <h5>Entegrasyon Bilgileri</h5>
            <p class="description" style="margin-bottom: 0px;">
                <strong>Ticket Number </strong>
                <dx:ASPxLabel ID="TicketNumber" ClientInstanceName="TicketNumber" runat="server" CssClass="descriptionLabel">
                </dx:ASPxLabel>
                <strong>Alternative Ticket Number </strong>
                <dx:ASPxLabel ID="AltTicketNumber" ClientInstanceName="AltTicketNumber" runat="server" CssClass="descriptionLabel">
                </dx:ASPxLabel>
            </p>
        </div>
        <div class="productDetailsChartsHolder2">
            <h5>Çağrı Bilgileri</h5>
            <p class="description2" style="margin-bottom: 0px;">
                <strong>Tarih </strong>
                <dx:ASPxLabel ID="Tarih" ClientInstanceName="Tarih" runat="server" CssClass="descriptionLabel2">
                </dx:ASPxLabel>
                <strong>Önem Derecesi </strong>
                <dx:ASPxLabel ID="OnemDerecesi" ClientInstanceName="OnemDerecesi" runat="server" CssClass="descriptionLabel2">
                </dx:ASPxLabel>
                <strong>Modül </strong>
                <dx:ASPxLabel ID="Modul" ClientInstanceName="Modul" runat="server" CssClass="descriptionLabel2">
                </dx:ASPxLabel>
                <strong>Açıklama </strong>
                <dx:ASPxLabel ID="Aciklama" ClientInstanceName="Aciklama" runat="server" CssClass="descriptionLabel2">
                </dx:ASPxLabel>
                <strong>Çözüm Açıklaması </strong>
                <dx:ASPxLabel ID="CozumAciklama" ClientInstanceName="CozumAciklama" runat="server" CssClass="descriptionLabel2">
                </dx:ASPxLabel>
            </p>
        
            <br /><br />
            <h5>Dosya Ekleri</h5>
            <p class="description2" style="margin-bottom: 0px;">
                <dx:ASPxLabel ID="ASPxLabel1" ClientInstanceName="ASPxLabel1" runat="server" CssClass="descriptionLabel2">
                </dx:ASPxLabel>
                <a id="dosya1" href="" target="_blank"></a> <br />
                <a id="dosya2" href="" target="_blank"></a> <br />
                <a id="dosya3" href="" target="_blank"></a> <br />
                <a id="dosya4" href="" target="_blank"></a> <br />
                <a id="dosya5" href="" target="_blank"></a>
             <%--   <br />
                <dx:ASPxButton ID="OkButton" AutoPostBack="false" runat="server" Text="Ek'deki Dosyalar" CssClass="button">
                        <ClientSideEvents Click="OnEkDosyaListesi" />
                </dx:ASPxButton>--%>
            </p>
        </div>
        <div class="clear"></div>
    </div>

<%--    <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="ASPxPopupControl1"
    HeaderText="Ek'deki Dosyalar" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" Theme="Moderno">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPanel ID="ASPxPanel2" runat="server" DefaultButton="btOK">
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <table style="width:300px;" >
                                <tr>
                                    <td style="text-align:center; padding:20px 0 20px 0;" >
                                        <dx:ASPxLabel ID="ASPxLabel2" ClientInstanceName="ASPxLabel2" runat="server" CssClass="descriptionLabel2">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:center" >
                                        <dx:ASPxButton ID="ASPxButton4" runat="server" Text="Kapat" Width="80px" AutoPostBack="False" >
                                            <ClientSideEvents Click="function(s, e) { ASPxPopupControl1.Hide(); }" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
    </dx:ASPxPopupControl>--%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="FooterRangeControlPlaceHolder" runat="Server">
    <%--    <div class="contentBox salesDateRangeContainer">
        <uc:FooterRangeControl runat="server" ID="FooterRangeControl" />
    </div>--%>
</asp:Content>
<asp:Content ID="HelpMenu" ContentPlaceHolderID="HelpMenuDescriptionPlaceHolder" runat="server">
    <p><a target="_blank" href="https://documentation.devexpress.com/#AspNet/CustomDocument5823">Grid View</a> - used to display products stored in the database. Sort values by clicking individual column headers.</p>
    <p><a target="_blank" href="https://documentation.devexpress.com/#AspNet/clsDevExpressXtraChartsWebWebChartControltopic">Pie Charts</a> - used to communicate the state of revenues by sector, region and sales channel.</p>
    <p><a target="_blank" href="https://documentation.devexpress.com/#AspNet/clsDevExpressWebASPxEditorsASPxTrackBartopic">Track Bar</a> – used to visually specify a date range for sales information displayed within the charts.</p>
</asp:Content>
