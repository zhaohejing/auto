using MyCompanyName.AbpZeroTemplate.CustomerService.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.OrderService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.CustomerService {
   public interface IOrderExcelExporter {
        FileDto ExportToFile(List<UserOrdersDto> userListDtos);
        FileDto ExportDayDueToFile(List<DayDueDto> userListDtos);

        FileDto ExportCustomOrderToFile(List<CustomOrderDto> userListDtos);

        FileDto ExportPointOrderToFile(List<PointOrderDto> userListDtos);
        FileDto ExportPointOrders(List<OrderRecords> orders);
        FileDto ExportprePaidList(List<PrePaidDto> predto);
    }
}
