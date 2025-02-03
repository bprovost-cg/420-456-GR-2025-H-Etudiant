using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleResponsibilityPrinciple.VeryBestService.Interfaces;

namespace SingleResponsibilityPrinciple.VeryBestService
{
    public class UserRepository : IUserRepository
    {
        public void SaveUser(string username, string password)
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
    }
}
