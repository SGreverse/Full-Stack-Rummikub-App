using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.PlayingEntities;

namespace RummiKubClasses.ObjectsForDataBase
{
    public abstract class PlayerConnection:BaseEntity
    {
        public int Approver { get; set; }

        public int Requester { get; set; }


        public override bool Equals(object obj)
        {
            return (obj is PlayerConnection con) &&
                con.Requester==this.Requester &&
                con.Approver==this.Approver;
        }
    }
}
