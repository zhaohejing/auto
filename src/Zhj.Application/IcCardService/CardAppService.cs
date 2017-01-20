using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.UI;
using MyCompanyName.AbpZeroTemplate.Authorization;
using MyCompanyName.AbpZeroTemplate.EntityModel;
using MyCompanyName.AbpZeroTemplate.IcCardService.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.IcCardService {
    public class CardAppService : AbpZeroTemplateAppServiceBase, ICardAppService {

        private readonly IRepository<IcCard> _cardRepository;
        private readonly IRepository<Customers> _customerRepository;
        private readonly IRepository<OrderPayList> _orderPayRepository;
        private readonly IRepository<RechargerRecord> _rechargerRepository;
        private readonly IRepository<Orders> _orderRepository;
        private readonly ISqlExecuter _sqlHelper;
        public CardAppService(IRepository<IcCard> cardRepository,
            ISqlExecuter sqlHelper, IRepository<OrderPayList> orderPayRepository,
            IRepository<Orders> orderRepository, IRepository<RechargerRecord> rechargerRepository,
            IRepository<Customers> customerRepository) {
            _cardRepository = cardRepository;
            _sqlHelper = sqlHelper;
            _orderPayRepository = orderPayRepository;
            _orderRepository = orderRepository;
            _rechargerRepository = rechargerRepository;
            _customerRepository = customerRepository;
        }
        public async Task ActionCard(ActionCardInput input) {
            var model = await _cardRepository.FirstOrDefaultAsync(input.CardId);
            if (model == null) {
                throw new UserFriendlyException(L("ThereIsNoAny"));
            }
            if (input.Type == RecordState.TopUp) {//充值
              
                var guid = Guid.NewGuid( ).ToString("N");
                await _rechargerRepository.InsertAsync(new RechargerRecord( ) {
                    CardId = model.Id,
                    RechargeCost = input.Nums,
                    RechargeNumber = guid,
                    State = RecordState.TopUp
                });
                model.Balance += input.Nums;
              await  CurrentUnitOfWork.SaveChangesAsync();
            }
            else if (input.Type == RecordState.WithDrawal) {
                if (model.Balance < input.Nums) {
                    throw new UserFriendlyException(L("ThereHadNoMuchMoney"));
                }
               
                var guid = Guid.NewGuid( ).ToString("N");
                await _rechargerRepository.InsertAsync(new RechargerRecord( ) {
                    CardId = model.Id,
                    RechargeCost = input.Nums,
                    RechargeNumber = guid,
                    State = RecordState.WithDrawal
                });
                model.Balance -= input.Nums;
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            else if (input.Type == RecordState.OffCard) {
                if (model.Balance > 0) {
                    throw new UserFriendlyException(L("ThereHasMoney"));
                }
                await _cardRepository.DeleteAsync(model);
                var guid = Guid.NewGuid( ).ToString("N");
                await _rechargerRepository.InsertAsync(new RechargerRecord( ) {
                    CardId = model.Id,
                    RechargeCost = input.Nums,
                    RechargeNumber = guid,
                    State = RecordState.OffCard
                });
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 退订订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
      
        public async Task BackOffOrder(IdInput<int> input) {

            var order = await _orderRepository.FirstOrDefaultAsync(input.Id);
            if (order == null) {
                throw new UserFriendlyException(L("ThereIsNoAny"));
            }


            if (DateTime.Now.Hour>=15) {

                if (order.OrderTime.Date <= DateTime.Now.AddDays(1).Date) {
                    throw new UserFriendlyException(L("OnlyBackTomorrowDish"));
                }

            }

            if (order.OrderTime.Date <= DateTime.Now.Date) {
                throw new UserFriendlyException(L("OnlyBackTomorrowDish"));
            }

            var cus = await _customerRepository.FirstOrDefaultAsync(order.CustomerId);
            var card = await _cardRepository.FirstOrDefaultAsync(cus.CardId);
            if ( card == null || cus == null) {
                throw new UserFriendlyException(L("ThereIsNoAny"));
            }
            order = _orderRepository.FirstOrDefault(input.Id);
            if (order.State!=OrderState.UserCancelled) {
                await _orderPayRepository.InsertAsync(new OrderPayList() {
                    Cost = order.AmountPayable,
                    OrderId = order.Id,
                    PayState = PayState.BackMoney
                });
                order.State = OrderState.UserCancelled;

                card.Balance += order.AmountPayable;
                await CurrentUnitOfWork.SaveChangesAsync();
            }
          
        }
        [AbpAuthorize(AppPermissions.Pages_TradingInfo_BeBackOff)]
        public async Task BeBackOffOrder(IdInput<int> input) {
            var order = await _orderRepository.FirstOrDefaultAsync(input.Id);
            //if (order.OrderTime.Date < DateTime.Now.Date) {
            //    throw new UserFriendlyException(L("OnlyBackCurrentDish"));
            //}
            var cus = await _customerRepository.FirstOrDefaultAsync(order.CustomerId);
            var card = await _cardRepository.FirstOrDefaultAsync(cus.CardId);
            if (order == null || card == null || cus == null) {
                throw new UserFriendlyException(L("ThereIsNoAny"));
            }
            order = _orderRepository.FirstOrDefault(input.Id);
            if (order.State!=OrderState.UserCancelled) {
                await _orderPayRepository.InsertAsync(new OrderPayList() {
                    Cost = order.AmountPayable,
                    OrderId = order.Id,
                    PayState = PayState.BackMoney
                });
                order.State = OrderState.UserCancelled;

                card.Balance += order.AmountPayable;
                await CurrentUnitOfWork.SaveChangesAsync();
            }
          
        }

        public async Task<dynamic> UpdateMeal(string orderid) {
            var order = await _orderRepository.FirstOrDefaultAsync(c => c.OrderNumber.Equals(orderid));
            if (order == null) {
                return new {
                    status_code = 400,
                    result = false
                };
            }
            order.TakeOff++;
            if (order.TakeOff==order.DishNumber) {
                order.State = OrderState.HasTheDelivery;
            }
            await CurrentUnitOfWork.SaveChangesAsync( );
            return new {
                status_code = 200,
                result = true
            };
        }
        /// <summary>
        /// 获取点餐信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public dynamic GetMealInfos(GeTMealInfoInput input) {
            var customer = _sqlHelper.GetOrderCardUser<OrderCardUserDto>( ).FirstOrDefault(c => c.InCard.Equals(input.cardId));
            if (customer == null) {
                return new {
                    status_code = 401,
                    message = "当前订单不存在"
                };
            }
            var list = _orderRepository.GetAll( ).Where(c => c.State == OrderState.PayforSuccess).ToList( );
            DateTime now = DateTime.Now.Date;
            if (input.date.HasValue) {
                var t = (DateTime)input.date;
                now = t.Date;
            }

            var res = list.Where(c => c.OrderTime == now && c.CustomerId == customer.Id)
                .WhereIf(input.pointId > 0, c => c.PointId == input.pointId).ToList( );
            if (res == null || res.Count <= 0) {
                return new {
                    status_code = 200,
                    items = new List<int>( )
                };
            }
            var result = res.Select(c => new {
                ponitid = c.PointId,
                dishid = c.DishId,
                orderid = c.OrderNumber
            });
            return new {
                status_code = 200,
                items = result
            };
        }


        public PagedResultOutput<IcCard> GetCardList(GetCardInput input) {
            var list = _sqlHelper.GetCardList<IcCard>( );
            if (list == null || list.Count( ) <= 0) {
                return new PagedResultOutput<IcCard>(0, null);
            }
            var res = list.WhereIf(!input.InNum.IsNullOrWhiteSpace( ), c => c.inCard.Contains(input.InNum))
               .WhereIf(!input.OutNum.IsNullOrWhiteSpace( ), c => c.outCard.Contains(input.OutNum));
            var count = res.Count( );
            var l = res.OrderByDescending(c => c.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount)
              .ToList( );
            return new PagedResultOutput<IcCard>(count, l);
        }
        public PagedResultOutput<IcCard> GetUsedCardList(GetCardInput input) {
            var list = _sqlHelper.GetUsedCardList<IcCard>( );
            if (list == null || list.Count( ) <= 0) {
                return new PagedResultOutput<IcCard>(0, null);
            }
            list = list.WhereIf(!input.OutNum.IsNullOrWhiteSpace(), c => c.outCard.Contains(input.OutNum));
            var count = list.Count( );
            var l = list.OrderByDescending(c => c.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount)
              .ToList( );
            return new PagedResultOutput<IcCard>(count, l);
        }
        public dynamic GetPointList( ) {
            var url = "http://api.efanji.com/api/GetPointList";
            var res = HttpConnectToServer(url, string.Empty);
            var temp = JsonConvert.DeserializeObject<dynamic>(res);
            return temp;

        }

        public dynamic GetMenuList(GetMenubyDateInput input) {
            var url = "http://api.efanji.com/api/GetMenuList";
            var res = HttpConnectToServer(url, $"pointid={input.pointId}&date={input.date}");
            var temp = JsonConvert.DeserializeObject<dynamic>(res);
            return temp;
      
        }
        /// <summary>
        /// 发送消息到ws服务器
        /// </summary>
        /// <param name="ServerPage"></param>
        /// <param name="strXml"></param>
        /// <returns></returns>
        private string HttpConnectToServer(string ServerPage, string strXml) {
            string postData = strXml;

            byte[] dataArray = Encoding.Default.GetBytes(postData);
            //创建请求
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ServerPage);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.CookieContainer = new CookieContainer( );
            request.Method = "POST";
            request.ContentLength = dataArray.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            //创建输入流
            Stream dataStream = null;
            try {
                dataStream = request.GetRequestStream( );
            }
            catch (Exception) {
                return null;//连接服务器失败
            }

            //发送请求
            dataStream.Write(dataArray, 0, dataArray.Length);
            dataStream.Close( );
            //读取返回消息
            string res = string.Empty;
            try {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse( );
                StreamReader reader = new StreamReader(response.GetResponseStream( ), Encoding.UTF8);
                res = reader.ReadToEnd( );
                reader.Close( );
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }

            return res;
        }

        public bool InsertOrder(CreateOrderInput input) {
            input.CreateUserId = (long)AbpSession.UserId;
            var res = _sqlHelper.InsertOrder(input);
            return res > 0;
        }
        public ListResultOutput<TempOrder> GetUserNoPayOrders(GetUserOrderInput input) {
            var res = _sqlHelper.GetUserOrders(input.CustomerId, input.Time);
            var a = res.Where(c => c.State == OrderState.NewOrder).ToList( );
            return new ListResultOutput<TempOrder>(a);
        }
        public async Task<dynamic> PayForOrders(PayForOrderInput input) {
            if (input.Orders == null || input.Orders.Count <= 0) {
                return new { error = "订单不存在" };
              //  throw new UserFriendlyException(L("ThereIsNoAny"));
            }
            if (input.PayState == PayState.Cash) {

                foreach (var order in input.Orders) {
                    await _orderPayRepository.InsertAsync(new OrderPayList( ) {
                        OrderId = order.Id,
                        Cost = order.DishCost,
                        PayState = input.PayState
                    });
                    var or = await _orderRepository.FirstOrDefaultAsync(order.Id);
                    or.State = OrderState.PayforSuccess;
                }
            }
            else if (input.PayState == PayState.MembershipCard) {

                var card = await _cardRepository.FirstOrDefaultAsync(c => c.outCard == input.InCard);

                if (card.Balance < input.TotalPrice) {
                    foreach (var order in input.Orders) {
                     await   _orderRepository.DeleteAsync(order.Id);
                      
                    }
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return new { error = "卡余额不足,请充值" };
                }
                else {

                    foreach (var order in input.Orders) {
                        var pay = await _orderPayRepository.FirstOrDefaultAsync(c => c.OrderId == order.Id);
                        if (pay!=null) {
                            continue;
                        }
                        await _orderPayRepository.InsertAsync(new OrderPayList() {
                            OrderId = order.Id,
                            Cost = order.DishCost,
                            PayState = input.PayState
                        });
                        card.Balance -= order.DishCost;
                        var or = await _orderRepository.FirstOrDefaultAsync(order.Id);
                        or.State = OrderState.PayforSuccess;
                    }

                   // card.Balance -= input.TotalPrice;

                }

            }
            await CurrentUnitOfWork.SaveChangesAsync( );
            return new { error = "成功" };
        }

        public ListResultOutput<UserOrderDto> GetUserOrders(GetUserOrderInput input) {
      
            var list = _sqlHelper.GetUserOrders(input.CustomerId, input.Time);
            var res = new List<UserOrderDto>( );
            if (list != null && list.Count( ) > 0) {
                foreach (var item in list) {
                    var d = new UserOrderDto( ) { date = item.OrderTime, events = new List<PayOrder>( ) };
                    foreach (var t in list) {
                        if (d.date == t.OrderTime) {
                            d.events.Add(new PayOrder { name = t.Dish, type = t.State });
                        }
                    }
                    res.Add(d);
                }
                return new ListResultOutput<UserOrderDto>(res);
            }
            return new ListResultOutput<UserOrderDto>( );
        }
        public ListResultOutput<OrderCardUserDto> GetOrderCardUser(GetOrderCardUser input) {
            var list = _sqlHelper.GetOrderCardUser<OrderCardUserDto>( );
            if (list != null && list.Count( ) > 0) {
                list = list.WhereIf(!input.Filter.IsNullOrWhiteSpace( ),
                      c => c.OutCard.Contains(input.Filter) || c.CustomerName.Contains(input.Filter));
                return new ListResultOutput<OrderCardUserDto>(list.ToList( ));
            }
            return new ListResultOutput<OrderCardUserDto>( );
        }


        public async Task InsertCard(CreateOrUpdateCardInput input) {
            var model = input.MapTo<IcCard>( );
            if (input.Id.HasValue) {
                await _cardRepository.UpdateAsync(model);
            }
            else {
                await _cardRepository.InsertAsync(model);

            }
        }

        public async Task DeleteCard(IdInput<int> input) {

            var user = await _cardRepository.FirstOrDefaultAsync(input.Id);
            if (user == null) {
                throw new UserFriendlyException(L("ThereIsNoAny"));
            }
            if (user.Balance > 0) {
                throw new UserFriendlyException(L("ThereHasMoney"));
            }
            await _cardRepository.DeleteAsync(user);
        }
    }
}

