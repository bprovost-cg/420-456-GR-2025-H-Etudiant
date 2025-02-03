using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple.VeryBestService
{
    internal class VeryBestService
    {
        private readonly UserValidator _validator;
        private readonly UserRepository _repository;
        private readonly EmailService _emailService;

        // On injecte les dépendances (principe d’inversion de dépendances)
        public VeryBestService(UserValidator validator, UserRepository repository, EmailService emailService)
        {
            _validator = validator;
            _repository = repository;
            _emailService = emailService;
        }

        public void CreateUser(string username, string password, string email)
        {
            if (!_validator.IsUserValid(username, password))
            {
                throw new ArgumentException("Nom d'utilisateur ou mot de passe invalide.");
            }

            _repository.SaveUser(username, password);

            _emailService.SendWelcomeEmail(email);
        }
    }
}
