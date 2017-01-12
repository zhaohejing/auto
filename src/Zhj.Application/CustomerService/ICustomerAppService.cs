using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.CustomerService.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.CustomerService {
   public interface ICustomerAppService :IApplicationService{
        PagedResultOutput<CreateOrUpdateCustomerInput> GetCustomerList(GetCustomerInput input);

         Task InsertCustomer(CreateOrUpdateCustomerInput input);

        Task DeleteCustomer(IdInput<int> input);
        PagedResultOutput<CreateOrUpdateCustomerInput> GetNoBindCustomerList(GetCustomerInput input);
        PagedResultOutput<UserOrdersDto> GetUserOrders(GetOrderListInput input);
        FileDto ExportOrderInfo(GetOrderListInput input);
    }
}
