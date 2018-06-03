using Inventory.WebApi.Data;
using Inventory.WebApi.Models;
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
        public IActionResult ListWeapon(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            try
            {
                var weaponTemplate = this._dbContext.WeaponTemplates.Where(t => t.Id == id).FirstOrDefault();
                return Ok(weaponTemplate);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex);
            }
        }

        [Route("weapons")]
        [HttpPost]
        public async Task<IActionResult> NewWeapon([FromBody]WeaponItemTemplate weaponItemTemplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (weaponItemTemplate.Id > 0)
                {
                    var dbEntity = _dbContext.WeaponTemplates.FirstOrDefault(wt => wt.Id == weaponItemTemplate.Id);

                    dbEntity.Name = weaponItemTemplate.Name;
                    dbEntity.Title = weaponItemTemplate.Title;
                    dbEntity.WeaponType = weaponItemTemplate.WeaponType;
                    dbEntity.MaterialType = weaponItemTemplate.MaterialType;
                    dbEntity.Damage = weaponItemTemplate.Damage;

                    _dbContext.WeaponTemplates.Update(dbEntity);

                }
                else
                {
                    _dbContext.WeaponTemplates.Add(weaponItemTemplate);
                }


                var changeCount = await _dbContext.SaveChangesAsync();

                if (changeCount != 1)
                {
                    return StatusCode(500, new { message = "Database error. Number of records updated is greater than one." });
                }

                return StatusCode(200, new { message = "New weapon is called" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }
    }
}
