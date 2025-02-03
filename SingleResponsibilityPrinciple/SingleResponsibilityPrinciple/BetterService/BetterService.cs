using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple.BetterService
{
    public class BetterService
    {
        // Méthode pour créer un nouvel utilisateur
        public void CreateUser(string username, string password, string email)
        {
            if (!IsUserValid(username, password))
            {
                throw new ArgumentException("Nom d'utilisateur ou mot de passe invalide.");
            }

            SaveToDatabase(username, password, email);

            SendEmail(email);
        }

        public bool IsUserValid(string username, string password)
        {
            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
        }

        public void SaveToDatabase(string username, string password, string email)
        {
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
        }

        public void SendEmail(string email)
        {
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
