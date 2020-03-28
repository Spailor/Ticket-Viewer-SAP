<%@ Control Language="C#" AutoEventWireup="true" CodeFile="VerticalBarChart.ascx.cs" Inherits="VerticalBarChart" %>
<%@ Register Src="~/UserControls/Common/DateSelectorControl.ascx" TagPrefix="uc" TagName="DateSelectorControl" %>
<dx:ASPxCallbackPanel ID="VerticalBarCallbackPanel" OnCallback="VerticalBarCallbackPanel_Callback"
    runat="server" EnableCallbackAnimation="true" ClientIDMode="AutoID">
    <SettingsLoadingPanel Enabled="false" />
    <PanelCollection>
        <dx:PanelContent>
            <span class="verticalBarChartTitle"><%= Title %></span>
            <div class="verticalBarChartHeader">
                <div>
                    <div class="verticalBarChartHeaderTitle">
                        <%= CurrentTitle %>
                    </div>
                    <div>
                        <%= string.Format(TitleFormatString, CurrentValue) %>
                    </div>
                </div>
                <div>
                    <div class="verticalBarChartHeaderTitle">
                        <%= PreviousTitle %>
                    </div>
                    <div>
                        <%= string.Format(TitleFormatString, PreviousValue) %>
                    </div>
                </div>
                <div>
                    <div class="verticalBarChartHeaderTitle">
                        <%= CustomPeriodTitle %>
                    </div>
                    <div>
                        <%= string.Format(TitleFormatString, CustomPeriodValue) %>
                    </div>
                </div>
                <div class="dateSelectorHolder">
                    <uc:DateSelectorControl runat="server" ID="DateSelectorControl" />
                </div>
            </div>
            <div class="verticalChart" style="margin-left: <%=ChartOffsetX%>px;">

                <dxchartsui:WebChartControl ID="VerticalChartControl" ClientIDMode="AutoID"
                    SeriesDataMember="SeriesName"
                    OnCustomDrawAxisLabel="VerticalChartControl_CustomDrawAxisLabel"
                    runat="server" Height="200px" CrosshairEnabled="True" Width="600px" BackColor="Transparent">
                    <Padding Bottom="0" Left="0" Right="0" Top="0" />
                    <BorderOptions Visibility="False" />
                    <FillStyle FillMode="Empty"></FillStyle>
                    <DiagramSerializable>
                        <dxcharts:XYDiagram>
                            <axisx visibleinpanesserializable="-1" color="224, 224, 224">
                                <scalebreakoptions color="Transparent" />
                                <tickmarks minorvisible="False" visible="False" />
                                <label enableantialiasing="True" font="Segoe UI, 13px">
                                </label>
                            </axisx>
                            <axisy visibleinpanesserializable="-1" color="Transparent">
                                <tickmarks minorvisible="False" visible="False" />
                                <label enableantialiasing="True" font="Segoe UI, 13px">
                                </label>
                                <visualrange autosidemargins="True" />
                                <wholerange autosidemargins="True" />
                                <gridlines color="224,224,224"></gridlines>
                            </axisy>
                            <margins bottom="0" left="0" right="0" top="0" />
                            <defaultpane bordervisible="False" backcolor="Transparent" weight="5">
                            </defaultpane>
                        </dxcharts:XYDiagram>
                    </DiagramSerializable>
                    <Legend Visibility="False"></Legend>
                    <SeriesTemplate CrosshairLabelVisibility="True" ArgumentDataMember="PointName" ValueDataMembersSerializable="Value">
                        <viewserializable>
                          <dxcharts:SideBySideBarSeriesView Color="128, 219, 219, 219">
                              <border visibility="False" />
                              <fillstyle fillmode="Solid">
                              </fillstyle>
                          </dxcharts:SideBySideBarSeriesView>
                      </viewserializable>
                    </SeriesTemplate>
                </dxchartsui:WebChartControl>

            </div>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>
