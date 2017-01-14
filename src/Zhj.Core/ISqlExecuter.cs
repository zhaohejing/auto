using Abp.Dependency;
using MyCompanyName.AbpZeroTemplate.EntityModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate {
    public interface ISqlExecuter : ITransientDependency {
        IEnumerable<T> GetUserList<T>(string name) where T : class, new();
        IEnumerable<T> GetCardList<T>() where T : class, new();
        IEnumerable<T> GetUsedCardList<T>() where T : class, new();
        int GetCustomerCard(string outCard);
        IEnumerable<T> GetCheekList<T>(DateTime? startTime, DateTime? end, string filter) where T : class, new();

        string GetCardCustomer(int customerId);
       IEnumerable<T> GetCustomerList<T>() where T : class, new();
        IEnumerable<T> GetNoBindCustomerList<T>() where T : class, new();
        IEnumerable<T> GetOrderCardUser<T>() where T : class, new();
        int InsertOrder(CreateOrderInput input);
        IEnumerable<TempOrder> GetUserOrders(int customerId, DateTime? time);
        IEnumerable<T> GetUserOrders<T>(DateTime? startTime, DateTime? end,
            PayState? payState, OrderState? orderState, string dishName,
            string customerName, string cardName, string deviceName) where T : class, new();
        IEnumerable<T> GetDayDueList<T>(DateTime? startTime, DateTime? end, string dishName) where T : class, new();
        IEnumerable<T> GetCustomOrderList<T>(DateTime? startTime, DateTime? end, string cardName) where T : class, new();
        IEnumerable<T> GetPointOrderList<T>(DateTime? startTime, DateTime? end, string cardName) where T : class, new();
        IEnumerable<T> GetprePaidList<T>() where T : class, new();

        IEnumerable<T> GetPointOrders<T>(DateTime? startTime, DateTime? end, string filter) where T : class, new();
    }
}
