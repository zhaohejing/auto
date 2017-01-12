using MyCompanyName.AbpZeroTemplate.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.IcCardService.Dtos {
  public  class ActionCardInput {
        public RecordState Type { get; set; }
        public int CardId { get; set; }
        public decimal Nums { get; set; }
    }


}
