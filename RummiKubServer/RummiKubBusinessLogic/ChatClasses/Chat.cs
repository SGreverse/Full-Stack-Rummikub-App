using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.ChatClasses;
using RummiKubClasses.PlayingEntities;

namespace RummiKubClasses.GamePieces
{
    public class Chat
    {
        Dictionary<int, Queue<MessageAction>> UserMessages = new Dictionary<int, Queue<MessageAction>>();

        public Chat(List<PlayerInGame> playerslist)
        {
            foreach(PlayerInGame player in playerslist)
            {
                this.UserMessages.Add(player.Id,new Queue<MessageAction>());
            }
        }
        public Chat(int Player1Id,int Player2Id)
        {
            this.UserMessages.Add(Player1Id,new Queue<MessageAction>());
            this.UserMessages.Add(Player2Id,new Queue<MessageAction>());
        }
        public MessageAction RecieveMessage(int playerid)
        {
            if (this.UserMessages[playerid].Count == 0)
                return null;
            return this.UserMessages[playerid].Dequeue();
        }
        public void AddMessage(MessageAction message)
        {
            foreach(Queue<MessageAction> queue in this.UserMessages.Values)
            {
                queue.Enqueue(message);
            }
        }
        public void ClearChat(int PlayerId)
        {
            UserMessages[PlayerId].Clear();
        }
        public bool IsEmpty(int playerid)
        {
            return UserMessages[playerid].Count ==0;
        }
        public bool IsPlayerInChat(int playerid)
        {
            return UserMessages.ContainsKey(playerid);
        }


    }
}
