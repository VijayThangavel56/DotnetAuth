﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public bool IsBilling { get; set; }
        public bool IsShipping { get; set; }

        public ApplicationUser User { get; set; } = null!;
    }

}
