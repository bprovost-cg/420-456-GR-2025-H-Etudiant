using System.ComponentModel.DataAnnotations;

namespace Exercice_Formulaire_Etudiant.Models
{
    public enum TypeRemuneration
    {
        [Display(Name = "Taux horaire")]
        TauxHoraire = 0,
        [Display(Name = "Pourboire")]
        Pourboire = 1,
        [Display(Name = "Salaire annuel")]
        Annule = 2
    }

}
