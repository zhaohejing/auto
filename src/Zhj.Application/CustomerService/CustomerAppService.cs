using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Runtime.Caching;
using Abp.Timing;
using Abp.UI;
using MyCompanyName.AbpZeroTemplate.CustomerService.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.CustomerService {
 
    public class CustomerAppService : AbpZeroTemplateAppServiceBase, ICustomerAppService {
        private readonly IOrderExcelExporter _orderExporter;
        private readonly IRepository<Customers> _customerRepository;
        private readonly IRepository<IcCard> _cardRepository;
        private readonly ISqlExecuter _sqlHelper;
        public CustomerAppService(IRepository<Customers> customerRepository, 
            ISqlExecuter sqlHelper,
            IOrderExcelExporter orderExporter,
             IRepository<IcCard> cardRepository) {
            _customerRepository = customerRepository;
            _sqlHelper = sqlHelper;
            _orderExporter = orderExporter;
            _cardRepository = cardRepository;
        }
        public PagedResultOutput<CreateOrUpdateCustomerInput> GetNoBindCustomerList(GetCustomerInput input) {
            var list = _sqlHelper.GetNoBindCustomerList<CreateOrUpdateCustomerInput>();
            if (list == null || list.Count() <= 0) {
                return new PagedResultOutput<CreateOrUpdateCustomerInput>(0, null);
            }
            var res = list
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), c => c.CustomerName.Contains(input.Name))
               .WhereIf(!input.Mobile.IsNullOrWhiteSpace(), c => c.CustomerPhone.Contains(input.Mobile))
               .WhereIf(!input.Card.IsNullOrWhiteSpace(), c => !c.OutCard.IsNullOrWhiteSpace() && c.OutCard.Contains(input.Card))
               .WhereIf(!input.Point.IsNullOrWhiteSpace(),c=>c.PointName.Contains(input.Point))
               ;
            var count = res.Count();
            var l = res.OrderByDescending(c => c.Id).Skip(input.SkipCount).Take(input.MaxResultCount)
              .ToList();
            return new PagedResultOutput<CreateOrUpdateCustomerInput>(count, l);
        }
        public FileDto ExportOrderInfo(GetOrderListInput input) {
            var list = _sqlHelper.GetUserOrders<UserOrdersDto>(input.StartTime, input.EndTime, input.PayState, input.OrderState,
           input.DishName, input.CustomerName, input.CardName, input.DeviceName);
            if (list == null || list.Count() <= 0) {
                return null;
            }
            return _orderExporter.ExportToFile(list.ToList());

            //var users = await UserManager.Users.Include(u => u.Roles).ToListAsync();
            //var userListDtos = users.MapTo<List<UserListDto>>();
            //await FillRoleNames(userListDtos);
          
        }



        public PagedResultOutput<UserOrdersDto> GetUserOrders(GetOrderListInput input) {
            var list = _sqlHelper.GetUserOrders<UserOrdersDto>(input.StartTime,input.EndTime,input.PayState,input.OrderState,
                input.DishName,input.CustomerName,input.CardName,input.DeviceName);

            if (list == null || list.Count() <= 0) {
                return new PagedResultOutput<UserOrdersDto>(0, null);
            }
            list = list.WhereIf(!input.DeviceName.IsNullOrWhiteSpace(), c => c.PointName.Contains(input.DeviceName));
            var count = list.Count();
            var l = list.OrderByDescending(c => c.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount)
              .ToList();
            return new PagedResultOutput<UserOrdersDto>(count, l);
        }


        public PagedResultOutput<CreateOrUpdateCustomerInput> GetCustomerList(GetCustomerInput input) {
            var list = _sqlHelper.GetCustomerList<CreateOrUpdateCustomerInput>();
            if (list == null || list.Count() <= 0) {
                return new PagedResultOutput<CreateOrUpdateCustomerInput>(0, null);
            }
            var res = list.WhereIf(!input.Name.IsNullOrWhiteSpace(), c => c.CustomerName.Contains(input.Name))
               .WhereIf(!input.Mobile.IsNullOrWhiteSpace(), c => c.CustomerPhone.Contains(input.Mobile))
               .WhereIf(!input.Card.IsNullOrWhiteSpace(), c => !c.OutCard.IsNullOrWhiteSpace()&&c.OutCard.Contains(input.Card))
               .WhereIf(!input.Point.IsNullOrWhiteSpace(), c => c.PointName.Contains(input.Point))
               ;
            var count = res.Count();
            var l = res.OrderByDescending(c => c.Id).Skip(input.SkipCount).Take(input.MaxResultCount)
              .ToList();
            return new PagedResultOutput<CreateOrUpdateCustomerInput>(count, l);
        }

        public async Task InsertCustomer(CreateOrUpdateCustomerInput input) {
          
            var model = input.MapTo<Customers>();
            model.Birthday = Clock.Normalize(input.Birthday);

            if (input.Id.HasValue) {
                var ot = _sqlHelper.GetCardCustomer((int)input.Id);
                if (!string.IsNullOrWhiteSpace(ot)&&!ot.Equals(input.OutCard)) {
                    var m = _sqlHelper.GetCustomerCard(input.OutCard);
                    if (m > 0) {
                        throw new UserFriendlyException(L("TheCardHadBind"));
                    }

                }
                    await _customerRepository.UpdateAsync(model);

            }
            else {
                var m = _sqlHelper.GetCustomerCard(input.OutCard);
                if (m > 0) {
                    throw new UserFriendlyException(L("TheCardHadBind"));
                }
                await _customerRepository.InsertAsync(model);

            }
        }

        public async Task DeleteCustomer(IdInput<int> input) {

            var user = await _customerRepository.FirstOrDefaultAsync(input.Id);
            if (user == null) {
                throw new UserFriendlyException(L("ThereIsNoAny"));
            }
            await _customerRepository.DeleteAsync(user);
        }
    }
}
