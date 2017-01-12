using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.EntityModel {
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class RechargerRecord: BasicModel {
        public string RechargeNumber { get; set; }

        public int CardId { get; set; }

        public decimal RechargeCost { get; set; }

        public RecordState State { get; set; }

       
    }
}
