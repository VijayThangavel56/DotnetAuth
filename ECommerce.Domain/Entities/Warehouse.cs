using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Location { get; set; }

        public ICollection<StockInventory> StockInventories { get; set; } = new List<StockInventory>();
    }

}
