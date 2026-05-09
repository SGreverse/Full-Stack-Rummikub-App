using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.PlayingEntities;

namespace RummiKubClasses.ObjectsForDataBase
{
    public class GameInviteList:List<GameInvite>
    {
        public GameInviteList()
        { }
        public GameInviteList(IEnumerable<GameInvite> list) : base(list)
        { }
        public GameInviteList(IEnumerable<BaseEntity> list) : base(list.Cast<GameInvite>().ToList()) { }
    }
}
