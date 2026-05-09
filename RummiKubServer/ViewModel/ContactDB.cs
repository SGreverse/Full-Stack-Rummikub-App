using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using RummiKubClasses.GamePieces;
using RummiKubClasses.ObjectsForDataBase;
using RummiKubClasses.PlayingEntities;

namespace ViewModel
{
    public class ContactDB : BaseDB
    {
        public UserProblemList SelectAll()
        {
            command.CommandText = "SELECT * FROM ContactTbl";
            UserProblemList userProblems = new UserProblemList(Select());
            return userProblems;
        }
        public UserProblemList SelectByEmail(string email)
        {
            command.CommandText = "SELECT * FROM ContactTbl WHERE Email=@Email";
            OleDbParameter p1 = new OleDbParameter("@Email", email);
            command.Parameters.Add(p1);
            UserProblemList userProblems = new UserProblemList(Select());
            return userProblems;
        }
        public override BaseEntity CreateModel(BaseEntity entity)
        {
            UserProblem userProblem = entity as UserProblem;
            if (userProblem != null)
            {
                userProblem.Id=(int)reader["ID"];
                userProblem.Email = reader["Email"].ToString();
                userProblem.Subject = reader["Subject"].ToString();
                userProblem.Message = reader["Message"].ToString();
            }
            return userProblem;
        }

        public override void Delete(BaseEntity entity)
        {
            UserProblem userProblem = entity as UserProblem;
            if (userProblem != null)
            {
                updated.Add(new ChangeEntity(userProblem, CreateDeleteSQL));
            }
        }

        public override BaseEntity NewEntity()
        {
            return new UserProblem();
        }

        public override void Update(BaseEntity entity)
        {
            UserProblem userProblem = entity as UserProblem;
            if (userProblem != null)
            {
                updated.Add(new ChangeEntity(userProblem, CreateUpdateSQL));
            }
        }

        protected override void CreateDeleteSQL(BaseEntity entity, OleDbCommand command)
        {
            UserProblem userProblem = entity as UserProblem;
            if (userProblem != null)
            {
                command.CommandText = $"DELETE FROM ContactTbl WHERE ID={userProblem.Id}";
            }
        }

        protected override void CreateInsertSQL(BaseEntity entity, OleDbCommand command)
        {
            UserProblem userProblem = entity as UserProblem;
            if (userProblem != null)
            {
                command.CommandText = $" Insert Into ContactTbl(Email,Subject,Message) VALUES(@Email,@Subject,@Message)";
                command.Parameters.AddWithValue("@Email", userProblem.Email);
                command.Parameters.AddWithValue("@Subject", userProblem.Subject);
                command.Parameters.AddWithValue("@Message", userProblem.Message);
            }
        }

        protected override void CreateUpdateSQL(BaseEntity entity, OleDbCommand command)
        {
            throw new NotImplementedException("Update operation is not implemented for ContactDB.");
        }
    }
}
