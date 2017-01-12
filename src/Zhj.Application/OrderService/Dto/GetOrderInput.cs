using Abp.Runtime.Validation;
using MyCompanyName.AbpZeroTemplate.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.OrderService.Dto {
   public class GetOrderInput : PagedAndSortedInputDto, IShouldNormalize{
        public string Filter { get; set; }     
        public DateTime? SearchStartTime { get; set; }

        public DateTime? SearchEndTime { get; set; }

        public void Normalize( ) {
            if (string.IsNullOrEmpty(Sorting)) {
                Sorting = "Name,Surname";
            }
        }
    }

    public class GetPointOrderInput : PagedAndSortedInputDto, IShouldNormalize {
        public string PointName { get; set; }

        public string DishName { get; set; }

        public DateTime? SearchStartTime { get; set; }

        public DateTime? SearchEndTime { get; set; }

        public void Normalize( ) {
            if (string.IsNullOrEmpty(Sorting)) {
                Sorting = "Name,Surname";
            }
        }
    }
    public class GetPointOrdersInput : PagedAndSortedInputDto, IShouldNormalize {
        public string Filter { get; set; }
        public DateTime? SearchStartTime { get; set; }

        public DateTime? SearchEndTime { get; set; }
        public void Normalize() {
            if (string.IsNullOrEmpty(Sorting)) {
                Sorting = "CreationTime desc";
            }
        }
    }
    public class GetprePaidInput : PagedAndSortedInputDto, IShouldNormalize {
        public string Name { get; set; }
        public string Card { get; set; }
        public void Normalize() {
            if (string.IsNullOrEmpty(Sorting)) {
                Sorting = "CreationTime desc";
            }
        }
    }
}
