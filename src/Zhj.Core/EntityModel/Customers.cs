using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using MyCompanyName.AbpZeroTemplate.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.EntityModel {

    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class Customers : CreationAuditedEntity, ISoftDelete {
        public string CustomerName { get; set; }

        //  [MaxLength(11)]
        [StringLength(50)]
        public string CustomerPhone { get; set; }

        public DateTime Birthday { get; set; }
        [StringLength(50)]

        public string Address { get; set; }
        public UserState CustomerState { get; set; }

        public int CardId { get; set; }

        public int PointId { get; set; }
        [StringLength(50)]

        public string PointName { get; set; }
       public bool IsDeleted { get; set; }
    }
    public class BasicModel: CreationAuditedEntity, ISoftDelete {
        public bool IsDeleted { get; set; }
    }
}
