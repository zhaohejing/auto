using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.EntityModel;
using MyCompanyName.AbpZeroTemplate.IcCardService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.IcCardService {
   public interface ICardAppService:IApplicationService {
        PagedResultOutput<IcCard> GetCardList(GetCardInput input);
        Task InsertCard(CreateOrUpdateCardInput input);
        Task DeleteCard(IdInput<int> input);
        PagedResultOutput<IcCard> GetUsedCardList(GetCardInput input);
        Task ActionCard(ActionCardInput input);
        ListResultOutput<OrderCardUserDto> GetOrderCardUser(GetOrderCardUser input);
        dynamic GetMenuList(GetMenubyDateInput input);
        bool InsertOrder(CreateOrderInput input);
        ListResultOutput<UserOrderDto> GetUserOrders(GetUserOrderInput input);
        ListResultOutput<TempOrder> GetUserNoPayOrders(GetUserOrderInput input);
        Task<dynamic> PayForOrders(PayForOrderInput input);
        Task BackOffOrder(IdInput<int> input);
        Task BeBackOffOrder(IdInput<int> input);
        Task<dynamic> UpdateMeal(string orderid);
        dynamic GetPointList();
        dynamic GetMealInfos(GeTMealInfoInput input);
    }
}
