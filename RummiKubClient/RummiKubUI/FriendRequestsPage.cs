using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using RummiKubUI.ServiceReference1;
using RummiKubUI.UserControls;
using RummiKubUI.UtilClasses;

namespace RummiKubUI
{
    public class FriendRequestsPage:FriendPage
    {
        public FriendRequestsPage() : base()
        {
            title.Text = "Friend Requests";
        }

        public override void ButtonFunctionality(Button btn)
        {
            Friend friend = btn.DataContext as Friend;
            RummiKubServiceClient service = new RummiKubServiceClient();
            service.ApproveFriend(MainWindow.CurrentUser.Id, friend.PlayerId);
            ForceRefresh();
        }

        public override FriendsListUtil GetList()
        {
            PlayerList lst= new PlayerList();
            if (Base_List == null )
            {
                RummiKubServiceClient service = new RummiKubServiceClient();
                Base_List = service.GetAllRequests(MainWindow.CurrentUser.Id, "").ReturnedObject;
                lst = Base_List;
            }
            else
            {
                foreach (var item in Base_List)
                {
                    if (item.Username.StartsWith(UsernameFilter.Text))
                        lst.Add(item);
                }
            }
            FriendsListUtil friendsListUtil = new FriendsListUtil(lst, "/Assets/Approve.jpg");
            return friendsListUtil;
        }
    }
}
