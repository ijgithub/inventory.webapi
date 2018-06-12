using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.WebApi.Models
{

    public enum TemplateType
    {
        Unspecified = 0,
        Weapon = 1,
        Armor = 2,
        AlchemyIngredient = 3,
        CraftingIngredient = 4,
        Consumable = 5
    }

    public enum ItemType
    {
        Unspecified = 0,

        ShortSword = 1,
        LongSword = 2,
        TwoHandedSword = 3,
        Hatchet = 4,
        Axe = 5,

        Vest = 6,
        Pants = 7,
        Helmet = 8,
        Curias = 9,
        Gloves = 10,
        Boots = 11,
        Gauntlets = 12,
        Greaves = 13,

        Coal,
        Charcoal,
    }

    public enum MaterialType
    {
        Unspecified = 0,
        Cloth = 1,
        Leather = 2,
        Iron = 3,
        Steel = 4,
        Organic = 5, // for alchemy ingredients
    }

    // item, id, title (computed material type + name), name, description, material type, damage
    public class ItemTemplate
    {
        public int Id { get; set; }
        public TemplateType TemplateType { get; set; }
        public ItemType ItemType { get; set; }
        public MaterialType MaterialType { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
    }
}
