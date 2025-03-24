using DesarrollodeProyectos.Identity;
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

        [HttpPost]
        public async Task<IActionResult> SweaterAdd(SweaterModel sweaterModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("El modelo del suéter no es válido");
                sweaterModel.SizeList = await _context.Sizes.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToListAsync();
                sweaterModel.MaterialList = await _context.Materials.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToListAsync();
                sweaterModel.CategoryList = await _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync();
                return View(sweaterModel);
            }

            if (sweaterModel.Image != null && sweaterModel.Image.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sweaterModel.Image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await sweaterModel.Image.CopyToAsync(stream);
                }
                sweaterModel.ImageUrl = "/images/" + sweaterModel.Image.FileName;
            }

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
                ImageUrl = sweaterModel.ImageUrl,
                CreationTime = DateTime.Now 
            };

            _context.Sweaters.Add(sweaterEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("SweaterList","Sweater");
        }

        public async Task<IActionResult> SweaterList()
    {
         var sweaters = await _context.Sweaters
        .Where(s => s.IsActive) 
        .Include(s => s.Size)
        .Include(s => s.Material)
        .Include(s => s.Category)
        .ToListAsync();

        var sweaterModels = sweaters.Select(s => new SweaterModel
        {
         Id = s.Id,
         Name = s.Name,
         Color = s.Color,
         Size = s.Size,
         Material = s.Material,
         Category = s.Category,
         Quantity = s.Quantity,
         Price = s.Price,
         ImageUrl = s.ImageUrl
        }).ToList();

    return View(sweaterModels);
    }


        public async Task<IActionResult> SweaterEdit(Guid id)
        {
            var sweater = await _context.Sweaters
                .Include(s => s.Size)
                .Include(s => s.Material)
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sweater == null)
            {
                return NotFound();
            }

            var sweaterModel = new SweaterModel
            {
                Id = sweater.Id,
                Name = sweater.Name,
                Color = sweater.Color,
                SizeId = sweater.SizeId,
                Price = sweater.Price,
                MaterialId = sweater.MaterialId,
                Quantity = sweater.Quantity,
                CategoryId = sweater.CategoryId,
                ImageUrl = sweater.ImageUrl,
                SizeList = await _context.Sizes.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToListAsync(),
                MaterialList = await _context.Materials.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToListAsync(),
                CategoryList = await _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync()
            };

            return View(sweaterModel);
        }

        [HttpPost]
        public async Task<IActionResult> SweaterEdit(SweaterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var sweaterToUpdate = await _context.Sweaters.FindAsync(model.Id);
            if (sweaterToUpdate == null)
            {
                return NotFound();
            }

            if (model.Image != null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", model.Image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
                model.ImageUrl = "/images/" + model.Image.FileName;
            }
            else
            {
                model.ImageUrl = sweaterToUpdate.ImageUrl;
            }

            sweaterToUpdate.Name = model.Name;
            sweaterToUpdate.Color = model.Color;
            sweaterToUpdate.SizeId = model.SizeId;
            sweaterToUpdate.Price = model.Price;
            sweaterToUpdate.MaterialId = model.MaterialId;
            sweaterToUpdate.Quantity = model.Quantity;
            sweaterToUpdate.CategoryId = model.CategoryId;
            sweaterToUpdate.ImageUrl = model.ImageUrl;

            _context.Update(sweaterToUpdate);
            await _context.SaveChangesAsync();

            return RedirectToAction("SweaterList");
        }

                public async Task<IActionResult> SweaterDeleted(Guid id)
        {
            var sweater = await this._context.Sweaters
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (sweater == null)
            {
                _logger.LogError("No se encontró el suéter");
                return RedirectToAction("SweaterList", "Sweater");
            }

            SweaterModel model = new SweaterModel
            {
                Id = sweater.Id,
                Name = sweater.Name,
                Quantity = sweater.Quantity,
                Color = sweater.Color,
                Size = sweater.Size,
                Price = sweater.Price,
                IsActive = sweater.IsActive 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SweaterDeleted(SweaterModel sweater)
        {
         bool exists = await this._context.Sweaters.AnyAsync(s => s.Id == sweater.Id);
         if (!exists)
         {
                _logger.LogError("No se encontró el suéter");
             return View(sweater);
         }

          // Obtener el suéter de la base de datos
         Sweater sweaterEntity = await this._context.Sweaters
                .Where(s => s.Id == sweater.Id)
                .FirstAsync();

         sweaterEntity.IsActive = false;

         // Guardar los cambios en la base de datos
         this._context.Update(sweaterEntity);
            await this._context.SaveChangesAsync();

             // Redirigir a la lista de suéteres
              return RedirectToAction("SweaterList", "Sweater");
        }

    }
}
