using GestionDeProjets.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionDeProjets.Services;

public class PerformanceComparisonService
{
    private readonly GestionDeProjetsContext _context;
    private readonly ILogger<PerformanceComparisonService> _logger;

    public PerformanceComparisonService(GestionDeProjetsContext context, ILogger<PerformanceComparisonService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public class PerformanceResult
    {
        public string Method { get; set; }
        public long TimeInMilliseconds { get; set; }
        public int EntitiesLoaded { get; set; }
        public string EstimatedQueryCount { get; set; }
    }

    public async Task<List<PerformanceResult>> CompareLoadingPerformance(int projetId)
    {
        var results = new List<PerformanceResult>();

        try
        {
            // Test de performance pour le chargement immédiat
            var stopwatch1 = System.Diagnostics.Stopwatch.StartNew();

            var projetEager = await _context.Projets
                .AsNoTracking() // Pour un test plus équitable
                .Include(p => p.Taches)
                    .ThenInclude(t => t.EmployeAssigne)
                .Include(p => p.Membres)
                    .ThenInclude(m => m.Employe)
                .FirstOrDefaultAsync(p => p.Id == projetId);

            stopwatch1.Stop();

            var eagerResult = new PerformanceResult
            {
                Method = "Chargement immédiat (Eager Loading)",
                TimeInMilliseconds = stopwatch1.ElapsedMilliseconds,
                EntitiesLoaded = (projetEager?.Taches?.Count ?? 0) + (projetEager?.Membres?.Count ?? 0) + 1, // +1 pour le projet
                EstimatedQueryCount = "1 requête SQL"
            };

            results.Add(eagerResult);
            _logger.LogInformation($"Chargement immédiat: {stopwatch1.ElapsedMilliseconds}ms");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du test de chargement immédiat");
            results.Add(new PerformanceResult
            {
                Method = "Chargement immédiat (Eager Loading)",
                TimeInMilliseconds = -1,
                EntitiesLoaded = 0,
                EstimatedQueryCount = "Erreur: " + ex.Message
            });
        }

        try
        {
            // Test de performance pour le chargement explicite
            var stopwatch2 = System.Diagnostics.Stopwatch.StartNew();

            var projetExplicit = await _context.Projets
                .AsNoTracking() // Pour un test plus équitable
                .FirstOrDefaultAsync(p => p.Id == projetId);

            int tacheCount = 0;
            int membreCount = 0;

            if (projetExplicit != null)
            {
                // Charger explicitement toutes les tâches
                var taches = await _context.Taches
                    .AsNoTracking()
                    .Where(t => t.ProjetId == projetExplicit.Id)
                    .ToListAsync();
                tacheCount = taches.Count;

                // Charger explicitement les employés pour les tâches
                var employeIds = taches.Where(t => t.EmployeId.HasValue)
                    .Select(t => t.EmployeId.Value)
                    .Distinct()
                    .ToList();

                if (employeIds.Any())
                {
                    var employesTaches = await _context.Employes
                        .AsNoTracking()
                        .Where(e => employeIds.Contains(e.Id))
                        .ToListAsync();
                }

                // Charger explicitement tous les membres
                var membres = await _context.MembresProjet
                    .AsNoTracking()
                    .Where(m => m.ProjetId == projetExplicit.Id)
                    .ToListAsync();
                membreCount = membres.Count;

                // Charger explicitement les employés pour les membres
                var employeIdsMembres = membres.Select(m => m.EmployeId).Distinct().ToList();
                if (employeIdsMembres.Any())
                {
                    var employesMembres = await _context.Employes
                        .AsNoTracking()
                        .Where(e => employeIdsMembres.Contains(e.Id))
                        .ToListAsync();
                }
            }

            stopwatch2.Stop();

            var explicitResult = new PerformanceResult
            {
                Method = "Chargement explicite (Explicit Loading)",
                TimeInMilliseconds = stopwatch2.ElapsedMilliseconds,
                EntitiesLoaded = tacheCount + membreCount + 1, // +1 pour le projet
                EstimatedQueryCount = "5 requêtes SQL (max)"
            };

            results.Add(explicitResult);
            _logger.LogInformation($"Chargement explicite: {stopwatch2.ElapsedMilliseconds}ms");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du test de chargement explicite");
            results.Add(new PerformanceResult
            {
                Method = "Chargement explicite (Explicit Loading)",
                TimeInMilliseconds = -1,
                EntitiesLoaded = 0,
                EstimatedQueryCount = "Erreur: " + ex.Message
            });
        }

        // Seulement si le lazy loading est activé
        try
        {
            // Test de performance pour le chargement différé
            var stopwatch3 = System.Diagnostics.Stopwatch.StartNew();

            var projetLazy = await _context.Projets
                .FirstOrDefaultAsync(p => p.Id == projetId);

            int tacheCount = 0;
            int membreCount = 0;

            if (projetLazy != null)
            {
                // Accéder aux propriétés pour déclencher le chargement différé
                tacheCount = projetLazy.Taches.Count;
                membreCount = projetLazy.Membres.Count;

                foreach (var tache in projetLazy.Taches)
                {
                    if (tache.EmployeAssigne != null)
                    {
                        var nomEmploye = tache.EmployeAssigne.Nom;
                    }
                }

                foreach (var membre in projetLazy.Membres)
                {
                    var nomEmploye = membre.Employe.Nom;
                }
            }

            stopwatch3.Stop();

            var lazyResult = new PerformanceResult
            {
                Method = "Chargement différé (Lazy Loading)",
                TimeInMilliseconds = stopwatch3.ElapsedMilliseconds,
                EntitiesLoaded = tacheCount + membreCount + 1, // +1 pour le projet
                EstimatedQueryCount = $"{1 + tacheCount + membreCount} requêtes SQL (approximatif)"
            };

            results.Add(lazyResult);
            _logger.LogInformation($"Chargement différé: {stopwatch3.ElapsedMilliseconds}ms");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du test de chargement différé");
            results.Add(new PerformanceResult
            {
                Method = "Chargement différé (Lazy Loading)",
                TimeInMilliseconds = -1,
                EntitiesLoaded = 0,
                EstimatedQueryCount = "Erreur: " + ex.Message + " (Lazy loading désactivé?)"
            });
        }

        return results;
    }
}