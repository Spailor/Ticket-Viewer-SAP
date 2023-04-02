using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hostAdi.InnerText = "Server Name " + Genel.GetHostAdi();
            ipAdresiServer.InnerText = "Server IP " + Genel.GetirDisIP();
            ipAdresi.InnerText = "Client IP " + Genel.GetClientIp();
            versiyonNumarasi.InnerText = "Version 1.0.18.6"; //+ System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (Membership.ValidateUser(tbUserName.Text, tbPassword.Text))
        {
            if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
            {
                FormsAuthentication.SetAuthCookie(tbUserName.Text, false);
                Response.Redirect("~/");
            }
            else
                FormsAuthentication.RedirectFromLoginPage(tbUserName.Text, false);

            //tarayıcı kontrolü yapıyorum.
            bool uygunBrowser = false;
            System.Web.HttpBrowserCapabilities browser = Request.Browser;
            string name = browser.Browser;
            float version = (float)(browser.MajorVersion + browser.MinorVersion);
            if (name == "IE" || name == "InternetExplorer" && version >= 11)
                uygunBrowser = true;
            else if (name == "Chrome" && version >= 51) //51 aslında microsoft edge olarak çalışıyor. 56 chrome olarak. daha sonra edge için araştırılacak. şimdilik ie 11 altında da çalışmasın yeter.
                uygunBrowser = true;
            //else if (name == "Firefox" && version >= 52)
            //    uygunBrowser = true;
            else
                uygunBrowser = false;

            if (!uygunBrowser)
                Response.Redirect("../BrowserError.aspx");
        }
        else
        {
            tbUserName.ErrorText = "Hata";
            tbUserName.IsValid = false;
        }
    }

}