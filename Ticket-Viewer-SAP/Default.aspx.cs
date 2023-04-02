using System;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Internal;
using System.Web.Security;

public partial class _Default : BasePage {
    protected override bool IsPopulateDatabasePage { get { return true; } }

    protected void Page_Load(object sender, EventArgs e) {
        //// The settings are defined in the Page_Load event to override ASPxProgressBar skin's settings specified in the MetropolisBlue theme
        //Progress.ShowPosition = true;
        //Progress.Height = Unit.Pixel(32);
        if(!IsPostBack)
        {
            Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
            //Genel.KullaniciGUID = userId;
            //Genel.KullaniciIDs = Membership.GetUser().ProviderUserKey.ToString();

            
        }

    }

    protected void Callback_Callback(object source, CallbackEventArgs e) {
        e.Result = HtmlConvertor.ToJSON(GetCallbackResult(e.Parameter));
    }

    object GetCallbackResult(string parameter) {
        if(parameter == "create") {
            if(!IsDatabasePopulating) {
                IsDatabasePopulating = true;
                try {
                    DataContext.SalesDataContext.PopulateDatabaseIfNecessary();
                    IsDatabasePopulated = true;
                } catch(Exception e) {
                    return e.Message;
                } finally {
                    IsDatabasePopulating = false;
                }
                return true;
            }
            else
                return false;
        } else if(parameter == "progress") {
            return !IsDatabasePopulated ? DataContext.SalesDataContext.DatabasePopulatingProgressPercentValue : -1;
        }
        throw new ArgumentException("Wrong parameter");
    }

}
