using ServiceReference2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserConverter
/// </summary>
public static class UserConverter
{
    public static string[] ConvertToUsername(GameWinnerOrder users)
    {
        string[] result= new string[4];
        for(int i = 0; i < 4; i++)
        {
            if (i >= users.WinnerOrderList.Count() || users.WinnerOrderList[i]==null)
                result[i] = "None";
            else
            {
                result[i]=users.WinnerOrderList[i].Username;
            }
        }
        return result;
    }
}