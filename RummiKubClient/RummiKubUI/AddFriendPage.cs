using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using RummiKubUI.ServiceReference1;
using RummiKubUI.UserControls;
using RummiKubUI.UtilClasses;

namespace RummiKubUI
{
    public class AddFriendPage:FriendPage
    {


        public AddFriendPage() : base()
        {
            title.Text = "Add Friends";
        }

        public override void ButtonFunctionality(Button btn)
        {
            Friend user = btn.DataContext as Friend;
            RummiKubServiceClient service = new RummiKubServiceClient();
            int index=Base_List.FindIndex(_=>user.PlayerId==_.Id);
            if (index != -1)
            {
                Base_List.RemoveAt(index);
            }
            service.FriendRequest(MainWindow.CurrentUser.Id,user.PlayerId);

            ForceRefresh();

        }

        public override FriendsListUtil GetList()
        {
            PlayerList lst=new PlayerList();
            if (Base_List == null)
            {
                RummiKubServiceClient service = new RummiKubServiceClient();
                 Base_List = service.GetAllNonFriends(MainWindow.CurrentUser.Id,"").ReturnedObject;
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
            FriendsListUtil friendsListUtil = new FriendsListUtil(lst,"/Assets/add-user.png");
            return friendsListUtil;

        }
    }
}
