using Abp.Application.Navigation;
using Abp.Dependency;
using Abp.Localization;
using MyCompanyName.AbpZeroTemplate.Authorization;
using MyCompanyName.AbpZeroTemplate.Navigation;
using MyCompanyName.AbpZeroTemplate.Web.Navigation;

namespace MyCompanyName.AbpZeroTemplate.Web.App.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// It uses ABP's menu system.
    /// When you add menu items here, they are automatically appear in angular application.
    /// See .cshtml and .js files under App/Main/views/layout/header to know how to render menu.
    /// </summary>
    public class AppNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {

            //  SetUsefulNavigaion(context);
            var navigationservice = IocManager.Instance.Resolve<INavigationAppService>();
            var list = navigationservice.GetNavigationList();
            if (list != null && list.Count > 0) {
                foreach (var item in list) {
                    if (item.ParentId != null) {
                        continue;
                    }

                    var temp = new MenuItemDefinition(item.Name,
                        L(item.Name), item.Icon,
                            item.Url, item.RequiresAuthentication,
                      string.IsNullOrWhiteSpace(item.RequiredPermissionName) ? null : item.RequiredPermissionName);
                    if (item.ChildNavigation != null) {
                        foreach (var child in item.ChildNavigation) {
                            temp.AddItem(new MenuItemDefinition(child.Name, L(child.Name), child.Icon, child.Url,
                                child.RequiresAuthentication, child.RequiredPermissionName));
                        }
                    }
                    context.Manager.MainMenu.AddItem(temp);
                }
            }

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ZhjConsts.LocalizationSourceName);
        }
    }
}
