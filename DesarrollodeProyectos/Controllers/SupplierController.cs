using DesarrollodeProyectos.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DesarrollodeProyectos.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(ApplicationDbContext context, ILogger<SupplierController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> SupplierAdd()
        {
            SupplierModel model = new SupplierModel();

            // Cargar la lista de materiales disponibles
            model.MaterialList = await _context.Materials
                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name })
                .ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SupplierAdd(SupplierModel supplierModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("El modelo del proveedor no es válido");
                // Volver a cargar la lista de materiales
                supplierModel.MaterialList = await _context.Materials
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name })
                    .ToListAsync();
                return View(supplierModel);
            }

            var supplierEntity = new Supplier
            {
                Id = Guid.NewGuid(),
                Name = supplierModel.Name,
                PhoneNumber = supplierModel.PhoneNumber,
                IsActive = supplierModel.IsActive,
                CreationTime = DateTime.Now,
                Materials = supplierModel.Materials // Asociar materiales seleccionados
            };

            _context.Suppliers.Add(supplierEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction("SupplierList", "Supplier");
        }

        public async Task<IActionResult> SupplierList()
        {
            var suppliers = await _context.Suppliers
                .Where(s => s.IsActive) // Filtrar proveedores activos
                .Include(s => s.Materials) // Incluir materiales asociados al proveedor
                .ToListAsync();

            var supplierModels = suppliers.Select(s => new SupplierModel
            {
                Id = s.Id,
                Name = s.Name,
                PhoneNumber = s.PhoneNumber,
                IsActive = s.IsActive,
                CreationTime = s.CreationTime,
                Materials = s.Materials
            }).ToList();

            return View(supplierModels);
        }

        public async Task<IActionResult> SupplierEdit(Guid id)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.Materials) // Incluir materiales asociados al proveedor
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null)
            {
                return NotFound();
            }

            var supplierModel = new SupplierModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                PhoneNumber = supplier.PhoneNumber,
                IsActive = supplier.IsActive,
                CreationTime = supplier.CreationTime,
                MaterialList = await _context.Materials.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToListAsync(),
                Materials = supplier.Materials // Cargar los materiales ya asociados
            };

            return View(supplierModel);
        }

        [HttpPost]
        public async Task<IActionResult> SupplierEdit(SupplierModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var supplierToUpdate = await _context.Suppliers.FindAsync(model.Id);
            if (supplierToUpdate == null)
            {
                return NotFound();
            }

            supplierToUpdate.Name = model.Name;
            supplierToUpdate.PhoneNumber = model.PhoneNumber;
            supplierToUpdate.IsActive = model.IsActive;
            supplierToUpdate.Materials = model.Materials; // Actualizar los materiales asociados

            _context.Update(supplierToUpdate);
            await _context.SaveChangesAsync();

            return RedirectToAction("SupplierList");
        }

        public async Task<IActionResult> SupplierDeleted(Guid id)
        {
            var supplier = await _context.Suppliers
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (supplier == null)
            {
                _logger.LogError("No se encontró el proveedor");
                return RedirectToAction("SupplierList", "Supplier");
            }

            SupplierModel model = new SupplierModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                PhoneNumber = supplier.PhoneNumber,
                IsActive = supplier.IsActive,
                CreationTime = supplier.CreationTime
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SupplierDeleted(SupplierModel supplier)
        {
            bool exists = await _context.Suppliers.AnyAsync(s => s.Id == supplier.Id);
            if (!exists)
            {
                _logger.LogError("No se encontró el proveedor");
                return View(supplier);
            }

            // Obtener el proveedor de la base de datos
            Supplier supplierEntity = await _context.Suppliers
                .Where(s => s.Id == supplier.Id)
                .FirstAsync();

            supplierEntity.IsActive = false;

            // Guardar los cambios en la base de datos
            _context.Update(supplierEntity);
            await _context.SaveChangesAsync();

            // Redirigir a la lista de proveedores
            return RedirectToAction("SupplierList", "Supplier");
        }
    }
}
