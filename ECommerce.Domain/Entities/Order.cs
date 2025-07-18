using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public int? ShippingAddressId { get; set; }
        public int? BillingAddressId { get; set; }

        public Address? ShippingAddress { get; set; }
        public Address? BillingAddress { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();
    }

}
