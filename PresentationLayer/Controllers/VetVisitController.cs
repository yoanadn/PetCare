using Business_Layer;
using Data_Layer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Controllers
{
    public class VetVisitController : Controller
    {
        private readonly PetCareDbContext _context;

        public VetVisitController(PetCareDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var visits = await _context.VetVisits
                .Include(v => v.Pet)
                .OrderByDescending(v => v.Date)
                .ToListAsync();

            return View(visits);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.VetVisits
                .Include(v => v.Pet)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        public IActionResult Create()
        {
            PopulatePetsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PetId,Date,Reason,Notes,VetName")] VetVisit vetVisit)
        {
            await ValidateVetVisitDateAsync(vetVisit);

            if (!ModelState.IsValid)
            {
                PopulatePetsDropDownList(vetVisit.PetId);
                return View(vetVisit);
            }

            _context.Add(vetVisit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.VetVisits.FindAsync(id);
            if (visit == null)
            {
                return NotFound();
            }

            PopulatePetsDropDownList(visit.PetId);
            return View(visit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PetId,Date,Reason,Notes,VetName")] VetVisit vetVisit)
        {
            if (id != vetVisit.Id)
            {
                return NotFound();
            }

            await ValidateVetVisitDateAsync(vetVisit);

            if (!ModelState.IsValid)
            {
                PopulatePetsDropDownList(vetVisit.PetId);
                return View(vetVisit);
            }

            try
            {
                _context.Update(vetVisit);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VetVisitExists(vetVisit.Id))
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

            var visit = await _context.VetVisits
                .Include(v => v.Pet)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visit = await _context.VetVisits.FindAsync(id);
            if (visit != null)
            {
                _context.VetVisits.Remove(visit);
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

        private bool VetVisitExists(int id)
        {
            return _context.VetVisits.Any(e => e.Id == id);
        }

        private async Task ValidateVetVisitDateAsync(VetVisit vetVisit)
        {
            var pet = await _context.Pets
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == vetVisit.PetId);

            if (pet == null)
            {
                ModelState.AddModelError(nameof(VetVisit.PetId), "Please select a valid pet.");
                return;
            }

            if (pet.BirthDate.HasValue && vetVisit.Date.Date < pet.BirthDate.Value.Date)
            {
                ModelState.AddModelError(nameof(VetVisit.Date), "Visit date cannot be before the pet birth date.");
            }
        }
    }
}
