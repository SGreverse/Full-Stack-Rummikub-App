using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.GamePieces;

namespace RummiKubClasses.ChatClasses
{
    public enum MessageActionType
    {
        Insert,
        Delete,
        Edit
    }
    public class MessageAction
    {
       public Message message {  get; set; } 

       public MessageActionType type { get; set; }

        public MessageAction()
        {

        }
       public MessageAction(Message message)
        {
            this.message = message;
            if (message.IsEdited)
            {
                type = MessageActionType.Edit;
            }
            else {
                type =MessageActionType.Insert;
             }
        }
        public MessageAction(Message message, MessageActionType type)
        {
            this.message = message;
            this.type = type;
        }

    }
}
