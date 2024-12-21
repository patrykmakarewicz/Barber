using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Barber.Data;
using Barber.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Barber.Data;
using Barber.Models;

namespace YourProject.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly SalonContext _context;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(SalonContext context, ILogger<AppointmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            // Pobieramy listę wizyt wraz z powiązanymi danymi
            var appointments = await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Service)
                .Include(a => a.Employee)
                .ToListAsync();
            return View(appointments);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            // Przekazujemy listy klientów, usług i pracowników do widoku
            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Services = _context.Services.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                // Logujemy błędy walidacji
                foreach (var state in ModelState.Values)
                {
                    foreach (var error in state.Errors)
                    {
                        _logger.LogError("Model error (Create Appointment): {ErrorMessage}", error.ErrorMessage);
                    }
                }

                ViewBag.Customers = _context.Customers.ToList();
                ViewBag.Services = _context.Services.ToList();
                ViewBag.Employees = _context.Employees.ToList();
                return View(appointment);
            }

            appointment.Status = AppointmentStatus.Scheduled;
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Services = _context.Services.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                // Logujemy błędy walidacji przy edycji
                foreach (var state in ModelState.Values)
                {
                    foreach (var error in state.Errors)
                    {
                        _logger.LogError("Model error (Edit Appointment): {ErrorMessage}", error.ErrorMessage);
                    }
                }

                ViewBag.Customers = _context.Customers.ToList();
                ViewBag.Services = _context.Services.ToList();
                ViewBag.Employees = _context.Employees.ToList();
                return View(appointment);
            }

            try
            {
                _context.Update(appointment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(appointment.Id))
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

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Service)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Service)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
