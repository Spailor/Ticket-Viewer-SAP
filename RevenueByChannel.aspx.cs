using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using System.Web.Security;

public partial class RevenueByChannel : BasePage
{
    public override IRangeControl RangeControl { get { return FooterRangeControl; } }
    public List<ChartData> ChannelsRevenue { get; set; }
    private DateTime EarliestDateTime { get; set; }

    protected void Page_Init(object sender, EventArgs e)
    {
        PaletteHelper.LoadCommonPalette(ChartControl);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        DateSelectorControl.CallbackPanelId = DailyRevenueCallbackPanel.ClientID;
        ProductSalesRevenue.SetData(SalesProvider.GetSalesGroupedByChannel(SalesStartDate, SalesEndDate));
        if (!IsCallback)
            PopulateDailySalesData();
    }
    private void PopulateDailySalesData()
    {
        AktiviteEntities db = new AktiviteEntities();
        Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
        var list = db.S_CagriChart1(-1, null, null, userId).ToList(); //24 000 kayıt çekiyor filtreleme yapmadan.
        if (list.Count > 0)
        {
            List<RangeChartData> listChart = new List<RangeChartData>();
            List<ChartData> listChartData = new List<ChartData>();
            List<S_CagriChart1_Result> cList = new List<S_CagriChart1_Result>();
            DateTime startDate = DateTimeHelper.GetIntervalStartDate(DateSelectorControl.CurrentDate.Date, SelectionInterval.Day);
            DateTime endDate = DateTimeHelper.GetIntervalEndDate(DateSelectorControl.CurrentDate.Date, SelectionInterval.Day);
            cList = list.Where(c => c.Argument > startDate && c.Argument < endDate).ToList();
            if (cList.Count > 0)
            {
                //sağ üst tarafdaki çizgili grafik.
                foreach (var item in cList)
                {
                    var c = new RangeChartData
                    {
                        Argument = Convert.ToDateTime(item.Argument),
                        SeriesName = item.SeriesName,
                        Value = Convert.ToDouble(item.Value)
                    };
                    listChart.Add(c);
                }

                //sol üst tarafdaki modül toplamları
                foreach (var item in cList)
                {
                    var x = new ChartData
                    {
                        PointName = item.SeriesName,
                        SeriesName = item.SeriesName,
                        Value = Convert.ToDouble(item.Value)
                    };
                    listChartData.Add(x);
                }
            }

            //IQueryable<BaseProvider<DataContext.Sale>> clist;
            //List<RangeChartData> data = SalesProvider.GetDailySalesGroupedByChannel(DateSelectorControl.CurrentDate).ToList();
            //ChannelsRevenue = data.GroupBy(d => d.SeriesName).Select(item =>
            //    new ChartData() {
            //        Value = item.Sum(x => x.Value), PointName = item.Key
            //    }).ToList();

            ChannelsRevenue = listChartData.GroupBy(d => d.SeriesName).Select(itemx =>
            new ChartData()
            {
                Value = itemx.Sum(q => q.Value),
                PointName = itemx.Key
            }).ToList();

            //EarliestDateTime = data.Min(x => x.Argument);
            //ChartControl.DataSource = data;

            if (listChart.Count > 0)
                EarliestDateTime = listChart.Min(x => x.Argument);

            ChartControl.DataSource = listChart;
            ChartControl.DataBind();
        }
        else
        {
            ChannelsRevenue = new List<ChartData>();
            ChartControl.DataSource = new List<RangeChartData>(); ;
            ChartControl.DataBind();
        }
    }

    protected void DailyRevenueCallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        int delta = 0;
        if (Int32.TryParse(e.Parameter, out delta) && delta != 0)
        {
            DateSelectorControl.ChangeDate(delta);
            PopulateDailySalesData();
        }
    }
    protected void ChartControl_CustomDrawAxisLabel(object sender, CustomDrawAxisLabelEventArgs e)
    {
        if (e.Item.Axis is AxisX && (DateTime)e.Item.AxisValue == EarliestDateTime)
            e.Item.Text = "";
    }
}
