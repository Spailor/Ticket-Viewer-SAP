using System;
using System.Collections.Generic;
using DataAccess;

public partial class RevenueFooter : UserControlBase {

    public string Title { get; set; } //SECTOR SALES buraya dinamik olarak gelitor 

    protected void Page_Load(object sender, EventArgs e) {
        DoughnutChart.Title = "ÇAĞRI İSTEKLERİ"; //Title;
        HorizontalBarChart.SubTitle = Title + " DAĞILIMI";
        HorizontalBarChart.Title = DateTimeHelper.GetDateRangeString(GetSalesStartDate(), GetSalesEndDate());
    }

    public void SetData(IEnumerable<ChartData> data) {
        DoughnutChart.SetData(data);
        HorizontalBarChart.SetData(data);
    }
}
