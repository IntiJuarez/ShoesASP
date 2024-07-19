using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoesASP.Models;

namespace ShoesASP.Controllers
{
    public class BrandController : Controller
    {
        //Obtener objeto de la inyección

        private readonly ShoesContext _context;

        //Construcctor
        
        public BrandController(ShoesContext context)
        {  _context = context; }


        public async Task<IActionResult> Index()
        {
            //regresa una vista.
            return View(await _context.Brands.ToListAsync());
        }
    }
}


//Una View es una archivo Razor.
//Por defecto todas las vistas hacen uso de _layout.cshtml
