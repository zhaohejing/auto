using MyCompanyName.AbpZeroTemplate.EntityModel;
using MyCompanyName.AbpZeroTemplate.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.OrderService.Dto {
    public class PointOrderDto {
        public int serialNo { get; set; }
        //  public string OrderNumber { get; set; }
        public string Dish { get; set; }

        public string PointName { get; set; }

        //   public string InCard { get; set; }
        public decimal DishNumber { get; set; }

        // public OrderState State { get; set; }

        public PayState PayState { get; set; }

        public DateTime OrderTime { get; set; }

    }

    public class OrderRecords {
        public UserState CustomerState { get; set; }
        public string PointName { get; set; }
        public decimal Count { get; set; }
        public decimal Payfor { get; set; }
        public int PointId { get; set; }

    }
    public class PrePaidDto {
        public string RechargeNumber { get; set; }
        public string CustomerName { get; set; }
        public string OutCard { get; set; }
        public decimal TopUp { get; set; }
        public decimal WithDrawal { get; set; }
        public DateTime CreationTime { get; set; }
        public string UserName { get; set; }
    }
}
