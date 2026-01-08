using Hospital_Management.Models;
using Hospital_Management.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientRepository _repo;

        public PatientController(IPatientRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var patients = _repo.GetAll();
            return View(patients);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PatientModel m)
        {
            if (!ModelState.IsValid)
                return View(m);

            _repo.Insert(m);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var patient = _repo.GetById(id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PatientModel m)
        {
            if (!ModelState.IsValid)
                return View(m);

            _repo.Update(m);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            //_repo.Delete(id);
            //return RedirectToAction(nameof(Index));

            try
            {
                _repo.Delete(id);
                TempData["Success"] = "Patient deleted successfully.";
            }
            catch
            {
                TempData["Error"] = "Cannot delete patient because appointments are already booked.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
