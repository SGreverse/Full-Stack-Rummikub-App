using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.PlayingEntities;

namespace RummiKubClasses.DataBaseObjects
{
    public class UserList:List<User>
    {
        public UserList()
        { }
        public UserList(IEnumerable<User> list) : base(list)
        { }
        public UserList(IEnumerable<BaseEntity> list) : base(list.Cast<User>().ToList()) { }
    }
}
