using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.EntityModel {
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class Orders:BasicModel {

        public string OrderNumber { get; set; }
        public int CustomerId { get; set; }

        public OrderState State { get; set; }
        public int DishId { get; set; }
        public int TakeOff { get; set; }

        public string Dish { get; set; }
        public decimal DishCost { get; set; }

        public int DishNumber { get; set; }

        public DateTime OrderTime { get; set; }

        public int PointId { get; set; }

        public string PointName { get; set; } 

        public decimal OrderAmount { get; set; }

        public decimal AmountPayable { get; set; }
    }
}
