using DesarrollodeProyectos.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DesarrollodeProyectos.Controllers
{
    public class SizeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SizeController> _logger;

        public SizeController(ApplicationDbContext context, ILogger<SizeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // Acción para agregar una nueva talla
        public IActionResult SizeAdd()
        {
            return View();
        }

        // Acción POST para agregar una nueva talla
        [HttpPost]
        public async Task<IActionResult> SizeAdd(SizeModel sizeModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("El modelo de talla no es válido");
                return View(sizeModel);
            }

            // Crear la nueva talla en la base de datos
            var sizeEntity = new Size
            {
                Id = Guid.NewGuid(),
                Name = sizeModel.Name
            };

            _context.Sizes.Add(sizeEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("SizeList"); // Redirige a la lista de tallas
        }

        // Acción para listar las tallas
        public async Task<IActionResult> SizeList()
        {
            var sizes = await _context.Sizes.ToListAsync();

            // Mapear las entidades a modelos de vista
            var sizeModels = sizes.Select(s => new SizeModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            return View(sizeModels); // Pasar los modelos a la vista
        }

        // Acción para editar una talla
        public async Task<IActionResult> SizeEdit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var size = await _context.Sizes.FindAsync(id);

            if (size == null)
            {
                return NotFound();
            }

            var sizeModel = new SizeModel
            {
                Id = size.Id,
                Name = size.Name
            };

            return View(sizeModel);
        }

        // Acción POST para editar una talla
        [HttpPost]
        public async Task<IActionResult> SizeEdit(SizeModel sizeModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("El modelo de talla no es válido");
                return View(sizeModel);
            }

            var sizeEntity = await _context.Sizes.FindAsync(sizeModel.Id);

            if (sizeEntity == null)
            {
                return NotFound();
            }

            // Actualizar la talla
            sizeEntity.Name = sizeModel.Name;

            _context.Sizes.Update(sizeEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("SizeList"); // Redirige a la lista de tallas
        }

        // Acción para eliminar una talla
        [HttpGet]
        public async Task<IActionResult> SizeDelete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var size = await _context.Sizes.FindAsync(id);

            if (size == null)
            {
                return NotFound();
            }

            var model = new SizeModel
            {
                Id = size.Id,
                Name = size.Name
            };

            return View(model); // Devolver la vista con el modelo
        }

        [HttpPost]
        public async Task<IActionResult> SizeDelete(SizeModel sizeModel)
        {
            var sizeEntity = await _context.Sizes.FindAsync(sizeModel.Id);

            if (sizeEntity == null)
            {
                return NotFound();
            }

            // Eliminar la talla
            _context.Sizes.Remove(sizeEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("SizeList"); // Redirige a la lista de tallas
        }
    }
}
