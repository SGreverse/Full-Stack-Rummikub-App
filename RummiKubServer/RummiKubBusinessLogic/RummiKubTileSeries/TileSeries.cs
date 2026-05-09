using RummiKubClasses.GamePieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses.Tiles
{
    //[DataContract]
    [KnownType(typeof(ColorSeries))]
    [KnownType(typeof(RisingSeries))]
    public abstract class TileSeries
    {
        //[DataMember]
        public int SeriesId { get; set; }
        //[DataMember]
        public List<Tile> set { get; set; } /*{ get; set; }*/
        //[DataMember]
        public TileSeries() { }
        public TileSeries(List<Tile> tiles)
        {
            set = new List<Tile>();
            try
            {
                CreateSeries(tiles);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public TileSeries(int seriesId, List<Tile> tiles):this(tiles)
        {
            SeriesId = seriesId;
        }

        public virtual void BorrowTile(Tile tile)
        {
            int index = -1;
            if (tile is Joker)
                tile = (Joker)tile;
            for(int i=0;i<this.set.Count;i++)
            {
                if (tile.Equals(this.set[i]))
                {
                    index = i;
                    break;
                }
            }
            if(index!=-1)
                this.set.RemoveAt(index);
        }
        public abstract void TryAddTile(Tile addedtile);
        public abstract void CreateSeries(List<Tile> tiles);
        public abstract bool IsValid(List<Tile> tiles);

        public override string ToString()
        {
            string s = "";
            foreach(Tile t in this.set)
            {
                s += $"[{t.ToString()}]";
            }
            return s;
        }
        public override bool Equals(object obj)
        {

            if (!(obj is TileSeries)) return false;
            TileSeries other = (obj as TileSeries);
            if (other.set.Count != this.set.Count) return false;
            for(int i = 0; i < this.set.Count; i++)
            {
                if (!other.set[i].Equals(this.set[i])) return false;
            }
            return true;
            
        }
        public override int GetHashCode()
        {
            int hash = 19;
            foreach (var tile in this.set)
            {
                hash = hash * 31 + tile.GetHashCode();

            }
            hash += SeriesId.GetHashCode();
            return hash;
        }
    }
}
