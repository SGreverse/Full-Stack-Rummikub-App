using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.PlayingEntities;

namespace RummiKubClasses.ObjectsForDataBase
{
    public class UserProblem:BaseEntity
    {
        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }


    }
}
