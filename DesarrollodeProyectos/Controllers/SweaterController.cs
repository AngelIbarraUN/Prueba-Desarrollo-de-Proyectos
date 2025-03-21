using DesarrollodeProyectos.Identity;
using DesarrollodeProyectos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DesarrollodeProyectos.Controllers
{
    public class SweaterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SweaterController> _logger;

        public SweaterController(ApplicationDbContext context, ILogger<SweaterController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // Acción para agregar un nuevo suéter
        public async Task<IActionResult> SweaterAdd()
        {
            SweaterModel model = new SweaterModel();

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

        // Acción POST para agregar un nuevo suéter con imagen
        [HttpPost]
        public async Task<IActionResult> SweaterAdd(SweaterModel sweaterModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("El modelo del suéter no es válido");

                // Volver a cargar las listas de selección para el formulario
                sweaterModel.SizeList = await _context.Sizes
                    .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                    .ToListAsync();

                sweaterModel.MaterialList = await _context.Materials
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name })
                    .ToListAsync();

                sweaterModel.CategoryList = await _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToListAsync();

                return View(sweaterModel);
            }

           
            // Crear el nuevo suéter en la base de datos
            var sweaterEntity = new Sweater
            {
                Id = Guid.NewGuid(),
                Name = sweaterModel.Name,
                Color = sweaterModel.Color,
                SizeId = sweaterModel.SizeId,
                Price = sweaterModel.Price,
                MaterialId = sweaterModel.MaterialId,
                Quantity = sweaterModel.Quantity,
                CategoryId = sweaterModel.CategoryId,
                ImageUrl = sweaterModel.ImageUrl // Almacenar la URL de la imagen
            };

            _context.Sweaters.Add(sweaterEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("SweaterList"); // Redirige a la lista de suéteres
        }

        // Acción para ver todos los suéteres (opcional)
        public async Task<IActionResult> SweaterList()
        {
            var sweaters = await _context.Sweaters
                .Include(s => s.Size)
                .Include(s => s.Material)
                .Include(s => s.Category)
                .ToListAsync();

            return View(sweaters);
        }
    }
}
