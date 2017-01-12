using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Runtime.Validation;
using MyCompanyName.AbpZeroTemplate.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.IcCardService.Dtos {

    [AutoMap(typeof(IcCard))]
    public class CreateOrUpdateCardInput : IInputDto, IValidate {
        public string inCard { get; set; }

        public string outCard { get; set; }

        public decimal Balance { get; set; }
        public int Integral { get; set; }

        public int State { get; set; }

        public int? Id { get; set; }

    }
    public class OrderCardUserDto : IInputDto, IValidate {
        public int Id { get; set; }
        public string InCard { get; set; }
        public string OutCard { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public decimal Balance { get; set; }
        public int Integral { get; set; }
        public int PointId { get; set; }

        public string PointName { get; set; }

    }
    public class MenubyDateDto : IInputDto, IValidate {
        public String Id { get; set; }
        public String Dishes { get; set; }
        public decimal Cost { get; set; }


    }
    public class UserOrderDto {
        public DateTime date { get; set; }
        public List<PayOrder> events { get; set; }
    }
    public class PayOrder {
        public string name { get; set; }
        public OrderState type { get; set; }
    }
    [AutoMap(typeof(Orders))]

    public class TempTOrder {
        public int DishId { get; set; }
        public int PointId { get; set; }
        public string OrderNumber { get; set; }
    }
}
