using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceReference2;
public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Session["User"] as Player) == null)
        {
            Response.Redirect("Login.aspx");
        }
        if (!IsPostBack) // only load data on the initial page load
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            GameWinnerOrderList lst = service.GetAllGamesByUser((Session["User"] as Player).Id).ReturnedObject;

            
            
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("First");
            dataTable.Columns.Add("Second");
            dataTable.Columns.Add("Third");
            dataTable.Columns.Add("Forth");
            dataTable.Columns.Add("GameId"); 

            foreach (GameWinnerOrder gameWinnerOrder in lst)
            {
                string[] usernames = UserConverter.ConvertToUsername(gameWinnerOrder);

                dataTable.Rows.Add(usernames[0], usernames[1], usernames[2], usernames[3], gameWinnerOrder.GameId);
            }

            GameDataList.DataSource = dataTable;
            GameDataList.DataBind();
        }
    }
}