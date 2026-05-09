using RummiKubClasses.GamePieces;
using RummiKubClasses.PlayingEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses.ObjectsForDataBase
{
    public class GameWinnerOrder
    {
        public List<VisualDisplayOfPlayer> WinnerOrderList {  get; set; }
        public int GameId {  get; set; }
        public GameWinnerOrder()
        {
            
        }
        public GameWinnerOrder(int Id,List<VisualDisplayOfPlayer> WinnerOrder)
        {
            this.WinnerOrderList = WinnerOrder;
            this.GameId = Id;
        }
    }
}
