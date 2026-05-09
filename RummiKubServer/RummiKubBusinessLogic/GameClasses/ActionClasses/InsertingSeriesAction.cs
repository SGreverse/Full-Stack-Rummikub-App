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
    public class InsertingSeriesAction : GameAction
    {
        public InsertingSeriesAction()
        {
            
        }
        public override PostRequestStatusCode ExecuteAction(Game executed_game)
        {

            if (!Player.Can_Change_Board() && tiles_from_board.Count > 0)

                    return new PostRequestStatusCode(3);
                try
            {
                RisingSeries risingSeries = new RisingSeries(All_Tiles);

                executed_game.GameBoard.AddSeries(risingSeries);

                base.ChangeBoardUponActionExecution(executed_game);


                return new PostRequestStatusCode();
            }
            catch { }
            try
            {
                ColorSeries colorseries = new ColorSeries(All_Tiles);

                executed_game.GameBoard.AddSeries(colorseries);

                base.ChangeBoardUponActionExecution(executed_game);


                return new PostRequestStatusCode();
            }
            catch
            {
                return new PostRequestStatusCode(2);
            }
        }
    }
}
