using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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


        public async Task<IActionResult> Index()
        {
            var shoes = _context.Shoes.Include(s => s.Brand);
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
