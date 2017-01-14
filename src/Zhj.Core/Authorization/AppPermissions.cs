namespace MyCompanyName.AbpZeroTemplate.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";
        

        public const string Pages_TradingInfo = "Pages.TradingInfo";
        public const string Pages_TradingInfo_PhoneBook = "Pages.TradingInfo.PhoneBook";
        public const string Pages_TradingInfo_BeBackOff = "Pages.TradingInfo.BeBackOff";



        public const string Pages_CustomerManagement = "Pages.CustomerManagement";
        public const string Pages_CustomerManagement_CustomerInfo = "Pages.CustomerManagement.CustomerInfo";
        public const string Pages_CustomerManagement_CustomerCardInfo = "Pages.CustomerManagement.CustomerCardInfo";


        public const string Pages_ReportManagement = "Pages.ReportManagement";
        public const string Pages_ReportManagement_DayDue = "Pages.ReportManagement.DayDue";
        public const string Pages_ReportManagement_MemberConsumptionRecord = "Pages.ReportManagement.MemberConsumptionRecord";
        public const string Pages_ReportManagement_PointOrder = "Pages.ReportManagement.PointOrder";
        public const string Pages_ReportManagement_PrepaidRecords = "Pages.ReportManagement.PrepaidRecords";
        public const string Pages_ReportManagement_PointOrderRecords = "Pages.ReportManagement.PointOrderRecords";
        public const string Pages_ReportManagement_TheEndOfCheckList = "Pages.ReportManagement.TheEndOfCheckList";
        

        public const string Pages_OrganizationUser = "Pages.OrganizationUser";
        public const string Pages_OrganizationUser_UserManagement = "Pages.OrganizationUser.UserManagement";
        public const string Pages_OrganizationUser_RoleManagement = "Pages.OrganizationUser.RoleManagement";

        public const string Pages_OrganizationUser_UserManagement_Create = "Pages.OrganizationUser.UserManagement.Create";
        public const string Pages_OrganizationUser_UserManagement_Edit = "Pages.OrganizationUser.UserManagement.Edit";
        public const string Pages_OrganizationUser_UserManagement_Delete = "Pages.OrganizationUser.UserManagement.Delete";

     
    }
}