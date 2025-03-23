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
    
    public async Task<IActionResult> ShirtList()
{
    var shirts = await _context.Shirts
        .Include(s => s.Size)
        .Include(s => s.Material)
        .Include(s => s.Category)
        .ToListAsync();

    var shirtModels = shirts.Select(s => new ShirtModel
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

    return View(shirtModels); 
}
        public async Task<IActionResult> ShirtEdit(Guid id)
{
    var shirt = await _context.Shirts
        .Include(s => s.Size)
        .Include(s => s.Material)
        .Include(s => s.Category)
        .FirstOrDefaultAsync(s => s.Id == id);

    if (shirt == null)
    {
        return NotFound();
    }

    // Crear el modelo de vista
    var shirtModel = new ShirtModel
    {
        Id = shirt.Id,
        Name = shirt.Name,
        Color = shirt.Color,
        SizeId = shirt.SizeId,
        Price = shirt.Price,
        MaterialId = shirt.MaterialId,
        Quantity = shirt.Quantity,
        CategoryId = shirt.CategoryId,
        ImageUrl = shirt.ImageUrl,
        // Cargar listas de selección
        SizeList = await _context.Sizes.Select(s => new SelectListItem
        {
            Value = s.Id.ToString(),
            Text = s.Name
        }).ToListAsync(),
        MaterialList = await _context.Materials.Select(m => new SelectListItem
        {
            Value = m.Id.ToString(),
            Text = m.Name
        }).ToListAsync(),
        CategoryList = await _context.Categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToListAsync()
    };

    return View(shirtModel);
}
         [HttpPost]
        public async Task<IActionResult> ShirtEdit(ShirtModel model)
        {
            if (ModelState.IsValid)
            {
                     // Obtener el producto existente desde la base de datos usando el ID
                    var shirtToUpdate = await _context.Shirts.FindAsync(model.Id); // Suponiendo que "Shirts" es el DbSet de camisetas

                    // Verifica si la camiseta existe
                      if (shirtToUpdate == null)
             {
            // Si no se encuentra, puedes redirigir a otro lugar o mostrar un mensaje de error
                       return NotFound();
              }

        // Si no se ha cambiado la imagen, mantenemos la URL anterior
           if (model.Image == null)
             {
                 model.ImageUrl = shirtToUpdate.ImageUrl; // Mantenemos la URL existente
             }
           else
             {
            // Procesar la nueva imagen (si se ha subido una)
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", model.Image.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }
            model.ImageUrl = "/images/" + model.Image.FileName;
             }

                  // Actualizar el modelo con los nuevos datos
                  shirtToUpdate.Name = model.Name;
                  shirtToUpdate.Color = model.Color;
                  shirtToUpdate.SizeId = model.SizeId;
                  shirtToUpdate.Price = model.Price;
                  shirtToUpdate.MaterialId = model.MaterialId;
                  shirtToUpdate.Quantity = model.Quantity;
                shirtToUpdate.CategoryId = model.CategoryId;
                shirtToUpdate.ImageUrl = model.ImageUrl;

                // Guardar los cambios en la base de datos
                _context.Update(shirtToUpdate);
                await _context.SaveChangesAsync();

                return RedirectToAction("ShirtList","Shirt");
        
           }

         // Si el modelo no es válido, devolvemos la vista con los datos del modelo
           return View(model);
      }
  
        public async Task<IActionResult> ShirtDeleted(Guid Id)
        {
            Shirt? shirt = await this._context.Shirts
            .Where(s => s.Id == Id)
            .FirstOrDefaultAsync();

            if (shirt == null)
            {
                _logger.LogError("No se encontró la camisa");
                return RedirectToAction("ShirtList", "Shirt");
            }

                ShirtModel model = new ShirtModel
            {
                 Id = shirt.Id,
                 Name = shirt.Name,
                 Quantity = shirt.Quantity,
                 Color = shirt.Color,
                 Size = shirt.Size,
                 Price = shirt.Price
            };

            return View(model);
        }

            [HttpPost]
        public async Task<IActionResult> ShirtDeleted(ShirtModel shirt)
        {
            bool exists = await this._context.Shirts.AnyAsync(s => s.Id == shirt.Id);
            if (!exists)
            {
                _logger.LogError("No se encontró la camisa");
                return View(shirt);
            }

                Shirt shirtEntity = await this._context.Shirts
                .Where(s => s.Id == shirt.Id)
                .FirstAsync();

                this._context.Shirts.Remove(shirtEntity);
                await this._context.SaveChangesAsync();

                return RedirectToAction("ShirtList", "Shirt");
        }

        
    }
}
