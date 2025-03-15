using System.ComponentModel.DataAnnotations;
using Exercice_Formulaire_Etudiant.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exercice_Formulaire_Etudiant.ViewModels
{
    public class EmployesCreateViewModel
    {
        public string? Nom { get; set; }

        [Display(Name = "Âge")]
        public int Age { get; set; }

        [Display(Name = "Date d'embauche")]
        [DataType(DataType.Date)]
        public DateTime DateEmbauche { get; set; }

        [Display(Name = "Salaire annuel")]
        public double SalaireAnnuel { get; set; }

        [Display(Name = "Présentement à l'emploi?")]

        public bool EstEnEmploi { get; set; }

        public IList<SelectListItem> ListePays { get; set; } = new List<SelectListItem>();

        public IList<SelectListItem> ListeDepartement { get; set; } = new List<SelectListItem>();

        [Display(Name = "Pays d'origine")]
        public int PaysId { get; set; }

        [Display(Name = "Département de travail")]
        public int DepartementId { get; set; }

        // Propriété de navigation
        public Pays? Pays { get; set; }

        // Propriété de navigation
        public Departement? Departement { get; set; }
    }
}
