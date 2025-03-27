using DesarrollodeProyectos.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DesarrollodeProyectos.Controllers
{
    public class MaterialController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MaterialController> _logger;

        public MaterialController(ApplicationDbContext context, ILogger<MaterialController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // Acción para agregar un nuevo material
        public async Task<IActionResult> MaterialAdd()
        {
            var model = new MaterialModel
            {
                // Cargar listas de opciones para los campos
                ShirtList = await _context.Shirts
                    .Where(s => s.IsActive)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    }).ToListAsync(),
                SweaterList = await _context.Sweaters
                    .Where(s => s.IsActive)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    }).ToListAsync(),
                CapList = await _context.Caps
                    .Where(c => c.IsActive)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToListAsync(),
                SupplierList = await _context.Suppliers
                    .Where(s => s.IsActive)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    }).ToListAsync()
            };

            return View(model);
        }

        // Acción POST para agregar un nuevo material
        [HttpPost]
        public async Task<IActionResult> MaterialAdd(MaterialModel materialModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("El modelo de material no es válido");
                return View(materialModel);
            }

            var materialEntity = new Material
            {
                Id = Guid.NewGuid(),
                Name = materialModel.Name,
                Description = materialModel.Description,
                Quantity = materialModel.Quantity,
                CreationTime = DateTime.Now,
                IsActive = materialModel.IsActive,
                SupplierId = materialModel.SupplierId
            };

            _context.Materials.Add(materialEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("MaterialList"); // Redirige a la lista de materiales
        }

        // Acción para listar los materiales
        public async Task<IActionResult> MaterialList()
        {
            var materials = await _context.Materials
                                   .Where(m => m.IsActive)
                                   .ToListAsync();

            var materialModels = materials.Select(m => new MaterialModel
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Quantity = m.Quantity,
                CreationTime = m.CreationTime,
                IsActive = m.IsActive,
                SupplierId = m.SupplierId
            }).ToList();

            return View(materialModels); 
        }

        // Acción para editar un material
        public async Task<IActionResult> MaterialEdit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var material = await _context.Materials.FindAsync(id);

            if (material == null)
            {
                return NotFound();
            }

            var materialModel = new MaterialModel
            {
                Id = material.Id,
                Name = material.Name,
                Description = material.Description,
                Quantity = material.Quantity,
                CreationTime = material.CreationTime,
                IsActive = material.IsActive,
                SupplierId = material.SupplierId,
                // Cargar listas de opciones
                ShirtList = await _context.Shirts
                    .Where(s => s.IsActive)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    }).ToListAsync(),
                SweaterList = await _context.Sweaters
                    .Where(s => s.IsActive)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    }).ToListAsync(),
                CapList = await _context.Caps
                    .Where(c => c.IsActive)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToListAsync(),
                SupplierList = await _context.Suppliers
                    .Where(s => s.IsActive)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    }).ToListAsync()
            };

            return View(materialModel);
        }

        // Acción POST para editar un material
        [HttpPost]
        public async Task<IActionResult> MaterialEdit(MaterialModel materialModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("El modelo de material no es válido");
                return View(materialModel);
            }

            var materialEntity = await _context.Materials.FindAsync(materialModel.Id);

            if (materialEntity == null)
            {
                return NotFound();
            }

            // Actualizar material
            materialEntity.Name = materialModel.Name;
            materialEntity.Description = materialModel.Description;
            materialEntity.Quantity = materialModel.Quantity;
            materialEntity.SupplierId = materialModel.SupplierId;

            _context.Materials.Update(materialEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("MaterialList"); // Redirige a la lista de materiales
        }

        // Acción para eliminar un material (GET)
        public async Task<IActionResult> MaterialDelete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var material = await _context.Materials.FindAsync(id);

            if (material == null)
            {
                return NotFound();
            }

            var model = new MaterialModel
            {
                Id = material.Id,
                Name = material.Name,
                Description = material.Description,
                Quantity = material.Quantity
            };

            return View("MaterialDeleted", model); // Devolver vista MaterialDeleted.cshtml con el modelo
        }

        // Acción POST para eliminar un material
        [HttpPost]
        public async Task<IActionResult> MaterialDelete(MaterialModel materialModel)
        {
            var materialEntity = await _context.Materials.FindAsync(materialModel.Id);

            if (materialEntity == null)
            {
                return NotFound();
            }

            // Marcar el material como inactivo
            materialEntity.IsActive = false;

            // Guardar cambios en la base de datos
            _context.Update(materialEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("MaterialList"); // Redirige a la lista de materiales
        }
    }
}
