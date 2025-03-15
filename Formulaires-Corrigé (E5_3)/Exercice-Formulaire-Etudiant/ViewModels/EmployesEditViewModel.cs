using Exercice_Formulaire_Etudiant.Models;

namespace Exercice_Formulaire_Etudiant.ViewModels
{
    public class EmployesEditViewModel : EmployesCreateViewModel
    {
        public int Id { get; set; }

        public StatutEmploye Statut { get; set; }

        public TypeRemuneration TypeRemuneration { get; set; }
    }
}
