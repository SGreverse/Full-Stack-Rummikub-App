using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.GamePieces;
using RummiKubClasses.PlayingEntities;

namespace RummiKubClasses.ObjectsForDataBase
{
    public class FriendConnectionList:List<FriendConnection>
    {
        public FriendConnectionList()
        { }
        public FriendConnectionList(IEnumerable<FriendConnection> list) : base(list)
        { }
        public FriendConnectionList(IEnumerable<BaseEntity> list) : base(list.Cast<FriendConnection>().ToList()) { }
    }
}
