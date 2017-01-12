using MyCompanyName.AbpZeroTemplate.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.OrderService.Dto {
   public class CustomOrderDto {
        public int serialNo { get; set; }
        public string OrderNumber { get; set; }

        public string CustomerName { get; set; }
        public string OutCard { get; set; }
        public OrderState State { get; set; }

        public PayState PayState { get; set; }

        public decimal OrderAmount { get; set; }

        public DateTime CreationTime { get; set; }

        public string Name { get; set; }
    }
}
