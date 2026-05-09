using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using RummiKubClasses.ObjectsForDataBase;
using RummiKubClasses.PlayingEntities;
using BCrypt.Net;
using RummiKubClasses;
namespace ViewModel
{
    public class LeaderBoardDB:UserDB
    {
        private string basesql = " *,UserTbl.Id as Id FROM (UserTbl INNER JOIN LeaderBoardTbl ON UserTbl.Id=LeaderBoardTbl.Id)";
        public PlayerList SelectLeaderBoard()
        {
            command.CommandText = "SELECT TOP 10 LeaderBoardTbl.Id, LeaderBoardTbl.Score, LeaderBoardTbl.ExpieriencePoints, UserTbl.Id AS Expr1, UserTbl.FirstName, UserTbl.[Password], UserTbl.Username, UserTbl.LastName, UserTbl.Email, UserTbl.City,   UserTbl.IconName, UserTbl.IsDeleted,UserTbl.IsAdmin, UserTbl.Id AS Expr2" +
                " FROM (LeaderBoardTbl INNER JOIN UserTbl ON LeaderBoardTbl.Id = UserTbl.Id) " +
                "WHERE (LeaderBoardTbl.Score > 0) " +
                "ORDER BY LeaderBoardTbl.Score DESC ";
            PlayerList best_users = new PlayerList(Select());
            return best_users;
        }
        public PlayerList SelectAll()
        {
            
            command.CommandText = "SELECT" + basesql+" WHERE IsDeleted=False";
            PlayerList all_users = new PlayerList(Select());
            return all_users;
        }
        public PlayerList SelectFriends(int playerid,string useranmesearchfilter)
        {
            command.CommandText = "SELECT *,UserTbl.Id as Id, LeaderBoardTbl.*" +
                "FROM (UserTbl INNER JOIN LeaderBoardTbl ON UserTbl.Id = LeaderBoardTbl.Id)" +
                $"WHERE (UserTbl.Id IN (SELECT Approver FROM FriendTbl WHERE Requester = {playerid} AND Approved=True) " +
                $" OR UserTbl.Id IN (SELECT Requester FROM FriendTbl WHERE Approver = { playerid} AND Approved=True))";
            if (useranmesearchfilter.Length > 0)
            {
                command.CommandText += $" AND Username LIKE '{useranmesearchfilter}%'";
            }
            PlayerList friends= new PlayerList(Select());
            return friends;
        }
        public PlayerList SelectFriendRequests(int playerid,string useranmesearchfilter)
        {
            command.CommandText = "SELECT *,UserTbl.Id as Id, LeaderBoardTbl.*" +
                " FROM (UserTbl INNER JOIN LeaderBoardTbl ON UserTbl.Id = LeaderBoardTbl.Id)" +
                $" WHERE (UserTbl.Id IN (SELECT Requester FROM FriendTbl WHERE Approver = {playerid} AND Approved=False)) ";
            if (useranmesearchfilter.Length > 0)
            {
                command.CommandText += $" AND Username LIKE '{useranmesearchfilter}%'";
            }
            PlayerList friends = new PlayerList(Select());
            return friends;
        }
        public PlayerList SelectNonFriends(int playerid,string useranmesearchfilter)
        {
            command.CommandText = "SELECT *,UserTbl.Id as Id, LeaderBoardTbl.*" +
              "FROM (UserTbl INNER JOIN LeaderBoardTbl ON UserTbl.Id = LeaderBoardTbl.Id)" +
              $"WHERE (UserTbl.Id NOT IN (SELECT Approver FROM FriendTbl WHERE Requester = {playerid}) " +
              $" AND UserTbl.Id NOT IN (SELECT Requester FROM FriendTbl WHERE Approver = {playerid})) " +
              $"AND NOT UserTbl.Id={playerid} " +
              $"AND  IsDeleted=false";



            if (useranmesearchfilter.Length > 0)
            {
                command.CommandText += $" AND Username LIKE '{useranmesearchfilter}%'";
            }
            PlayerList friends = new PlayerList(Select());
            return friends;
        }
        public Player SelectById(int id)
        {
            command.CommandText ="SELECT"+ basesql + $" WHERE UserTbl.Id={id}";
            PlayerList users = new PlayerList(Select());
            if (users.Count > 0)
                return users[0];
            return null;
        }
        public Player SelectByLoginUserData(string username)//return user if he is in the database
        {

            command.CommandText ="SELECT"+ basesql + " WHERE Username=@Username AND IsDeleted=False";
            OleDbParameter p1 = new OleDbParameter("@Username", username);
            command.Parameters.Add(p1);

            PlayerList users = new PlayerList(Select());
            if (users.Count == 0)
                return null;

            return users[0];
        }
        public override BaseEntity CreateModel(BaseEntity entity)
        {
            Player user = entity as Player;
            base.CreateModel(user);
            user.Score = (int)reader["Score"];
            user.ExpierincePoints = (int)reader["ExpieriencePoints"];
            user.Level = Player.CalculateLevel(user.ExpierincePoints);
            AllPlayedGamesDB allPlayedGamesDB = new AllPlayedGamesDB();
            user.GamesWon = allPlayedGamesDB.GetWinningGamesCount(user.Id);
            user.GamesPlayed = allPlayedGamesDB.GetGamesCount(user.Id);
            return user;
        }
        public override BaseEntity NewEntity()
        {
            return new Player();
        }
        //score and id are automatically asigned so i didnt care about injections
        protected override void CreateInsertSQL(BaseEntity entity, OleDbCommand command)
        {
            Player user = entity as Player;
            if (user != null)
            {
                string sqlStr = "Insert INTO LeaderBoardTbl(Id,Score,ExpieriencePoints) " + $"Values({user.Id},{user.Score},{user.ExpierincePoints})";
                command.CommandText = sqlStr;
            }
        }

        protected override void CreateDeleteSQL(BaseEntity entity, OleDbCommand command)
        {
            Player user = entity as Player;
            if (user != null)
            {
                string sqlStr = $"Delete From LeaderBoardTbl WHERE Id={user.Id}";
                command.CommandText = sqlStr;
            }
        }
        protected override void CreateUpdateSQL(BaseEntity entity, OleDbCommand command)
        {
            Player user = entity as Player;
            if (user != null)
            {

                string sqlStr = $"Update LeaderBoardTbl Set Score={user.Score},ExpieriencePoints={user.ExpierincePoints} WHERE Id={user.Id}";
                command.CommandText = sqlStr;
            }
        }
        public override void Insert(BaseEntity entity)
        {
            Player player = entity as Player;
            if (player != null)
            {
                inserted.Add(new ChangeEntity(player, base.CreateInsertSQL));
                inserted.Add(new ChangeEntity(player, this.CreateInsertSQL));

            }
        }
        public override void Update(BaseEntity entity)
        {
            Player player = entity as Player;
            if (player != null)
            {
                updated.Add(new ChangeEntity(player, base.CreateUpdateSQL));
                updated.Add(new ChangeEntity(player, this.CreateUpdateSQL));

            }

        }
        //missing deleting from the UserTbl(not sure if needed because you wont be able to delete from the leaderboard)
        public override void Delete(BaseEntity entity)
        {
            Player player = entity as Player;
            if (player != null)
            {

                updated.Add(new ChangeEntity(player, base.CreateDeleteSQL));
            }
        }
    }
}
