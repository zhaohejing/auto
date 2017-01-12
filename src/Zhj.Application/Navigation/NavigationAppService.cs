using Abp.Domain.Repositories;
using MyCompanyName.AbpZeroTemplate;
using MyCompanyName.AbpZeroTemplate.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhj.Application.Navigation {
    public class NavigationAppService : AbpZeroTemplateAppServiceBase, INavigationAppService {
        private readonly IRepository<BaseNavigation> _navigationRepository;
        public NavigationAppService(IRepository<BaseNavigation> navigationRepository) {
            _navigationRepository = navigationRepository;
        }
        public virtual IList<BaseNavigation> GetNavigationList() {
            return _navigationRepository.GetAll().OrderByDescending(c => c.SortOrder).ToList();
        }

    }
}
