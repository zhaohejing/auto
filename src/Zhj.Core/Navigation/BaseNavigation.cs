using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.Navigation {
 
    [Table("BaseNavigation")]
    public class BaseNavigation : CreationAuditedEntity, ISoftDelete, IPassivable {

        public BaseNavigation() {
        }
        public BaseNavigation(string name, string displayName, string url, string icon, int? parentId) {
            Name = name; DisplayName = displayName; Url = url; Icon = icon;
            ParentId = parentId;
        }
        public BaseNavigation(string name, string displayName, string url, string icon, int sortorder, int? parentId) {
            Name = name; DisplayName = displayName; Url = url; Icon = icon; SortOrder = sortorder;
            ParentId = parentId;
        }
        public BaseNavigation(string name, string displayName, string url, string icon,
            bool requiresAuthentication, string requiredPermissionName, int? parentId) {
            Name = name; DisplayName = displayName; Url = url; Icon = icon;
            RequiresAuthentication = requiresAuthentication; RequiredPermissionName = requiredPermissionName;
            ParentId = parentId;
        }
        public BaseNavigation(string name, string displayName, string url, string icon,
        bool requiresAuthentication, string requiredPermissionName, int sortorder, int? parentId) {
            Name = name; DisplayName = displayName; Url = url; Icon = icon; SortOrder = sortorder;
            RequiresAuthentication = requiresAuthentication; RequiredPermissionName = requiredPermissionName;
            ParentId = parentId;
        }
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string DisplayName { get; set; }
        [MaxLength(100)]

        public string Url { get; set; }
        [MaxLength(100)]
        public string Icon { get; set; }
        public bool RequiresAuthentication { get; set; }
        public string RequiredPermissionName { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public int? ParentId { get; set; }


        public virtual BaseNavigation FatherNavigation { get; set; }

        [ForeignKey("ParentId")]
        public ICollection<BaseNavigation> ChildNavigation { get; set; }

    }
}
