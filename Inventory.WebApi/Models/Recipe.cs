using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.WebApi.Models
{
    public class CraftingInput
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public int CraftingIngredientId { get; set; }
        public ItemTemplate CraftingIngredient { get; set; }
    }

    public class CraftingOutput
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public int CraftedItemId { get; set; }
        public ItemTemplate CraftedItem { get; set; }
    }

    public class Recipe
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int Title { get; set; }

        public List<CraftingInput> CraftingIngredients { get; set; }
        public List<CraftingOutput> CraftedItems { get; set; }
    }
}
