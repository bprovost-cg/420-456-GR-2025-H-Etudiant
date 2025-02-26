using GestionDeProjets.Data;
using GestionDeProjets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionDeProjets.Controllers
{
    public class EagerLoadingController : Controller
    {
        private readonly GestionDeProjetsContext _context;
        private readonly ILogger<EagerLoadingController> _logger;

        public EagerLoadingController(GestionDeProjetsContext context, ILogger<EagerLoadingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Récupère les détails d'un projet avec toutes ses tâches et membres
        public async Task<IActionResult> DetailsProjet(int id)
        {
            // Utilisation du chargement immédiat avec Include et ThenInclude
            var projet = await _context.Projets
                .Include(p => p.Taches)  // Inclut toutes les tâches du projet
                    .ThenInclude(t => t.EmployeAssigne)  // Pour chaque tâche, inclut l'employé assigné
                .Include(p => p.Taches)  // Nécessaire de répéter pour une nouvelle chaîne ThenInclude
                    .ThenInclude(t => t.Commentaires)  // Pour chaque tâche, inclut les commentaires
                        .ThenInclude(c => c.Employe)  // Pour chaque commentaire, inclut l'employé
                .Include(p => p.Membres)  // Inclut tous les membres du projet
                    .ThenInclude(m => m.Employe)  // Pour chaque membre, inclut l'employé
                .FirstOrDefaultAsync(p => p.Id == id);

            if (projet == null)
            {
                return NotFound();
            }

            // Log pour montrer la requête SQL générée (visible dans la console ou les logs)
            _logger.LogInformation("Projet chargé avec toutes les données associées en une seule requête");

            return View(projet);
        }

        // Exemple de filtrage avec chargement immédiat
        public async Task<IActionResult> TachesHautePriorite(int projetId)
        {
            var projet = await _context.Projets
                .Include(p => p.Taches.Where(t => t.Priorite == PrioriteEnum.Haute || t.Priorite == PrioriteEnum.Urgente))
                    .ThenInclude(t => t.EmployeAssigne)
                .FirstOrDefaultAsync(p => p.Id == projetId);

            if (projet == null)
            {
                return NotFound();
            }

            return View(projet);
        }
    }

}
