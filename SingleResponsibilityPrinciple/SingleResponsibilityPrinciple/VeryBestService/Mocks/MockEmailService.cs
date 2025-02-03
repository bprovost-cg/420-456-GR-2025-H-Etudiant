using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleResponsibilityPrinciple.VeryBestService.Interfaces;

namespace SingleResponsibilityPrinciple.VeryBestService.Mocks
{
    internal class MockEmailService : IEmailService
    {
        private bool _success;
        public MockEmailService(bool success)
        {
            _success = success;
        }
        public void SendWelcomeEmail(string email)
        {
            if (_success)
            {
                Console.WriteLine("Ah HA! Je n'envoie pas réellement de mail.");
            }
            else
            {
                throw new InvalidOperationException("Une exception valide du service de mail");
            }
            
        }
    }
}
