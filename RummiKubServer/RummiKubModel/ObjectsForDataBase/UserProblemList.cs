using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.PlayingEntities;

namespace RummiKubClasses.ObjectsForDataBase
{
    [CollectionDataContract]
    public class UserProblemList:List<UserProblem>
    {
        public UserProblemList()
        { }
        public UserProblemList(IEnumerable<UserProblem> list) : base(list)
        { }
        public UserProblemList(IEnumerable<BaseEntity> list) : base(list.Cast<UserProblem>().ToList()) { }
    }
}
