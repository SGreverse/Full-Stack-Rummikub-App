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
    public class CityDB:BaseDB
    {
        private static CityList cityList = new CityList();

        public override BaseEntity NewEntity()
        {
            return new City();
        }
        public override BaseEntity CreateModel(BaseEntity entity)
        {
            City city = entity as City;
            city.CityName = reader["CityName"].ToString();
            city.Id = (int)reader["CityId"];
            return city;
        }
        public CityList SelectAll()
        {
            if (cityList.Count != 0)
                return cityList;
            command.CommandText = "SELECT * FROM CityTbl ORDER BY CityName";
            cityList = new CityList(Select());
            return cityList;
        }
        public static City SelectByID(int id)
        {
            if (cityList == null || cityList.Count == 0)
            {

                CityDB db = new CityDB();
                cityList = db.SelectAll();
            }
            City city = cityList.FirstOrDefault(c => c.Id == id);
            return city;
        }

        public override void Update(BaseEntity entity)
        {
            City city = entity as City;
            if (city != null)
            {
                updated.Add(new ChangeEntity(city, this.CreateUpdateSQL));
            }
        }
        //missing deleting from the rest of the data bases
        public override void Delete(BaseEntity entity)
        {
            City city = entity as City;
            if (city != null)
                updated.Add(new ChangeEntity(city, this.CreateDeleteSQL));
        }
        protected override void CreateInsertSQL(BaseEntity entity, OleDbCommand command)
        {
            City city = entity as City;
            string sqlStr = "Insert INTO CityTbl(CityName) " + $"VALUES(@CityName)";
            command.Parameters.Add(new OleDbParameter("@CityName", city.CityName));
            command.CommandText = sqlStr;

        }
        protected override void CreateUpdateSQL(BaseEntity entity, OleDbCommand command)
        {
            City city = entity as City;
            string sqlStr = $"UPDATE CityTbl SET CityName=@CityName WHERE CityId={city.Id}";
            command.Parameters.Add(new OleDbParameter("@CityName", city.CityName));
            command.CommandText = sqlStr;
        }

        protected override void CreateDeleteSQL(BaseEntity entity, OleDbCommand command)
        {
            City city = entity as City;
            string sqlstr = $"DELETE FROM CityTbl WHERE CityId=@ID";// {city.Id}";
            command.CommandText = sqlstr;
            command.Parameters.Add(new OleDbParameter("@ID", city.Id));
        }
    }
}
