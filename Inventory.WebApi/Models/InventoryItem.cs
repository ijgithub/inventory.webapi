using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Inventory.WebApi.Models
{
    public class InventoryItem
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public InventoryItemCategory Category { get; set; }
    }

    public class InventoryItemFilter
    {
        public string Query { get; set; }
        public InventoryItemCategory Category { get; set; }
    }
}
