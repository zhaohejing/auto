using Abp.Dependency;
using Abp.Timing;
using MyCompanyName.AbpZeroTemplate.EntityFramework;
using MyCompanyName.AbpZeroTemplate.EntityModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate {
    public class SqlExecuter : ISqlExecuter {
        public IEnumerable<T> GetUserList<T>(string name) where T : class, new() {

            string usersql = $@" select au.*,ar.Name RoleName,ar.DisplayName RoleDisplayName from abpusers au left join abpuserroles 
aur on au.id = aur.UserId left join abproles ar on aur.RoleId = ar.Id where au.isdeleted = 0;";

            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, usersql);
            return ConvertToModel<T>(dt);
        }



        public IEnumerable<T> GetUsedCardList<T>( ) where T : class, new() {
            var sql = $@"select a.* from (select * from iccards where IsDeleted=0)  a left join
(select * from customers where IsDeleted=0) b on a.id=b.CardId where  b.Id Is null 
and a.State=1;";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            return ConvertToModel<T>(dt);
        }
        public int GetCustomerCard(string outCard)  {
            var sql = $@"select b.Id CustomerId from (select * from iccards where IsDeleted=0)  a left join
(select * from customers where IsDeleted=0) b on a.id=b.CardId where a.outCard='{outCard}' and a.State=1;";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            int id=0;
            if (int.TryParse(dt?.Rows[0]?.ItemArray[0]?.ToString(), out id)) {
                return id;
            }
            return 0;
        }
        public string GetCardCustomer(int customerId) {
            var sql = $@"select a.outCard  from (select * from iccards where IsDeleted=0)  a left join
(select * from customers where IsDeleted=0) b on a.id=b.CardId where b.Id={customerId} and a.State=1;";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            return dt?.Rows[0]?.ItemArray[0]?.ToString(); 
        }
        public IEnumerable<T> GetOrderCardUser<T>( ) where T : class, new() {
            var sql = $@"select b.Id,a.InCard ,a.OutCard,a.Balance,a.Integral,b.CustomerName,
b.CustomerPhone,b.PointId,b.PointName from iccards a left join customers b on a.Id=b.CardId   
where a.IsDeleted=0 and b.IsDeleted=0 and a.State=1;";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            return ConvertToModel<T>(dt);
        }
        public IEnumerable<T> GetprePaidList<T>() where T : class, new() {
            var sql = $@"select a.RechargeNumber,c.CustomerName,b.OutCard,e.TopUp,e.WithDrawal,a.CreationTime,d.UserName
from rechargerrecords a inner join (select * from iccards where isdeleted=0) b on a.CardId =b.Id inner join
(select * from customers where isdeleted=0) c on b.Id=c.CardId left join abpusers d on a.CreatorUserId=d.Id
inner join (select RechargeNumber, max(case State when 1 then RechargeCost else 0 end ) TopUp,
max(case State when 2 then RechargeCost else 0 end ) WithDrawal, max(creationTime) CreationTIme
  from rechargerrecords group by RechargeNumber) e on a.RechargeNumber =e.RechargeNumber;";

            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            return ConvertToModel<T>(dt);
        }


        public int InsertOrder(CreateOrderInput input) {
            string sql = string.Empty;
            decimal totalprice = 0.0M;


            totalprice += input.Price * input.Number;
            var order = Guid.NewGuid( ).ToString("N");
            sql += $@"insert into orders(OrderNumber,CustomerId,State,DishId,Dish,DishNumber,OrderTime,PointId,PointName
,OrderAmount,AmountPayable,DishCost,CreationTime,CreatorUserId,IsDeleted) values('{order}',{input.CustomerId},
{(int)OrderState.NewOrder},{input.DishId},'{input.Dish}',{input.Number},'{input.OrderTime}',
{input.PointId},'{input.PointName}',{totalprice},{totalprice},{input.Price},'{DateTime.Now}',{input.CreateUserId},0) ;";
            var dt = MySqlHelper.ExecteNonQuery(CommandType.Text, sql);
            return dt;
        }
        public IEnumerable<T> GetUserOrders<T>(DateTime? startTime, DateTime? end,
            PayState? payState, OrderState? orderState, string dishName,
            string customerName, string cardName, string deviceName) where T : class, new() {
            var parm = string.Empty;
            if (startTime.HasValue) {
                var t = Clock.Normalize((DateTime)startTime);
                parm += $@" and a.OrderTime >='{t}' ";
            }
            if (end.HasValue) {
                var c = Clock.Normalize((DateTime)end);
                var t = Convert.ToDateTime(c.ToString("yyyy-MM-dd") + " 23:59:59");

                parm += $@" and a.OrderTime <='{t}' ";
            }
            if (payState.HasValue && payState > 0) {
                parm += $@" and d.PayState ={(int)payState} ";
            }
            if (orderState.HasValue && orderState > 0) {
                parm += $@" and a.State ={(int)orderState} ";
            }
            if (!string.IsNullOrWhiteSpace(dishName)) {
                parm += $@" and a.Dish  like '%{dishName}%' ";
            }
            if (!string.IsNullOrWhiteSpace(customerName)) {
                parm += $@" and b.customerName  like '%{customerName}%' ";
            }
            if (!string.IsNullOrWhiteSpace(cardName)) {
                parm += $@" and c.InCard  like '%{cardName}%' ";
            }
            if (!string.IsNullOrWhiteSpace(deviceName)) {
                // parm += $@" and c.InCard  like '%{deviceName}%' ";
                parm += $@"and a.pointName like '{deviceName}'";
            }
            var sql = $@"select a.Id,b.CustomerState, b.customerName,c.OutCard,a.State,a.Dish,a.DishNumber,
a.OrderTime,a.CreationTime,a.OrderAmount,a.AmountPayable,
a.PointName,d.PayState,e.Name 
from (select * from orders where isdeleted=0) a inner join (select * from customers where isdeleted=0)  b on a.CustomerId =b.Id inner join iccards c
on b.CardId=c.Id inner join (select a.* from orderpaylists a  inner join (  select orderId,max(creationTime) creationTime  from orderpaylists group by orderId) 
b on a.creationTime=b.creationTime and a.orderId=b.orderId) d on a.Id=d.OrderId left join abpusers e on a.CreatorUserId=e.Id
  where b.IsDeleted=0 and c.IsDeleted=0 {parm} ;";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            return ConvertToModel<T>(dt);
        }

        public IEnumerable<TempOrder> GetUserOrders(int customerId, DateTime? start) {
            var parm = string.Empty;
            if (start.HasValue) {
                var t = (DateTime)start;
                var first = t.AddDays(1 - t.Day);
                string left = first.ToString("yyyy/MM/dd");
                parm += $@"and  date_format(OrderTime,'%Y/%m/%d') >='{left}' ";
                DateTime endMonth = first.AddMonths(1).AddDays(-1);
                parm += $@"and  date_format(OrderTime,'%Y/%m/%d') <='{endMonth}' ";
            }
            // string start = DateTime.Now.ToString("yyyy/MM/dd");
            var sql = $@"select Id,OrderTime,Dish,State,DishCost,DishNumber from orders where isdeleted=0 and  CustomerId={customerId}  {parm};";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            return ConvertToModel<TempOrder>(dt);
        }

        public IEnumerable<T> GetCardList<T>( ) where T : class, new() {
            var sql = $"select * from iccards where IsDeleted=0";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            return ConvertToModel<T>(dt);
        }
        public IEnumerable<T> GetCustomerList<T>( ) where T : class, new() {
            var sql = $@"select a.Id,a.CustomerState, a.CustomerName,a.CustomerPhone,a.Birthday,a.Address,a.CardId,b.outCard,
a.PointId,a.PointName,b.Balance from customers a left join (select * from iccards where isdeleted=0) b on a.CardId = b.id where a.IsDeleted=0;";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            return ConvertToModel<T>(dt);
        }
        public IEnumerable<T> GetNoBindCustomerList<T>( ) where T : class, new() {
            var sql = $@"select a.Id, a.CustomerName,a.CustomerPhone,a.Birthday,a.Address,a.CardId,b.outCard,
a.PointId,a.PointName,b.Balance from customers a left join (select * from iccards where isdeleted=0) b on a.CardId = b.id where a.IsDeleted=0 and b.id is null;";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            return ConvertToModel<T>(dt);
        }

        public IEnumerable<T> GetDayDueList<T>(DateTime? startTime, DateTime? end, string dishName) where T : class, new() {
            var parm = string.Empty;

            if (startTime.HasValue) {
                var t = Clock.Normalize((DateTime)startTime);
                parm += $@" and a.OrderTime >='{t}' ";
            }
            if (end.HasValue) {
                var c = Clock.Normalize((DateTime)end);
                var t = Convert.ToDateTime(c.ToString("yyyy-MM-dd") + " 23:59:59");

                parm += $@" and a.OrderTime <='{t}' ";
            }

            if (!string.IsNullOrWhiteSpace(dishName)) {
                parm += $@" and a.Dish  like '%{dishName}%' ";
            }

            var sql = $@"select a.Dish, sum(a.DishNumber) as DishNumber,a.OrderTime,Sum(a.OrderAmount) as OrderAmount,a.DishCost
from orders a inner join customers b on a.CustomerId =b.Id inner join iccards c
on b.CardId=c.Id  where b.IsDeleted=0 and c.IsDeleted=0  and  a.State in (2) {parm}
group by a.Dish, a.OrderTime,a.DishCost;";

        
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            return ConvertToModel<T>(dt);
        }
        public IEnumerable<T> GetCustomOrderList<T>(DateTime? startTime, DateTime? end, string cardName) where T : class, new() {

            var parm = string.Empty;

            if (startTime.HasValue) {
                var t = Clock.Normalize((DateTime)startTime);
                parm += $@" and a.OrderTime >='{t}' ";
            }
            if (end.HasValue) {
                var c = Clock.Normalize((DateTime)end);
              var   t = Convert.ToDateTime(c.ToString("yyyy-MM-dd") + " 23:59:59");

                parm += $@" and a.OrderTime <='{t}' ";
            }

            //if (!string.IsNullOrWhiteSpace(cardName)) {
            //    parm += $@" and c.InCard  like '%{cardName}%' ";
            //} 

            var sql = $@"select a.Id,a.OrderNumber, b.customerName,c.OutCard,a.State,
a.OrderTime,a.CreationTime,a.OrderAmount,a.AmountPayable,d.PayState,e.Name 
from orders a inner join customers b on a.CustomerId =b.Id inner join iccards c
on b.CardId=c.Id inner join orderpaylists d on a.Id=d.OrderId left join abpusers e on a.CreatorUserId=e.Id
  where b.IsDeleted=0 and c.IsDeleted=0 {parm};";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            return ConvertToModel<T>(dt);
        }

        public IEnumerable<T> GetPointOrderList<T>(DateTime? startTime, DateTime? end, string cardName) where T : class, new() {

            var parm = string.Empty;
            if (startTime.HasValue) {
                var t = Clock.Normalize((DateTime)startTime);
                parm += $@" and a.OrderTime >='{t}' ";
            }
            if (end.HasValue) {
                var c = Clock.Normalize((DateTime)end);
                var t = Convert.ToDateTime(c.ToString("yyyy-MM-dd") + " 23:59:59");
                parm += $@" and a.OrderTime <='{t}' ";
            }

            if (!string.IsNullOrWhiteSpace(cardName)) {
                parm += $@" and c.InCard  like '%{cardName}%' ";
            }

            var sql = $@"select a.PointName,a.Dish,sum(a.DishNumber) as DishNumber,a.OrderTime
from orders a inner join customers b on a.CustomerId =b.Id inner join iccards c
on b.CardId=c.Id inner join orderpaylists d on a.Id=d.OrderId left join abpusers e on a.CreatorUserId=e.Id
  where  a.State in (2) and b.IsDeleted=0 and c.IsDeleted=0  {parm}
  group by  a.PointName,a.Dish, a.OrderTime;";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            return ConvertToModel<T>(dt);
        }

     
        public static List<T> ConvertToModel<T>(DataTable dt) where T : class, new() {
            // 定义集合    
            List<T> ts = new List<T>( );

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows) {
                T t = new T( );
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType( ).GetProperties( );
                foreach (PropertyInfo pi in propertys) {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    

                    if (dt.Columns.Contains(tempName)) {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        public IEnumerable<T> GetCheekList<T>(DateTime? startTime, DateTime? end, string filter) where T : class, new() {
            string parms = string.Empty;
            if (startTime.HasValue) {
                var t = Clock.Normalize((DateTime)startTime);
                parms += $@" and a.OrderTime >='{t}' ";
            }
            if (end.HasValue) {
                var c = Clock.Normalize((DateTime)end);
                var t = Convert.ToDateTime(c.ToString("yyyy-MM-dd") + " 23:59:59");
                parms += $@" and a.OrderTime <='{t}' ";
            }

            if (!string.IsNullOrWhiteSpace(filter)) {
                parms += $@" and a.PointName  like '{filter}' ";
            }

            var sql = $@"select a.PointId,max(a.PointName) PointName,b.CustomerState,sum(a.DishNumber) Count,sum(a.AmountPayable)  Payfor
from orders a inner join customers b
on a.CustomerId =b.Id where a.IsDeleted=0 and b.Isdeleted=0 and a.State in (2,3)  {parms} group by a.PointId,b.CustomerState ;";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            return ConvertToModel<T>(dt);
        }


        public IEnumerable<T> GetPointOrders<T>(DateTime? startTime, DateTime? end, string filter) where T : class, new() {
            string parms = string.Empty;
            if (startTime.HasValue) {
                var t = Clock.Normalize((DateTime)startTime);
                parms += $@" and a.OrderTime >='{t}' ";
            }
            if (end.HasValue) {
                var c = Clock.Normalize((DateTime)end);
                var t = Convert.ToDateTime(c.ToString("yyyy-MM-dd") + " 23:59:59");
                parms += $@" and a.OrderTime <='{t}' ";
            }

            if (!string.IsNullOrWhiteSpace(filter)) {
                parms += $@" and a.PointName  like '{filter}' ";
            }
     
        var sql = $@"select a.PointId,max(a.PointName) PointName,b.CustomerState,sum(a.DishNumber) Count,sum(a.AmountPayable)  Payfor
from orders a inner join customers b
on a.CustomerId =b.Id where a.IsDeleted=0 and b.Isdeleted=0 and a.State in (2)  {parms} group by a.PointId,b.CustomerState ;";
            var dt = MySqlHelper.ExecuteDataTable(CommandType.Text, sql);
            return ConvertToModel<T>(dt);
        }
    }
}

