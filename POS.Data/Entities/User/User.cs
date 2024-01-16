﻿using Microsoft.AspNetCore.Identity;
using POS.Data.Dto;
using System;
using System.Collections.Generic;

namespace POS.Data
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string ProfilePhoto { get; set; }
        public string Provider { get; set; }
        public string Address { get; set; }
        public bool IsSuperAdmin { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }
        public Guid? CounterId { get; set; }
        public Counter Counter { get; set; }        
        public Guid? NonCSDCanteensId { get; set; }
        public List<NonCSDCanteen> NonCSDCanteens { get; set; }
        //public NonCSDCanteen NonCSDCanteen { get; set; }
        public string? PinCode { get; set; }
        public virtual ICollection<UserClaim> UserClaims { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
        public virtual ICollection<UserToken> UserTokens { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
