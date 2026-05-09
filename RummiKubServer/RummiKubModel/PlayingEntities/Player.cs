using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.ObjectsForDataBase;

namespace RummiKubClasses.PlayingEntities
{
    [KnownType(typeof(PlayerInGame))]
    public class Player:User
    {
        public int GamesWon { get; set; }

        public int GamesPlayed { get; set; }
        public int Level { get; set; }

        public int Score { get; set; }


        public int ExpierincePoints { get; set; }

        public Player()
        {

        }
        public Player(Player player):base(player.Id,player.Username, player.Password,player.FirstName,player.LastName,player.Email,player.User_City,player.IconName,player.IsAdmin)
        {
            this.GamesWon = player.GamesWon;
            this.GamesPlayed = player.GamesPlayed;
            this.Level = player.Level;
            this.Score = player.Score;
            this.ExpierincePoints = player.ExpierincePoints;

        }

        public static int CalculateLevel(long exp)
        {
            if (exp < 0) return 1;

            int level = 1;
            long requiredExp = 100;

            while (exp >= requiredExp)
            {
                exp -= requiredExp;
                level++;
                requiredExp = (long)(100 * Math.Pow(1.1, level - 1));
            }

            return level;
        }

    }
}
