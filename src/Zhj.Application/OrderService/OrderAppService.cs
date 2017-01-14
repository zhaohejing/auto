using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Timing;
using MyCompanyName.AbpZeroTemplate.CustomerService;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.EntityModel;
using MyCompanyName.AbpZeroTemplate.OrderService.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.OrderService {
    public class OrderAppService : AbpZeroTemplateAppServiceBase, IOrderAppService {

        private readonly ISqlExecuter _sqlExecuter;
        private readonly IOrderExcelExporter _orderExporter;
        private readonly IRepository<Orders> _orderRepository;
        public OrderAppService(ISqlExecuter sqlExecuter,
            IOrderExcelExporter orderExporter, IRepository<Orders> orderRepository) {
            _sqlExecuter = sqlExecuter;
            _orderExporter = orderExporter;
            _orderRepository = orderRepository;

        }
        public PagedResultOutput<DayDueDto> GetDaydueList(GetOrderInput input) {

            var list = _sqlExecuter.GetDayDueList<DayDueDto>(input.SearchStartTime, 
                input.SearchEndTime, input.Filter).OrderByDescending(c=>c.OrderTime)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), c => c.Dish.Contains(input.Filter)).ToList( );

            var totalcount = list.Count();
            var a = list.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();


            return new PagedResultOutput<DayDueDto>(totalcount, a);
        }
        public PagedResultOutput<OrderRecords> GetPointOrders(GetPointOrdersInput input) {
            var orders = _sqlExecuter.GetPointOrders<OrderRecords>(input.SearchStartTime,
                input.SearchEndTime, input.Filter);
            var totalcount = orders.Count();
            var a = orders.OrderBy(c=>c.PointName).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return new PagedResultOutput<OrderRecords>(totalcount, a);

        }
        public FileDto ExportPointOrders(GetPointOrdersInput input) {
            var orders = _sqlExecuter.GetPointOrders<OrderRecords>(input.SearchStartTime,
                input.SearchEndTime, input.Filter);
      
            return _orderExporter.ExportPointOrders(orders.ToList());
        }

        public PagedResultOutput<PrePaidDto> GetprePaidList(GetprePaidInput input) {
            var res = _sqlExecuter.GetprePaidList<PrePaidDto>();
            DateTime? start = null; DateTime? end = null;
            if (input.SearchStartTime.HasValue) {
                start = Clock.Normalize((DateTime)input.SearchStartTime);
            }
            if (input.SearchEndTime.HasValue) {
                end = Clock.Normalize((DateTime)input.SearchEndTime);

            }
            var a = res.WhereIf(!input.Name.IsNullOrWhiteSpace(), c => c.CustomerName.Contains(input.Name))
                .WhereIf(!input.Card.IsNullOrWhiteSpace(), c => c.OutCard.Contains(input.Card))
                .WhereIf(input.SearchStartTime.HasValue,c=>c.CreationTime>= start)
                .WhereIf(input.SearchEndTime.HasValue,c=>c.CreationTime<= end)
                ;
            var totalcount = a.Count();
            var list = a.OrderByDescending(c=>c.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();


            return new PagedResultOutput<PrePaidDto>(totalcount, list);
        }
        public FileDto ExportprePaidList(GetprePaidInput input) {
            var res = _sqlExecuter.GetprePaidList<PrePaidDto>();
            DateTime? start = null; DateTime? end=null;
            if (input.SearchStartTime.HasValue) {
                start = Clock.Normalize((DateTime)input.SearchStartTime);
            }
            if (input.SearchEndTime.HasValue) {
                end = Clock.Normalize((DateTime)input.SearchEndTime);

            }

            var a = res.WhereIf(!input.Name.IsNullOrWhiteSpace(), c => c.CustomerName.Contains(input.Name))
               .WhereIf(!input.Card.IsNullOrWhiteSpace(), c => c.OutCard.Contains(input.Card))
               .WhereIf(input.SearchStartTime.HasValue, c => c.CreationTime >= start)
               .WhereIf(input.SearchEndTime.HasValue, c => c.CreationTime <= end)
               ;

            return _orderExporter.ExportprePaidList(a.ToList());


        }
        public FileDto ExportOrderInfo(GetOrderInput input) {
            var list = _sqlExecuter.GetDayDueList<DayDueDto>(input.SearchStartTime, input.SearchEndTime, input.Filter).OrderByDescending(c => c.OrderTime)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), c => c.Dish.Contains(input.Filter)).ToList( );

            if (list == null || list.Count( ) <= 0) {
                return null;
            }
            int i = 0;
            foreach (var a in list) {
                a.serialNo = i + 1;
                i++;
            }

            return _orderExporter.ExportDayDueToFile(list.ToList());


        }
        public PagedResultOutput<CustomOrderDto> GetCustomOrderList(GetOrderInput input) {

            var list = _sqlExecuter.GetCustomOrderList<CustomOrderDto>(input.SearchStartTime, input.SearchEndTime, input.Filter).ToList()
              .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
              c => c.OutCard.Contains(input.Filter));

            var totalcount = list.Count();
            var a = list.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();


            return new PagedResultOutput<CustomOrderDto>(totalcount, a);
        }

        public FileDto ExportCustomOrder(GetOrderInput input) {
            var list = _sqlExecuter.GetCustomOrderList<CustomOrderDto>(input.SearchStartTime, input.SearchEndTime, input.Filter).ToList()
              .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
              c => c.OutCard.Contains(input.Filter));
            if (list == null || list.Count() <= 0) {
                return null;
            }
            int i = 0;
            foreach (var a in list) {
                a.serialNo = i + 1;
                i++;
            }
            return _orderExporter.ExportCustomOrderToFile(list.ToList());


        }

        public PagedResultOutput<PointOrderDto> GetPointOrderList(GetPointOrderInput input) {

            var list = _sqlExecuter.GetPointOrderList<PointOrderDto>(input.SearchStartTime, input.SearchEndTime, "").ToList( )
              .WhereIf(!string.IsNullOrWhiteSpace(input.PointName), c => c.PointName.Equals(input.PointName))
              .WhereIf(!string.IsNullOrWhiteSpace(input.DishName), c => c.Dish.Contains(input.DishName));

            var totalcount = list.Count();
            var a = list.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();


            return new PagedResultOutput<PointOrderDto>(totalcount, a);
        }
        public FileDto ExportPointOrder(GetPointOrderInput input) {
            var list = _sqlExecuter.GetPointOrderList<PointOrderDto>(input.SearchStartTime, input.SearchEndTime, "").ToList()
              .WhereIf(!string.IsNullOrWhiteSpace(input.PointName), c => c.PointName.Contains(input.PointName))
              .WhereIf(!string.IsNullOrWhiteSpace(input.DishName), c => c.Dish.Contains(input.DishName));
            if (list == null || list.Count() <= 0) {
                return null;
            }

            int i = 0;
            foreach (var a in list) {
                a.serialNo = i + 1;
                i++;
            }
            return _orderExporter.ExportPointOrderToFile(list.ToList());


        }
        public PagedResultOutput<OrderRecords> GetCheckLists(GetPointOrdersInput input) {

            var orders = _sqlExecuter.GetCheekList<OrderRecords>(input.SearchStartTime,
               input.SearchEndTime, input.Filter);
            var totalcount = orders.Count();
            var a = orders.OrderBy(c => c.PointName).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return new PagedResultOutput<OrderRecords>(totalcount, a);
        }
        public FileDto ExportCheckLists(GetPointOrdersInput input) {
            var orders = _sqlExecuter.GetCheekList<OrderRecords>(input.SearchStartTime,
               input.SearchEndTime, input.Filter);

            return _orderExporter.ExportPointOrders(orders.ToList());
        }

        public dynamic GetDepartmentList() {
            var url = "http://api.efanji.com/api/GetDepartmentList";
            var res = HttpConnectToServer(url, string.Empty);
            var temp = JsonConvert.DeserializeObject<dynamic>(res);
            return temp;

        }


        /// <summary>
        /// 发送消息到ws服务器
        /// </summary>
        /// <param name="ServerPage"></param>
        /// <param name="strXml"></param>
        /// <returns></returns>
        public string HttpConnectToServer(string ServerPage, string strXml) {
            string postData = strXml;

            byte[] dataArray = Encoding.Default.GetBytes(postData);
            //创建请求
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ServerPage);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.CookieContainer = new CookieContainer();
            request.Method = "POST";
            request.ContentLength = dataArray.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            //创建输入流
            Stream dataStream = null;
            try {
                dataStream = request.GetRequestStream();
            }
            catch (Exception) {
                return null;//连接服务器失败
            }

            //发送请求
            dataStream.Write(dataArray, 0, dataArray.Length);
            dataStream.Close();
            //读取返回消息
            string res = string.Empty;
            try {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                res = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }

            return res;
        }
      

    }
}
