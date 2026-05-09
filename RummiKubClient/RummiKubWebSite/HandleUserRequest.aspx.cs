using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceReference2;

public partial class HandleUserRequest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Session["User"] as Player) == null || !(Session["User"] as Player).IsAdmin)
        {
            Response.Redirect("Login.aspx");
        }
    }
}