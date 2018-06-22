using Inventory.WebApi.Data;
using Inventory.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.WebApi.Controllers
{
    [Route("api/recipes")]
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public RecipesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> ListRecipes()
        {
            try
            {
                var recipes = _dbContext.Recipes.ToList();
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ListRecipes(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            try
            {
                var recipeEntity = this._dbContext.Recipes.Where(t => t.Id == id).FirstOrDefault();
                return Ok(recipeEntity);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> NewRecipe(Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (recipe.Id > 0)
                {
                    var dbEntity = _dbContext.Recipes.FirstOrDefault(wt => wt.Id == recipe.Id);

                    dbEntity.Name = recipe.Name;
                    dbEntity.Title = recipe.Title;

                    // handle crafting ingredients

                    // handle crafted results

                    _dbContext.Recipes.Update(dbEntity);

                }
                else
                {
                    _dbContext.Recipes.Add(recipe);
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
        public async Task<IActionResult> RemoveRecipe(int id)
        {
            try
            {
                var exists = _dbContext.Recipes.Any(it => it.Id == id);
                if (!exists)
                {
                    return NotFound();
                }

                var entity = _dbContext.Recipes.FirstOrDefault(it => it.Id == id);
                _dbContext.Recipes.Remove(entity);
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
