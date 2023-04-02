using System;
using DataAccess;
using DevExpress.Web;

public partial class RangeControl : UserControlBase, IRangeControl
{
    const string ChartUrlPattern = "~/Chart.aspx?w=1141px&h=50px&start={0:s}&end={1:s}",
                 CurrentYearSessionKey = "CurrentYear";
    protected DateTime MinDateTime { get; set; }
    protected DateTime MaxDateTime { get; set; }
    protected int CurrentYear
    {
        get
        {
            if (Session[CurrentYearSessionKey] == null)
                Session[CurrentYearSessionKey] = DateTime.Now.Year;
            return (int)Session[CurrentYearSessionKey];
        }
        set { Session[CurrentYearSessionKey] = value; }
    }
    //public DateTime GetStartDate()
    //{
    //    DateTime tarih;
    //    tarih = DateTimeHelper.GetIntervalStartDate(new DateTime(CurrentYear, (int)SalesDateRange.PositionStart + 1, 1), SelectionInterval.Month);
    //    return tarih;
    //}
    //public DateTime GetEndDate()
    //{
    //    DateTime tarih;
    //    tarih = DateTimeHelper.GetIntervalEndDate(new DateTime(CurrentYear, (int)SalesDateRange.PositionEnd + 1, 1), SelectionInterval.Month);
    //    return tarih;
    //}
    public DateTime GetStartDate()
    {
        return DateTimeHelper.GetIntervalStartDate(new DateTime(CurrentYear, (int)SalesDateRange.PositionStart + 1, 1), SelectionInterval.Month);
    }
    public DateTime GetEndDate()
    {
        //PROJE AÇILIŞINDA YIL 2018 OLUNCA PATLIYOR. REVENUE BY CHANNEL ASPX SAYFASINDA İLK YÜKLENEDE BURADAKİ TARİHTEN DOLAYI SANIRIM. 
        //BU CHARTI PROJEDE KOMPLE DEVRE DIŞI BIRAKTIM. LOAD DAKİ VE INIT DEKİ KODLARI COMMENTLEDİM.
        //int positionEnd = 0;
        //if(-1 == (int)SalesDateRange.PositionEnd)
        //{
        //    positionEnd = 0;
        //}

        ////    return DateTimeHelper.GetIntervalEndDate(new DateTime(CurrentYear, 0 + 1, 1), SelectionInterval.Month);
        ////else

        //return DateTimeHelper.GetIntervalEndDate(new DateTime(CurrentYear, positionEnd + 1, 1), SelectionInterval.Month);

        return DateTimeHelper.GetIntervalEndDate(new DateTime(CurrentYear, (int)SalesDateRange.PositionEnd + 1, 1), SelectionInterval.Month);
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        //UpdateMinMaxDate();
        //PopulateTrackBarItems();
        //if (!IsPostBack)
        //    InitializePositions();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //PrepareControl();
    }
    private string GetBackgroundChartImageUrl()
    {
        return string.Format(ChartUrlPattern, DateTimeHelper.GetIntervalStartDate(new DateTime(CurrentYear, 1, 1), SelectionInterval.Month),
                                              DateTimeHelper.GetIntervalEndDate(new DateTime(CurrentYear, GetLastPossibleMonth(), 1), SelectionInterval.Month));
    }
    private int GetLastPossibleMonth()
    {
        if (CurrentYear == DateTime.Now.Year)
            return DateTime.Now.Month;
        return 12;
    }
    private void UpdateMinMaxDate()
    {

        MinDateTime = DateTimeHelper.GetIntervalStartDate(SalesProvider.GetMinDate(), SelectionInterval.Month);
        MaxDateTime = DateTimeHelper.GetIntervalEndDate(SalesProvider.GetMaxDate(), SelectionInterval.Month);
    }
    private void PopulateTrackBarItems()
    {
        SalesDateRange.Items.Clear();
        DateTime startDate = new DateTime(CurrentYear, 1, 1);
        DateTime endDate = new DateTime(CurrentYear, 12, 1);
        if (endDate > MaxDateTime)
            endDate = MaxDateTime;
        if (startDate < MinDateTime)
            startDate = MinDateTime;

        DateTime tempDate = startDate;
        while (tempDate <= endDate)
        {
            string mask =
                DateTimeHelper.HasSameYearAndMonth(tempDate, startDate) || DateTimeHelper.HasSameYearAndMonth(tempDate, endDate) ? "MMM yyyy" : "MMM";
            SalesDateRange.Items.Add(tempDate.ToString(mask), tempDate.Month - 1);
            tempDate = tempDate.AddMonths(1);
        }
    }
    private void PrepareControl()
    {
        LeftShiftButton.Enabled = MinDateTime.Year < CurrentYear;
        RightShiftButton.Enabled = MaxDateTime.Year > CurrentYear;
        BackgroundChartImage.ImageUrl = GetBackgroundChartImageUrl();
    }
    protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        int deltaYear;
        if (Int32.TryParse(e.Parameter, out deltaYear) && (CurrentYear + deltaYear) <= MaxDateTime.Year && (CurrentYear + deltaYear) >= MinDateTime.Year)
        {
            CurrentYear += deltaYear;
            PopulateTrackBarItems();
            PrepareControl();
            InitializePositions();
        }
    }
    private void InitializePositions()
    {
        SalesDateRange.PositionEnd = SalesDateRange.Items.Count - 1;
        SalesDateRange.PositionStart = 0;
    }
}
