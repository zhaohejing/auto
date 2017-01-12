using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace MyCompanyName.AbpZeroTemplate.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)


            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("页面"));
            //控制台
            var trading = pages.CreateChildPermission(AppPermissions.Pages_TradingInfo, L("交易信息"));
            var a = trading.CreateChildPermission(AppPermissions.Pages_TradingInfo_PhoneBook, L("电话订餐"));
            var b = trading.CreateChildPermission(AppPermissions.Pages_TradingInfo_BeBackOff, L("强制退订"));

            var cus = pages.CreateChildPermission(AppPermissions.Pages_CustomerManagement, L("客户管理"));
            var aa = cus.CreateChildPermission(AppPermissions.Pages_CustomerManagement_CustomerInfo, L("客户信息"));
            var bb = cus.CreateChildPermission(AppPermissions.Pages_CustomerManagement_CustomerCardInfo, L("会员卡信息"));

            var rep = pages.CreateChildPermission(AppPermissions.Pages_ReportManagement, L("报表管理"));
            var aaa = rep.CreateChildPermission(AppPermissions.Pages_ReportManagement_DayDue, L("日预定商品"));
            var bbb = rep.CreateChildPermission(AppPermissions.Pages_ReportManagement_MemberConsumptionRecord, L("会员消费记录"));
            var ccc = rep.CreateChildPermission(AppPermissions.Pages_ReportManagement_PointOrder, L("每点位每天预定餐表"));
            var ddd = rep.CreateChildPermission(AppPermissions.Pages_ReportManagement_PrepaidRecords, L("充值记录"));
            var eee = rep.CreateChildPermission(AppPermissions.Pages_ReportManagement_PointOrderRecords, L("交易汇总统计"));


            //public const string Pages_OrganizationUser = "Pages.OrganizationUser";
            //public const string Pages_OrganizationUser_UserManagement = "Pages.OrganizationUser.UserManagement";

            var administration = pages.CreateChildPermission(AppPermissions.Pages_OrganizationUser, L("机构用户"));
         
            var users = administration.CreateChildPermission(AppPermissions.Pages_OrganizationUser_UserManagement, L("用户管理"));
            users.CreateChildPermission(AppPermissions.Pages_OrganizationUser_UserManagement_Create, L("创建用户"));
            users.CreateChildPermission(AppPermissions.Pages_OrganizationUser_UserManagement_Edit, L("编辑用户"));
            users.CreateChildPermission(AppPermissions.Pages_OrganizationUser_UserManagement_Delete, L("删除用户"));
            var roles = administration.CreateChildPermission(AppPermissions.Pages_OrganizationUser_RoleManagement, L("角色管理"));

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ZhjConsts.LocalizationSourceName);
        }
    }
}
