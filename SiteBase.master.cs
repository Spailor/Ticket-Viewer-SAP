using System;
using System.Web.UI;
using DevExpress.Web;
using System.Web.Security;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.IO;

public partial class SiteMasterBase : MasterPage
{
    AktiviteEntities db = new AktiviteEntities();
    protected void DownloadButton_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e)
    {
        e.Properties["cpTrialUrl"] = "http://www.tecs.com.tr/"; //AssemblyInfo.DXLinkTrial;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string geldigiSayfaAdi = Request.RawUrl;
        if (geldigiSayfaAdi == "/Account/Register.aspx")
            return;

        if (!IsPostBack)
        {
            Session["CagriIstegiID"] = "";
            AktiviteEntities db = new AktiviteEntities();
            var list = db.S_Tip(3).ToList();
            if (list.Count > 0)
            {
                var OnemList = new List<OnemDerecesi>();
                foreach (var item in list)
                {
                    var c = new OnemDerecesi
                    {
                        ID = item.ID,
                        Aciklama = item.Aciklama
                    };
                    OnemList.Add(c);
                }
                cmbOnemDerecesi.DataSource = OnemList;
                cmbOnemDerecesi.DataBind();
            }

            var modul = db.S_Modul(-1).ToList();
            if (modul.Count > 0)
            {
                var modulList = new List<Moduller>();
                foreach (var item in modul)
                {
                    var q = new Moduller
                    {
                        ID = item.ModulID,
                        Aciklama = item.ModulAdi
                    };
                    modulList.Add(q);
                }
                tbModul.DataSource = modulList;
                tbModul.DataBind();
            }

            Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
            var sirketler = db.S_Destec_UsersInSirket_GetKullanicininSirketleri(userId).ToList();
            if (sirketler.Count > 0)
            {
                var sirketList = new List<Sirketler>();
                foreach (var item in sirketler)
                {
                    var c = new Sirketler
                    {
                        SirketId = item.SirketId.Value,
                        SirketAdi = item.SirketAdi
                    };
                    sirketList.Add(c);
                }
                cmbSirket.DataSource = sirketList;
                cmbSirket.DataBind();
            }
        }
    }

    protected void ASPxCallback1_Callback(object source, CallbackEventArgs e)
    {
        try
        {
            DateTime tarih = Convert.ToDateTime(deTarih.Value);
            string sirketId = cmbSirket.Value.ToString();
            string sirketAdi = cmbSirket.Text;// txtSirketAdi.Value.ToString();
            string adiSoyadi = "";// txtAdiSoyadi.Value.ToString(); //guid olarak sp ye gönderildiği için kapatıldı.
            string aciklama = mtxtAciklama.Value.ToString();
            int onemDerecesi = Convert.ToInt32(cmbOnemDerecesi.Value);
            string modul = tbModul.Value.ToString();

            string extTicNum, altTicNum, sirketKodu, kisiMail;
            if (txtTicNum.Value != null)
                extTicNum = txtTicNum.Value.ToString();
            else
                extTicNum = "";

            if (txtAltTicNum.Value != null)
                altTicNum = txtAltTicNum.Value.ToString();
            else
                altTicNum = "";

            if (txtSirketKodu.Value != null)
                sirketKodu = txtSirketKodu.Value.ToString();
            else
                sirketKodu = "";

            if (txtKisiMail.Value != null)
                kisiMail = txtKisiMail.Value.ToString();
            else
                kisiMail = "";

            Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
            Guid SirketID = new Guid(sirketId);

            var list = db.I_Destec_Istek(extTicNum, altTicNum, adiSoyadi, 
                tarih, sirketAdi, aciklama, userId, onemDerecesi, modul, 
                sirketKodu, kisiMail, SirketID).ToList();
            if (list.Count > 0)
            {
                string cagriidno = list.First().ID.ToString();
                e.Result = string.Format("{0};{1};{2};{3}", 0, 0, "Çağrı talebiniz, " + cagriidno + " numaralı istek numarası ile gönderilmiştir.", "Bilgi Mesajı");
                Session["CagriIstegiID"] = list.FirstOrDefault().ID.ToString();

                // mail fonksiyonu
                string strHTML = File.ReadAllText(HttpContext.Current.Server.MapPath("Mail/HTML/DestecIstekBilgilendirmesi.html"));
                strHTML = strHTML.Replace("{OLUSTURMATARIHI}", tarih.ToString())
                    .Replace("{ISTEKID}", cagriidno)
                    .Replace("{FIRMAADI}", sirketAdi)
                    .Replace("{KULLANICIADI}", Membership.GetUser().UserName.ToString())
                    .Replace("{ICERIK}", "Çağrı isteğiniz başarılı şekilde oluşturulmuştur. <br /> İsteğiniz kaynak yöneticinize gönderilmiş olup, 'Onay Bekleyen İsteklerim' ekranından durumunu takip edebilirsiniz.");
                Guid userid = new Guid(Membership.GetUser().ProviderUserKey.ToString());
                Guid sirketid = new Guid(cmbSirket.Value.ToString());
                var alicikisiler = db.S_Destec_GonderilecekMailAdresleri(1, userid, sirketid, -1).ToList().First().MailAdresleri; //"cenk (cenk.yenikoylu@tecs.com.tr);mcy (mcyenikoylu@gmail.com)";
                if (alicikisiler != null)
                {
                    string[] alicikisi = alicikisiler.ToString().Split(';');
                    string mailadresleri = "";
                    foreach (var item in alicikisi)
                    {
                        mailadresleri = item.Split('(')[1].Trim().Replace(")", "").ToString();
                        db.I_Mail(-1, mailadresleri, strHTML, false, null, "DESTEC Ticket Viewer <mailservice@tecs.com.tr>", cagriidno + " numaralı çağri isteği", "DestecCagriIstegi", false, "");
                    }
                }
                // mail fonksiyonu - bitti
            }
            else
                e.Result = string.Format("{0};{1};{2};{3}", 0, 0, "Çağrı talebiniz gönderilemedi!", "Hata Mesajı");

        }
        catch (Exception hata)
        {
            e.Result = string.Format("{0};{1};{2};{3}", 0, 0, "Çağrı talebiniz gönderilemedi!", "Hata Mesajı");
        }
    }

    protected void ASPxCallback2_Callback(object source, CallbackEventArgs e)
    {
        //Çıkış
        Session.Abandon();
        FormsAuthentication.SignOut();
        ASPxWebControl.RedirectOnCallback(VirtualPathUtility.ToAbsolute("~/Account/Login.aspx"));
    }

    const string UploadDirectory = "~/UploadControl/";
    protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
    {
        string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
        string resultFileName = Path.ChangeExtension(Path.GetRandomFileName(), resultExtension);
        string resultFileUrl = UploadDirectory + resultFileName;
        string resultFilePath = MapPath(resultFileUrl);
        e.UploadedFile.SaveAs(resultFilePath);

        string cagriIstegiID = Session["CagriIstegiID"].ToString();

        db.I_Destec_CagriIstegiEvrak(Convert.ToInt32(cagriIstegiID), resultFileName);

        //UploadingUtils.RemoveFileWithDelay(resultFileName, resultFilePath, 5);

        //string name = e.UploadedFile.FileName;
        //string url = ResolveClientUrl(resultFileUrl);
        //long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
        //string sizeText = sizeInKilobytes.ToString() + " KB";
        //e.CallbackData = name + "|" + url + "|" + sizeText;
    }

    public class OnemDerecesi
    {
        public int ID { get; set; }
        public string Aciklama { get; set; }
    }
    public class Moduller
    {
        public int ID { get; set; }
        public string Aciklama { get; set; }
    }
    public class Sirketler
    {
        public Guid SirketId { get; set; }
        public string SirketAdi { get; set; }
    }

    protected void CallbackSirketComboBox_Callback(object source, CallbackEventArgs e)
    {
        string result = "";
        string SirketId = e.Parameter.ToString();
        Guid SirketGuid = new Guid(SirketId);
        var modul = db.S_Destec_ModulInSirket(SirketGuid).ToList();
        if (modul.Count > 0)
        {
            var modulList = new List<Moduller>();
            foreach (var item in modul)
            {
                //var q = new Moduller
                //{
                //    ID = Convert.ToInt32(item.ModulID),
                //    Aciklama = item.ModulAdi
                //};
                //modulList.Add(q);
                result += item.ModulAdi + ":" + item.ModulID + ";";
            }
            //tbModul.DataSource = modulList;
            //tbModul.DataBind();
        }


        //e.Result = "test:1;test2:2;";
        //e.Result = string.Format("{0};{1};{2};{3}", 0, 0, "test:1,test2:2,", "Hata Mesajı");
        result = result.Substring(0, result.Length - 1);
        e.Result = string.Format("{0}", result);
    }



}
