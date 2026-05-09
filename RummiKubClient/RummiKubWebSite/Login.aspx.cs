using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceReference2;
public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void LoginButton_Click(object sender, EventArgs e)
    {
        string username = Username.Value; 
        string password = Password.Value; 

        password=RSASecurity.Encrypt(password);

        RummiKubServiceClient rummiKubServiceClient = new RummiKubServiceClient();

        var status=rummiKubServiceClient.Login(username, password);
        if (status.Code>0)
        {
            ErrorMessage.Visible = true;
        }
        else
        {
            ErrorMessage.Visible = false;
            Session["User"] = status.ReturnedObject;
            Response.Redirect("AllGames.aspx");  
        }
    }
}