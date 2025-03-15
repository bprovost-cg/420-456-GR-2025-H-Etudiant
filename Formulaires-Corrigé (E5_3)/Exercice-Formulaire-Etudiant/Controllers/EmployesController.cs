using Exercice_Formulaire_Etudiant.Data;
using Exercice_Formulaire_Etudiant.Models;
using Exercice_Formulaire_Etudiant.Services;
using Exercice_Formulaire_Etudiant.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Exercice_Formulaire_Etudiant.Controllers
{
    public class EmployesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmployesService _employesService;

        public EmployesController(ApplicationDbContext context, EmployesService employesService)
        {
            _context = context;
            _employesService = employesService;
        }

        #region CreateSync
        /*************** CREATE SYNCHRONE *****************/
        public IActionResult Create()
        {
            var vm = new EmployesCreateViewModel()
            {
                DateEmbauche = DateTime.Now,
                ListePays = _context.Pays
                    .Select(p => new SelectListItem(p.Nom, p.Id.ToString()))
                    .ToList(),
                ListeDepartement = _context.Departements
                    .Select(d => new SelectListItem(d.Nom, d.Id.ToString()))
                    .ToList()
            };
            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(EmployesCreateViewModel vm)
        {
            var employe = new Employe
            {
                Age = vm.Age,
                DateEmbauche = vm.DateEmbauche,
                DepartementId = vm.DepartementId,
                Nom = vm.Nom,
                PaysId = vm.PaysId,
                SalaireAnnuel = vm.SalaireAnnuel
            };
            _employesService.Create(employe);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region EditSync
        /*************** EDIT SYNCHRONE *****************/

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employe = _employesService.Get(id.Value);
            if (employe == null)
            {
                return NotFound();
            }

            var vm = new EmployesEditViewModel
            {
                Age = employe.Age,
                DateEmbauche = employe.DateEmbauche,
                DepartementId = employe.DepartementId,
                Id = employe.Id,
                Nom = employe.Nom,
                PaysId = employe.PaysId,
                SalaireAnnuel = employe.SalaireAnnuel,
                ListePays = _context.Pays
                    .Select(p => new SelectListItem(p.Nom, p.Id.ToString()))
                    .ToList(),
                ListeDepartement = _context.Departements
                    .Select(d => new SelectListItem(d.Nom, d.Id.ToString()))
                    .ToList()
            };
            return View("Edit", vm);
        }

        [HttpPost]
        public IActionResult Edit(int id, EmployesEditViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            var employe = _employesService.Get(id);

            if (employe == null)
            {
                return NotFound();
            }

            employe.Age = vm.Age;
            employe.DateEmbauche = vm.DateEmbauche;
            employe.DepartementId = vm.DepartementId;
            employe.Nom = vm.Nom;
            employe.PaysId = vm.PaysId;
            employe.SalaireAnnuel = vm.SalaireAnnuel;

            _employesService.Update(employe);

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region CreateAsync
        /*************** CREATE ASYNCHRONE *****************/

        [ActionName("CreateAsync")] // Nécessaire uniquement parce que la version synchrone est dans la même classe
        public async Task<IActionResult> CreateAsync()
        {
            var vm = new EmployesCreateViewModel()
            {
                DateEmbauche = DateTime.Now,
                ListePays = await _context.Pays
                    .Select(p => new SelectListItem(p.Nom, p.Id.ToString()))
                    .ToListAsync(),
                ListeDepartement = await _context.Departements
                    .Select(r => new SelectListItem(r.Nom, r.Id.ToString()))
                    .ToListAsync()
            };
            return View("Create",vm);
        }

        [HttpPost]
        [ActionName("CreateAsync")] // Nécessaire uniquement parce que la version synchrone est dans la même classe
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(EmployesCreateViewModel vm)
        {
            var employe = new Employe
            {
                Age = vm.Age,
                DateEmbauche = vm.DateEmbauche,
                DepartementId = vm.DepartementId,
                Nom = vm.Nom,
                PaysId = vm.PaysId,
                SalaireAnnuel = vm.SalaireAnnuel,
            };
            await _employesService.CreateAsync(employe);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region EditAsync
        /*************** EDIT ASYNCHRONE *****************/

        [ActionName("EditAsync")] // Nécessaire uniquement parce que la version synchrone est dans la même classe
        public async Task<IActionResult> EditAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employe = await _employesService.GetAsync(id.Value);
            if (employe == null)
            {
                return NotFound();
            }

            var vm = new EmployesEditViewModel
            {
                Age = employe.Age,
                DateEmbauche = employe.DateEmbauche,
                DepartementId = employe.DepartementId,
                Id = employe.Id,
                Nom = employe.Nom,
                PaysId = employe.PaysId,
                SalaireAnnuel = employe.SalaireAnnuel,
                ListePays = await _context.Pays
                    .Select(p => new SelectListItem(p.Nom, p.Id.ToString()))
                    .ToListAsync(),
                ListeDepartement = await _context.Departements
                    .Select(d => new SelectListItem(d.Nom, d.Id.ToString()))
                    .ToListAsync()
            };
            return View("Edit", vm);
        }


        [HttpPost]
        [ActionName("EditAsync")] // Nécessaire uniquement parce que la version synchrone est dans la même classe
        public async Task<IActionResult> EditAsync(int id, EmployesEditViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            var employe = await _employesService.GetAsync(id);

            if (employe == null)
            {
                return NotFound();
            }

            employe.Age = vm.Age;
            employe.DateEmbauche = vm.DateEmbauche;
            employe.DepartementId = vm.DepartementId;
            employe.Nom = vm.Nom;
            employe.PaysId = vm.PaysId;
            employe.SalaireAnnuel = vm.SalaireAnnuel;

            await _employesService.UpdateAsync(employe);

            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
