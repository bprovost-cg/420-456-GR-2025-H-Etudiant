using MagasinDeVoitures.Models;
using MagasinDeVoitures.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;

namespace MagasinDeVoitures.Controllers
{
    public class VoituresController : Controller
    {
        public IList<Voiture> Voitures { get; set; } =
            [
                new () { Id = 1, Make = "Honda", Model = "Civic", Year = 2023 },
                new () { Id = 2, Make = "Tesla", Model = "Cybertruck", Year = 2025 },
                new () { Id = 3, Make = "Mazda", Model = "CX-5", Year = 2016, Edition = "SE" },
            ];
        // GET: VoituresController
        public ActionResult Index()
        {
            var vm = new VoituresIndexViewModel()
            {
                Voitures = Voitures
            };
            return View(vm);
            // Retours équivalents
            return View("Index", vm);
            return View(nameof(Index), vm);
        }

        // GET: VoituresController/Details/5
        public ActionResult Details(int id)
        {
            var voiture = Voitures.FirstOrDefault(v => v.Id == id);

            if( voiture == null)
            {
                // Lancer une exception utilise la gestion d'erreur et l'affiche à la place de votre page
                throw new InvalidOperationException($"La voiture avec l'id: {id}, n'existe pas.");
            }

            var vm = new VoituresDetailsViewModel()
            {
                Voiture = voiture
            };

            return View(vm);
        }
    }
}
