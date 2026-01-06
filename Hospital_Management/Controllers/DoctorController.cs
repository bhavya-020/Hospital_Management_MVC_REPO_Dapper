using Hospital_Management.Models;
using Hospital_Management.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospital_Management.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _repo;

        public DoctorController(IDoctorRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var doctors = _repo.GetAll();
            return View(doctors);
        }

        public IActionResult Create()
        {
            var model = new DoctorModel
            {
                SpecializationList = GetSpecializations()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DoctorModel m)
        {
            if (!ModelState.IsValid)
            {
                //
                m.SpecializationList = GetSpecializations();
                return View(m);
            }

            try
            {
                _repo.Insert(m);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                m.SpecializationList = GetSpecializations();
                return View(m);
            }
        }

        public IActionResult Edit(int id)
        {
            var doctor = _repo.GetById(id);

            if (doctor == null)
                return NotFound();

            doctor.SpecializationList = GetSpecializations();
            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DoctorModel m)
        {
            if (!ModelState.IsValid)
            {
                //
                m.SpecializationList = GetSpecializations();
                return View(m);
            }

            try
            {
                _repo.Update(m);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                m.SpecializationList = GetSpecializations();
                return View(m);
            }
        }

        public IActionResult Delete(int id)
        {
            _repo.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> GetSpecializations()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Cardiologist", Value = "Cardiologist" },
                new SelectListItem { Text = "Dermatologist", Value = "Dermatologist" },
                new SelectListItem { Text = "Neurologist", Value = "Neurologist" },
                new SelectListItem { Text = "Orthopedic", Value = "Orthopedic" },
                new SelectListItem { Text = "Pediatrician", Value = "Pediatrician" },
                new SelectListItem { Text = "Other", Value = "Other" }
            };
        }
    }
}
