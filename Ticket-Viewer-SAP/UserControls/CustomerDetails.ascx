<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CustomerDetails.ascx.cs" Inherits="CustomerDetails" %>
<%@ Register Src="~/UserControls/HorizontalBarChart.ascx" TagPrefix="uc" TagName="HorizontalBarChart" %>
<script charset="UTF-8" type="text/javascript" src="//ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0&s=1">
</script>
<dx:ASPxCallbackPanel ID="CustomerDetailsCallbackPanel" EnableCallbackAnimation="true" runat="server">
    <ClientSideEvents Init="OnDataDependentControlInit" />
    <PanelCollection>
        <dx:PanelContent>
            <dx:ASPxPanel ID="CustomerDetailsHolder" runat="server" Visible="false">
                <PanelCollection>
                    <dx:PanelContent>
                        <div>
                            <div id="mapHolder"></div>
                            <script type="text/javascript" id="dxss_map">
                                CreateMap('<%= Location.Latitude %>', '<%= Location.Longitude %>');
                            </script>
                        </div>
                        <div id="customerChart">
                            <uc:HorizontalBarChart runat="server" ID="HorizontalBarChart" SubTitle="PURCHASES BY PRODUCT" ShowLegend="true" Height="180px" Width="520px" />
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
            <dx:ASPxPanel ID="EmptyDataMessageHolder" runat="server" Visible="true">
                <PanelCollection>
                    <dx:PanelContent>
                        <h3 class="verticalAlignMiddle">No data to display</h3>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>
