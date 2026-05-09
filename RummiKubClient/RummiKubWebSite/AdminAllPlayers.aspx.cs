using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceReference2;

public partial class AdminAllPlayers : System.Web.UI.Page
{
    private PlayerList AllPlayers
    {
        get { return ViewState["Players"] as PlayerList; }
        set { ViewState["Players"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Session["User"] as Player) == null || !(Session["User"] as Player).IsAdmin)
        {
            Response.Redirect("Login.aspx");
        }
        if (!IsPostBack)
        {
            LoadPlayers();
        }
    }

    private void LoadPlayers()
    {
        RummiKubServiceClient service = new RummiKubServiceClient();
        var status = service.GetAllUsers();
        if (status.Code == 0)
        {
            AllPlayers = status.ReturnedObject;
            PlayersGridView.DataSource = AllPlayers;
            PlayersGridView.DataBind();
        }
    }
    protected void PlayerAction_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Int32.Parse(e.CommandArgument.ToString());
        RummiKubServiceClient service = new RummiKubServiceClient();
        if (e.CommandName == "DeletePlayer")
        {
            service.DeletePlayer(AllPlayers[index].Id);
            LoadPlayers();
        }
        if (e.CommandName == "EditPlayer")
        {
            Session["ViewedPlayer"] = AllPlayers[index];
            Response.Redirect("PlayerTableEdit.aspx");
        }
    }
    protected void HeadToPlayerTableEdit(object sender, EventArgs e)
    {
        Session["ViewedPlayer"] = null;
        Response.Redirect("PlayerTableEdit.aspx");
    }
}