using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses.Tiles
{
    public enum RummiColor
    {
        Black,
        Orange,
        Red,
        Blue
    }
    [KnownType(typeof(Joker))]

    public class Tile
    {
        public RummiColor Color { get; set; }

        public int Value {  get; set; }
        public Tile()
        {
            
        }
        public Tile(Tile other)
        {
            Color = other.Color;
            Value = other.Value;
            HashSet<Tile> set = new HashSet<Tile>();
        }
        public Tile(int value,RummiColor color)
        {
            this.Value = value;
            this.Color = color;
        }

        public override bool Equals(object obj)
        {
            return obj is Tile tile &&
                   !(obj is Joker) &&
                   Color == tile.Color &&
                   Value == tile.Value;
        }

        public override int GetHashCode()
        {
            int hashCode = -1984162526;
            hashCode = hashCode * -1521134295 + Color.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }

        // 
        public override string ToString()
        {
            return $"Value:{this.Value},Color:{this.Color}";
        }

    }

}
