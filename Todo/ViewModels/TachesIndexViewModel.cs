using Todo.Models;

namespace Todo.ViewModels
{
    public class TachesIndexViewModel
    {
        public IEnumerable<TacheIndexViewModel> Taches { get; set; } = [];

        public Categorie Categorie { get; set; }
    }

    public class TacheIndexViewModel
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public bool EstCompletee { get; set; }
        public Categorie Categorie { get; set; }
        public DateTimeOffset? DateCompletion { get; set; }
        public DateTimeOffset? DateLimite { get; set; }
    }
}
