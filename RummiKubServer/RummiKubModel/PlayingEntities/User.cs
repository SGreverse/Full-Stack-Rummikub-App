using RummiKubClasses.ObjectsForDataBase;
using RummiKubClasses.Tiles;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses.PlayingEntities
{
    //[DataContract]

    [KnownType(typeof(Player))]

    public abstract class User: BaseEntity
    {
        //[DataMember]
        public string Username { get; set; }
        //[DataMember]
        public string Password { get; set; }
        //[DataMember]
        public string FirstName { get;set; }
        public string LastName { get;set; }
        public string Email { get; set; }
        //[DataMember]

        public City User_City { get; set; }

        public string IconName {  get; set; }

        public bool IsAdmin {  get; set; }
        public User()
        {
            
        }
        public User(int id,string username, string password, string firstName, string lastName, string email, City user_City, string iconName,bool isadmin) 
        {
            this.Id = id;
            this.Username = username;
            this.Password = password;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.User_City = user_City;
            this.IconName = iconName;
            this.IsAdmin = isadmin;
        }


    }
}
