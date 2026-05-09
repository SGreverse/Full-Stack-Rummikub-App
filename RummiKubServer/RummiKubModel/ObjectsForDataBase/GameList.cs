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
    public class GameList:List<VisualDisplayOfGame>
    {
        public GameList()
        { }
        public GameList(IEnumerable<VisualDisplayOfGame> list) : base(list)
        { }
        public GameList(IEnumerable<BaseEntity> list) : base(list.Cast<VisualDisplayOfGame>().ToList()) { }
    }
}
