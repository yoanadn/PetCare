using Business_Layer;
using Data_Layer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Controllers
{
    public class PetController : Controller
    {
        private readonly PetCareDbContext _context;

        public PetController(PetCareDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var pets = await _context.Pets
                .Include(p => p.Owner)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return View(pets);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        public IActionResult Create()
        {
            PopulateOwnersDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Species,Breed,BirthDate,Weight,OwnerId")] Pet pet)
        {
            if (!_context.Owners.Any(o => o.Id == pet.OwnerId))
            {
                ModelState.AddModelError(nameof(Pet.OwnerId), "Please select a valid owner.");
            }

            if (!ModelState.IsValid)
            {
                PopulateOwnersDropDownList(pet.OwnerId);
                return View(pet);
            }

            _context.Pets.Add(pet);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save pet. Please check the selected owner.");
                PopulateOwnersDropDownList(pet.OwnerId);
                return View(pet);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            PopulateOwnersDropDownList(pet.OwnerId);
            return View(pet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Species,Breed,BirthDate,Weight,OwnerId")] Pet pet)
        {
            if (id != pet.Id)
            {
                return NotFound();
            }

            if (!_context.Owners.Any(o => o.Id == pet.OwnerId))
            {
                ModelState.AddModelError(nameof(Pet.OwnerId), "Please select a valid owner.");
            }

            if (!ModelState.IsValid)
            {
                PopulateOwnersDropDownList(pet.OwnerId);
                return View(pet);
            }

            try
            {
                _context.Update(pet);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetExists(pet.Id))
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

            var pet = await _context.Pets
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet != null)
            {
                _context.Pets.Remove(pet);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = "Pet cannot be deleted because it is used by related records (appointments, vaccines, or visits).";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private void PopulateOwnersDropDownList(object? selectedOwner = null)
        {
            var ownersQuery = _context.Owners
                .OrderBy(o => o.FullName)
                .AsNoTracking()
                .ToList();

            ViewBag.Owners = new SelectList(ownersQuery, "Id", "FullName", selectedOwner);
        }

        private bool PetExists(int id)
        {
            return _context.Pets.Any(e => e.Id == id);
        }
    }
}
