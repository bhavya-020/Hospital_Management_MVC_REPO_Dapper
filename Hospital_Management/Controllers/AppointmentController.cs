using Hospital_Management.Models;
using Hospital_Management.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentRepository _repo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly IPatientRepository _patientRepo;

        public AppointmentController(
            IAppointmentRepository repo,
            IDoctorRepository doctorRepo,
            IPatientRepository patientRepo)
        {
            _repo = repo;
            _doctorRepo = doctorRepo;
            _patientRepo = patientRepo;
        }

        // ================= INDEX =================
        public IActionResult Index(
            string searchType,
            string searchText,
            DateTime? fromDate,
            DateTime? toDate,
            int page = 1)
        {
            int pageSize = 10;

            var result = _repo.GetFiltered(
                searchType,
                searchText,
                fromDate,
                toDate,
                page,
                pageSize
            );

            ViewBag.TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize);
            ViewBag.CurrentPage = page;

            ViewBag.SearchType = searchType;
            ViewBag.SearchText = searchText;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(result.Data);
        }

        // ================= CREATE =================
        public IActionResult Create()
        {
            ViewBag.Doctors = _doctorRepo.GetAll();
            ViewBag.Patients = _patientRepo.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AppointmentModel m)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Doctors = _doctorRepo.GetAll();
                ViewBag.Patients = _patientRepo.GetAll();
                return View(m);
            }

            int result = _repo.Insert(m);

            if (result == -1)
                ModelState.AddModelError("", "This doctor is already booked.");
            else if (result == -2)
                ModelState.AddModelError("", "This patient already has an appointment.");

            if (!ModelState.IsValid)
            {
                ViewBag.Doctors = _doctorRepo.GetAll();
                ViewBag.Patients = _patientRepo.GetAll();
                return View(m);
            }

            return RedirectToAction(nameof(Index));
        }

        // ================= EDIT =================
        public IActionResult Edit(int id)
        {
            var appt = _repo.GetById(id);
            if (appt == null) return NotFound();

            ViewBag.Doctors = _doctorRepo.GetAll();
            ViewBag.Patients = _patientRepo.GetAll();
            return View(appt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AppointmentModel m)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Doctors = _doctorRepo.GetAll();
                ViewBag.Patients = _patientRepo.GetAll();
                return View(m);
            }

            int result = _repo.Update(m);

            if (result == -1)
                ModelState.AddModelError("", "This doctor is already booked.");
            else if (result == -2)
                ModelState.AddModelError("", "This patient already has an appointment.");

            if (!ModelState.IsValid)
            {
                ViewBag.Doctors = _doctorRepo.GetAll();
                ViewBag.Patients = _patientRepo.GetAll();
                return View(m);
            }

            return RedirectToAction(nameof(Index));
        }

        // ================= DELETE =================
        public IActionResult Delete(int id)
        {
            _repo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
