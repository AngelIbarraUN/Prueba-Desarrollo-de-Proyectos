using DesarrollodeProyectos.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DesarrollodeProyectos.Controllers
{
    public class CapController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CapController> _logger;

        public CapController(ApplicationDbContext context, ILogger<CapController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> CapAdd()
        {
            CapModel model = new CapModel();

            model.SizeList = await _context.Sizes
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToListAsync();

            model.MaterialList = await _context.Materials
                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name })
                .ToListAsync();

            model.CategoryList = await _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CapAdd(CapModel capModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("El modelo de la gorra no es válido");

                // Volver a cargar las listas de selección para el formulario
                capModel.SizeList = await _context.Sizes
                    .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                    .ToListAsync();

                capModel.MaterialList = await _context.Materials
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name })
                    .ToListAsync();

                capModel.CategoryList = await _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToListAsync();

                return View(capModel);
            }

            // Guardar la imagen si se sube un archivo
            if (capModel.Image != null && capModel.Image.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", capModel.Image.FileName);

                // Guardar la imagen en la carpeta wwwroot/images
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await capModel.Image.CopyToAsync(stream);
                }

                // Guardar la URL o ruta relativa de la imagen en el modelo
                capModel.ImageUrl = "/images/" + capModel.Image.FileName;
            }

            // Crear la nueva gorra en la base de datos
            var capEntity = new Cap
            {
                Id = Guid.NewGuid(),
                Name = capModel.Name,
                Color = capModel.Color,
                SizeId = capModel.SizeId,
                Price = capModel.Price,
                MaterialId = capModel.MaterialId,
                Quantity = capModel.Quantity,
                CategoryId = capModel.CategoryId,
                ImageUrl = capModel.ImageUrl 
            };

            _context.Caps.Add(capEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("CapList"); 
        }

        public async Task<IActionResult> CapList()
        {
            var caps = await _context.Caps
                .Include(c => c.Size)
                .Include(c => c.Material)
                .Include(c => c.Category)
                .ToListAsync();

            return View(caps);
        }
    }
}
