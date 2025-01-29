using MagasinDeVoitures.Models;

namespace MagasinDeVoitures.ViewModels
{
    public class VoituresComparisonVM
    {
        public string Marque1 { get; set; }
        public string Marque2 { get; set; }
        public IList<Voiture> VoituresMarque1 { get; set; }
        public IList<Voiture> VoituresMarque2 { get; set; }
    }
}
