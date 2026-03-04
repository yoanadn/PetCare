using Business_Layer;
using Data_Layer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Controllers
{
    public class VaccineController : Controller
    {
        private readonly PetCareDbContext _context;

        public VaccineController(PetCareDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vaccines = await _context.Vaccines
                .Include(v => v.Pet)
                .OrderByDescending(v => v.DateGiven)
                .ToListAsync();

            return View(vaccines);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccine = await _context.Vaccines
                .Include(v => v.Pet)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vaccine == null)
            {
                return NotFound();
            }

            return View(vaccine);
        }

        public IActionResult Create()
        {
            PopulatePetsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PetId,Type,DateGiven,NextDoseDate")] Vaccine vaccine)
        {
            if (!ModelState.IsValid)
            {
                PopulatePetsDropDownList(vaccine.PetId);
                return View(vaccine);
            }

            _context.Add(vaccine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccine = await _context.Vaccines.FindAsync(id);
            if (vaccine == null)
            {
                return NotFound();
            }

            PopulatePetsDropDownList(vaccine.PetId);
            return View(vaccine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PetId,Type,DateGiven,NextDoseDate")] Vaccine vaccine)
        {
            if (id != vaccine.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                PopulatePetsDropDownList(vaccine.PetId);
                return View(vaccine);
            }

            try
            {
                _context.Update(vaccine);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VaccineExists(vaccine.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccine = await _context.Vaccines
                .Include(v => v.Pet)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vaccine == null)
            {
                return NotFound();
            }

            return View(vaccine);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vaccine = await _context.Vaccines.FindAsync(id);
            if (vaccine != null)
            {
                _context.Vaccines.Remove(vaccine);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void PopulatePetsDropDownList(object? selectedPet = null)
        {
            var pets = _context.Pets
                .OrderBy(p => p.Name)
                .AsNoTracking()
                .ToList();

            ViewData["PetId"] = new SelectList(pets, "Id", "Name", selectedPet);
        }

        private bool VaccineExists(int id)
        {
            return _context.Vaccines.Any(e => e.Id == id);
        }
    }
}
