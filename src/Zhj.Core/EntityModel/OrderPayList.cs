using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.EntityModel {
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class OrderPayList: BasicModel {
        public int OrderId { get; set; }

        public decimal Cost { get; set; }

        public PayState PayState { get; set; }
    }
  


}
