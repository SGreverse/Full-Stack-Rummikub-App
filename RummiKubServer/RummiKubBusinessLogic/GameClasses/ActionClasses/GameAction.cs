using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.GamePieces;
using RummiKubClasses.PlayingEntities;
using RummiKubClasses.StatusCodes;
using RummiKubClasses.Tiles;

namespace RummiKubClasses.GameClasses
{
    [KnownType(typeof(InsertingSeriesAction))]
    [KnownType(typeof(AddingTileAction))]
    
    public abstract class GameAction
    {
        public PlayerInGame Player { get; set; }

        public List<Tile> All_Tiles { get; set; }

        public List<Tile> tiles_from_player { get; set; }

        public List<Tuple<Tile, int>> tiles_from_board { get; set; }

        public abstract PostRequestStatusCode ExecuteAction(Game executed_game);

        public void ChangeBoardUponActionExecution(Game executed_game)
        {

            Player.Put_On_Board(tiles_from_player);

            foreach (Tuple<Tile, int> tile_and_series_tup in tiles_from_board)
            {
                RemoveTile(executed_game,tile_and_series_tup.Item1, tile_and_series_tup.Item2);
            }
            AlertPlayersAboutBoardChange(executed_game);

        }
        public void AlertPlayersAboutBoardChange(Game executed_game)
        {

            foreach (PlayerInGame p in executed_game.PlayersInGame.Values)
            {
                p.APlayerChangedTheBoard();
            }

        }
        public void RemoveTile(Game executed_Game,Tile tile, int SeriesId )
        {
            Board board = executed_Game.GameBoard;
            board.RemoveTile(SeriesId, tile);
        }
    }
}
