using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.WebApi.Models
{
    public class CraftingInput
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int CraftingIngredientId { get; set; }
        public ItemTemplate CraftingIngredient { get; set; }
    }

    public class CraftingOutput
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int CraftedItemId { get; set; }
        public ItemTemplate CraftedItem { get; set; }
    }

    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Title { get; set; }

        public List<CraftingInput> CraftingIngredients { get; set; }
        public List<CraftingOutput> CraftedItems { get; set; }
    }
}
