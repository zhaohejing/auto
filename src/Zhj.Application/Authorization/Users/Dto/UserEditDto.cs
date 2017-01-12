﻿using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Runtime.Validation;
using MyCompanyName.AbpZeroTemplate.Enum;

namespace MyCompanyName.AbpZeroTemplate.Authorization.Users.Dto
{
    //Mapped to/from User in CustomDtoMapper
    public class UserEditDto : IValidate, IPassivable
    {
        /// <summary>
        /// Set null to create a new user. Set user's Id to update a user
        /// </summary>
        public long? Id { get; set; }

        [Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }
        
        public string Surname { get; set; }

        [Required]
        public string WorkNo { get; set; }

        [Required]
        public string Phone { get; set; }

    

        public string Emarks { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        [StringLength(User.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(User.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        // Not used "Required" attribute since empty value is used to 'not change password'
        [StringLength(User.MaxPlainPasswordLength)]
        public string Password { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }


        public bool IsActive { get; set; }

        public bool ShouldChangePasswordOnNextLogin { get; set; }
    }
}