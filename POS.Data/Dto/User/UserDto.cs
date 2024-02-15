using System;
using System.Collections.Generic;

namespace POS.Data.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ProfilePhoto { get; set; }
        public string Provider { get; set; }
        public bool IsActive { get; set; }
        public string? PinCode { get; set; }
        public Guid? CounterId { get; set; }
        public Guid? NonCSDCanteensId { get; set; }
        public Guid? MainCategoryId { get; set; }
        public int? Otp { get; set; }
        //public NonCSDCanteenDto NonCSDCanteen { get; set; }
        public List<UserRoleDto> UserRoles { get; set; } = new List<UserRoleDto>();
        public List<UserClaimDto> UserClaims { get; set; } = new List<UserClaimDto>();

    }
}
