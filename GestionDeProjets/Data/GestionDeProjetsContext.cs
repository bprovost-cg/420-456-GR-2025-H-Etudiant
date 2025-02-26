using GestionDeProjets.Models;
using GestionProjets.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace GestionDeProjets.Data
{
    public class GestionDeProjetsContext : DbContext
    {
        public GestionDeProjetsContext(DbContextOptions<GestionDeProjetsContext> options)
        : base(options)
        { }

        public DbSet<Projet> Projets { get; set; }
        public DbSet<Employe> Employes { get; set; }
        public DbSet<Tache> Taches { get; set; }
        public DbSet<MembreProjet> MembresProjet { get; set; }
        public DbSet<Commentaire> Commentaires { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration des relations
            modelBuilder.Entity<Tache>()
                .HasOne(t => t.Projet)
                .WithMany(p => p.Taches)
                .HasForeignKey(t => t.ProjetId);

            modelBuilder.Entity<Tache>()
                .HasOne(t => t.EmployeAssigne)
                .WithMany(e => e.TachesAssignees)
                .HasForeignKey(t => t.EmployeId);

            modelBuilder.Entity<MembreProjet>()
                .HasOne(mp => mp.Projet)
                .WithMany(p => p.Membres)
                .HasForeignKey(mp => mp.ProjetId);

            modelBuilder.Entity<MembreProjet>()
                .HasOne(mp => mp.Employe)
                .WithMany(e => e.Projets)
                .HasForeignKey(mp => mp.EmployeId);

            modelBuilder.Entity<Commentaire>()
                .HasOne(c => c.Tache)
                .WithMany(t => t.Commentaires)
                .HasForeignKey(c => c.TacheId);

            modelBuilder.Entity<Commentaire>()
                .HasOne(c => c.Employe)
                .WithMany()
                .HasForeignKey(c => c.EmployeId);


            DataGenerator.SeedData(modelBuilder);
        }
    }
}
