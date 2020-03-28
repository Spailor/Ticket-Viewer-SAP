using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DataAccess
{
    public class SalesProvider : BaseProvider<DataContext.Sale>
    {

        public IEnumerable<ChartData> GetSalesGroupedByRegion(int productId, DateTime minDate, DateTime maxDate)
        {
            return TryGetResult<IEnumerable<ChartData>>(() =>
            {
                return (from s in DataTable
                        where s.SaleDate >= minDate &&
                        s.SaleDate <= maxDate &&
                        s.ProductId == productId
                        group s by new { PointName = s.Region.Name } into saleGroup
                        select new ChartData
                        {
                            PointName = saleGroup.Key.PointName,
                            Value = saleGroup.Sum(x => x.TotalCost)
                        }).ToList();
            });
        }

        List<ChartData> listChartData = new List<ChartData>();
        AktiviteEntities db = new AktiviteEntities();

        public IEnumerable<ChartData> GetSalesGroupedByChannel(int productId, DateTime minDate, DateTime maxDate)
        {

            //return TryGetResult<IEnumerable<ChartData>>(() =>
            //{
            //    IQueryable<DataContext.Sale> query = (from s in DataTable
            //                                          where s.SaleDate >= minDate && s.SaleDate <= maxDate
            //                                          select s);
            //    if (productId > -1)
            //        query = query.Where(s => s.ProductId == productId);

            //    return (from s in query
            //            group s by new { PointName = s.Channel.Name } into saleGroup
            //            select new ChartData
            //            {
            //                PointName = saleGroup.Key.PointName,
            //                Value = saleGroup.Sum(x => x.TotalCost)
            //            });
            //});

            return TryGetResult<IEnumerable<ChartData>>(() =>
            {

                //IQueryable<DataContext.Sale> query = (from s in DataTable
                //                                      where s.SaleDate >= minDate && s.SaleDate <= maxDate
                //                                      select s);

                try
                {
                    Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
                    var list = db.S_CagriChart2(minDate, maxDate,userId).ToList();
                    foreach (var item in list)
                    {
                        var x = new ChartData
                        {
                            PointName = item.SeriesName,
                            SeriesName = "Series1",
                            Value = Convert.ToDouble(item.Value)
                        };
                        listChartData.Add(x);
                    }

                    return listChartData;
                }
                catch (Exception hata)
                {
                    return listChartData;
                }
            });
        }
        public IEnumerable<ChartData> GetSalesGroupedByChannel(DateTime minDate, DateTime maxDate)
        {
            return GetSalesGroupedByChannel(-1, minDate, maxDate);
        }

        public IEnumerable<ChartData> GetSalesGroupedBySector(int productId, DateTime minDate, DateTime maxDate)
        {
            //return TryGetResult<IEnumerable<ChartData>>(() =>
            //{
            //    IQueryable<DataContext.Sale> query = (from s in DataTable
            //                                          where s.SaleDate >= minDate && s.SaleDate <= maxDate
            //                                          select s);
            //    if (productId > -1)
            //        query = query.Where(s => s.ProductId == productId);
            //    return (from s in query
            //            group s by new { PointName = s.Sector.Name } into saleGroup
            //            select new ChartData
            //            {
            //                PointName = saleGroup.Key.PointName,
            //                Value = saleGroup.Sum(x => x.TotalCost)
            //            });
            //});

            List<ChartData> listChartData = new List<ChartData>();
            List<S_DestecChartCagriDurum_Result> cList = new List<S_DestecChartCagriDurum_Result>();
            var list = db.S_DestecChartCagriDurum(minDate, maxDate).ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    var x = new ChartData
                    {
                        PointName = item.SeriesName,
                        SeriesName = "Series1",
                        Value = Convert.ToDouble(item.Value)
                    };
                    listChartData.Add(x);
                }
            }
            return listChartData;
        }
        public IEnumerable<ChartData> GetSalesGroupedBySector(DateTime minDate, DateTime maxDate)
        {
            return GetSalesGroupedBySector(-1, minDate, maxDate);
        }

        public IEnumerable<ChartData> GetCagriGroupedByModul(DateTime minDate, DateTime maxDate)
        {
            try
            {
                List<ChartData> listChartData = new List<ChartData>();
                List<S_CagriChart2_Result> cList = new List<S_CagriChart2_Result>();
                Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
                var list = db.S_CagriChart2(minDate, maxDate,userId).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        var x = new ChartData
                        {
                            PointName = item.SeriesName,
                            SeriesName = "Series1",
                            Value = Convert.ToDouble(item.Value)
                        };
                        listChartData.Add(x);
                    }
                }
                return listChartData;
            }
            catch (Exception hata)
            {
                return listChartData;
            }
        }

        public IEnumerable<ChartData> GetSalesGroupedByProduct(DateTime minDate, DateTime maxDate)
        {

            //return TryGetResult<IEnumerable<ChartData>>(() =>
            //{
            //    return (from s in DataTable
            //            where s.SaleDate >= minDate &&
            //            s.SaleDate <= maxDate
            //            group s by new { PointName = s.Product.Name } into saleGroup
            //            select new ChartData
            //            {
            //                PointName = saleGroup.Key.PointName,
            //                Value = saleGroup.Sum(x => x.TotalCost)
            //            }).ToList();
            //});

            try
            {
                List<ChartData> listChartData = new List<ChartData>();
                List<S_CagriChart2_Result> cList = new List<S_CagriChart2_Result>();

                AktiviteEntities db = new AktiviteEntities();
                Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
                var list = db.S_CagriChart2(minDate, maxDate,userId).ToList();

                cList = list;

                if (cList.Count > 0)
                {
                    foreach (var item in cList)
                    {
                        var x = new ChartData
                        {
                            PointName = item.SeriesName,
                            SeriesName = "Series1",
                            Value = Convert.ToDouble(item.Value)
                        };
                        listChartData.Add(x);
                    }
                }
                return listChartData;
            }
            catch (Exception hata)
            {
                return listChartData;
            }
            //var xlist = listChartData.GroupBy(z => z.PointName);

            //return TryGetResult<IEnumerable<ChartData>>(() =>
            //{
            //    return (from s in xlist
            //          select new ChartData
            //          {
            //              PointName = xlist.FirstOrDefault().FirstOrDefault().PointName,
            //              Value = xlist.FirstOrDefault().FirstOrDefault().Value
            //          }
            //            ).ToList();
            //});
        }

        public IEnumerable<ChartData> GetCustomerPurchasesGroupedByProduct(int customerId, DateTime minDate, DateTime maxDate)
        {
            return TryGetResult<IEnumerable<ChartData>>(() =>
            {
                return (from s in DataTable
                        where s.SaleDate >= minDate &&
                        s.SaleDate <= maxDate &&
                        s.CustomerId == customerId
                        group s by new { PointName = s.Product.Name } into saleGroup
                        select new ChartData
                        {
                            PointName = saleGroup.Key.PointName,
                            Value = saleGroup.Sum(x => x.TotalCost)
                        }).ToList();
            });
        }

        #region Revenue
        public IEnumerable<ChartData> GetSaleCountGroupedByProduct(DateTime minDate, DateTime maxDate)
        {
            //return TryGetResult<IEnumerable<ChartData>>(() =>
            //{
            //    return (from s in DataTable
            //            where s.SaleDate >= minDate &&
            //            s.SaleDate <= maxDate
            //            group s by new { PointName = s.Product.Name } into saleGroup
            //            select new ChartData
            //            {
            //                PointName = saleGroup.Key.PointName,
            //                Value = saleGroup.Count()
            //            }).ToList();
            //});

            try
            {
                List<ChartData> listChartData = new List<ChartData>();
                AktiviteEntities db = new AktiviteEntities();
                Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
                var list = db.S_CagriChart2(minDate, maxDate,userId).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        var x = new ChartData
                        {
                            PointName = item.SeriesName,
                            SeriesName = item.SeriesName,//"Series1",
                            Value = Convert.ToDouble(item.Value)
                        };
                        listChartData.Add(x);
                    }
                }
                return listChartData;
            }
            catch (Exception hata)
            {
                return listChartData;
            }
        }
        public IEnumerable<ChartData> GetSaleCountGroupedBySector(DateTime minDate, DateTime maxDate)
        {
            //return TryGetResult<IEnumerable<ChartData>>(() =>
            //{
            //    return (from s in DataTable
            //            where s.SaleDate >= minDate &&
            //            s.SaleDate <= maxDate
            //            group s by new { PointName = s.Sector.Name } into saleGroup
            //            select new ChartData
            //            {
            //                PointName = saleGroup.Key.PointName,
            //                Value = saleGroup.Count()
            //            }).ToList();
            //});
            List<ChartData> listChartData = new List<ChartData>();
            var list = db.S_DestecChartCagriDurum(minDate, maxDate).ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    var x = new ChartData
                    {
                        PointName = item.SeriesName,
                        SeriesName = item.SeriesName,//"Series1",
                        Value = Convert.ToDouble(item.Value)
                    };
                    listChartData.Add(x);
                }
            }
            return listChartData;
        }
        public int GetSaleCount(DateTime minDate, DateTime maxDate)
        {
            return TryGetResult<int>(() =>
            {
                List<ChartData> listChartData = new List<ChartData>();
                List<S_CagriChart1_Result> cList = new List<S_CagriChart1_Result>();
                Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
                AktiviteEntities db = new AktiviteEntities();
                var list = db.S_CagriChart1(-1, null, null, userId).ToList();

                //DateTime startDate = DateTimeHelper.GetIntervalStartDate(DateTime.Now.Date, SelectionInterval.Day);
                //DateTime endDate = DateTimeHelper.GetIntervalEndDate(DateTime.Now.Date, SelectionInterval.Day);

                cList = list.Where(c => c.Argument > minDate && c.Argument < maxDate).ToList();
                int i = 0;
                if (cList.Count > 0)
                    i = Convert.ToInt32(cList.Sum(s => s.Value));

                return i;
                //return DataTable.Count(s => s.SaleDate >= minDate && s.SaleDate <= maxDate);
            }, useCache: true, keySuffix: string.Format("{0}.{1}", minDate, maxDate));
        }
        public double GetSalesRevenue(DateTime minDate, DateTime maxDate)
        {
            return TryGetResult<double>(() =>
            {
                List<ChartData> listChartData = new List<ChartData>();
                List<S_CagriChart1_Result> cList = new List<S_CagriChart1_Result>();
                Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
                AktiviteEntities db = new AktiviteEntities();
                var list = db.S_CagriChart1(-1, null, null,userId).ToList();

                //DateTime startDate = DateTimeHelper.GetIntervalStartDate(DateTime.Now.Date, SelectionInterval.Day);
                //DateTime endDate = DateTimeHelper.GetIntervalEndDate(DateTime.Now.Date, SelectionInterval.Day);

                cList = list.Where(c => c.Argument > minDate && c.Argument < maxDate).ToList();
                double d = Convert.ToDouble(cList.Sum(s => s.Value));
                return d;
            }, useCache: true, keySuffix: string.Format("{0}.{1}", minDate, maxDate));
        }
        public IEnumerable<RangeChartData> GetDailySalesGroupedByChannel(DateTime day)
        {
            return TryGetResult<IEnumerable<RangeChartData>>(() =>
            {
                DateTime startDate = DateTimeHelper.GetIntervalStartDate(day, SelectionInterval.Day);
                DateTime endDate = DateTimeHelper.GetIntervalEndDate(day, SelectionInterval.Day);


                return (from s in DataTable
                        where s.SaleDate >= startDate && s.SaleDate <= endDate
                        group s by new
                        {
                            SeriesName = s.Channel.Name,
                            Year = s.SaleDate.Year,
                            Month = s.SaleDate.Month,
                            Day = s.SaleDate.Day,
                            Hour = s.SaleDate.Hour
                        } into saleGroup
                        select new
                        {
                            TotalCost = saleGroup.Sum(x => x.TotalCost),
                            SeriesName = saleGroup.Key.SeriesName,
                            Year = saleGroup.Key.Year,
                            Month = saleGroup.Key.Month,
                            Day = saleGroup.Key.Day,
                            Hour = saleGroup.Key.Hour
                        }).ToList().Select(s => new RangeChartData()
                        {
                            Argument = new DateTime(s.Year, s.Month, s.Day, s.Hour, 0, 0, 0),
                            SeriesName = s.SeriesName,
                            Value = s.TotalCost
                        });
            });
        }
        #endregion

        // Footer Range Control

        public IEnumerable<RangeChartData> GetRangeChartData(DateTime startDate, DateTime endDate)
        {
            //List<RangeChartData> listChart = new List<RangeChartData>();
            Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
            var xlist = db.S_CagriChart1(-1, null, null,userId).ToList();
            //var list = db.S_CagriChart1(-1, startDate, endDate).ToList(); //TARİHLİ GÖNDERİNCE SORGU ÇOK UZUN BEKLEYİP ÇATLATIYOR UYGULAMAYI. ÇOK İLGİNÇ :)
            var list = xlist.Where(c => c.Argument >= startDate & c.Argument <= endDate).ToList();
            foreach (var item in list)
            {
                var c = new RangeChartData
                {
                    Argument = Convert.ToDateTime(item.Argument),
                    SeriesName = item.SeriesName,
                    Value = Convert.ToDouble(item.Value)
                };
                listChart.Add(c);
            }
            return TryGetResult<IEnumerable<RangeChartData>>(() =>
            {
                //return (from s in DataTable
                //        where s.SaleDate >= startDate && s.SaleDate <= endDate
                //        group s by new { Year = s.SaleDate.Year, Month = s.SaleDate.Month, Day = s.SaleDate.Date.Day } into groupS
                //        select new
                //        {
                //            Year = groupS.Key.Year,
                //            Month = groupS.Key.Month,
                //            Day = groupS.Key.Day,
                //            TotalCost = groupS.Sum(x => x.TotalCost)
                //        }).ToList().Select(x => new RangeChartData()
                return (from s in listChart
                        where s.Argument >= startDate && s.Argument <= endDate
                        group s by new { Year = s.Argument.Year, Month = s.Argument.Month, Day = s.Argument.Date.Day } into groupS
                        select new
                        {
                            Year = groupS.Key.Year,
                            Month = groupS.Key.Month,
                            Day = groupS.Key.Day,
                            TotalCost = groupS.Sum(x => x.Value)
                        }).ToList().Select(x => new RangeChartData()
                        {
                            Argument = new DateTime(x.Year, x.Month, x.Day),
                            Value = x.TotalCost
                        }).OrderBy(s => s.Argument);
            }, useCache: true, keySuffix: string.Format("{0}.{1}", startDate, endDate));
        }

        List<RangeChartData> listChart = new List<RangeChartData>();
        public DateTime GetMinDate()
        {
            string durum = "bos";
            try
            {
                Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
                var list = db.S_CagriChart1(-1, null, null,userId).ToList();

                foreach (var item in list)
                {
                    var c = new RangeChartData
                    {
                        Argument = Convert.ToDateTime(item.Argument),
                        SeriesName = item.SeriesName,
                        Value = Convert.ToDouble(item.Value)
                    };
                    listChart.Add(c);
                    durum = item.Value.ToString();
                }

                //    return TryGetResult<DateTime>(() =>
                //{
                //    return DataTable.Min(s => s.SaleDate);
                //}, useCache: true);

                string a = "";

                return TryGetResult<DateTime>(() =>
                {
                    if (listChart.Count > 0)
                        return listChart.Min(c => c.Argument);
                    else
                        return new DateTime(DateTime.Now.Year,1,1);// Convert.ToDateTime("01.01.1900");
                }, useCache: true);
            }
            catch (Exception hata)
            {
                string catlama = durum;
                return TryGetResult<DateTime>(() =>
                {
                    return Convert.ToDateTime("01.01.1900");
                }, useCache: true);

            }
        }
        public DateTime GetMaxDate()
        {
            //return TryGetResult<DateTime>(() =>
            //{
            //    return DataTable.Where(s => s.SaleDate <= DateTimeHelper.GetIntervalEndDate(DateTime.Now, SelectionInterval.Day)).Max(s => s.SaleDate);
            //}, useCache: true);
            return TryGetResult<DateTime>(() =>
            {
                if (listChart.Count > 0)
                    return listChart.Max(c => c.Argument);
                else
                    return new DateTime(DateTime.Now.Year, 1, 1);//Convert.ToDateTime("01.01.1900");
            }, useCache: true);
        }
    }

    #region Data Transfer Objects
    public class Sale
    {
        public string ProductName { get; set; }
        public DateTime SaleDate { get; set; }
        public double TotalCost { get; set; }
    }

    public class ChartDataBase
    {
        public double Value { get; set; }
        public string SeriesName { get; set; }

        public ChartDataBase()
        {
            SeriesName = "Series1";
        }
    }
    public class ChartData : ChartDataBase
    {
        public string PointName { get; set; }
    }
    public class RangeChartData : ChartDataBase
    {
        public DateTime Argument { get; set; }
    }
    #endregion
}
