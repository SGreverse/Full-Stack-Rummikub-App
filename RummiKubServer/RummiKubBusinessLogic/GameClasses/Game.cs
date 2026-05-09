using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.ChatClasses;
using RummiKubClasses.PlayingEntities;
using RummiKubClasses.Tiles;
namespace RummiKubClasses.GamePieces
{
    //[DataContract]

    public class Game 
    {
        //[DataMember]
        public Dictionary<int, PlayerInGame> PlayersInGame;
        //[DataMember]
        public PlayerOrderList CurrentOneToMove { get; set; }

        public int Id { get; set; } // Unique identifier for the game, can be used to track the game in a database or a list

        //[DataMember]
        public TileBank GameBank { get; set; }
        //[DataMember]
        public Board GameBoard { get; set; }

        public GameStates GameState { get; set; }

        public GameType GameType { get; set; }
        public Board GameBoardCopy { get; set; }
        //[DataMember]
        public List<PlayerInGame> WinnerOrderList { get; set; }

        public Chat GameChat { get; set; }
        public Game() { }
        public Game(Dictionary<int, PlayerInGame> players,GameType gameType)
        {
            this.PlayersInGame = players;
            CurrentOneToMove = new PlayerOrderList(PlayersInGame.Select(_=>_.Value).ToList());
            GameBank = new TileBank();
            GameBoard = new Board();
            GameBoardCopy = new Board();
            WinnerOrderList = new List<PlayerInGame>();
            GameChat = new Chat(PlayersInGame.Values.ToList());
            GameState = GameStates.Running;
            GameType = gameType;
        }
        public void NextOne()// update the original board to the the current and go to the next player
        {
            CurrentOneToMove.Node.player.This_PlayerState = PlayerState.Waiting;
            CurrentOneToMove.ContinueToNextPlayer();
            CurrentOneToMove.Node.player.This_PlayerState = PlayerState.Playing;

        }
        public void PlayerDrawTile(int PlayerId)
        {
            if (GameBank.IsEmpty())
            {
                GameState = GameStates.Ended;
                return;
            }

            Tile t = GameBank.Draw();
            PlayerInGame player= GetPlayerById(PlayerId);

            player.Draw(t);
            player.UpdateCopy();

        }
        public void UpdateOriginalBoard()
        {
            this.GameBoardCopy = new Board(GameBoard);

        }
        public PlayerInGame GetPlayerById(int playerId)
        {
            return PlayersInGame[playerId];
        }
        public bool AddNewSeriesToBoard(TileSeries series)
        {
            try
            {
                this.GameBoard.AddSeries(series);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void ReturnTurnBack(int PlayerId)// if a player did too many moves and wants to reset, he can click on the button
        {
            this.GameBoard = new Board(GameBoardCopy);

            PlayerInGame player = GetPlayerById(PlayerId);

            player.ReturnChanges();
        }
        public void AddWinningPlayer(int PlayerId)
        {
            PlayerInGame player = GetPlayerById(PlayerId);
            player.This_PlayerState = PlayerState.Finished;
            this.WinnerOrderList.Add(player);
            CurrentOneToMove.PlayerFinished(player);
            CheckGameState();
        }
        public void PlayerSurrended(PlayerInGame p)
        {
            p.This_PlayerState = PlayerState.Surrender;
            CurrentOneToMove.PlayerFinished(p);
            CheckGameState();

        }
        private void CheckGameState()
        {
            if (CurrentOneToMove.Length == 1)
            {
                this.GameState = GameStates.Ended;
                this.WinnerOrderList.Add(CurrentOneToMove.GetCurrentPlayer());
            }
        }
        public void Updateplayers()
        {

            for (int i = 0; i < this.WinnerOrderList.Count; i++)// calculate only for players who finished their board.
            {
                PlayerInGame player = this.WinnerOrderList[i];
                int remainingTiles = WinnerOrderList[i].playerBoard.Amount;
                player.Score = Math.Max(0, player.Score + ScoreCalculationAlgorithm(i + 1, this.PlayersInGame.Count, remainingTiles));//Avoid Negative Score
                player.ExpierincePoints += ExperiencePointsGain(i + 1, this.PlayersInGame.Count, remainingTiles);//Cannot Be Negative So It Doesn't Matter
                
            }
            foreach(PlayerInGame player in PlayersInGame.Values)
            {
                if (!WinnerOrderList.Contains(player))//give penalty
                {
                    player.Score = Math.Max(0, player.Score - 500);
                    WinnerOrderList.Add(player);
                }
            }
        }
        public int ScoreCalculationAlgorithm(int Place, int PlayersCount, int remainingTileCount)
        {
            int base_score = 1000;
            double placeMultiplier;
            switch (Place)
            {
                case 1:
                    placeMultiplier = 1.0;
                    break;
                case 2:
                    placeMultiplier = 0.75;
                    break;
                case 3:
                    placeMultiplier = 0.5;
                    break;
                default:
                    placeMultiplier = 0.25;
                    break;
            }
            base_score = (int)(base_score * placeMultiplier * (1 + PlayersCount / 10));
            base_score -= remainingTileCount * 20;
            return base_score;

        }
        public int ExperiencePointsGain(int place, int PlayerCount, int remainingTileCount)
        {
            int baseXP = 100;

            double playerCountMultiplier = 1 + (PlayerCount - 2) * 0.1;

            // Place Multiplier:  Better place => more XP
            double placeMultiplier;
            switch (place)
            {
                case 1:
                    placeMultiplier = 1.0;
                    break;
                case 2:
                    placeMultiplier = 0.75;
                    break;
                case 3:
                    placeMultiplier = 0.5;
                    break;
                default:
                    placeMultiplier = 0.25;
                    break;
            }

            // Remaining Tile Count Penalty: More tiles left => less XP
            double tilePenalty = remainingTileCount * 0.05; // Each tile reduces XP by 5%
            tilePenalty = Math.Min(1, tilePenalty); // Cap the penalty at 100% 
            double tileMultiplier = 1 - tilePenalty; // Apply the penalty

            // Calculate Final XP
            double experience = baseXP * playerCountMultiplier * placeMultiplier * tileMultiplier;

            // Round to nearest integer
            return (int)Math.Round(experience);
        }

        public void AddMessageToChat(string Msg,int playerid)
        {
            PlayerInGame playerInGame=GetPlayerById(playerid);
            this.GameChat.AddMessage(new MessageAction(new Message(Msg,playerInGame.Username)));
        }
        public Message ReciveResentMessage(int playerid)
        {
            return this.GameChat.RecieveMessage(playerid).message;
        }
        public override bool Equals(object obj)
        {
            return obj is Game && (obj as Game).Id == Id;

        }


    }
}
