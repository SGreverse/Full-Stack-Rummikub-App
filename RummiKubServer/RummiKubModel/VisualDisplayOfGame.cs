using RummiKubClasses.GamePieces;
using RummiKubClasses.PlayingEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses
{
    public enum GameStates
    {
        Running,//game in the middle
        Ended//game ended
    }
    public enum PlayerState
    {
        Waiting,
        Playing,
        Finished,
        Surrender
    }
    public enum GameType
    {
        Practice,
        Private,
        Public
    }
    public class VisualDisplayOfGame : BaseEntity
    {
        public GameStates GameState {  get; set; }


        public List<VisualDisplayOfPlayer> visualDisplayOfPlayers {  get; set; }

        public int PlayerCount {  get; set; }

        public VisualDisplayOfGame()
        {
            
        }
        public VisualDisplayOfGame(GameStates gameStates, List<VisualDisplayOfPlayer> players)
        {
            this.GameState = gameStates;
            this.visualDisplayOfPlayers = players;
        }
    }
}
