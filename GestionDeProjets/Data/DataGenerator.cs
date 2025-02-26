using Bogus;
using GestionDeProjets.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestionProjets.Data
{
    public static class DataGenerator
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            // Configuration de Bogus pour la langue française
            Randomizer.Seed = new Random(2435619);

            // Constantes pour contrôler le volume de données
            const int NombreEmployes = 50;
            const int NombreProjets = 20;
            const int MinTachesParProjet = 20;
            const int MaxTachesParProjet = 100;
            const int MinMembresParProjet = 3;
            const int MaxMembresParProjet = 12;
            const int MinCommentairesParTache = 0;
            const int MaxCommentairesParTache = 5;

            // Liste pour stocker les entités générées
            var employes = new List<Employe>();
            var projets = new List<Projet>();
            var taches = new List<Tache>();
            var membresProjet = new List<MembreProjet>();
            var commentaires = new List<Commentaire>();

            // Générateur d'employés
            var employeId = 1;
            var employeGenerator = new Faker<Employe>()
                .RuleFor(e => e.Id, _ => employeId++)
                .RuleFor(e => e.Nom, f => f.Name.LastName())
                .RuleFor(e => e.Prenom, f => f.Name.FirstName())
                .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.Prenom, e.Nom))
                .RuleFor(e => e.Poste, f => f.PickRandom(new[] {
                    "Développeur", "Designer UX/UI", "Chef de projet",
                    "Analyste d'affaires", "Testeur QA", "DevOps",
                    "Architecte solution", "Administrateur système"
                }));

            employes = employeGenerator.Generate(NombreEmployes);

            // Générateur de projets
            var projetId = 1;
            var projetGenerator = new Faker<Projet>()
                .RuleFor(p => p.Id, _ => projetId++)
                .RuleFor(p => p.Nom, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
                .RuleFor(p => p.DateDebut, f => f.Date.Past(2))
                .RuleFor(p => p.DateFin, (f, p) => f.Random.Bool(0.7f) ? f.Date.Future(1, p.DateDebut) : null)
                .RuleFor(p => p.Budget, f => Math.Round(f.Random.Decimal(10000, 500000), 2));

            projets = projetGenerator.Generate(NombreProjets);

            // Générateur de membres de projet
            var membreProjetId = 1;

            foreach (var projet in projets)
            {
                // Sélection aléatoire d'employés pour ce projet
                var nombreMembres = new Random().Next(MinMembresParProjet, MaxMembresParProjet + 1);
                var employesProjet = employes.OrderBy(_ => Guid.NewGuid()).Take(nombreMembres).ToList();

                // Attribution des rôles au hasard (au moins un chef de projet)
                var chefDeProjet = employesProjet[0];
                var respTechnique = employesProjet.Count > 1 ? employesProjet[1] : chefDeProjet;

                foreach (var employe in employesProjet)
                {
                    var role = RoleEnum.Membre;
                    if (employe == chefDeProjet)
                        role = RoleEnum.ChefDeProjet;
                    else if (employe == respTechnique)
                        role = RoleEnum.ResponsableTechnique;

                    membresProjet.Add(new MembreProjet
                    {
                        Id = membreProjetId++,
                        ProjetId = projet.Id,
                        EmployeId = employe.Id,
                        Role = role,
                        DateAjout = new Faker().Date.Between(projet.DateDebut, DateTime.Now)
                    });
                }
            }

            // Générateur de tâches
            var tacheId = 1;
            var random = new Random();

            foreach (var projet in projets)
            {
                // Déterminer le nombre de tâches pour ce projet
                var nombreTaches = random.Next(MinTachesParProjet, MaxTachesParProjet + 1);

                // Récupérer les membres de ce projet
                var membresDuProjet = membresProjet
                    .Where(m => m.ProjetId == projet.Id)
                    .Select(m => m.EmployeId)
                    .ToList();

                // Générer les tâches pour ce projet
                var tacheGenerator = new Faker<Tache>()
                    .RuleFor(t => t.Id, _ => tacheId++)
                    .RuleFor(t => t.ProjetId, _ => projet.Id)
                    .RuleFor(t => t.EmployeId, f => f.Random.Bool(0.9f) ? f.PickRandom(membresDuProjet) : (int?)null)
                    .RuleFor(t => t.Titre, f => f.Lorem.Sentence(3, 5).TrimEnd('.'))
                    .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
                    .RuleFor(t => t.DateCreation, f => f.Date.Between(projet.DateDebut, DateTime.Now))
                    .RuleFor(t => t.DateEcheance, (f, t) => f.Random.Bool(0.8f) ? f.Date.Future(1, t.DateCreation) : null)
                    .RuleFor(t => t.DateAchievement, (f, t) =>
                        f.Random.Bool(0.6f) && t.DateEcheance.HasValue ?
                        f.Date.Between(t.DateCreation, t.DateEcheance.Value > DateTime.Now ? DateTime.Now : t.DateEcheance.Value) :
                        null)
                    .RuleFor(t => t.Priorite, f => f.PickRandom<PrioriteEnum>())
                    .RuleFor(t => t.Statut, (f, t) =>
                        t.DateAchievement.HasValue ? StatutEnum.Terminee :
                        f.Random.Bool(0.1f) ? StatutEnum.Annulee :
                        f.Random.Bool(0.2f) ? StatutEnum.EnAttente :
                        f.Random.Bool(0.6f) ? StatutEnum.EnCours :
                        StatutEnum.NonCommencee);

                var tachesProjet = tacheGenerator.Generate(nombreTaches);
                taches.AddRange(tachesProjet);
            }

            // Générateur de commentaires
            var commentaireId = 1;

            foreach (var tache in taches)
            {
                // Déterminer le nombre de commentaires pour cette tâche
                var nombreCommentaires = random.Next(MinCommentairesParTache, MaxCommentairesParTache + 1);
                if (nombreCommentaires == 0) continue;

                // Récupérer les membres du projet de cette tâche
                var membresDuProjet = membresProjet
                    .Where(m => m.ProjetId == tache.ProjetId)
                    .Select(m => m.EmployeId)
                    .ToList();

                // Générer une liste de dates triées pour les commentaires
                var datesTaches = new List<DateTime>();
                for (int i = 0; i < nombreCommentaires; i++)
                {
                    datesTaches.Add(new Faker().Date.Between(tache.DateCreation, DateTime.Now));
                }
                datesTaches.Sort();

                // Générer les commentaires pour cette tâche
                for (int i = 0; i < nombreCommentaires; i++)
                {
                    commentaires.Add(new Commentaire
                    {
                        Id = commentaireId++,
                        TacheId = tache.Id,
                        EmployeId = new Faker().PickRandom(membresDuProjet),
                        Contenu = new Faker().Lorem.Paragraph(),
                        DateCreation = datesTaches[i]
                    });
                }
            }

            // Configuration des données pour le seeding
            modelBuilder.Entity<Employe>().HasData(employes);
            modelBuilder.Entity<Projet>().HasData(projets);
            modelBuilder.Entity<MembreProjet>().HasData(membresProjet);
            modelBuilder.Entity<Tache>().HasData(taches);
            modelBuilder.Entity<Commentaire>().HasData(commentaires);

            // Logique des statistiques (facultatif pour le debug)
            Console.WriteLine($"Générés: {NombreEmployes} employés, {NombreProjets} projets, {taches.Count} tâches, {membresProjet.Count} membres, {commentaires.Count} commentaires");
        }
    }
}