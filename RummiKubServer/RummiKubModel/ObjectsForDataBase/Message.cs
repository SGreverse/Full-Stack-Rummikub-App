using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.PlayingEntities;

namespace RummiKubClasses.GamePieces
{
    public class Message:BaseEntity
    {
       public  string SentBy {  get; set; } 
       public DateTime SendingTime {  get; set; }

        public int SenderId {  get; set; }

        public int RecieverId {  get; set; }
        public string Msg {  get; set; }

        public bool IsEdited {  get; set; }
            
        public Message() { }
        public Message(string Msg,string SentBy)
        {
            this.Msg = Msg;
            this.SentBy = SentBy;
            this.SendingTime = DateTime.Now;
        }

    }
}
