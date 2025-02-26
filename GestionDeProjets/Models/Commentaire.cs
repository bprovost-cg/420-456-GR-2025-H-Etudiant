namespace GestionDeProjets.Models
{
    public class Commentaire
    {
        public int Id { get; set; }
        public int TacheId { get; set; }
        public int EmployeId { get; set; }
        public string Contenu { get; set; }
        public DateTime DateCreation { get; set; }

        // Propriétés de navigation pour le chargement différé (lazy loading)
        public virtual Tache Tache { get; set; }
        public virtual Employe Employe { get; set; }
    }
}
