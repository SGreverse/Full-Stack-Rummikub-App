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
    public class PlayerList:List<Player>
    {
        public PlayerList()
        { }
        public PlayerList(IEnumerable<Player> list) : base(list)
        { }
        public PlayerList(IEnumerable<BaseEntity> list) : base(list.Cast<Player>().ToList()) { }
    }
}
