using DesarrollodeProyectos.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DesarrollodeProyectos.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ApplicationDbContext context, ILogger<CategoryController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // Acción para agregar una nueva categoría
        public IActionResult CategoryAdd()
        {
            return View();
        }

        // Acción POST para agregar una nueva categoría
        [HttpPost]
        public async Task<IActionResult> CategoryAdd(CategoryModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("El modelo de categoría no es válido");
                return View(categoryModel);
            }

            // Crear la nueva categoría en la base de datos
            var categoryEntity = new Category
            {
                Id = Guid.NewGuid(),
                Name = categoryModel.Name,
                Description = categoryModel.Description,
                CreationTime = DateTime.Now
            };

            _context.Categories.Add(categoryEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("CategoryList"); // Redirige a la lista de categorías
        }

                public async Task<IActionResult> CategoryList()
        {
            var categories = await _context.Categories
                                   .Where(c => c.IsActive)  
                                   .ToListAsync();

            var categoryModels = categories.Select(c => new CategoryModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive 
            }).ToList();

            return View(categoryModels); 
        }



        // Acción para editar una categoría
        public async Task<IActionResult> CategoryEdit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryModel = new CategoryModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreationTime = DateTime.Now
            };

            return View(categoryModel);
        }

        // Acción POST para editar una categoría
        [HttpPost]
        public async Task<IActionResult> CategoryEdit(CategoryModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("El modelo de categoría no es válido");
                return View(categoryModel);
            }

            var categoryEntity = await _context.Categories.FindAsync(categoryModel.Id);

            if (categoryEntity == null)
            {
                return NotFound();
            }

            // Actualizar la categoría
            categoryEntity.Name = categoryModel.Name;
            categoryEntity.Description = categoryModel.Description;

            _context.Categories.Update(categoryEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("CategoryList"); // Redirige a la lista de categorías
        }

       [HttpGet]
public async Task<IActionResult> CategoryDelete(Guid id)
{
    if (id == Guid.Empty)
    {
        return NotFound();
    }

    var category = await _context.Categories.FindAsync(id);

    if (category == null)
    {
        return NotFound();
    }

    var model = new CategoryModel
    {
        Id = category.Id,
        Name = category.Name,
        Description = category.Description
    };

    return View("CategoryDeleted",model); // Devolver la vista CategoryDeleted.cshtml con el modelo
}


    [HttpPost]
public async Task<IActionResult> CategoryDelete(CategoryModel categoryModel)
{
    var categoryEntity = await _context.Categories.FindAsync(categoryModel.Id);

    if (categoryEntity == null)
    {
        return NotFound();
    }

    // Marcar la categoría como inactiva
    categoryEntity.IsActive = false;

    // Actualizar productos asociados para asignarles una categoría nula
    var shirts = await _context.Shirts.Where(s => s.CategoryId == categoryModel.Id).ToListAsync();
    var sweaters = await _context.Sweaters.Where(s => s.CategoryId == categoryModel.Id).ToListAsync();
    var caps = await _context.Caps.Where(c => c.CategoryId == categoryModel.Id).ToListAsync();

    foreach (var shirt in shirts)
    {
        shirt.CategoryId = null; // Desasociar la categoría de las camisetas
    }

    foreach (var sweater in sweaters)
    {
        sweater.CategoryId = null; // Desasociar la categoría de los suéteres
    }

    foreach (var cap in caps)
    {
        cap.CategoryId = null; // Desasociar la categoría de las gorras
    }

    // Guardar cambios en la base de datos
    _context.Update(categoryEntity);
    await _context.SaveChangesAsync();

    return RedirectToAction("CategoryList"); // Redirige a la lista de categorías
}



    }
}
