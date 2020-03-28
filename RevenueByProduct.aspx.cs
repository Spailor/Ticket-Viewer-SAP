using System;
using System.Collections.Generic;
using DataAccess;
using System.Linq;

public partial class RevenueByProduct : RevenueBasePage
{
    public override IRangeControl RangeControl { get { return FooterRangeControl; } }

    protected override VerticalChartControlBase DailySalesPerformanceVerticalChart { get { return DailySalesPerformanceChart; } }
    protected override VerticalChartControlBase UnitSalesVerticalChart { get { return UnitSalesChart; } }

    protected override IEnumerable<ChartData> GetChartSalesCountCollection(DateTime startDate, DateTime endDate)
    {
        return SalesProvider.GetSaleCountGroupedByProduct(startDate, endDate);
        
        //List<ChartData> listChartData = new List<ChartData>();
        //return listChartData;

    }
    protected override IEnumerable<ChartData> GetChartSalesRevenueCollection(DateTime startDate, DateTime endDate)
    {
        //return SalesProvider.GetSalesGroupedByProduct(startDate, endDate);
        return SalesProvider.GetCagriGroupedByModul(startDate, endDate);

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ProductSalesRevenue.SetData(SalesProvider.GetCagriGroupedByModul(SalesStartDate, SalesEndDate));
    }
}
