using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RummiKubUI.ServiceReference1;

namespace RummiKubUI.UtilClasses
{
    public class FriendsListUtil : List<Friend>
    {
        public FriendsListUtil(PlayerList lst, string ButtonImage)
        {
            foreach (Player player in lst)
            {
                Friend friend = new Friend() { ButtonImage = ButtonImage, Username = player.Username, Level = player.Level, IconName =player.IconName,PlayerId=player.Id  };
                this.Add(friend);
            }
            
        }
            


    }
}
