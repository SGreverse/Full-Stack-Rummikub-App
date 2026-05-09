using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceReference2;

public partial class AllUserRequests : System.Web.UI.Page
{
    UserProblemList userProblems;
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Session["User"] as Player) == null || !(Session["User"] as Player).IsAdmin)
        {
            Response.Redirect("Login.aspx");
        }
        RummiKubServiceClient service=new RummiKubServiceClient();
        UserProblemList list = service.GetAllUserProblems().ReturnedObject;
        userProblems = list;
        PlayersGridView.DataSource =userProblems;
        PlayersGridView.DataBind();
    }
    protected void PlayerAction_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Int32.Parse(e.CommandArgument.ToString());
        if (e.CommandName == "LookIntoProblem")
        {
            Session["ViewedProblem"] = userProblems[index];
            Response.Redirect("HandleUserRequest.aspx");
        }

    }
}