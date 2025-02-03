using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleResponsibilityPrinciple.VeryBestService.Interfaces;

namespace SingleResponsibilityPrinciple.VeryBestService
{
    public class EmailService : IEmailService
    {
        private IDateTimeService _dateTimeService;

        public EmailService(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }
        public void SendWelcomeEmail(string email)
        {
            // On envoie un email différent la semaine ou la fin de semaine
            var now = _dateTimeService.Now();
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
