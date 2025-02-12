using Microsoft.AspNetCore.Mvc;
using Todo.Models;
using Todo.ViewModels;

namespace Todo.Controllers
{
    public class TachesController : Controller
    {

        // Pour ce TP, on peut utiliser une liste statique 
        private static List<Tache> _taches = new List<Tache>();
        private static int _compteurId = 1;

        // GET: /Taches/Index
        [HttpGet]
        public IActionResult Index(string nom_categorie)
        {
            // Confirme si la categorie fournie est valide
            var categorie_valide = Enum.TryParse(nom_categorie, out Categorie categorie);


            // On convertit la liste de Tache en TachesIndexViewModel
            var listeVM = _taches
                // On liste toutes les tâches si la catégorie fournie est invalide
                // Sinon, uniquement celles qui correspondent
                .Where(t => !categorie_valide || t.Categorie == categorie)
                .Select(t => new TacheIndexViewModel
                {
                    Id = t.Id,
                    Titre = t.Titre,
                    EstCompletee = t.EstCompletee,
                    Categorie = t.Categorie,
                    DateCompletion = t.DateCompletion,
                    DateLimite = t.DateLimite,
                })
                .ToList();

            ViewData["indexMessage"] = "Message du Index ViewData";

            ViewBag.IndexMessage = "Message du Index ViewBag";

            TempData["indexMessage"] = "Message du Index TempData";

            return View(new TachesIndexViewModel { Taches = listeVM, Categorie = categorie });
        }

        // GET: /Taches/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["createMessage"] = "Message du Create ViewData";

            ViewBag.CreateMessage = "Message du Create ViewBag";

            TempData["createMessage"] = "Message du Create TempData";
            return View();
        }

        // POST: /Taches/Create
        [HttpPost]
        public IActionResult Create(TachesCreateViewModel model)
        {

            if (model.Categorie == Categorie.Aucune)
            {
                ModelState.AddModelError("Categorie", "Vous devez choisir une catégorie");
            }

            if (!ModelState.IsValid)
            {
                // On renvoie la même vue avec les validations 
                return View(model);
            }

            

            // Création d’un nouvel objet Tache à partir du ViewModel
            var nouvelleTache = new Tache
            {
                Id = _compteurId++,
                Titre = model.Titre,
                EstCompletee = false,
                Categorie = model.Categorie,
                DateLimite = model.DateLimite
            };

            _taches.Add(nouvelleTache);

            ViewData["createMessage"] = "Message du Create ViewData";

            ViewBag.CreateMessage = "Message du Create ViewBag";

            TempData["createMessage"] = "Message du Create TempData";

            // Redirection vers la liste
            return RedirectToAction("Index");
        }

        // GET: /Taches/Complete/5
        [HttpGet("Taches/Complete/{id}")]
        public IActionResult Complete(int id)
        {
            var tache = _taches.FirstOrDefault(t => t.Id == id);
            if (tache == null) return NotFound();

            tache.EstCompletee = true;
            tache.DateCompletion = DateTimeOffset.Now;
            return RedirectToAction("Index");
        }

        // GET: /Taches/Delete/5
        [HttpGet("Taches/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var tache = _taches.FirstOrDefault(t => t.Id == id);
            if (tache == null) return NotFound();

            _taches.Remove(tache);
            return RedirectToAction("Index");
        }
    }
}
