using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Todo.Models;

namespace Todo.ViewModels
{
    public class TachesCreateViewModel
    {
        public string Titre { get; set; }
        public Categorie Categorie { get; set; }
        public DateTimeOffset? DateLimite { get; set; }
    }

}
