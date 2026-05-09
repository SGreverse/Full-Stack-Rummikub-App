using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.ObjectsForDataBase;
using RummiKubClasses.PlayingEntities;

namespace ViewModel
{
    public class FriendDB : BaseDB
    {
        public override BaseEntity CreateModel(BaseEntity entity)
        {
            throw new NotImplementedException();
        }
        public override BaseEntity NewEntity()
        {
            throw new NotImplementedException();
        }
        public override void Delete(BaseEntity entity)
        {
            throw new NotImplementedException();
        }


        public override void Insert(BaseEntity entity)
        {
            FriendConnection friendconnection = entity as FriendConnection;
            if (friendconnection != null)
            {
                inserted.Add(new ChangeEntity(friendconnection, this.CreateInsertSQL));
            }
        }
        public override void Update(BaseEntity entity)
        {
            FriendConnection friendConnection=entity as FriendConnection;
            if (friendConnection!=null)
            {
                updated.Add(new ChangeEntity(friendConnection, this.CreateUpdateSQL));
            }
        }

        protected override void CreateDeleteSQL(BaseEntity entity, OleDbCommand command)
        {
            throw new NotImplementedException();
        }

        protected override void CreateInsertSQL(BaseEntity entity, OleDbCommand command)
        {
            FriendConnection friendConnection = entity as FriendConnection;
            if (friendConnection != null)
            {
                string sqlstr = $"Insert INTO FriendTbl(Requester,Approver,Approved) Values({friendConnection.Requester},{friendConnection.Approver},False)  ";
                command.CommandText = sqlstr ;
            }
        }

        protected override void CreateUpdateSQL(BaseEntity entity, OleDbCommand command)
        {
            FriendConnection friendConnection= entity as FriendConnection;
            if(friendConnection != null)
            {
                string sqlstr = $"Update FriendTbl SET Approved=True WHERE Requester={friendConnection.Requester} AND Approver={friendConnection.Approver}";
                command.CommandText = sqlstr;
            }
        }
    }
}
