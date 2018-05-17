using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.WebApi.Models
{


    // item, id, title (computed material type + name), name, description, material type, damage
    public class ItemTemplate
    {
        public int Id { get; set; }
        public ItemType Type { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }
}
