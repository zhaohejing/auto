using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.EntityModel {
    /// <summary>
    /// 支付状态
    /// </summary>
  public  enum PayState {
        //vm.payState = [{id: 0, text: '全部渠道'},{id:1,text:'现金'},{id:2,text:'会员卡'}
        //        ,{id:3,text:'支付宝'},{id:4,text:'微信'},{id:5,text:'积分'},{id:6,text:'兑换券'}]

        /// <summary>
        /// 现金
        /// </summary>
        Cash=1,/// <summary>
        /// 会员卡
        /// </summary>
        MembershipCard =2,
    /// <summary>
    /// 支付宝
    /// </summary>
          PayTreasure=3,
          /// <summary>
          /// 微信
          /// </summary>
          WeChat=4,
          /// <summary>
          /// 积分
          /// </summary>
          Integral=5,
          /// <summary>
          /// 兑换券
          /// </summary>
        Voucher=6,
        /// <summary>
        /// 退订款
        /// </summary>
        BackMoney=7
    }
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderState {
        /// <summary>
        /// 新订单
        /// </summary>
        NewOrder=1,
        /// <summary>
        /// 支付成功
        /// </summary>
        PayforSuccess =2,
        /// <summary>
        /// 已交货
        /// </summary>
        HasTheDelivery =3,
        /// <summary>
        /// 失效订单
        /// </summary>
        FailureOrder =4,
        /// <summary>
        /// 用户取消
        /// </summary>
        UserCancelled =5,
        /// <summary>
        /// 发货中
        /// </summary>
        DeliveryOf =6,
        /// <summary>
        /// 异常订单
        /// </summary>
        AbnormalOrder=7,
        /// <summary>
        /// 已退货
        /// </summary>
        HaveToReturn=8
    }
    /// <summary>
    /// 会员卡状态
    /// </summary>
    public enum CardState {
        /// <summary>
        /// 可用
        /// </summary>
        CanUse=1,
        /// <summary>
        /// 不可用
        /// </summary>
        CanNotUse =0
    }
    public enum RecordState {
        /// <summary>
        /// 充值
        /// </summary>
        TopUp=1,
        /// <summary>
        /// 提现
        /// </summary>
        WithDrawal=2,
        /// <summary>
        /// 退卡
        /// </summary>
        OffCard=3
    }
}
