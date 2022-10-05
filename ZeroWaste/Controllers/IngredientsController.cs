using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZeroWaste.Data;
using ZeroWaste.Data.Services;
using ZeroWaste.Models;

namespace ZeroWaste.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IIngredientsService _ingredientsService;

        public IngredientsController(AppDbContext context, IIngredientsService ingredientsService)
        {
            _context = context;
            _ingredientsService = ingredientsService;
        }

        // GET: Ingredients
        public async Task<IActionResult> Index(string searchString)
        {
            List<Ingredient> ingredients;
            if (!string.IsNullOrEmpty(searchString))
            {
                ingredients = await _ingredientsService.GetAllAsync(searchString);
            }
            else
            {
                ingredients = await _ingredientsService.GetAllAsync();
            }
            return View(ingredients);
        }

        // GET: Ingredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ingredients == null)
            {
                return NotFound();
            }

            var ingredient = await _context.Ingredients
                .Include(i => i.IngredientType)
                .Include(i => i.UnitOfMeasure)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        // GET: Ingredients/Create
        public async Task<IActionResult> Create()
        {
            var ingredientDropdownsData = await _ingredientsService.GetNewIngredientDropdownsWM();
            ViewBag.IngredientTypes = new SelectList(ingredientDropdownsData.IngredientTypes, "Id", "Name");
            ViewBag.UnitOfMeasures = new SelectList(ingredientDropdownsData.UnitOfMeasures, "Id", "Name");

            return View();
        }

        // POST: Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,IngredientTypeId,UnitOfMeasureId")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingredient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IngredientTypeId"] = new SelectList(_context.IngredientTypes, "Id", "Id", ingredient.IngredientTypeId);
            ViewData["UnitOfMeasureId"] = new SelectList(_context.UnitOfMeasures, "Id", "Id", ingredient.UnitOfMeasureId);
            return View();
        }

        // GET: Ingredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ingredients == null)
            {
                return NotFound();
            }

            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            ViewData["IngredientTypeId"] = new SelectList(_context.IngredientTypes, "Id", "Id", ingredient.IngredientTypeId);
            ViewData["UnitOfMeasureId"] = new SelectList(_context.UnitOfMeasures, "Id", "Id", ingredient.UnitOfMeasureId);
            return View();
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IngredientTypeId,UnitOfMeasureId")] Ingredient ingredient)
        {
            if (id != ingredient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingredient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientExists(ingredient.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IngredientTypeId"] = new SelectList(_context.IngredientTypes, "Id", "Id", ingredient.IngredientTypeId);
            ViewData["UnitOfMeasureId"] = new SelectList(_context.UnitOfMeasures, "Id", "Id", ingredient.UnitOfMeasureId);
            return View(ingredient);
        }

        // GET: Ingredients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ingredients == null)
            {
                return NotFound();
            }

            var ingredient = await _context.Ingredients
                .Include(i => i.IngredientType)
                .Include(i => i.UnitOfMeasure)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ingredients == null)
            {
                return Problem("Entity set 'AppDbContext.Ingredients'  is null.");
            }
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngredientExists(int id)
        {
          return _context.Ingredients.Any(e => e.Id == id);
        }
    }
}
