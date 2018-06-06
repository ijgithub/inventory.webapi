using System.ComponentModel.DataAnnotations;

namespace Inventory.WebApi.Models
{
    public enum ArmorType
    {
        Unspecified = 0, // for error detection
        Vest,
        Pants,
        Helmet,
        Curias,
        Gloves,
        Boots,
        Gauntlets,
        Greaves,
    }

    public class ArmorItemTemplate
    {
        [Key]
        public int Id { get; set; }
        public ArmorType ArmorType { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public MaterialType MaterialType { get; set; }
        public int Defense { get; set; }
    }
}
