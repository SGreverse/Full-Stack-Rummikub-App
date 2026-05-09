using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses;
using RummiKubClasses.GamePieces;
using RummiKubClasses.ObjectsForDataBase;
using RummiKubClasses.PlayingEntities;

namespace ViewModel
{
    public class AllPlayedGamesDB:BaseDB
    {
        public override BaseEntity NewEntity()
        {
            return new VisualDisplayOfGame();
        }
        public GameList SelectByUserId(int UserId)
        {
            command.CommandText = $"Select * From AllPlayedGamesTbl WHERE [First]={UserId} OR [Second]={UserId} OR Third={UserId} OR Forth={UserId} ORDER BY GameId";
            GameList lst = new GameList(Select());
            return lst; 
        }
        public int GetWinningGamesCount(int UserId)
        {
            command.CommandText = $"Select Count(*) From AllPlayedGamesTbl WHERE [First]={UserId}";
            int result = NumberSelection();
            return result;
        }
        public int GetGamesCount(int UserId)
        {
            command.CommandText = $"Select Count(*) From AllPlayedGamesTbl WHERE [First]={UserId} OR [Second]={UserId} OR Third={UserId} OR Forth={UserId}";
            int result = NumberSelection();
            return result;
        }
        public GameWinnerOrderList ConvertGameToGameOrderList(GameList gamelist)
        {
            GameWinnerOrderList lst = new GameWinnerOrderList();
            foreach(VisualDisplayOfGame game in gamelist)
            {
                lst.Add(new GameWinnerOrder(game.Id,game.visualDisplayOfPlayers));
            }
            return lst;

        }
        public override void Insert(BaseEntity entity)
        {
            VisualDisplayOfGame game = entity as VisualDisplayOfGame;
            if (game != null)
            {
                inserted.Add(new ChangeEntity(game, this.CreateInsertSQL));

            }
        }
        public override void Update(BaseEntity entity)
        {
            VisualDisplayOfGame game = entity as VisualDisplayOfGame;
            if (game != null)
            {
                updated.Add(new ChangeEntity(game,this.CreateUpdateSQL));
            }

        }
        public override void Delete(BaseEntity entity)//not any reason to implement
        {
        }
        public override BaseEntity CreateModel(BaseEntity entity)
        {
            LeaderBoardDB userDB = new LeaderBoardDB();
            VisualDisplayOfGame game= entity as VisualDisplayOfGame;
            List<VisualDisplayOfPlayer> WinnerOrderList= new List<VisualDisplayOfPlayer>();
            List<string> Places = new List<string>() { "First", "Second", "Third", "Forth" };
            game.Id = (int)reader["GameId"];
            int PlaceId;
            foreach(string place in Places)
            {
                PlaceId = (int)reader[place];
                if (PlaceId == 0)//if the game had less then 4 players
                    WinnerOrderList.Add(null);
                else
                {
                    Player player = userDB.SelectById(PlaceId);
                    WinnerOrderList.Add(new VisualDisplayOfPlayer() { IconName=player.IconName,Id=player.Id,Username=player.Username});
                }
            }
            game.visualDisplayOfPlayers = WinnerOrderList;
            return game;

        }
        protected override void CreateDeleteSQL(BaseEntity entity, OleDbCommand command)//not any reason to implement
        {
        }
        protected override void CreateInsertSQL(BaseEntity entity, OleDbCommand command)
        {
            VisualDisplayOfGame game = entity as VisualDisplayOfGame;
            string sqlstr = $"Insert INTO AllPlayedGamesTbl([First],[Second],Third,Forth) VALUES(0,0,0,0)";
            command.CommandText = sqlstr;

        }
        protected override void CreateUpdateSQL(BaseEntity entity, OleDbCommand command)
        {
            int[] winner_arr = new int[4];
            int index = 0;
            VisualDisplayOfGame game = entity as VisualDisplayOfGame;
            foreach(VisualDisplayOfPlayer u in game.visualDisplayOfPlayers)
            {
                winner_arr[index] = u.Id;
                index++;
            }
            string sqlstr = $"Update AllPlayedGamesTbl SET [First]={winner_arr[0]}, [Second]={winner_arr[1]}, Third={winner_arr[2]}, Forth={winner_arr[3]} WHERE GameId={game.Id}";
            command.CommandText = sqlstr;

        }

    }
}
