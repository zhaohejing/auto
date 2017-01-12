using MyCompanyName.AbpZeroTemplate.CustomerService.Dtos;
using MyCompanyName.AbpZeroTemplate.DataExporting.Excel.EpPlus;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.EntityModel;
using MyCompanyName.AbpZeroTemplate.Enum;
using MyCompanyName.AbpZeroTemplate.OrderService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.CustomerService {
    public class OrderExcelExporter : EpPlusExcelExporterBase, IOrderExcelExporter {
        public FileDto ExportToFile(List<UserOrdersDto> userListDtos) {
            return CreateExcelPackage(
                "客户订单.xlsx",
                excelPackage => {
                    var sheet = excelPackage.Workbook.Worksheets.Add("客户订单");
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        "客户名称",
                        "会员卡号",
                        "订单状态",
                        "产品名称",
                        "数量",
                        "创建时间",
                        "预定时间",
                        "订单金额",
                        "应付金额",
                        "点位名称",
                        "支付渠道",
                        "操作人"
                        );
                    AddObjects(
                        sheet, 2, userListDtos,
                        _ => _.CustomerName,
                        _ => _.OutCard,
                        _ => ChangeOrderState(_.State),
                        _ => _.Dish,
                        _ => _.DishNumber,
                        _ => _.CreationTime,
                        _ => _.OrderTime,
                        _ => _.OrderAmount,
                        _ => _.AmountPayable,
                        _ => _.PointName,
                        _ => ChangePayState(_.PayState),
                        _ => _.Name
                        );


                    //Formatting cells

                    var creationName = sheet.Column(6);
                    creationName.Style.Numberformat.Format = "yyyy-MM-dd HH:mm";

                    var orderTime = sheet.Column(7);
                    orderTime.Style.Numberformat.Format = "yyyy-MM-dd";
                    for (var i = 1; i <= 12; i++) {
                        sheet.Column(i).AutoFit();
                    }
                });
        }

        public FileDto ExportDayDueToFile(List<DayDueDto> userListDtos) {
            return CreateExcelPackage(
                "日预定商品.xlsx",
                excelPackage => {
                    var sheet = excelPackage.Workbook.Worksheets.Add("日预定商品");
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        "序号",
                        "商品名称",
                        "商品数量",
                        "单价",
                        "总计",
                        "预定时间"
                        );
                    AddObjects(
                        sheet, 2, userListDtos,
                        _ => _.serialNo,
                        _ => _.Dish,
                        _ => _.DishNumber,
                        _ => _.DishCost,
                        _ => _.OrderAmount,
                        _ => _.OrderTime
                        );

                    var row = userListDtos.Count + 2;

                    var sump = userListDtos.Sum(c => c.DishNumber).ToString();

                    var oump = userListDtos.Sum(c => c.OrderAmount).ToString();

                    AddFooters(sheet, row,
                 "总计",
                 string.Empty,
                 sump,
                 string.Empty,
                 oump,
                 string.Empty
                 );
                    //Formatting cells

                    var creationName = sheet.Column(6);
                    creationName.Style.Numberformat.Format = "yyyy-MM-dd";


                });
        }

        public FileDto ExportprePaidList(List<PrePaidDto> predto) {
            return CreateExcelPackage(
              "会员充值提现表.xlsx",
              excelPackage => {
                  var sheet = excelPackage.Workbook.Worksheets.Add("会员充值提现");
                  sheet.OutLineApplyStyle = true;

                  AddHeader(
                      sheet,
                      "姓名",
                      "卡号",
                      "充值(元)",
                      "提现(元)",
                      "操作人",
                      "操作时间"
                      );
                  var top = predto.Sum(c => c.TopUp);
                  var wit = predto.Sum(c => c.WithDrawal);
                  AddObjects(
                      sheet, 2, predto,
                      _ => _.CustomerName,
                      _ => _.OutCard,
                      _ => _.TopUp,
                      _ => _.WithDrawal,
                      _ => _.UserName,
                      _ => _.CreationTime.ToString("yyyy-MM-dd")
                      );
                  AddFooters(sheet, predto.Count + 2,
                      "总计",
                      string.Empty,
                      top.ToString(),
                      wit.ToString(),
                      string.Empty,
                      string.Empty
                      );
              });
        }

        public FileDto ExportPointOrders(List<OrderRecords> orders) {
            return CreateExcelPackage(
               "销售量日报表.xlsx",
               excelPackage => {
                   var sheet = excelPackage.Workbook.Worksheets.Add("销售量日报表");
                   sheet.OutLineApplyStyle = true;

                   AddHeader(
                       sheet,
                       "序号",
                       "点位名称",
                       "用户类型",
                       "数量(份)",
                       "总价(元)"
                       );
                   AddObjects(
                       sheet, 2, orders,
                       _ => _.PointId,
                       _ => _.PointName,
                           _ => _.CustomerState == UserState.CardUser ? "社区员工" : (_.CustomerState == UserState.CompanyUser ? "公司员工" : "普通用户"),
                       _ => _.Count,
                       _ => _.Payfor
                       );

                   var row = orders.Count + 2;
                   var sumc = orders.Sum(c => c.Count).ToString();
                   var sump = orders.Sum(c => c.Payfor).ToString();
                   AddFooters(sheet, row,
                   "总计",
                   string.Empty,
                   sumc,
                   sump
                   );
               });
        }

        public FileDto ExportCustomOrderToFile(List<CustomOrderDto> userListDtos) {
            return CreateExcelPackage(
                "用户消费记录表.xlsx",
                excelPackage => {
                    var sheet = excelPackage.Workbook.Worksheets.Add("用户消费记录表");
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        "序号",
                        "卡号",
                        "会员名",
                        "交易额",
                        "类型",
                        "操作员",
                        "时间"
                        );
                    AddObjects(
                        sheet, 2, userListDtos,
                        _ => _.serialNo,
                        _ => _.OutCard,
                        _ => _.CustomerName,
                        _ => _.OrderAmount,
                        _ => ChangePayState(_.PayState),
                        _ => _.Name,
                        _ => _.CreationTime
                        );

                    //Formatting cells

                    var creationName = sheet.Column(7);
                    creationName.Style.Numberformat.Format = "yyyy-MM-dd HH:mm";


                });
        }
        public FileDto ExportPointOrderToFile(List<PointOrderDto> userListDtos) {
            return CreateExcelPackage(
                "每点位每天预订餐表.xlsx",
                excelPackage => {
                    var sheet = excelPackage.Workbook.Worksheets.Add("每点位每天预订餐表");
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        "序号",
                        "点位名称",
                        "商品名称",
                        "数量（份）",
                        // "交易类型",
                        "预订时间"
                        );
                    AddObjects(
                        sheet, 2, userListDtos,
                        _ => _.serialNo,
                        _ => _.PointName,
                        _ => _.Dish,
                        _ => _.DishNumber,
                        //   _ => ChangePayState( _.PayState),

                        _ => _.OrderTime
                        );

                    var row = userListDtos.Count + 2;

                    var sump = userListDtos.Sum(c => c.DishNumber).ToString();

                    AddFooters(sheet, row,
                 "总计",
                 string.Empty,
                 string.Empty,
                 sump,
                 string.Empty
                 );

                    //Formatting cells

                    var creationName = sheet.Column(5);
                    creationName.Style.Numberformat.Format = "yyyy-MM-dd";


                });
        }

        public string ChangeOrderState(OrderState state) {
            switch (state) {
                case OrderState.NewOrder:
                    return "新订单";
                case OrderState.PayforSuccess:
                    return "支付成功";

                case OrderState.HasTheDelivery:
                    return "已交货";

                case OrderState.FailureOrder:
                    return "失效订单";
                case OrderState.UserCancelled:
                    return "用户取消";
                case OrderState.DeliveryOf:
                    return "发货中";
                case OrderState.AbnormalOrder:
                    return "异常订单";
                case OrderState.HaveToReturn:
                    return "已退货";
                default:
                    return "未知状态";
            }
        }
        public string ChangePayState(PayState state) {
            switch (state) {
                case PayState.Cash:
                    return "现金";
                case PayState.MembershipCard:
                    return "会员卡";


                case PayState.PayTreasure:
                    return "支付宝";
                case PayState.WeChat:
                    return "微信";
                case PayState.Integral:
                    return "积分";
                case PayState.Voucher:
                    return "优惠券";
                case PayState.BackMoney:
                    return "退订款";
                default:
                    return "未知支付";
            }

        }
    }
}