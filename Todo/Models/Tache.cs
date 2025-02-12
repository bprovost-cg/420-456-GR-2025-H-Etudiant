namespace Todo.Models
{
    public class Tache
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public bool EstCompletee { get; set; }
        public Categorie Categorie { get; set; }
        public DateTimeOffset DateCompletion { get; set; }
        public DateTimeOffset? DateLimite { get; set; }
    }
}
