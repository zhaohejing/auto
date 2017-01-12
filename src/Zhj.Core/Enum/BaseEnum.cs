using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.Enum {
    public enum UserType {
        /// <summary>
        /// 系统管理 
        /// </summary>
        SystemManager = 1,
        /// <summary>
        /// 部门经理
        /// </summary>
        DepartmentManager = 2,
        /// <summary>
        /// 普通用户
        /// </summary>
        User = 3,
     
    }
    public enum UserState {
        /// <summary>
        /// 公司员工
        /// </summary>
        CompanyUser=2,
        /// <summary>
        /// 普通用户
        /// </summary>
        NormalUser =1,
        /// <summary>
        /// 会员卡用户
        /// </summary>
        CardUser=3
    }

}
