using Data_Layer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;
using System.Diagnostics;

namespace PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PetCareDbContext _context;

        public HomeController(ILogger<HomeController> logger, PetCareDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardStatsViewModel
            {
                OwnersCount = await _context.Owners.CountAsync(),
                PetsCount = await _context.Pets.CountAsync(),
                AppointmentsCount = await _context.Appointments.CountAsync(),
                VaccinesCount = await _context.Vaccines.CountAsync(),
                VetVisitsCount = await _context.VetVisits.CountAsync()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
