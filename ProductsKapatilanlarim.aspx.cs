using System;
using DataAccess;
using DevExpress.Web;
using System.Linq;
using System.Collections.Generic;
using System.Web.Security;

public partial class Products : BasePage
{
    //public override IRangeControl RangeControl { get { return FooterRangeControl; } }
    AktiviteEntities db = new AktiviteEntities();
    Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            Session["SessCagriIstekID"] = "";

        //var list = db.S_Destec_CagriIstek(-1).ToList();
        var list = db.S_Destec_CagriKapatilanlarim(-1,userId).ToList();
        ProductsGridView.DataSource = list;
        ProductsGridView.DataBind();

        //using (ProductsProvider provider = new ProductsProvider())
        //{
        //    ProductsGridView.DataSource = provider.GetList();
        //    ProductsGridView.DataBind();

        if (!ProductsGridView.IsCallback)
        {
            //Product focusedRow = ProductsGridView.GetRow(ProductsGridView.FocusedRowIndex) as Product;
            //if (focusedRow != null)
            //    ProductDetails.LoadContent(focusedRow.Id);
            if (list.Count > 0)
            {
                string id = ProductsGridView.GetRowValues(0, "ID").ToString();
                CagriDetaylariniYukle(Convert.ToInt32(id));
            }
            //string id = ProductsGridView.GetRowValues(ProductsGridView.FocusedRowIndex, "ID").ToString();
            //int idnum = Convert.ToInt32(id);

        }
        //}

    }

    private const int TextMaxLength = 100;
    protected void ProductsGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "ID" && Convert.ToInt32(e.CellValue) < 0)
            e.Cell.Font.Bold = true;

        if (e.DataColumn.FieldName == "IstekAciklama")
            if (e.CellValue != null)
                e.Cell.ToolTip = e.CellValue.ToString();

    }
    protected void ProductsGridView_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        if (e.Column.FieldName == "IstekAciklama")
        {
            if (e.Value != null)
            {
                string cellValue = e.Value.ToString();
                if (cellValue.Length > TextMaxLength)
                    e.DisplayText = cellValue.Substring(0, TextMaxLength) + "...";
            }
        }
    }

    private void CagriDetaylariniYukle(int _ID)
    {
        int id = 0;
        id = _ID;
        var list = db.S_Destec_CagriKapatilanlarim(id,userId).ToList();
        if(list.Count > 0)
        {
            SirketKodu.Text = "Yok";
            SirketAdi.Text = list.First().IstekSirketAdi;
            AdiSoyadi.Text = list.First().IstekSahibiAdiSoyadi;
            EMailAdresi.Text = "Yok";

            TicketNumber.Text = "Yok";
            AltTicketNumber.Text = "Yok";

            DateTime tarih = list.First().IstekTarihiSaati.Value;
            Tarih.Text = tarih.Date.ToShortDateString();
            OnemDerecesi.Text = list.First().OnemDerecesiAdi;
            Modul.Text = "Yok";
            Aciklama.Text = list.First().IstekAciklama;
            CozumAciklama.Text = list.First().SonucAciklama;

            var evraklist = db.S_Destec_CagriIstegiEvrak(Convert.ToInt32(id)).ToList();
            if (evraklist.Count > 0)
            {
                ASPxLabel1.Text = evraklist.First().EvrakAdi;
            }
        }
        
    }

    protected void callbackOnayla_Callback(object source, CallbackEventArgs e)
    {
        string id = e.Parameter.ToString();
        Session["SessCagriIstekID"] = id;
    }

    protected void callbackPopupEvet_Callback(object source, CallbackEventArgs e)
    {
        int id = Convert.ToInt32(Session["SessCagriIstekID"]);
        db.U_Destec_CagriTestiniOnayla(id, 4,"");
        Session["SessCagriIstekID"] = "";
        var list = db.S_Destec_CagriKapatilanlarim(-1,userId).ToList();
        ProductsGridView.DataSource = list;
        ProductsGridView.DataBind();
    }

    protected void callbackReddet_Callback(object source, CallbackEventArgs e)
    {
        string id = e.Parameter.ToString();
        Session["SessCagriIstekID"] = id;
    }

    protected void callbackPopupReddetEvet_Callback(object source, CallbackEventArgs e)
    {
        int id = Convert.ToInt32(Session["SessCagriIstekID"]);
        db.U_Destec_CagriTestiniOnayla(id, 2,"");
        Session["SessCagriIstekID"] = "";
        var list = db.S_Destec_CagriKapatilanlarim(-1,userId).ToList();
        ProductsGridView.DataSource = list;
        ProductsGridView.DataBind();
    }
    //protected void callbackEvrak_Callback(object source, CallbackEventArgs e)
    //{
    //    string id = e.Parameter.ToString();
    //    var evraklist = db.S_Destec_CagriIstegiEvrak(Convert.ToInt32(id)).ToList();
    //    if (evraklist.Count > 0)
    //        e.Result = evraklist.First().EvrakAdi;
    //    else
    //        e.Result = "";
    //}

    //protected void callbackEkDosyalar_Callback(object source, CallbackEventArgs e)
    //{
    //    string id = e.Parameter.ToString();
    //    e.Result = id;
    //}
}
