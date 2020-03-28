using System;
using DataAccess;
using DevExpress.Web;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Security;

public partial class Products : BasePage
{
    //public override IRangeControl RangeControl { get { return FooterRangeControl; } }
    AktiviteEntities db = new AktiviteEntities();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            Session["SessCagriIstekID"] = "";

        Guid UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
        var list = db.S_Destec_CagriIstek(-1,UserId).ToList();
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
        Guid UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

        var list = db.S_Destec_CagriIstek(id,UserId).ToList();

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
        
        var evraklist = db.S_Destec_CagriIstegiEvrak(Convert.ToInt32(id)).ToList();
        if (evraklist.Count > 0)
        {
            ASPxLabel1.Text = evraklist.First().EvrakAdi;
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
        db.U_Destec_CagriIstegiKaynakYoneticisiOnayi(id, true);

        // mail fonksiyonu
        var cagriisteklist = db.S_Destec_CagriIstekIDVer(id).ToList();
        if(cagriisteklist.Count>0)
        {
            string strHTML = File.ReadAllText(HttpContext.Current.Server.MapPath("Mail/HTML/DestecCagriBilgilendirmesi.html"));
            string icerik = "Çağrı isteğiniz kaynak yöneticiniz "+ Membership.GetUser().UserName +" tarafından onaylanmıştır. <br /> İstek talebiniz çağrıya dönüşmüş olup, danışman ataması için sıraya alınmıştır. <br /> Çağrınız ile ilgili 'Açık Çağrılarım' ekranından durumunu takip edebilirsiniz.";
            strHTML = strHTML.Replace("{OLUSTURMATARIHI}", DateTime.Now.ToShortDateString())
                .Replace("{CAGRIID}", id.ToString())
                .Replace("{FIRMAADI}", cagriisteklist.First().IstekSirketAdi)
                .Replace("{KULLANICIADI}", cagriisteklist.First().IstekSahibiAdiSoyadi)
                .Replace("{ICERIK}", icerik);
            Guid userid = new Guid(Membership.GetUser().ProviderUserKey.ToString());
            var sirketidver = db.S_Destec_SirketIDVer(cagriisteklist.First().IstekSirketAdi).ToList();
            Guid sirketid = sirketidver.First().SirketId;
            var alicikisiler = db.S_Destec_GonderilecekMailAdresleri(2, userid, sirketid,id).ToList().First().MailAdresleri;
            string[] alicikisi = alicikisiler.ToString().Split(';');
            string mailadresleri = "";
            foreach (var item in alicikisi)
            {
                mailadresleri = item.Split('(')[1].Trim().Replace(")", "").ToString();
                db.I_Mail(-1, mailadresleri, strHTML, false, null, "DESTEC Ticket Viewer <mailservice@tecs.com.tr>", id.ToString() + " numaralı çağrı isteği", "DestecCagriIstegi", false, "");
            }
        }
        // mail fonksiyonu - bitti

        Session["SessCagriIstekID"] = "";
        Guid UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

        var list = db.S_Destec_CagriIstek(-1,UserId).ToList();
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
        db.U_Destec_CagriIstegiKaynakYoneticisiOnayi(id, false);
        Session["SessCagriIstekID"] = "";
        Guid UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

        var list = db.S_Destec_CagriIstek(-1,UserId).ToList();
        ProductsGridView.DataSource = list;
        ProductsGridView.DataBind();
    }
    protected void callbackEvrak_Callback(object source, CallbackEventArgs e)
    {
        string id = e.Parameter.ToString();
        if(id != "")
        {
            var evraklist = db.S_Destec_CagriIstegiEvrak(Convert.ToInt32(id)).ToList();
            if (evraklist.Count > 0)
                e.Result = evraklist.First().EvrakAdi;
            else
                e.Result = "";
        }
        else
            e.Result = "";

    }

    protected void callbackEkDosyalar_Callback(object source, CallbackEventArgs e)
    {
        string id = e.Parameter.ToString();
        e.Result = id;
    }
}
