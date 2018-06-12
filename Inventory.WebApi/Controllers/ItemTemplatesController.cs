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

        [HttpGet]
        public IActionResult List()
        {
            try
            {
                var itemTemplate = this._dbContext.ItemTemplates.ToList();
                return Ok(itemTemplate);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex);
            }
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            try
            {
                var itemTemplate = this._dbContext.ItemTemplates.Where(t => t.Id == id).FirstOrDefault();
                return Ok(itemTemplate);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> NewWeapon([FromBody]ItemTemplate itemTemplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (itemTemplate.Id > 0)
                {
                    var dbEntity = _dbContext.ItemTemplates.FirstOrDefault(wt => wt.Id == itemTemplate.Id);

                    dbEntity.Name = itemTemplate.Name;
                    dbEntity.Title = itemTemplate.Title;
                    dbEntity.ItemType = itemTemplate.ItemType;
                    dbEntity.MaterialType = itemTemplate.MaterialType;
                    dbEntity.Value = itemTemplate.Value;

                    _dbContext.ItemTemplates.Update(dbEntity);

                }
                else
                {
                    _dbContext.ItemTemplates.Add(itemTemplate);
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

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> RemoveTemplate(int id)
        {
            try
            {
                var exists = _dbContext.ItemTemplates.Any(it => it.Id == id);
                if (!exists)
                {
                    return NotFound();
                }

                var entity = _dbContext.ItemTemplates.FirstOrDefault(it => it.Id == id);
                _dbContext.ItemTemplates.Remove(entity);
                var result = await _dbContext.SaveChangesAsync();

                if (result != 1)
                {
                    return StatusCode(500, "Error saving changes to database");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

            return Ok();
        }
    }
}
