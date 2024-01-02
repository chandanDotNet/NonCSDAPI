﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class Customer : BaseEntity
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsVarified { get; set; }
        public bool IsUnsubscribe { get; set; }
        public string CustomerProfile { get; set; }
        public string Address { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? CityId { get; set; }
        public bool IsWalkIn { get; set; }
        [NotMapped]
        public string ImageUrl { get; set; }
        public City City { get; set; }
        [ForeignKey("CountryId")]
        public Country Country { get; set; }

        [ForeignKey("ModifiedBy")]
        public User ModifiedByUser { get; set; }
        [ForeignKey("DeletedBy")]
        public User DeletedByUser { get; set; }

        public string Password { get; set; }
        public int OTP { get; set; }
        public string PinCode { get; set; }
        public string RewardPoints  { get; set; }
        public string? Category { get; set; }
        public string? AadharCard { get; set; }
        public string? DependantCard { get; set; }
    }
}
