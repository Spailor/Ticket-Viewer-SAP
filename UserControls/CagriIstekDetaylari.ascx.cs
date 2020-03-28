using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_CagriIstekDetaylari : System.Web.UI.UserControl
{
    public void Page_Load(int ID)
    {
        AktiviteEntities db = new AktiviteEntities();
        Guid UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
        var list = db.S_Destec_CagriIstek(ID,UserId).ToList();
        Aciklama.Text = list.First().IstekAciklama;
    }
}