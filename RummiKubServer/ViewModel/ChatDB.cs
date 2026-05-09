using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.GamePieces;
using RummiKubClasses.PlayingEntities;

namespace ViewModel
{
    public class ChatDB : BaseDB
    {
        public List<Message> SelectAllChatMessages(int User1, int User2)
        {
            command.CommandText = $"SELECT UserTbl.Username, ChatTbl.*" +
                $" FROM (ChatTbl INNER JOIN" +
                $" UserTbl ON ChatTbl.Sender = UserTbl.Id) " +
                $"WHERE  (ChatTbl.Sender = {User1}) AND (ChatTbl.Reciever = {User2}) OR " +
                $"(ChatTbl.Sender = {User2}) AND (ChatTbl.Reciever = {User1}) " +
                $"ORDER BY ChatTbl.SendingTime";
            List<Message> list = Select().Cast<Message>().ToList();
            return list;
        }
        public override BaseEntity CreateModel(BaseEntity entity)
        {
            Message message = entity as Message;
            message.Id = (int)reader["ID"];
            message.SentBy = reader["Username"].ToString();
            message.SendingTime =(DateTime) reader["SendingTime"];
            message.Msg = reader["Message"].ToString();
            message.SenderId = (int)reader["Sender"];
            message.RecieverId = (int)reader["Reciever"];
            message.IsEdited = (bool)reader["IsEdited"];
            return message; 
        }

        public override void Delete(BaseEntity entity)
        {
            Message message = entity as Message;
            if (message != null)
            {
                updated.Add(new ChangeEntity(message, CreateDeleteSQL));
            }
        }

        public override BaseEntity NewEntity()
        {
            return new Message();
        }

        public override void Update(BaseEntity entity)
        {
            Message message= entity as Message;
            if(message != null)
            {
                updated.Add(new ChangeEntity(message,CreateUpdateSQL));
            }
        }

        protected override void CreateDeleteSQL(BaseEntity entity, OleDbCommand command)
        {
            Message message= entity as Message;
            command.CommandText = $"Delete FROM ChatTbl WHERE ID={message.Id}";
        }

        protected override void CreateInsertSQL(BaseEntity entity, OleDbCommand command)
        {
            Message message= entity as Message;
            command.CommandText = $"INSERT INTO ChatTbl(Sender,Reciever,SendingTime,Message,IsEdited) Values({message.SenderId},{message.RecieverId},'{message.SendingTime.ToString("yyyy-MM-dd HH:mm:ss")}',@Msg,False)";
            OleDbParameter p1 = new OleDbParameter("@Msg",message.Msg);
            command.Parameters.Add(p1);
        }

        protected override void CreateUpdateSQL(BaseEntity entity, OleDbCommand command)
        {
            Message message = entity as Message;
            command.CommandText = $"Update ChatTbl Set Message=@Msg, IsEdited=True WHERE ID={message.Id}";
            OleDbParameter p1 = new OleDbParameter("@Msg", message.Msg);
            command.Parameters.Add(p1);
        }
    }
}
