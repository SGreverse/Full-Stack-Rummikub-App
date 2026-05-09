using RummiKubClasses.PlayingEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses.GamePieces
{
    public class PlayerOrderList
    {
        public PlayerNode Node;

        public int Length;
        public PlayerOrderList(List<PlayerInGame> list)
        {
            this.Node = BuildPlayerOrder(list);
            this.Length = list.Count;
        }
        public void ContinueToNextPlayer()
        {
            this.Node = this.Node.next;
        }
        public void PlayerFinished(PlayerInGame p)
        {
            PlayerNode temp = this.Node;
            while (temp.player != p)
            {
                temp = temp.next;
            }
            temp.player = temp.next.player;
            temp.next = temp.next.next;
            this.Length--;
        }
        public PlayerInGame GetCurrentPlayer()
        {
            return this.Node.player;
        }
        public PlayerNode BuildPlayerOrder(List<PlayerInGame> list)
        {
            list[0].This_PlayerState = PlayerState.Playing;
            PlayerNode current = new PlayerNode(list[0]);
            PlayerNode head = current;
            for (int i = 1; i < list.Count; i++)
            {


                if (i == list.Count - 1)
                {
                    current.SetNext(new PlayerNode(list[i], head));
                }
                else
                {
                    current.SetNext(new PlayerNode(list[i], current));
                }
                current = current.next;

            }
            return head;


        }
    }
}
