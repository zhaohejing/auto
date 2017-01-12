using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.OrderService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.OrderService {
    public interface IOrderAppService : IApplicationService {
        PagedResultOutput<DayDueDto> GetDaydueList(GetOrderInput input);
        FileDto ExportOrderInfo(GetOrderInput input);

        PagedResultOutput<CustomOrderDto> GetCustomOrderList(GetOrderInput input);

        FileDto ExportCustomOrder(GetOrderInput input);

        PagedResultOutput<PointOrderDto> GetPointOrderList(GetPointOrderInput input);

        FileDto ExportPointOrder(GetPointOrderInput input);
        dynamic GetDepartmentList( );
        PagedResultOutput<OrderRecords> GetPointOrders(GetPointOrdersInput input);
        FileDto ExportPointOrders(GetPointOrdersInput input);
        PagedResultOutput<PrePaidDto> GetprePaidList(GetprePaidInput input);
        FileDto ExportprePaidList(GetprePaidInput input);
    }
}
