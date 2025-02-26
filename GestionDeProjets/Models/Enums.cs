namespace GestionDeProjets.Models
{
    public enum PrioriteEnum
    {
        Basse,
        Moyenne,
        Haute,
        Urgente
    }

    public enum StatutEnum
    {
        NonCommencee,
        EnCours,
        EnAttente,
        Terminee,
        Annulee
    }

    public enum RoleEnum
    {
        Membre,
        ResponsableTechnique,
        ChefDeProjet
    }
}
