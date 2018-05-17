using Inventory.WebApi.Data;
using Inventory.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public InventoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(InventoryItemFilter filter)
        {
            var inventoryQuery = _dbContext.Inventory.AsQueryable<InventoryItem>();
            if(filter != null)
            {
                if (filter.Category > 0)
                {
                    inventoryQuery = inventoryQuery.Where(item => item.Category == filter.Category);
                }

                if (!string.IsNullOrWhiteSpace(filter.Query))
                {
                    inventoryQuery = inventoryQuery.Where(item => item.Title.Contains(filter.Query));
                }
            }

            var result = inventoryQuery.ToList();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]InventoryItem inventoryItem)
        {
            if (inventoryItem == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                _dbContext.Inventory.Add(inventoryItem);
                var result = await _dbContext.SaveChangesAsync();
                if (result == 0) return StatusCode(500, "Database error occured");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            return Ok(inventoryItem);
        }
    }
}

