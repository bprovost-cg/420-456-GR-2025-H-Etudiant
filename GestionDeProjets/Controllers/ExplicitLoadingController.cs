using GestionDeProjets.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionDeProjets.Controllers
{
    public class ExplicitLoadingController : Controller
    {
        private readonly GestionDeProjetsContext _context;
        private readonly ILogger<ExplicitLoadingController> _logger;

        public ExplicitLoadingController(GestionDeProjetsContext context, ILogger<ExplicitLoadingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Chargement explicite des entités associées
        public async Task<IActionResult> DetailsProjet(int id)
        {
            // Récupère uniquement le projet
            var projet = await _context.Projets
                .FirstOrDefaultAsync(p => p.Id == id);

            if (projet == null)
            {
                return NotFound();
            }

            // Charge explicitement toutes les tâches du projet
            await _context.Entry(projet)
                .Collection(p => p.Taches)
                .LoadAsync();

            // Charge explicitement tous les membres du projet
            await _context.Entry(projet)
                .Collection(p => p.Membres)
                .LoadAsync();

            // Pour chaque tâche, charge l'employé assigné
            foreach (var tache in projet.Taches)
            {
                if (tache.EmployeId.HasValue)
                {
                    await _context.Entry(tache)
                        .Reference(t => t.EmployeAssigne)
                        .LoadAsync();
                }
            }

            // Pour chaque membre, charge les données de l'employé
            foreach (var membre in projet.Membres)
            {
                await _context.Entry(membre)
                    .Reference(m => m.Employe)
                    .LoadAsync();
            }

            _logger.LogInformation("Projet chargé avec toutes les données associées via chargement explicite");

            return View(projet);
        }

        // Exemple de chargement explicite avec filtrage
        public async Task<IActionResult> TachesRecentes(int id)
        {
            var projet = await _context.Projets
                .FirstOrDefaultAsync(p => p.Id == id);

            if (projet == null)
            {
                return NotFound();
            }

            // Charge explicitement uniquement les tâches créées au cours des 7 derniers jours
            await _context.Entry(projet)
                .Collection(p => p.Taches)
                .Query()
                .Where(t => t.DateCreation >= DateTime.Now.AddDays(-7))
                .LoadAsync();

            _logger.LogInformation("Projet chargé avec tâches récentes via chargement explicite filtré");

            return View(projet);
        }
    }
}
