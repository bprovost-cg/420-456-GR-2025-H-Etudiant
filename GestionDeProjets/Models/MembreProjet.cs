namespace GestionDeProjets.Models
{
    public class MembreProjet
    {
        public int Id { get; set; }
        public int ProjetId { get; set; }
        public int EmployeId { get; set; }
        public RoleEnum Role { get; set; }
        public DateTime DateAjout { get; set; }

        // Propriétés de navigation pour le chargement différé (lazy loading)
        public virtual Projet Projet { get; set; }
        public virtual Employe Employe { get; set; }
    }
}
