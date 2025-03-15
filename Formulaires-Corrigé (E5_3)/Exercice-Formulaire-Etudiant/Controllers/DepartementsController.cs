using Exercice_Formulaire_Etudiant.Data;
using Exercice_Formulaire_Etudiant.Models;
using Exercice_Formulaire_Etudiant.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exercice_Formulaire_Etudiant.Controllers
{
    public class DepartementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            var vm = new FormulaireDepartementVM();
            return View("CreateOrEdit", vm);
        }


        public IActionResult Edit(int id)
        {
            var departement = _context.Departements.Find(id);

            if(departement == null)
            {
                return NotFound();
            }

            var vm = new FormulaireDepartementVM()
            {
                Id = departement.Id,
                Budget = departement.Budget,
                Nom = departement.Nom,
            };
            return View("CreateOrEdit", vm);
        }

        public IActionResult Save(FormulaireDepartementVM vm)
        {
            // Création
            if (vm.Id == 0)
            {
                var departement = new Departement
                {
                    Id = vm.Id,
                    Budget = vm.Budget,
                    Nom = vm.Nom,
                };

                _context.Add(departement);
            }
            //Modification
            else
            {
                var departement = _context.Departements.Find(vm.Id);
                if (departement == null)
                {
                    return NotFound();
                }
                departement.Budget = vm.Budget;
                departement.Nom = vm.Nom;
                _context.Update(departement);
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departement = _context.Departements.Find(id);
            if(departement == null)
            {
                return NotFound();
            }

            var vm = new FormulaireDepartementVM
            {
                Id = departement.Id,
                Budget = departement.Budget,
                Nom = departement.Nom,
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var departement = _context.Departements.Find(id);
            if (departement == null)
            {
                return NotFound();
            }

            _context.Remove(departement);
            return RedirectToAction("Index", "Home");
        }
    }
}
