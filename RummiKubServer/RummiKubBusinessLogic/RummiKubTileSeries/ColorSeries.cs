using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses.Tiles
{
    //[DataContract]
    public class ColorSeries:TileSeries
    {
        public ColorSeries() { }
        public ColorSeries(List<Tile> tiles):base(tiles)
        {
           
        }
        public ColorSeries(List<Tile> tiles,int SeriesId) : base(SeriesId,tiles)
        {

        }
        public override void TryAddTile(Tile addedtile)
        {
            base.set.Add(addedtile);
            if (addedtile is Joker)
                addedtile.Value = set[1].Value;


        }
        public override void CreateSeries(List<Tile> tiles)
        {
            if (IsValid(tiles))
                base.set = new List<Tile>(tiles);
            else
                throw new Exception("Cannot Make a Color Series");

        }
        public override bool IsValid(List<Tile> tiles)
        {
            HashSet<RummiColor> presentcolors = new HashSet<RummiColor>();
            int jokers = 0;
            int value = 0;
            if (tiles.Count <3)
                return false;
            Joker joker=null;
            foreach (Tile tile in tiles) //gets the count of all the jokers in the series
            {
                if (tile is Joker) //add all the jokers to the end of the list
                {
                    jokers++;
                    joker = tile as Joker;
                }
            }
            if (joker != null)
            {
                tiles.Add((joker));
                tiles.Remove(joker);
            }
            value = tiles[0].Value;
            presentcolors.Add(tiles[0].Color);
            for (int i = 1; i < tiles.Count; i++)
            {
                if (tiles[i] is Joker)
                {
                    tiles[i].Value=value;
                    continue;
                }
                if (tiles[i].Value == value && !presentcolors.Contains(tiles[i].Color))
                {
                    presentcolors.Add(tiles[i].Color);
                }
                else
                {
                    return false;
                }
            }
            presentcolors.Clear();
            return true;
        }

    }
}
