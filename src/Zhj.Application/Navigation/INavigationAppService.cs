using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.Navigation {
    public interface INavigationAppService : IApplicationService {
        IList<BaseNavigation> GetNavigationList();
    }
}
