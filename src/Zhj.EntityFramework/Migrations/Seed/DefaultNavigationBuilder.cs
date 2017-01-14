using MyCompanyName.AbpZeroTemplate.Authorization;
using MyCompanyName.AbpZeroTemplate.EntityFramework;
using MyCompanyName.AbpZeroTemplate.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.Migrations.Seed {
    public class DefaultNavigationBuilder {
        private readonly ZhjDbContext _context;

        public DefaultNavigationBuilder(ZhjDbContext context) {
            _context = context;
        }

        public void Build() {
            CreateDefaultNavigation();
        }
        /// <summary>
        /// 添加默认导航
        /// </summary>
        private void CreateDefaultNavigation() {
            //if (_context.BaseNavigation.Count() > 0) {
            //    _context.BaseNavigation = new System.Data.Entity.IDbSet<BaseNavigation>;
            //    _context.SaveChanges();
            //}
            var list = GetNavigationList();
            foreach (var level in list) {
                if (_context.BaseNavigation.Where(c => c.Name.Equals(level.Name)).Count() > 0) {
                    continue;
                }
                var one = _context.BaseNavigation.Add(
                    new BaseNavigation(
                    level.Name, level.DisplayName, level.Url,
                    level.Icon, !string.IsNullOrWhiteSpace(level.RequiredPermissionName), level.RequiredPermissionName,
                    level.Name == "TradingInfo" || level.Name == "TradingInfo" ? 1 : 0, null));

                _context.SaveChanges();

                if (level.ChildLevel != null) {
                    foreach (var leveltwo in level.ChildLevel) {
                        _context.BaseNavigation.Add(new BaseNavigation(leveltwo.Name, leveltwo.DisplayName, leveltwo.Url,
                            leveltwo.Icon,
                            !string.IsNullOrWhiteSpace(leveltwo.RequiredPermissionName), leveltwo.RequiredPermissionName,
                            one.Id));
                    }
                    _context.SaveChanges();
                }
            }
        }

        private List<LevelOne> GetNavigationList() {
            var list = new List<LevelOne>();
            var twotrading = new List<LevelTwo>() {
                     new LevelTwo("PhoneBook","电话预定","phonebook","fa fa-commenting",AppPermissions.Pages_TradingInfo_PhoneBook),
            };
            //预定
            var trading = new LevelOne("TradingInfo", "交易信息", "", "fa fa-dashboard", AppPermissions.Pages_TradingInfo, twotrading);
            list.Add(trading);


            //客户管理
            var customers = new List<LevelTwo>() {
                new LevelTwo("CustomerInfo","客户信息","customerinfo","fa fa-commenting",AppPermissions.Pages_CustomerManagement_CustomerInfo),
                new LevelTwo("CustomerCardInfo","会员卡信息","customercardinfo","fa fa-commenting",AppPermissions.Pages_CustomerManagement_CustomerCardInfo),
            };

            var eve = new LevelOne("CustomerManagement", "客户管理", "", "fa fa-building", AppPermissions.Pages_CustomerManagement, customers);
            list.Add(eve);

            //报表
            var deal = new List<LevelTwo>() {
                new LevelTwo("DayDue","日预定商品","daydue","fa fa-gear",AppPermissions.Pages_ReportManagement_DayDue),
                new LevelTwo("MemberConsumptionRecord","用户消费记录表","memberconsumptionrecord","fa fa-gear",AppPermissions.Pages_ReportManagement_MemberConsumptionRecord),
                new LevelTwo("PointOrder","每点位每天预定餐表","pointorder","fa fa-gear",AppPermissions.Pages_ReportManagement_PointOrder),
                new LevelTwo("PrepaidRecords","充值记录","prepaidrecords","fa fa-gear",AppPermissions.Pages_ReportManagement_PointOrder),
                new LevelTwo("PointOrderRecords","订单汇总统计","pointorderrecords","fa fa-gear",AppPermissions.Pages_ReportManagement_PointOrder),
                new LevelTwo("TheEndOfCheckList","月底对账清单","theendofchecklist","fa fa-gear",AppPermissions.Pages_ReportManagement_TheEndOfCheckList)

            };
            var dealevent = new LevelOne("ReportManagement", "报表管理", "", "fa fa-gavel", AppPermissions.Pages_ReportManagement, deal);
            list.Add(dealevent);

            //机构用户
            var knowledgebase = new List<LevelTwo>() {
                new LevelTwo("UserManagement","用户管理","usermanagement","fa fa-plug",AppPermissions.Pages_OrganizationUser_UserManagement),
                new LevelTwo("Role","角色管理","role","fa fa-building",AppPermissions.Pages_OrganizationUser_RoleManagement),
            };

            var knowledges = new LevelOne("OrganizationUser", "机构用户", "organizationuser", "fa fa-mortar-board", AppPermissions.Pages_OrganizationUser, knowledgebase);
            list.Add(knowledges);

         
            return list;

        }
    }


    public class LevelOne {
        public LevelOne(string name, string displayname, string url, string icon, List<LevelTwo> twolist) {
            Name = name;
            ChildLevel = twolist;
            Url = url;
            DisplayName = displayname;
            Icon = icon;
        }
        public LevelOne(string name, string displayname, string url, string icon, string requiredPermissionName, List<LevelTwo> twolist) {
            Name = name;
            ChildLevel = twolist;
            Url = url;
            DisplayName = displayname;
            Icon = icon;
            RequiredPermissionName = requiredPermissionName;
        }
        public string RequiredPermissionName { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public List<LevelTwo> ChildLevel { get; set; }
    }
    public class LevelTwo {
        public LevelTwo(string name, string displayname, string url, string icon, string requiredPermissionName) {
            Name = name;
            DisplayName = displayname;
            Url = url;
            Icon = icon;
            RequiredPermissionName = requiredPermissionName;
        }
        public string RequiredPermissionName { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
    }
}
