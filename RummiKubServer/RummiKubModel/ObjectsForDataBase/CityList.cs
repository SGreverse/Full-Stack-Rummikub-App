using RummiKubClasses.PlayingEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses.ObjectsForDataBase
{
    [CollectionDataContract]
    public class CityList:List<City>
    {
        public CityList()
        { }
        public CityList(IEnumerable<City> list) : base(list)
        { }
        public CityList(IEnumerable<BaseEntity> list) : base(list.Cast<City>().ToList()) { }
    }
}
