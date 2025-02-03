using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple.BadService
{
    public class BadService
    {
        // Méthode pour créer un nouvel utilisateur
        public void CreateUser(string username, string password, string email)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Nom d'utilisateur ou mot de passe invalide.");
            }

            // Enregistrement dans une (hypothétique) base de données
            var saveToDbSuccessful = true;
            if (saveToDbSuccessful)
            {
                Console.WriteLine("Utilisateur enregistré dans la base de données.");
            }
            else
            {
                throw new InvalidOperationException("Enregistrement dans la base de données échoué.");
            }

            // On envoie un email différent la semaine ou la fin de semaine
            var now = DateTimeOffset.Now;
            if (now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday)
            {
                Console.WriteLine($"E-mail de bienvenue envoyé à {email} la fin de semaine.");
            }
            else
            {
                Console.WriteLine($"E-mail de bienvenue envoyé à {email} la semaine.");
            }
        }
    }
}
