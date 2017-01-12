using MyCompanyName.AbpZeroTemplate.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.OrderService.Dto {
   public class DayDueDto {
        public int serialNo { get; set; }
        public string OrderNumber { get; set; }
        public string Dish { get; set; }
        public decimal DishNumber { get; set; }
       // public int State { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal DishCost { get; set; }
    }
}
