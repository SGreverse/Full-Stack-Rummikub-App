using RummiKubClasses.PlayingEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses.GamePieces
{
    public class PlayerNode
    {
        public PlayerInGame player;
        public PlayerNode next;

        public PlayerNode(PlayerInGame player)
        {
            this.player = player;
            this.next = null;
        }
        public PlayerNode(PlayerInGame player, PlayerNode next) 
        {
            this.player = player;
            this.next = next;
        }
        public void SetNext(PlayerNode next)
        {
            this.next = next;
        }
    }

}
