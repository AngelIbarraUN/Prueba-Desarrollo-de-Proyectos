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
                Description = categoryModel.Description
            };

            _context.Categories.Add(categoryEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("CategoryList"); // Redirige a la lista de categorías
        }

        public async Task<IActionResult> CategoryList()
{
    var categories = await _context.Categories.ToListAsync();
    
    // Mapear las entidades a modelos de vista
    var categoryModels = categories.Select(c => new CategoryModel
    {
        Id = c.Id,
        Name = c.Name,
        Description = c.Description
    }).ToList();

    return View(categoryModels); // Pasar los modelos a la vista
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
                Description = category.Description
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

    return View(model); // Devolver la vista CategoryDeleted.cshtml con el modelo
}


    [HttpPost]
public async Task<IActionResult> CategoryDelete(CategoryModel categoryModel)
{
    var categoryEntity = await _context.Categories.FindAsync(categoryModel.Id);

    if (categoryEntity == null)
    {
        return NotFound();
    }

    // Actualizar productos asociados para asignarles una categoría nula
    var shirts = await _context.Shirts.Where(s => s.CategoryId == categoryModel.Id).ToListAsync();
    var sweaters = await _context.Sweaters.Where(s => s.CategoryId == categoryModel.Id).ToListAsync();
    var caps = await _context.Caps.Where(c => c.CategoryId == categoryModel.Id).ToListAsync();

    foreach (var shirt in shirts)
    {
        shirt.CategoryId = null;
    }

    foreach (var sweater in sweaters)
    {
        sweater.CategoryId = null;
    }

    foreach (var cap in caps)
    {
        cap.CategoryId = null;
    }

    // Eliminar la categoría
    _context.Categories.Remove(categoryEntity);
    await _context.SaveChangesAsync();

    return RedirectToAction("CategoryList"); // Redirige a la lista de categorías
}



    }
}
