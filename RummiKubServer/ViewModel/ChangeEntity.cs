using RummiKubClasses.PlayingEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class ChangeEntity
    {
        private BaseEntity entity;
        private CreateSql createSql;

        public ChangeEntity(BaseEntity entity, CreateSql createSql)
        {
            this.Entity = entity;
            this.CreateSql = createSql;
        }

        public BaseEntity Entity { get => entity; set => entity = value; }
        public CreateSql CreateSql { get => createSql; set => createSql = value; }
    }
}
