using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RummiKubClasses.PlayingEntities;

namespace RummiKubClasses.DataBaseObjects
{
    public class CityList:List<City>
    {
        public CityList()
        { }
        public CityList(IEnumerable<City> list) : base(list)
        { }
        public CityList(IEnumerable<BaseEntity> list) : base(list.Cast<City>().ToList()) { }
    }
}
