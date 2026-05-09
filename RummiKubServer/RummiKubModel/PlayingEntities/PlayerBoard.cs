using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.Tiles;

namespace RummiKubClasses.GamePieces
{
    //[DataContract]
    public class PlayerBoard
    {
        //[DataMember]
        public List<Tile> TileList;
        //[DataMember]
        public int Amount;

        public PlayerBoard()
        {
            TileList = new List<Tile>();
            Amount = 0;
        }
        public PlayerBoard(PlayerBoard OtherBoard)//copy the data of the other board
        {   
            this.TileList =new List<Tile>( OtherBoard.TileList);
            this.Amount = OtherBoard.Amount;
        }
        public void AddTile(Tile t)
        {
            this.TileList.Add(t);
            Amount++;
        }
        public void RemoveTile(Tile t)
        {
            this.TileList.Remove(this.TileList.First(tile=>t.Equals(tile)));
            Amount--;
        }
        public bool IsEmpty()
        {
            return Amount==0;

        }
    }
}
