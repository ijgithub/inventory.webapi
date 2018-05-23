using Inventory.WebApi.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.WebApi.Controllers
{
    [Route("api/templates")]
    public class ItemTemplatesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ItemTemplatesController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [Route("weapons")]
        [HttpGet]
        public IActionResult ListWeapons()
        {
            try
            {
                var weaponTemplates = this._dbContext.WeaponTemplates.ToList();
                return Ok(weaponTemplates);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex);
            }
        }

        [Route("weapons/{id}")]
        [HttpGet]
        public IActionResult ListWeapon(string id)
        {
            return this.StatusCode(200, $"Weapon id is {id}");
        }

        [Route("weapons")]
        [HttpPost]
        public IActionResult NewWeapon()
        {
            return this.StatusCode(200, "New weapon is called");
        }
    }
}
