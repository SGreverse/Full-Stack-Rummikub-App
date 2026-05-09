using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses.Tiles
{
    //[DataContract]

    public class Joker : Tile
    {
        public Joker()
        {

        }
        public Joker(RummiColor color) : base(0, color)
        {

        }



        public override bool Equals(object obj)
        {
            return obj is Joker joker && 
                   Value==joker.Value &&
                   Color==joker.Color;
        }
        public override int GetHashCode()//not relavent now
        {
            return base.GetHashCode();
        }
    }
}
