using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using ShoesASP.Models;
using ShoesASP.Models.ViewModels;

namespace ShoesASP.Controllers
{
    public class ShoesController : Controller
    {

        //Obtener objeto de la inyección
        private readonly ShoesContext _context;

        //Construcctor
        public ShoesController(ShoesContext context)
        { _context = context; }


        /* 21/7/24
         Agrego Filtro de búsqueda.
         */
        public async Task<IActionResult> Index(string ordenar, string buscar)
        {
            ViewBag.OrdenarNombre = string.IsNullOrEmpty(ordenar) ? "nombre_desc" : "";
            ViewBag.OrdenarMarca = ordenar == "marca" ? "marca_desc" : "marca";
            buscar = buscar?.Trim();

            //var shoes = _context.Shoes.Include(s => s.Brand);
            var shoes = from s in _context.Shoes.Include(s => s.Brand)
                        select s;

            

            //Buscar
            if(!string.IsNullOrEmpty(buscar))
            {
                shoes = shoes.Where(s => s.Name.Contains(buscar));
            }

            switch (ordenar)
            {
                case "nombre_desc":
                    shoes = shoes.OrderByDescending(s => s.Name);
                    break;
                case "marca":
                    shoes = shoes.OrderBy(s => s.Brand.Name);
                    break;
                case "marca_desc":
                    shoes = shoes.OrderByDescending(s => s.Brand.Name);
                    break;
                default:
                    shoes = shoes.OrderBy(s => s.Name);
                    break;
            }

            return View(await shoes.ToListAsync());
        }

        //GET Create
        public IActionResult Create()
        {
            ViewData["Brands"] = new SelectList(_context.Brands, "BrandId", "Name");
            return View();
        }

        //POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShoesViewModel model)
        {
            if (ModelState.IsValid)
            {

                //Chequeo si la zapatilla existe
                var existingShoe = await _context.Shoes
                    .FirstOrDefaultAsync(s => s.Name == model.Name && s.BrandId == model.BrandId);

                if (existingShoe != null)
                {
                    //Mensaje de error:
                    ModelState.AddModelError(string.Empty, "Zapatilla existente");
                    ViewData["Brands"] = new SelectList(_context.Brands, "BrandId", "Name", model.BrandId);
                    return View(model);
                }


                var shoes = new Shoe()
                {
                    Name = model.Name,
                    BrandId = model.BrandId,
                };
                _context.Add(shoes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Brands"] = new SelectList(_context.Brands, "BrandId", "Name");
            return View();
        }

        //Delete
        //Necesito recibir por parámetro un Id.
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null || _context.Shoes == null)
            {
                return NotFound();
            }

            //Creo variable para almacenar una "zapatilla"
            var shoes = await _context.Shoes
                .FirstOrDefaultAsync(s => s.ShoesId == id);

            if (shoes == null)
            {
                return NotFound();
            }

            return View(shoes);
        }

        //Delete
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoes = await _context.Shoes.FindAsync(id);

            if (shoes == null)
            {
                return NotFound();
            }

            _context.Shoes.Remove(shoes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
        //Editar
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoes = await _context.Shoes.FindAsync(id);

            if(shoes == null)
            {
                return NotFound();
            }

            ViewData["Brands"] = new SelectList(_context.Brands, "BrandId", "Name", shoes.BrandId);
            
            return View(shoes);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("ShoesId", "Name", "BrandId")] Shoe shoe)
        {
            if (id != shoe.ShoesId)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoe);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!ShoeExists(shoe.ShoesId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));

            }

            ViewData["Brands"] = new SelectList(_context.Brands,"BrandId","Name", shoe.ShoesId);
            return View(shoe);

        }


        private bool ShoeExists(int id)
        {
            return _context.Shoes.Any(s => s.ShoesId == id);
        }


    }
}
