namespace GestionDeProjets.Models
{
    public class Projet
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public decimal Budget { get; set; }

        // Propriété de navigation pour le chargement différé (lazy loading)
        public virtual ICollection<Tache> Taches { get; set; }
        public virtual ICollection<MembreProjet> Membres { get; set; }
    }
}
