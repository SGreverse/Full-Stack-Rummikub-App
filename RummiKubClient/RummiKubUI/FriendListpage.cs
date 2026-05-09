using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using RummiKubUI.ServiceReference1;
using RummiKubUI.UserControls;
using RummiKubUI.UtilClasses;

namespace RummiKubUI
{
    public class FriendListpage: FriendPage
    {
        public FriendListpage() : base()
        {
            title.Text = "Friend List";
        }

        public override void ButtonFunctionality(Button btn)
        {
            Friend user = btn.DataContext as Friend;
            MainWindow.This_current_window.GameFrame.Navigate(new PrivateChatPage(user));
        }

        public override FriendsListUtil GetList()
        {
            PlayerList lst=new PlayerList();
            if (Base_List == null)
            {
                RummiKubServiceClient service = new RummiKubServiceClient();
                 Base_List = service.GetFriends(MainWindow.CurrentUser.Id,"").ReturnedObject;
                lst = Base_List;
            }
            else
            {
                foreach (var item in Base_List)
                {
                    if(item.Username.StartsWith(UsernameFilter.Text)) 
                        lst.Add(item);
                }
            }
            FriendsListUtil friendsListUtil = new FriendsListUtil(lst, "/Assets/Message.png");
            return friendsListUtil;

        }
    }
}
