namespace GestionDeProjets.Models
{
    public class Employe
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Poste { get; set; }

        // Propriété de navigation pour le chargement différé (lazy loading)
        public virtual ICollection<MembreProjet> Projets { get; set; }
        public virtual ICollection<Tache> TachesAssignees { get; set; }
    }
}
