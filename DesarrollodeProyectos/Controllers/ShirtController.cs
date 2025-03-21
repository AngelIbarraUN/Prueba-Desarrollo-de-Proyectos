using DesarrollodeProyectos.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DesarrollodeProyectos.Controllers
{
    public class ShirtController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ShirtController> _logger;

        public ShirtController(ApplicationDbContext context, ILogger<ShirtController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // Acción para agregar una nueva camisa
        public async Task<IActionResult> ShirtAdd()
        {
            ShirtModel model = new ShirtModel();

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

        // Acción POST para agregar una nueva camisa con imagen
        [HttpPost]
        public async Task<IActionResult> ShirtAdd(ShirtModel shirtModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("El modelo de la camisa no es válido");

                // Volver a cargar las listas de selección para el formulario
                shirtModel.SizeList = await _context.Sizes
                    .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                    .ToListAsync();

                shirtModel.MaterialList = await _context.Materials
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name })
                    .ToListAsync();

                shirtModel.CategoryList = await _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToListAsync();

                return View(shirtModel);
            }

            // Guardar la imagen si se sube un archivo
            if (shirtModel.Image != null && shirtModel.Image.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", shirtModel.Image.FileName);

                // Guardar la imagen en la carpeta wwwroot/images
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await shirtModel.Image.CopyToAsync(stream);
                }

                // Guardar la URL o ruta relativa de la imagen en el modelo
                shirtModel.ImageUrl = "/images/" + shirtModel.Image.FileName;
            }

            // Crear la nueva camisa en la base de datos
            var shirtEntity = new Shirt
            {
                Id = Guid.NewGuid(),
                Name = shirtModel.Name,
                Color = shirtModel.Color,
                SizeId = shirtModel.SizeId,
                Price = shirtModel.Price,
                MaterialId = shirtModel.MaterialId,
                Quantity = shirtModel.Quantity,
                CategoryId = shirtModel.CategoryId,
                ImageUrl = shirtModel.ImageUrl // Almacenar la URL de la imagen
            };

            _context.Shirts.Add(shirtEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("ShirtList"); // Redirige a la lista de camisas
        }

        // Acción para ver todas las camisas (opcional)
        public async Task<IActionResult> ShirtList()
        {
            var shirts = await _context.Shirts
                .Include(s => s.Size)
                .Include(s => s.Material)
                .Include(s => s.Category)
                .ToListAsync();

            return View(shirts);
        }
    }
}
