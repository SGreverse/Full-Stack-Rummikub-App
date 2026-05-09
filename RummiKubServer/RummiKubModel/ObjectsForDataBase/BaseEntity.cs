using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses.PlayingEntities
{
    //[DataContract]
    public abstract class BaseEntity
    {
        //[DataMember]
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if(obj.GetType() != this.GetType()) return false;
            return obj is BaseEntity Base && Base.Id == this.Id;
        }

    }
}
