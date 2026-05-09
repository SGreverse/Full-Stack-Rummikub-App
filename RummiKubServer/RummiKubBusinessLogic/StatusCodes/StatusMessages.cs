using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses.StatusCodes
{
    public static class StatusMessages
    {
        private static Dictionary<int, string> codeToMessageDict = new Dictionary<int, string>()
        {
            {0,"Ok" },
            {1,"Nothing Changed In The Game" },
            {2,"This Series Is Not Legal" },
            {3,"Cannot Make A Change To The Board Until The Tile Value Is 30" },
            {4,"The Board Is Not Legal" },
            {5,"Game Not Found" },
            {6,"The User Is Already In Game" },
            {7,"Invalid Amount For Adding Tile" },
            {8,"Invalid Added Tile" },
            {9,"Invalid UserName Or Password" },
            {10,"User Already Taken" },
            {11,"Registration Process Failed" },
            {12,"Failed To Invite Player" },
            {13,"Player Is Not Online" },
            {14,"User Connected From Another Device" },
            {15,"The Game Already Started,Cannot Cancel" },
            {16,"User Declined The Invite" },
            {17,"User Cancelled Their Invite" },
            {18,"Already invited This Player" },
            {19,"Only the Party Leader May Start The Game" },
            {20,"Player Count Too Low" },
            {21," Queue At Max Capacity" },
            {22,"One Of the Players Have Logged Off Or Is In A Game,Try Again " },
            {23,"One Of The Players Is In Game,Try Again" },
            {24,"Game Code Not Found" },
            {25,"Email Already Exist" },
            {26,"Failed To retrive Key" },
            {27,"Email Not Registered" },
            {28,"You Have Reached Your Requests limit,\ncheck your email for messages from the admins about your problems" }

        };
        public static string GetMessage(int code)
        {
            return codeToMessageDict[code];
        }
    }
}
