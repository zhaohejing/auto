using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace MyCompanyName.AbpZeroTemplate.Authorization.Users.Dto
{
    public class CreateOrUpdateUserInput : IInputDto
    {
        [Required]
        public UserEditDto User { get; set; }



        [Required]
        public string[] AssignedRoleNames { get; set; }

        public bool SendActivationEmail { get; set; }
    }
    public class ChangePasswordInput : IInputDto {
        [Required]
        [StringLength(User.MaxPlainPasswordLength)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(User.MaxPlainPasswordLength, MinimumLength = User.MinPlainPasswordLength)]
        public string NewPassword { get; set; }
    }
}