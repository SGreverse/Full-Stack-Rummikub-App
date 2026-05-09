using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.GamePieces;
using RummiKubClasses.PlayingEntities;
using RummiKubClasses.StatusCodes;
using RummiKubClasses.Tiles;

namespace RummiKubClasses.GameClasses
{
    public class AddingTileAction : GameAction
    {
        public int NewSeriesId {  get; set; }
        public AddingTileAction()
        {

        }
        public override PostRequestStatusCode ExecuteAction(Game executed_game)
        {

            if(All_Tiles.Count != 1)
                return new PostRequestStatusCode(8);

            Tile tile = All_Tiles[0];
            Tile copy_of_tile = tile;

            if (tile is Joker)//avoid tile change to affect the original tile
            {
                copy_of_tile = new Joker();
                copy_of_tile.Value = tile.Value;
                copy_of_tile.Color = tile.Color;
            }


            if (!Player.Can_Change_Board())
                return new PostRequestStatusCode(3);

            executed_game.GameBoard.AddTile(NewSeriesId, tile);

            All_Tiles.RemoveAt(0);
            All_Tiles.Add(copy_of_tile);

            base.ChangeBoardUponActionExecution(executed_game);

            return new PostRequestStatusCode();
        }
    }
}
