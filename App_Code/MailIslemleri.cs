using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;

/// <summary>
/// Summary description for MailIslemleri
/// </summary>
public class MailIslemleri
{
    public static bool MailGonder(string MailTo, MailSubject Konu, string MailFrom, string Html, string PromotionName, int FirmaId, out string Mesaj)
    {
        Mesaj = "";
        string subject = "";

        switch (Konu)
        {
            case MailSubject.YeniKayit:
                subject = "Mutabakat sistemine hoşgeldiniz";
                break;
            case MailSubject.SifreTalep:
                subject = "Mutabakat yeni şifre talebi";
                break;
            case MailSubject.DenemekIstiyorum:
                subject = "Mutabakat denemek isteyen yeni bir kullanici talebin var !";
                break;
            default:
                break;
        }
        //markaIslemleri = new MarkaIslemleri();
        //Marka marka = markaIslemleri.SelectMarkaByFirmaId(FirmaId);
        //MailFrom = marka.MailFrom;

        ////mailParametreIslemleri = new MailParametreIslemleri();
        //MailParametre mailParametre = mailParametreIslemleri.SelectMailParametreByAktif(true, FirmaId);
        string userName = "cenk.yenikoylu@tecs.com.tr";// mailParametre.Usermail;
        string apiKey = "3038d5857b9141c7a65a161826fe991d";// Crypto.Decrypt(mailParametre.Userkey);

        RestClient client = new RestClient();
        client.BaseUrl = new Uri("https://api.madmimi.com/mailer");

        RestRequest request = new RestRequest();

        request.AddParameter("username", userName);
        request.AddParameter("api_key", apiKey);
        request.AddParameter("promotion_name", PromotionName);
        request.AddParameter("recipient", MailTo);
        request.AddParameter("subject", subject);
        request.AddParameter("from", MailFrom);
        request.AddParameter("raw_html", Html);
        request.Method = Method.POST;
        var response = client.Execute(request);

        if (response != null)
        {
            Mesaj = response.Content;

            if (response.StatusCode.ToString() == "OK")
                return true;
            else
                return false;
        }
        else
        {
            Mesaj = "Cevap gelmedi.";
            return false;
        }

    }

    public static bool MailGonder(string MailTo, string Konu, string MailFrom, string Html, string PromotionName, int FirmaId, out string Mesaj)
    {
        Mesaj = "";
        //MailParametre mailParametre = mailParametreIslemleri.SelectMailParametreByAktif(true, FirmaId);
        string userName = "cenk.yenikoylu@tecs.com.tr";// mailParametre.Usermail;
        string apiKey = "3038d5857b9141c7a65a161826fe991d";// Crypto.Decrypt(mailParametre.Userkey);

        RestClient client = new RestClient();
        client.BaseUrl = new Uri("https://api.madmimi.com/mailer");

        RestRequest request = new RestRequest();

        request.AddParameter("username", userName);
        request.AddParameter("api_key", apiKey);
        request.AddParameter("promotion_name", PromotionName);
        request.AddParameter("recipient", MailTo);
        request.AddParameter("subject", Konu);
        request.AddParameter("from", MailFrom);
        request.AddParameter("raw_html", Html);
        request.Method = Method.POST;
        var response = client.Execute(request);

        if (response != null)
        {
            Mesaj = response.Content;

            if (response.StatusCode.ToString() == "OK")
                return true;
            else
                return false;
        }
        else
        {
            Mesaj = "Cevap gelmedi.";
            return false;
        }

    }

    public static string Sorgula(string TransactionId, int FirmaId)
    {
        string Sonuc = "";

        //MailParametre mailParametre = mailParametreIslemleri.SelectMailParametreByAktif(true, FirmaId);
        string userName = "cenk.yenikoylu@tecs.com.tr";// mailParametre.Usermail;
        string apiKey = "3038d5857b9141c7a65a161826fe991d";// Crypto.Decrypt(mailParametre.Userkey);

        RestClient client = new RestClient();
        client.BaseUrl = new Uri("https://madmimi.com/mailers/status/" + TransactionId);

        RestRequest request = new RestRequest();
        request.AddParameter("username", userName);
        request.AddParameter("api_key", apiKey);

        request.Method = Method.GET;
        var response = client.Execute(request);

        if (response != null)
        {
            Sonuc = response.Content;
        }

        return Sonuc;

    }

    public enum MailSubject
    {
        YeniKayit,
        SifreTalep,
        SifreHatirlatma,
        DenemekIstiyorum,
        MutabakatGonderim
    }
}