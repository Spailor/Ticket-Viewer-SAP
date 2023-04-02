using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for Genel
/// </summary>
public class Genel
{
    public static string GetIPAddress()
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        return ipAddress.ToString();
    }
    public static string GetHostAdi()
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
        return ipHostInfo.HostName.ToString();
    }
    public static string GetClientIp()
    {
        var ipAddress = string.Empty;
        if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        { ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString(); }

        else if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null && System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].Length != 0) { ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"]; } else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0) { ipAddress = System.Web.HttpContext.Current.Request.UserHostName; }

        return ipAddress;
    }
    public static string GetirDisIP()
    {
        try
        {
            string DisIP;
            DisIP = (new System.Net.WebClient()).DownloadString("http://checkip.dyndns.org/");
            DisIP = (new System.Text.RegularExpressions.Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")).Matches(DisIP)[0].ToString();
            return DisIP;
        }
        catch (Exception)
        {
            return "";
        }

        //string q = "";
        //return q;
    }
}