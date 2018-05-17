using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.WebApi.Models
{
    public enum InventoryItemCategory
    {
        Unspecified = 0,
        General,
        Armor,
        Weapons,
        AlchemyIngredient,
        CraftingIngredient,
        AlchemyRecipe,
        CraftingRecipe,
        Miscellaneous
    }
}
