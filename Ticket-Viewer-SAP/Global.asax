<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e) {
        DevExpress.Web.ASPxWebControl.CallbackError += new EventHandler(CallbackError);
    }

    void CallbackError(object sender, EventArgs e) {
        // Logging exceptions occur on callback events of DevExpress ASP.NET controls. 
        // To learn more, see http://www.devexpress.com/Support/Center/Example/Details/E2398      
    }
</script>
