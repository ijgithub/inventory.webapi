using Inventory.WebApi.Data;
using Inventory.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        public IActionResult ListRecipes()
        {
            try
            {
                var recipes = _dbContext.Recipes
                    .Include("CraftingIngredients")
                    .Include("CraftedItems")
                    .ToList();

                recipes.ForEach(recipe =>
                {
                    recipe.CraftingIngredients.ForEach(ing =>
                    {
                        ing.CraftingIngredient = _dbContext.ItemTemplates.FirstOrDefault(it => it.Id == ing.CraftingIngredientId);
                    });

                    recipe.CraftedItems.ForEach(ci =>
                    {
                        ci.CraftedItem = _dbContext.ItemTemplates.FirstOrDefault(it => it.Id == ci.CraftedItemId);
                    });
                });

                return Ok(recipes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Details(int id)
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
        public async Task<IActionResult> NewRecipe([FromBody] Recipe recipe)
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

                    var addedInputs = recipe.CraftingIngredients.Except(dbEntity.CraftingIngredients);
                    var updatedInputs = recipe.CraftingIngredients.Intersect(dbEntity.CraftingIngredients);
                    var deletedInputs = dbEntity.CraftingIngredients.Except(recipe.CraftingIngredients);

                    // handle added ingredients 
                    foreach(var input in addedInputs)
                    {
                        var dbIngredient = _dbContext.ItemTemplates.FirstOrDefault(it => it.Id == input.CraftingIngredientId);
                        if (dbIngredient == null)
                        {
                            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity, recipe);
                        }

                        var craftingInput = new CraftingInput();
                        craftingInput.Quantity = input.Quantity;
                        craftingInput.CraftingIngredient = dbIngredient;
                        craftingInput.CraftingIngredientId = input.CraftingIngredientId;

                        dbEntity.CraftingIngredients.Add(craftingInput);
                    }

                    // handle updated ingredients
                    foreach (var input in updatedInputs)
                    {
                        var existingInput = dbEntity.CraftingIngredients.FirstOrDefault(ci => ci.Id == input.Id);

                        var dbIngredient = _dbContext.ItemTemplates.FirstOrDefault(it => it.Id == input.CraftingIngredientId);
                        if (dbIngredient == null)
                        {
                            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity, recipe);
                        }

                        existingInput.Quantity = input.Quantity;
                        existingInput.CraftingIngredient = dbIngredient;
                        existingInput.CraftingIngredientId = input.CraftingIngredientId;
                    }

                    // handle deleted inputs 
                    foreach (var input in deletedInputs)
                    {
                        var existingInput = dbEntity.CraftingIngredients.FirstOrDefault(ci => ci.Id == input.Id);
                        dbEntity.CraftingIngredients.Remove(existingInput);
                    }

                    var addedOutputs = recipe.CraftedItems.Except(dbEntity.CraftedItems);
                    var updatedOutputs= recipe.CraftedItems.Intersect(dbEntity.CraftedItems);
                    var deletedOutputs = dbEntity.CraftedItems.Except(recipe.CraftedItems);

                    // handle added outputs
                    foreach (var output in addedOutputs)
                    {
                        var dbIngredient = _dbContext.ItemTemplates.FirstOrDefault(it => it.Id == output.CraftedItemId);
                        if (dbIngredient == null)
                        {
                            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity, recipe);
                        }

                        var craftedOutput = new CraftingOutput();
                        craftedOutput.Quantity = output.Quantity;
                        craftedOutput.CraftedItem = dbIngredient;
                        craftedOutput.CraftedItemId = output.CraftedItemId;

                        dbEntity.CraftedItems.Add(craftedOutput);
                    }

                    // handle updated ingredients
                    foreach (var output in updatedOutputs)
                    {
                        var existingOutput = dbEntity.CraftedItems.FirstOrDefault(ci => ci.Id == output.Id);

                        var dbIngredient = _dbContext.ItemTemplates.FirstOrDefault(it => it.Id == output.CraftedItemId);
                        if (dbIngredient == null)
                        {
                            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity, recipe);
                        }

                        existingOutput.Quantity = output.Quantity;
                        existingOutput.CraftedItem = dbIngredient;
                        existingOutput.CraftedItemId = output.CraftedItemId;
                    }

                    // handle deleted inputs 
                    foreach (var output in deletedOutputs)
                    {
                        var exstingOutput = dbEntity.CraftedItems.FirstOrDefault(ci => ci.Id == output.Id);
                        dbEntity.CraftedItems.Remove(exstingOutput);
                    }

                    _dbContext.Recipes.Update(dbEntity);
                }
                else
                {
                    var newRecipe = new Recipe();
                    newRecipe.Name = recipe.Name;
                    newRecipe.Title = recipe.Title;
                    newRecipe.CraftingIngredients = new List<CraftingInput>();
                    newRecipe.CraftedItems = new List<CraftingOutput>();

                    // handle crafting ingredients
                    foreach (var ingredient in recipe.CraftingIngredients)
                    {
                        var dbIngredient = _dbContext.ItemTemplates.FirstOrDefault(it => it.Id == ingredient.CraftingIngredientId);
                        if (dbIngredient == null) continue;

                        var newIngredient = new CraftingInput();
                        newIngredient.Quantity = ingredient.Quantity;
                        newIngredient.CraftingIngredientId = ingredient.CraftingIngredientId;
                        newIngredient.CraftingIngredient = dbIngredient;

                        newRecipe.CraftingIngredients.Add(newIngredient);
                    }

                    // handle crafted results
                    foreach (var craftedItem in recipe.CraftedItems)
                    {
                        var dbCraftedItem = _dbContext.ItemTemplates.FirstOrDefault(it => it.Id == craftedItem.CraftedItemId);
                        if (dbCraftedItem == null) continue;

                        var newCraftedItem = new CraftingOutput();
                        newCraftedItem.Quantity = craftedItem.Quantity;
                        newCraftedItem.CraftedItemId = craftedItem.CraftedItemId;
                        newCraftedItem.CraftedItem = dbCraftedItem;

                        newRecipe.CraftedItems.Add(newCraftedItem);
                    }

                    _dbContext.Recipes.Add(recipe);
                }


                var changeCount = await _dbContext.SaveChangesAsync();

                if (changeCount == 0)
                {
                    return StatusCode(500, new { message = "Database error. Number of records updated is zero." });
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

                var entity = _dbContext.Recipes
                    .Include("CraftingIngredients")
                    .Include("CraftedItems")
                    .FirstOrDefault(it => it.Id == id);
                
                if (entity.CraftingIngredients != null)
                {
                    entity.CraftingIngredients.Clear();
                }

                if (entity.CraftedItems != null)
                {
                    entity.CraftedItems.Clear();
                }

                _dbContext.Recipes.Remove(entity);
                var result = await _dbContext.SaveChangesAsync();

                if (result == 0)
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
