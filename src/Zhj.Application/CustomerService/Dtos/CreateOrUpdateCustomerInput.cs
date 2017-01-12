using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using MyCompanyName.AbpZeroTemplate.EntityModel;
using MyCompanyName.AbpZeroTemplate.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.CustomerService.Dtos {
  
    [AutoMap(typeof(Customers))]
    public class CreateOrUpdateCustomerInput : IInputDto, IValidate {
        public int? Id { get; set; }
        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }
        public UserState CustomerState { get; set; }
        public DateTime Birthday { get; set; }

        public string Address { get; set; }

        public int CardId { get; set; }
        public string OutCard { get; set; }
        public decimal Balance { get; set; }

        public int PointId { get; set; }

        public string PointName { get; set; }

    }


    public class UserOrdersDto {
        //        select a., b., c., a.State, a.Dish, a.DishNumber,
        //a.OrderTime, a., a., a.AmountPayable,
        //a.PointName, d.PayState, e.Name

        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string OutCard { get; set; }
        public OrderState State { get; set; }
        public UserState CustomerState { get; set; }
        public string Dish { get; set; }
        public int DishNumber { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime CreationTime { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal AmountPayable { get; set; }
        public string PointName { get; set; }
        public PayState PayState { get; set; }
        public string Name { get; set; }
    }
}
