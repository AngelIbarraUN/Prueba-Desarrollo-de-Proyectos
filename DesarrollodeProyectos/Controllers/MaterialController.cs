using DesarrollodeProyectos.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult MaterialAdd()
        {
            var materialModel = new MaterialModel
            {
                ShirtList = _context.Shirts.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList(),
                SweaterList = _context.Sweaters.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList(),
                CapList = _context.Caps.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };

            return View(materialModel);
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

            // Crear el nuevo material en la base de datos
            var materialEntity = new Material
            {
                Id = Guid.NewGuid(),
                Name = materialModel.Name,
                Description = materialModel.Description
            };

            _context.Materials.Add(materialEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("MaterialList"); // Redirige a la lista de materiales
        }

        // Acción para listar todos los materiales
        public async Task<IActionResult> MaterialList()
        {
            var materials = await _context.Materials.ToListAsync();

            var materialModels = materials.Select(m => new MaterialModel
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description
            }).ToList();

            return View(materialModels); // Pasar los modelos a la vista
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
                // Cargar proveedores
                SupplierList = await _context.Suppliers.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToListAsync(),
                SupplierId = material.SupplierId // Si el material tiene un proveedor, lo asignamos
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

            // Actualizar el material
            materialEntity.Name = materialModel.Name;
            materialEntity.Description = materialModel.Description;
            materialEntity.SupplierId = materialModel.SupplierId; // Asignar el proveedor seleccionado

            _context.Materials.Update(materialEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("MaterialList","Material"); // Redirige a la lista de materiales
        }


        // Acción para eliminar un material (GET)
        [HttpGet]
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

            var materialModel = new MaterialModel
            {
                Id = material.Id,
                Name = material.Name,
                Description = material.Description
            };

            return View(materialModel); // Devolver la vista MaterialDelete.cshtml con el modelo
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

            // Actualizar productos asociados para asignarles un material nulo (si corresponde)
            var shirts = await _context.Shirts.Where(s => s.MaterialId == materialModel.Id).ToListAsync();
            var sweaters = await _context.Sweaters.Where(s => s.MaterialId == materialModel.Id).ToListAsync();
            var caps = await _context.Caps.Where(c => c.MaterialId == materialModel.Id).ToListAsync();

            foreach (var shirt in shirts)
            {
                shirt.MaterialId = null;
            }

            foreach (var sweater in sweaters)
            {
                sweater.MaterialId = null;
            }

            foreach (var cap in caps)
            {
                cap.MaterialId = null;
            }

            // Eliminar el material
            _context.Materials.Remove(materialEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("MaterialList"); // Redirige a la lista de materiales
        }
    }
}