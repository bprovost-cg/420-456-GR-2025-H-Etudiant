using GestionDeProjets.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionDeProjets.Controllers
{
    // Nécessite d'avoir activé options.UseLazyLoadingProxies() dans Program.cs
    public class LazyLoadingController : Controller
    {
        private readonly GestionDeProjetsContext _context;
        private readonly ILogger<LazyLoadingController> _logger;

        public LazyLoadingController(GestionDeProjetsContext context, ILogger<LazyLoadingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Affichage des détails d'un projet avec chargement différé
        public async Task<IActionResult> DetailsProjet(int id)
        {
            // Récupère uniquement le projet sans les entités associées
            var projet = await _context.Projets
                .FirstOrDefaultAsync(p => p.Id == id);

            if (projet == null)
            {
                return NotFound();
            }

            // Les données associées seront chargées automatiquement quand elles seront accédées

            // Dans la vue, quand on accède à projet.Taches, une requête SQL sera exécutée
            // Exemple: int nombreTaches = projet.Taches.Count; // Déclenche une requête SQL

            // Quand on parcourt les tâches et qu'on accède à leur employé, une requête SQL sera exécutée
            // pour chaque tâche (problème N+1)
            // foreach (var tache in projet.Taches)
            // {
            //     string nomEmploye = tache.EmployeAssigne?.Nom; // Déclenche une requête SQL par tâche
            // }

            _logger.LogInformation("Projet chargé avec chargement différé");

            return View(projet);
        }
    }
}
