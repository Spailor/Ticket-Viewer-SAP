using DevExpress.Web;
using System;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using MenuItem = DevExpress.Web.MenuItem;
using System.Web.Security;
using System.Linq;

public partial class HeaderMenu : UserControl {

    protected void Page_Load(object sender, EventArgs e) {

        AktiviteEntities db = new AktiviteEntities();

        //MembershipUser user = Membership.GetUser();

        //string kullaniciAdi = user.UserName;

        //string[] usersInRole;
        //usersInRole = Roles.GetRolesForUser(kullaniciAdi);

        if (Roles.IsUserInRole("KaynakYoneticisi"))
        {
            var ayarlar = db.S_Destec_AyarlarFirma((Guid)Membership.GetUser().ProviderUserKey).ToList();
            if (Convert.ToBoolean(ayarlar.FirstOrDefault().KaynakYoneticisiSistemi))
                siteMapDataSource.SiteMapFileName = "~/Web.sitemap"; //kaynak yöneticisi sistemi çalışır durumda.
            else
                siteMapDataSource.SiteMapFileName = "~/Web3.sitemap"; //kaynak yöneticisi ayarı pasif durumda.
        }
        else if (Roles.IsUserInRole("StandartKullanici"))
        {
            siteMapDataSource.SiteMapFileName = "~/Web2.sitemap";
        }

        //if (Roles.IsUserInRole(kullaniciAdi, "KaynakYoneticisi"))
        //{
        //    siteMapDataSource.SiteMapFileName = "~/Web.sitemap";
        //}
        //else if (Roles.IsUserInRole(kullaniciAdi, "StandartKullanici"))
        //{
        //    siteMapDataSource.SiteMapFileName = "~/Web2.sitemap";
        //}

        mainMenu.DataBind();

        if(mainMenu.SelectedItem != null && mainMenu.SelectedItem.Parent != mainMenu.RootItem)
            mainMenu.SelectedItem.Parent.Text = string.Format("{0}: {1}", mainMenu.SelectedItem.Parent, mainMenu.SelectedItem.Text);

        //MenuItem userInfo = mainMenu.Items.Add("Hoşgeldin, ", "userInfo");
        //userInfo.ItemStyle.CssClass = "dx-vam";

        //MenuItem cikis = mainMenu.Items.Add("Ç", "cikis");
        //cikis.ItemStyle.CssClass = "helpMenuItem";

        MenuItem helpMenuItem = mainMenu.Items.Add("i", "helpMenuItem");
        helpMenuItem.ItemStyle.CssClass = "helpMenuItem";


        


    }

    
}
