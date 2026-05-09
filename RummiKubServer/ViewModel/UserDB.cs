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
    public abstract class UserDB : BaseDB
    {
        private string basesql = "SELECT *,UserTbl.Id as Id FROM (UserTbl INNER JOIN LeaderBoardTbl ON UserTbl.Id=LeaderBoardTbl.Id)";


        public PlayerList SelectByRegistrationUserData(string username)//check if username already taken
        {
            command.CommandText = basesql + $" WHERE Username=@Username AND IsDeleted=False";
            OleDbParameter p1 = new OleDbParameter("@Username", username);
            command.Parameters.Add(p1);
            PlayerList users = new PlayerList(Select());
            return users;
        }
        public PlayerList SelectByEmailInfo(string email)
        {
            command.CommandText = basesql + $" WHERE Email=@Email AND IsDeleted=False";
            OleDbParameter p1 = new OleDbParameter("@Email", email);
            command.Parameters.Add(p1);
            PlayerList users = new PlayerList(Select());
            return users;
        }
        public override BaseEntity CreateModel(BaseEntity entity)
        {
            User user = entity as User;
            user.Id = (int)reader["Id"];
            user.FirstName = reader["FirstName"].ToString();
            user.LastName = reader["LastName"].ToString();
            user.Username = reader["Username"].ToString();
            user.Password = reader["Password"].ToString();
            user.Email = reader["Email"].ToString();
            int city = (int)reader["City"];
            user.User_City = CityDB.SelectByID(city);
            user.IconName = reader["IconName"].ToString();
            user.IsAdmin = (bool)reader["IsAdmin"];
            return user;
        }
         

        //score and id are automatically asigned so i didnt care about injections
        protected override void CreateInsertSQL(BaseEntity entity, OleDbCommand command)
        {
            User user = entity as User;
            if (user != null)
            {
                string sqlStr = $"Insert INTO UserTbl(FirstName,LastName,Username,[Password],Email,City,IconName,IsAdmin) Values(@FirstName,@LastName,@Username,@Password,'{user.Email}',@City,@IconName,{user.IsAdmin})";
                OleDbParameter p1 = new OleDbParameter("@FirstName", user.FirstName);
                OleDbParameter p2 = new OleDbParameter("@LastName", user.LastName);
                OleDbParameter p3 = new OleDbParameter("@Username", user.Username);
                OleDbParameter p4 = new OleDbParameter("@Password", user.Password);
                OleDbParameter p5 = new OleDbParameter("@City", user.User_City.Id);
                OleDbParameter p6 = new OleDbParameter("@IconName", user.IconName);
                command.CommandText = sqlStr;
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                command.Parameters.Add(p3);
                command.Parameters.Add(p4);
                command.Parameters.Add(p5);
                command.Parameters.Add(p6);
            }
        }

        protected override void CreateDeleteSQL(BaseEntity entity, OleDbCommand command)
        {
            User user = entity as User;
            if (user != null)
            {
                string sqlStr = $"Update UserTbl Set IsDeleted=True WHERE Id={user.Id}";
                command.CommandText = sqlStr;
            }
        }
        protected override void CreateUpdateSQL(BaseEntity entity, OleDbCommand command)
        {
            User user = entity as User;
            if (user != null)
            {
                string sqlStr = $"Update UserTbl Set FirstName=@FirstName,LastName=@LastName,Username=@Username,[Password]=@Password,City=@City,Email=@Email,IconName=@IconName, IsAdmin={user.IsAdmin} WHERE Id={user.Id}";
                OleDbParameter p1 = new OleDbParameter("@FirstName", user.FirstName);
                OleDbParameter p2 = new OleDbParameter("@LastName", user.LastName);
                OleDbParameter p3 = new OleDbParameter("@Username", user.Username);
                OleDbParameter p4 = new OleDbParameter("@Password", user.Password);
                OleDbParameter p5 = new OleDbParameter("@City", user.User_City.Id);
                OleDbParameter p6 = new OleDbParameter("@Email", user.Email);
                OleDbParameter p7 = new OleDbParameter("@IconName", user.IconName);

                command.CommandText = sqlStr;
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                command.Parameters.Add(p3);
                command.Parameters.Add(p4);
                command.Parameters.Add(p5);
                command.Parameters.Add(p6);
                command.Parameters.Add(p7);
            }
        }
     
    }
}
