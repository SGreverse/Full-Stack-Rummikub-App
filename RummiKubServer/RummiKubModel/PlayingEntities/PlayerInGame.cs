using RummiKubClasses.GamePieces;
using RummiKubClasses.PlayingEntities;
using RummiKubClasses.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Security.Cryptography;
namespace RummiKubClasses.PlayingEntities
{
    //[DataContract]
    public class PlayerInGame:Player
    {
        //[DataMember]
        public PlayerBoard playerBoard { get; set; }

        public PlayerBoard CopyOfPlayerBoard { get; set; }
        //[DataMember]
        public int Total_value_of_tiles { get; set; }// for the rule where you need to put on a total value of 30 before changing the board

        public int Original_total_value_of_tiles {  get; set; }
        public bool Has_Updated_Board {  get; set; }

        public PlayerState This_PlayerState { get; set; }
        //[DataMember]
        public PlayerInGame() {  }
        public PlayerInGame(Player player):base(player)
        {
            this.playerBoard = new PlayerBoard();
            this.CopyOfPlayerBoard = new PlayerBoard(playerBoard);
            this.Total_value_of_tiles = 0;
            Has_Updated_Board=true;
        }
        public void Take(Tile t)
        {
            this.playerBoard.RemoveTile(t);
            AddValue(t.Value);
        }
        public void AddValue(int val)
        {
            this.Total_value_of_tiles += val;
        }
        public void Draw(Tile t)//asks for a new card from the tilebank that's on the server 
        {
            this.playerBoard.AddTile(t);
            
        }
        public void UpdateCopy()//update the copy if the player successfully ended his turn
        {
            this.CopyOfPlayerBoard=new PlayerBoard(this.playerBoard);
            this.Original_total_value_of_tiles=this.Total_value_of_tiles;
        }
        public void ReturnChanges()//if the player wants to reset the board and his hand
        {
            this.playerBoard=new PlayerBoard(CopyOfPlayerBoard);
            this.Total_value_of_tiles = this.Original_total_value_of_tiles;
        }
        public void Put_On_Board(List<Tile> tiles_to_remove)
        {
            foreach(Tile t in tiles_to_remove)
            {
                Take(t);
            }

        }
        public void PlayerUpdatedBoard()
        {
            Has_Updated_Board = true;
        }
        public void APlayerChangedTheBoard()
        {
            Has_Updated_Board = false;
        }
        public bool Can_Change_Board()
        {
            return Total_value_of_tiles >= 30;
        }
        public void AddScore(int score)
        {
            this.Score += score;
        }
        public override bool Equals(object obj)
        {
            return obj is PlayerInGame&& (obj as PlayerInGame).Id== this.Id;
        }
    }
}
