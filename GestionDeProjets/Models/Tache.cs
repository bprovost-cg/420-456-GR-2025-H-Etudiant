namespace GestionDeProjets.Models
{
    public class Tache
    {
        public int Id { get; set; }
        public int ProjetId { get; set; }
        public int? EmployeId { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? DateEcheance { get; set; }
        public DateTime? DateAchievement { get; set; }
        public PrioriteEnum Priorite { get; set; }
        public StatutEnum Statut { get; set; }

        // Propriétés de navigation pour le chargement différé (lazy loading)
        public virtual Projet Projet { get; set; }
        public virtual Employe EmployeAssigne { get; set; }
        public virtual ICollection<Commentaire> Commentaires { get; set; }
    }
}
