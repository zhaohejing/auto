using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.Authorization.Users.Dto {
    [AutoMapFrom(typeof(User))]
    public class UsersDto : EntityDto<long>, IPassivable, IHasCreationTime {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }
        public string Password { get; set; }


        public bool IsActive { get; set; }

        public DateTime CreationTime { get; set; }
        public string WorkNo { get; set; }

        public string Phone { get; set; }
     
        public string Emarks { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int Type { get; set; }

        public string RoleName { get; set; }

        public string RoleDisplayName { get; set; }

        public int State { get; set; }
    }
}
