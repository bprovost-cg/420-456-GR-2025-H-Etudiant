using System.Text;
using E2_3.Models;
using Microsoft.AspNetCore.Mvc;

namespace E2_3.Controllers
{
    public class EmployeesController : Controller
    {
        private IList<Employee> Employees { get; set; } = GenerateEmployees();

        public IActionResult Index(int? id)
        {
            if (id == null)
            {
                return Content(EmployeesToString(Employees));
            }
            else
            {
                var employee = Employees.FirstOrDefault(e => e.Id == id)?.ToString() ?? "";
                return Content(employee.ToString());

            }
        }

        public IActionResult Aleatoire()
        {
            var randomIndex = Random.Shared.Next(0, Employees.Count);
            var randomEmployee = Employees[randomIndex];
            return Content(randomEmployee.ToString());
        }

        [Route("employees/chercher/{name}")]
        public IActionResult Chercher(string name)
        {
            var employee = Employees.FirstOrDefault(e => String.Equals(e.Name, name, StringComparison.InvariantCultureIgnoreCase));

            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                return Content(employee.ToString());
            }
        }


        [Route("employees/embauche/{annee:int:length(4)}/{mois:range(1,12)}")]
        public IActionResult Embauche(string annee, string mois)
        {
            var year = int.Parse(annee);
            var month = int.Parse(mois);
            var embauches = Employees
                .Where(e => e.HiringDate.Year == year && e.HiringDate.Month <= month || e.HiringDate.Year < year)
                .ToArray();
            return Content(EmployeesToString(embauches));

        }

        [Route("employees/moyenne/{propriete}")]
        public IActionResult Moyenne(string propriete)
        {
            var experience_moyenne = Employees.Average(e => e.YearsOfExperience);
            var salaire_moyen = Employees.Average(e => e.YearlySalary);
            return propriete switch
            {
                "experience" => Content(experience_moyenne.ToString()),
                "salaire" => Content(salaire_moyen.ToString()),
                _ => new EmptyResult()
            };
        }


        /// <summary>
        /// Génère les employés
        /// </summary>
        /// <returns>Liste d'employés</returns>
        public static IList<Employee> GenerateEmployees()
        {
            return
                [
                    new Employee() { Id = 1, Name = "Adam", HiringDate = DateTimeOffset.Parse("2016-11-23"), YearlySalary = 28000 },
                    new Employee() { Id = 2, Name = "Benoit", HiringDate = DateTimeOffset.Parse("2006-08-17"), YearlySalary = 70000 },
                    new Employee() { Id = 3, Name = "Charles", HiringDate = DateTimeOffset.Parse("2014-03-14"), YearlySalary = 60000 },
                    new Employee() { Id = 4, Name = "Denis", HiringDate = DateTimeOffset.Parse("2019-01-22"), YearlySalary = 19000 },
                    new Employee() { Id = 5, Name = "Émile", HiringDate = DateTimeOffset.Parse("2017-06-01"), YearlySalary = 84000 },
                    new Employee() { Id = 6, Name = "Fanny", HiringDate = DateTimeOffset.Parse("2022-07-12"), YearlySalary = 30000 },
                    new Employee() { Id = 7, Name = "Gaétan", HiringDate = DateTimeOffset.Parse("2020-12-07"), YearlySalary = 40000 },
                    new Employee() { Id = 8, Name = "Hugo", HiringDate = DateTimeOffset.Parse("2003-08-23"), YearlySalary = 80000 },
                    new Employee() { Id = 9, Name = "Ibrahem", HiringDate = DateTimeOffset.Parse("2018-04-09"), YearlySalary = 65000 },
                    new Employee() { Id = 10, Name = "Jonathan", HiringDate = DateTimeOffset.Parse("2016-01-19"), YearlySalary = 70000 }

                ];
        }


        public static string EmployeesToString(IList<Employee> employees)
        {
            return string.Join("\r\n", employees.Select(e => e.ToString()));
        }
    }
}
