using RummiKubClasses.PlayingEntities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public delegate void CreateSql(BaseEntity be, OleDbCommand command);

    public abstract class BaseDB
    {
        protected static List<ChangeEntity> inserted = new List<ChangeEntity>();
        protected static List<ChangeEntity> updated = new List<ChangeEntity>();
        protected static string connectionstring;
        protected OleDbConnection connection;
        protected OleDbCommand command;
        protected OleDbDataReader reader;
        public BaseDB()
        {
            if (connectionstring == null)
                connectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path() +@"\RummiKubDataBase.accdb; Persist Security Info = True";
            connection = new OleDbConnection(connectionstring);
            command = new OleDbCommand();
            command.Connection = connection;
        }
        private static string Path()
        {
            string[] arguments = Environment.GetCommandLineArgs();
            string s;
            if (arguments.Length == 1)
            {
                s = arguments[0];
            }
            else
            {
                s = arguments[1];
                s = s.Replace("/service:", "");

            }
            string[] ss = s.Split('\\');
            int x = ss.Length - 4;
            ss[x] = "ViewModel";
            ss[x + 1] = "DataBase";
            Array.Resize(ref ss, x + 2);
            string str = String.Join("\\", ss);
            return str;
        }
        abstract public BaseEntity NewEntity();
        abstract public BaseEntity CreateModel(BaseEntity entity);
        public virtual void Insert(BaseEntity entity)
        {
            BaseEntity reqEntity = this.NewEntity();
            if (entity != null & reqEntity.GetType() == entity.GetType())
            {
                inserted.Add(new ChangeEntity(entity, this.CreateInsertSQL));
            }
        }
        public abstract void Update(BaseEntity entity);
        public abstract void Delete(BaseEntity entity);
        protected abstract void CreateInsertSQL(BaseEntity entity, OleDbCommand command);
        protected abstract void CreateDeleteSQL(BaseEntity entity, OleDbCommand command);
        protected abstract void CreateUpdateSQL(BaseEntity entity, OleDbCommand command);
        public List<BaseEntity> Select()
        {
            List<BaseEntity> userlist = new List<BaseEntity>();
            try
            {
                command.Connection = connection;
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    BaseEntity user = NewEntity();
                    userlist.Add(CreateModel(user));
                }
            }
            catch (Exception e)
            { 
                System.Diagnostics.Debug.WriteLine(e.Message + "\nSQL:" + command.CommandText);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }

            return userlist;
        }
        public int NumberSelection()
        {
            int result = 0;
            try
            {
                command.Connection = connection;
                connection.Open();
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    result= reader.GetInt32(0);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\nSQL:" + command.CommandText);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            return result;
        }
        public static int SaveChanges()
        {
            OleDbCommand command = new OleDbCommand();
            OleDbTransaction trans = null;
            int records = 0;
            try
            {
                command.Connection = new OleDbConnection(connectionstring);
                command.Connection.Open();
                trans = command.Connection.BeginTransaction();
                command.Transaction = trans;
                foreach (var item in inserted)
                {
                    command.Parameters.Clear();
                    item.CreateSql(item.Entity, command);
                    records += command.ExecuteNonQuery();

                    command.CommandText = "Select @@identity";
                    item.Entity.Id = (int)command.ExecuteScalar();
                }
                foreach (var item in updated)
                {
                    command.Parameters.Clear();
                    item.CreateSql(item.Entity, command);
                    records += command.ExecuteNonQuery();
                }
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                System.Diagnostics.Debug.WriteLine(e.Message + "\nSQL:" + command.CommandText);

            }
            finally
            {
                inserted.Clear();
                updated.Clear();
                if (command.Connection.State == System.Data.ConnectionState.Open)
                    command.Connection.Close();
            }
            return records;
        }
    }
}
