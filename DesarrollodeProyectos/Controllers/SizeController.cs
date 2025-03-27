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

        // Método para mostrar la vista de agregar tamaño
        public IActionResult SizeAdd()
        {
            var model = new SizeModel();
            return View(model);
        }

        // Método POST para agregar un nuevo tamaño
        [HttpPost]
        public async Task<IActionResult> SizeAdd(SizeModel sizeModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("El modelo de la talla no es válido");
                return View(sizeModel);
            }

            var sizeEntity = new Size
            {
                Id = Guid.NewGuid(),
                Name = sizeModel.Name,
                CreationTime = DateTime.Now,
                IsActive = sizeModel.IsActive
            };

            _context.Sizes.Add(sizeEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("SizeList");
        }

        // Método para listar las tallas
        public async Task<IActionResult> SizeList()
        {
            var sizes = await _context.Sizes
                .Where(s => s.IsActive) // Solo tallas activas
                .ToListAsync();

            var sizeModels = sizes.Select(s => new SizeModel
            {
                Id = s.Id,
                Name = s.Name,
                CreationTime = s.CreationTime,
                IsActive = s.IsActive
            }).ToList();

            return View(sizeModels);
        }

        // Método para editar una talla existente
        public async Task<IActionResult> SizeEdit(Guid id)
        {
            var size = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);
            if (size == null)
            {
                return NotFound();
            }

            var sizeModel = new SizeModel
            {
                Id = size.Id,
                Name = size.Name,
                CreationTime = size.CreationTime,
                IsActive = size.IsActive
            };

            return View(sizeModel);
        }

        // Método POST para actualizar la talla
        [HttpPost]
        public async Task<IActionResult> SizeEdit(SizeModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var sizeToUpdate = await _context.Sizes.FindAsync(model.Id);
            if (sizeToUpdate == null)
            {
                return NotFound();
            }

            sizeToUpdate.Name = model.Name;
            sizeToUpdate.IsActive = model.IsActive;

            _context.Update(sizeToUpdate);
            await _context.SaveChangesAsync();

            return RedirectToAction("SizeList");
        }

        // Método para eliminar una talla
        public async Task<IActionResult> SizeDeleted(Guid id)
        {
            var size = await _context.Sizes
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (size == null)
            {
                _logger.LogError("No se encontró la talla");
                return RedirectToAction("SizeList");
            }

            var model = new SizeModel
            {
                Id = size.Id,
                Name = size.Name,
                IsActive = size.IsActive
            };

            return View(model);
        }

        // Método POST para eliminar una talla (marcar como inactiva)
        [HttpPost]
        public async Task<IActionResult> SizeDeleted(SizeModel size)
        {
            var sizeEntity = await _context.Sizes
                .Where(s => s.Id == size.Id)
                .FirstOrDefaultAsync();

            if (sizeEntity == null)
            {
                _logger.LogError("No se encontró la talla");
                return View(size);
            }

            sizeEntity.IsActive = false; // Marcar la talla como inactiva

            _context.Update(sizeEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("SizeList");
        }
    }
}
