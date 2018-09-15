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
                    var dbEntity = _dbContext.Recipes.
                        Include("CraftingIngredients")
                        .Include("CraftedItems")
                        .FirstOrDefault(wt => wt.Id == recipe.Id);

                    dbEntity.CraftingIngredients.ForEach(ing =>
                    {
                        ing.CraftingIngredient = _dbContext.ItemTemplates.FirstOrDefault(it => it.Id == ing.CraftingIngredientId);
                    });

                    dbEntity.CraftedItems.ForEach(ci =>
                    {
                        ci.CraftedItem = _dbContext.ItemTemplates.FirstOrDefault(it => it.Id == ci.CraftedItemId);
                    });

                    dbEntity.Name = recipe.Name;
                    dbEntity.Title = recipe.Title;

                    var dbEntityCraftingIngredientsIds = dbEntity.CraftingIngredients.Select(ci => ci.Id);
                    var recipeCraftingIngredientsIds = recipe.CraftingIngredients.Select(ci => ci.Id);

                    var addedInputsIds = recipeCraftingIngredientsIds.Except(dbEntityCraftingIngredientsIds);
                    var updatedInputsIds = recipeCraftingIngredientsIds.Intersect(dbEntityCraftingIngredientsIds);
                    var deletedInputsIds = dbEntityCraftingIngredientsIds.Except(recipeCraftingIngredientsIds);

                    // handle added ingredients 
                    foreach (var inputId in addedInputsIds)
                    {
                        var recipeInput = recipe.CraftingIngredients.FirstOrDefault(ci => ci.CraftingIngredientId == inputId);

                        var dbIngredient = _dbContext.ItemTemplates.FirstOrDefault(it => it.Id == recipeInput.CraftingIngredientId);
                        if (dbIngredient == null)
                        {
                            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity, recipe);
                        }

                        var craftingInput = new CraftingInput();
                        craftingInput.Quantity = recipeInput.Quantity;
                        craftingInput.CraftingIngredient = dbIngredient;
                        craftingInput.CraftingIngredientId = dbIngredient.Id;

                        dbEntity.CraftingIngredients.Add(craftingInput);
                    }

                    // handle updated ingredients
                    foreach (var inputId in updatedInputsIds)
                    {
                        var recipeInput = recipe.CraftingIngredients.FirstOrDefault(ci => ci.Id == inputId);
                        var existingInput = dbEntity.CraftingIngredients.FirstOrDefault(ci => ci.Id == inputId);

                        var dbIngredient = _dbContext.ItemTemplates.FirstOrDefault(it => it.Id == recipeInput.CraftingIngredientId);
                        if (dbIngredient == null)
                        {
                            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity, recipe);
                        }

                        existingInput.Quantity = recipeInput.Quantity;
                        existingInput.CraftingIngredient = dbIngredient;
                        existingInput.CraftingIngredientId = dbIngredient.Id;
                    }

                    // handle deleted inputs 
                    foreach (var inputId in deletedInputsIds)
                    {
                        var recipeInput = recipe.CraftingIngredients.FirstOrDefault(ci => ci.CraftingIngredientId == inputId);

                        var existingInput = dbEntity.CraftingIngredients.FirstOrDefault(ci => ci.Id == recipeInput.Id);
                        dbEntity.CraftingIngredients.Remove(existingInput);
                    }

                    var dbEntityCraftedItemsIds = dbEntity.CraftedItems.Select(ci => ci.Id);
                    var recipeCraftedItemsIds = recipe.CraftedItems.Select(ci => ci.Id);

                    var addedOutputsIds = recipeCraftedItemsIds.Except(dbEntityCraftedItemsIds);
                    var updatedOutputsIds = recipeCraftedItemsIds.Intersect(dbEntityCraftedItemsIds);
                    var deletedOutputsIds = dbEntityCraftedItemsIds.Except(recipeCraftedItemsIds);

                    // handle added outputs
                    foreach (var outputId in addedOutputsIds)
                    {
                        var recipeOutput = recipe.CraftedItems.FirstOrDefault(ci => ci.Id == outputId);

                        var dbIngredient = _dbContext.ItemTemplates.FirstOrDefault(it => it.Id == recipeOutput.CraftedItemId);
                        if (dbIngredient == null)
                        {
                            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity, recipe);
                        }

                        var craftedOutput = new CraftingOutput();
                        craftedOutput.Quantity = recipeOutput.Quantity;
                        craftedOutput.CraftedItem = dbIngredient;
                        craftedOutput.CraftedItemId = dbIngredient.Id;

                        dbEntity.CraftedItems.Add(craftedOutput);
                    }

                    // handle updated ingredients
                    foreach (var outputId in updatedOutputsIds)
                    {
                        var recipeOutput = recipe.CraftedItems.FirstOrDefault(ci => ci.Id == outputId);
                        var existingOutput = dbEntity.CraftedItems.FirstOrDefault(ci => ci.Id == recipeOutput.Id);

                        var dbIngredient = _dbContext.ItemTemplates.FirstOrDefault(it => it.Id == recipeOutput.CraftedItemId);
                        if (dbIngredient == null)
                        {
                            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity, recipe);
                        }

                        existingOutput.Quantity = recipeOutput.Quantity;
                        existingOutput.CraftedItem = dbIngredient;
                        existingOutput.CraftedItemId = dbIngredient.Id;
                    }

                    // handle deleted inputs 
                    foreach (var outputId in deletedOutputsIds)
                    {
                        var recipeOutput = recipe.CraftedItems.FirstOrDefault(ci => ci.Id == outputId);
                        var exstingOutput = dbEntity.CraftedItems.FirstOrDefault(ci => ci.Id == recipeOutput.Id);
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
