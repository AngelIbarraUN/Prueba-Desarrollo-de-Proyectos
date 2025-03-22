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

            if (capModel.Image != null && capModel.Image.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", capModel.Image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await capModel.Image.CopyToAsync(stream);
                }
                capModel.ImageUrl = "/images/" + capModel.Image.FileName;
            }

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

            var capModels = caps.Select(c => new CapModel
            {
                Id = c.Id,
                Name = c.Name,
                Color = c.Color,
                Size = c.Size,
                Material = c.Material,
                Category = c.Category,
                Quantity = c.Quantity,
                Price = c.Price,
                ImageUrl = c.ImageUrl
            }).ToList();

            return View(capModels);
        }

        public async Task<IActionResult> CapEdit(Guid id)
        {
            var cap = await _context.Caps
                .Include(c => c.Size)
                .Include(c => c.Material)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cap == null)
            {
                return NotFound();
            }

            var capModel = new CapModel
            {
                Id = cap.Id,
                Name = cap.Name,
                Color = cap.Color,
                SizeId = cap.SizeId,
                Price = cap.Price,
                MaterialId = cap.MaterialId,
                Quantity = cap.Quantity,
                CategoryId = cap.CategoryId,
                ImageUrl = cap.ImageUrl,
                SizeList = await _context.Sizes.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToListAsync(),
                MaterialList = await _context.Materials.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToListAsync(),
                CategoryList = await _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync()
            };

            return View(capModel);
        }

        [HttpPost]
        public async Task<IActionResult> CapEdit(CapModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var capToUpdate = await _context.Caps.FindAsync(model.Id);
            if (capToUpdate == null)
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
                model.ImageUrl = capToUpdate.ImageUrl;
            }

            capToUpdate.Name = model.Name;
            capToUpdate.Color = model.Color;
            capToUpdate.SizeId = model.SizeId;
            capToUpdate.Price = model.Price;
            capToUpdate.MaterialId = model.MaterialId;
            capToUpdate.Quantity = model.Quantity;
            capToUpdate.CategoryId = model.CategoryId;
            capToUpdate.ImageUrl = model.ImageUrl;

            _context.Update(capToUpdate);
            await _context.SaveChangesAsync();

            return RedirectToAction("CapList");
        }

        public async Task<IActionResult> CapDeleted(Guid id)
        {
            var cap = await _context.Caps.FirstOrDefaultAsync(c => c.Id == id);
            if (cap == null)
            {
                _logger.LogError("No se encontró la gorra");
                return RedirectToAction("CapList");
            }

            return View(new CapModel { Id = cap.Id, Name = cap.Name, Quantity = cap.Quantity, Color = cap.Color, Price = cap.Price });
        }

        [HttpPost]
        public async Task<IActionResult> CapDeleted(CapModel cap)
        {
            var capEntity = await _context.Caps.FindAsync(cap.Id);
            if (capEntity == null)
            {
                _logger.LogError("No se encontró la gorra");
                return View(cap);
            }

            _context.Caps.Remove(capEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("CapList");
        }
    }
}
