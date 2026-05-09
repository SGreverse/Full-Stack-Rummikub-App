using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceReference2;

public partial class PlayerTableEdit : System.Web.UI.Page
{
    private Player ViewedPlayer
    {
        get
        {
            if (Session["ViewedPlayer"] != null)
                return Session["ViewedPlayer"] as Player;
            return null;
        }
        set { Session["ViewedPlayer"] = value; }
    }

    private bool IsEditMode
    {
        get { return ViewedPlayer != null; }
    }
    private CityList AllCitiesList
    {
        get
        {
            if (ViewState["AllCities"] != null)
                return ViewState["AllCities"] as CityList;
            return null;
        }
        set { ViewState["AllCities"]= value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        // Check admin authentication
        //if (Session["User"] == null || (Session["User"] as Player) == null || !(Session["User"] as Player).IsAdmin)
        //{
        //    Response.Redirect("Login.aspx");
        //}

        if (!IsPostBack)
        {


            LoadCities();

            if (IsEditMode)
            {
                LoadPlayerData();
                PageTitleLiteral.Text = "Edit Player";
                DeleteButton.Visible = true;
            }
            else
            {
                PageTitleLiteral.Text = "Add Player";
                // Set default values for new player
                ExperiencePointsTextBox.Text = "0";
                ScoreTextBox.Text = "0";
            }
        }
    }

    private void LoadCities()
    {
        try
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            // Assuming you have a method to get all cities
            // Replace this with your actual service call
            var citiesResponse = service.GetAllCities(); // You'll need to implement this
            CityList selectedcities=new CityList();
            if (citiesResponse.Code == 0)
            {
                for(int i = 0; i < 50; i++)
                {
                    selectedcities.Add(citiesResponse.ReturnedObject[i]);
                }
                ViewState["AllCities"]=citiesResponse.ReturnedObject;
                CityDropDownList.DataSource = selectedcities;
                CityDropDownList.DataTextField = "CityName"; // Adjust based on your city object structure
                CityDropDownList.DataValueField = "Id";   // Adjust based on your city object structure
                CityDropDownList.DataBind();
            }
            else
            {
                ShowMessage("Error loading Cities data",false);
            }
        }
        catch(Exception ex)
        {
            ShowMessage("Error loading Cities data: "+ex.Message, false);

        }

        CityDropDownList.Items.Insert(0, new ListItem("Select a city...", ""));
    }


    private void LoadPlayerData()
    {
        try
        {

            Player player = ViewedPlayer;
            UsernameTextBox.Text = player.Username ?? "";
            FirstNameTextBox.Text = player.FirstName ?? "";
            LastNameTextBox.Text = player.LastName ?? "";
            EmailTextBox.Text = player.Email ?? "";
            IconNameTextBox.Text = player.IconName ?? "";
            ExperiencePointsTextBox.Text = player.ExpierincePoints.ToString();
            ScoreTextBox.Text = player.Score.ToString();
            IsAdminCheckBox.Checked = player.IsAdmin;

            ListItem cityItem = CityDropDownList.Items.FindByText(player.User_City.CityName);
            if (cityItem != null)
                cityItem.Selected = true;


        }
        catch (Exception ex)
        {
            ShowMessage("Error loading player data: " + ex.Message, false);
        }
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                RummiKubServiceClient service = new RummiKubServiceClient();
                int Response_Code;
                int id=0;
                string password = "Man Encrypt this bruh";
                string iconName = "ManIcon.png";
                if (IsEditMode)
                {
                    id=ViewedPlayer.Id;
                    password = ViewedPlayer.Password;
                    iconName = ViewedPlayer.IconName;
                }
                Player player = new Player()
                {
                    Id =id ,
                    Password =RSASecurity.Encrypt( password),
                    Username = UsernameTextBox.Text.Trim(),
                    FirstName = FirstNameTextBox.Text.Trim(),
                    LastName = LastNameTextBox.Text.Trim(),
                    Email = EmailTextBox.Text.Trim(),
                    User_City = AllCitiesList[CityDropDownList.SelectedIndex],
                    IconName = iconName,
                    ExpierincePoints = string.IsNullOrEmpty(ExperiencePointsTextBox.Text) ? 0 : int.Parse(ExperiencePointsTextBox.Text),
                    Score = string.IsNullOrEmpty(ScoreTextBox.Text) ? 0 : int.Parse(ScoreTextBox.Text),
                    IsAdmin = IsAdminCheckBox.Checked
                };


                if (IsEditMode)
                {
                    // Update existing player
                    Response_Code= service.UpdateUser(player).Code;
                }
                else
                {
                    // Create new player
                    Response_Code = service.Register(player).Code;
                }

                if (Response_Code == 0)
                {
                    ShowMessage(IsEditMode ? "Player updated successfully!" : "Player created successfully!", true);

                    // Redirect back to admin players page after a short delay
                    ScriptManager.RegisterStartupScript(this, GetType(), "redirect",
                        "setTimeout(function(){ window.location.href='AdminAllPlayers.aspx'; }, 2000);", true);
                }
                else
                {
                    ShowMessage("Error saving player", false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error saving player: " + ex.Message, false);
            }
        }
    }

    protected void DeleteButton_Click(object sender, EventArgs e)
    {
        if (IsEditMode)
        {
            try
            {
                RummiKubServiceClient service = new RummiKubServiceClient();
                var response = service.DeletePlayer(ViewedPlayer.Id);

                if (response.Code == 0)
                {
                    ShowMessage("Player deleted successfully!", true);
                    Thread.Sleep(2000); 
                    Response.Redirect("AdminAllPlayers.aspx");
                }
                else
                {
                    ShowMessage("Error deleting player: " + response.Message, false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting player: " + ex.Message, false);
            }
        }
    }

    protected void CancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdminAllPlayers.aspx");
    }

    private void ShowMessage(string message, bool isSuccess)
    {
        MessageLiteral.Text = message;
        MessagePanel.Visible = true;

        if (isSuccess)
        {
            alertDiv.Attributes["class"] = "alert alert-success";
        }
        else
        {
            alertDiv.Attributes["class"] = "alert alert-error";
        }
    }
}