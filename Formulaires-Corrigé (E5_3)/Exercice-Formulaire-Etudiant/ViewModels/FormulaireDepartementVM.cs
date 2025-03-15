using Exercice_Formulaire_Etudiant.Models;
using System.ComponentModel.DataAnnotations;

namespace Exercice_Formulaire_Etudiant.ViewModels
{
    public class FormulaireDepartementVM
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Nom { get; set; } = default!;

        public double Budget { get; set; }
    }
}
