using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace DesarrollodeProyectos.Controllers
{
    public class GoogleFormsController : Controller
    {
        public IActionResult GoogleForm()
        {
            return View();
        }
    }
}