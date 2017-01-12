using Abp.Runtime.Validation;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.CustomerService.Dtos {
 
    public class GetCustomerInput : PagedAndSortedInputDto, IShouldNormalize {
        public string Name { get; set; }
        public string Card { get; set; }
        public string Mobile { get; set; }
        public string Point { get; set; }
        public void Normalize() {
            if (string.IsNullOrEmpty(Sorting)) {
                Sorting = "Id";
            }
        }
    }

    public class GetOrderListInput : PagedAndSortedInputDto, IShouldNormalize {
        public string DeviceName { get; set; }
        public string DishName { get; set; }
        public string CustomerName { get; set; }
        public string CardName { get; set; }
        public OrderState OrderState { get; set; }
        public PayState PayState { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public void Normalize() {
            if (string.IsNullOrEmpty(Sorting)) {
                Sorting = "Id";
            }
        }
    }

    
}
