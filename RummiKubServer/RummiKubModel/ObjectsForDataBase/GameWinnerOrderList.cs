using RummiKubClasses.GamePieces;
using RummiKubClasses.PlayingEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses.ObjectsForDataBase
{
    [CollectionDataContract]
    public class GameWinnerOrderList:List<GameWinnerOrder>
    {
        public GameWinnerOrderList()
        { }
        public GameWinnerOrderList(IEnumerable<GameWinnerOrder> list) : base(list)
        { }
        public GameWinnerOrderList(IEnumerable<BaseEntity> list) : base(list.Cast<GameWinnerOrder>().ToList()) { }
    }
}
