using RummiKubClasses.GamePieces;
using RummiKubClasses.PlayingEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses
{
    
    public class VisualDisplayOfPlayer
    {

        public int Id {  get; set; }
        public string Username { get; set; }
        public int TileAmount {  get; set; }
        public string IconName {  get; set; }
        public PlayerState PlayerState {  get; set; }

        public int Level {  get; set; }
        public VisualDisplayOfPlayer()
        {

        }
        public VisualDisplayOfPlayer(int Id,int tileamount, PlayerState playerState)
        {
            this.Id = Id; 
            this.TileAmount=tileamount;
            this.PlayerState = playerState;
        }


    }
}
