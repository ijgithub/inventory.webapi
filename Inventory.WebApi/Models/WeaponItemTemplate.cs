using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.WebApi.Models
{
    public enum WeaponType
    {
        Unspecified = 0, // for error detection
        ShortSword,
        LongSword,
        TwoHandedSword,
        Hatchet,
        Axe,
    }

    public class WeaponItemTemplate
    {
        [Key]
        public int Id { get; set; }
        public WeaponType WeaponType { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public MaterialType MaterialType { get; set; }
        public int Damage { get; set; }
    }
}
