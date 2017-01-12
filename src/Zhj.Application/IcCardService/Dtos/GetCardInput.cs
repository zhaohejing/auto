using Abp.Runtime.Validation;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.IcCardService.Dtos {
   public class GetCardInput : PagedAndSortedInputDto, IShouldNormalize {
        public string InNum { get; set; }
        public string OutNum { get; set; }
        public void Normalize() {
            if (string.IsNullOrEmpty(Sorting)) {
                Sorting = "Id";
            }
        }
    }
    public class GeTMealInfoInput {
        public DateTime? date { get; set; }
        public int pointId { get; set; }
        public string cardId { get; set; }
    }

    public class PayForOrderInput {
        public List<TempOrder> Orders { get; set; }
        public int CustomerId { get; set; }
        public string InCard { get; set; }
        public PayState PayState { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? Time { get; set; }
    }
    public class GetUserOrderInput {
        public int CustomerId { get; set; }
        public DateTime? Time { get; set; }
    }
    public class GetOrderCardUser {
        public string Filter { get; set; }
    }
    public class GetMenubyDateInput {
        public string pointId { get; set; }

        public DateTime date { get; set; }

    }
}
