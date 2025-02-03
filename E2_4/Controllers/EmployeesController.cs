using System.Text;
using E2_4.Models;
using E2_4.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace E2_4.Controllers
{
    public class EmployeesController : Controller
    {
        private IList<Employee> Employees { get; set; } = GenerateEmployees();

        /// <summary>
        ///  Ici je vous montre 4 façons de faire la même chose, selon la syntaxe et les types
        ///  avec lesquels vous êtes à l'aise
        /// </summary>
        /// <returns></returns>
        public PasserInfoVM GetPasserInfoVM()
        {
            var passerInfoVM = new PasserInfoVM();

            // Méthode 1: avec foreach et un Dictionnaire avec index
            foreach (Employee employee in Employees)
            {
                // Substitue par un pays "valide"
                if (employee.Country == null)
                {
                    employee.Country = new Country { Name = "Pays Inconnu" };
                }

                try
                {
                    var value = passerInfoVM.EmployeesPerCountry_dict[employee.Country.Name];
                    passerInfoVM.EmployeesPerCountry_dict[employee.Country.Name] += 1;
                }
                catch (KeyNotFoundException)
                {
                    passerInfoVM.EmployeesPerCountry_dict[employee.Country.Name] = 1;
                }
            }

            // On réinitialise pour la méthode 2
            passerInfoVM.EmployeesPerCountry_dict = new Dictionary<string, int>();




            // Méthode 2: avec foreach et TryGetValue
            foreach (Employee employee in Employees)
            {
                // Substitue par un pays "valide"
                if (employee.Country == null)
                {
                    employee.Country = new Country { Name = "Pays Inconnu" };
                }

                if (passerInfoVM.EmployeesPerCountry_dict.TryGetValue(employee.Country.Name, out int value))
                {
                    passerInfoVM.EmployeesPerCountry_dict[employee.Country.Name] += 1;
                }
                else
                {
                    passerInfoVM.EmployeesPerCountry_dict[employee.Country.Name] = 1;
                }
            }


            // Méthode 3: avec LINQ, GroupBy et ToDictonnary
            passerInfoVM.EmployeesPerCountry_dict = Employees
                // On fait une "projection" vers le pays en gérant le cas null
                .Select(e => e.Country ?? new Country { Name = "Pays Inconnu" })
                // Ça créé des IGrouping<clé, type>, le nom du pays sera la clé
                .GroupBy(c => c.Name)
                // On transforme à partir du Grouping, la clé sera le nom, mais le Grouping
                // Est lui même une collection, alors Count() donne le nombre d'éléments
                .ToDictionary(g => g.Key, g => g.Count());


            // Méthode 4: Au lieu d'un dictionnaire on peut faire une liste d'un type sur mesure
            // Sinon c'est comme la méthode 3. ToArray() est imporatnt pour matérialiser la collection.
            passerInfoVM.EmployeesPerCountry = Employees
                .Select(e => e.Country ?? new Country { Name = "Pays Inconnu" })
                .GroupBy(c => c.Name)
                .Select(g => new PasserInfoVM.CountryCount() { Name = g.Key, Count = g.Count() })
                .ToArray();

            return passerInfoVM;
        }

        public IActionResult PasserInfoVM()
        {
            return View(GetPasserInfoVM());
        }

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

        public static IList<Country> Countries { get; set; } = [
            new (){ CountryId = 1, Name = "Canada" },
            new (){ CountryId = 2, Name = "France" },
            new (){ CountryId = 3, Name = "États-Unis" },
        ];

        /// <summary>
        /// Génère les employés
        /// </summary>
        /// <returns>Liste d'employés</returns>
        public static IList<Employee> GenerateEmployees()
        {
            return
                [
                    new Employee() { Id = 1, Name = "Adam", HiringDate = DateTimeOffset.Parse("2016-11-23"), YearlySalary = 28000, Country = Countries[2] },
                    new Employee() { Id = 2, Name = "Benoit", HiringDate = DateTimeOffset.Parse("2006-08-17"), YearlySalary = 70000, Country = Countries[1] },
                    new Employee() { Id = 3, Name = "Charles", HiringDate = DateTimeOffset.Parse("2014-03-14"), YearlySalary = 60000, Country = Countries[2] },
                    new Employee() { Id = 4, Name = "Denis", HiringDate = DateTimeOffset.Parse("2019-01-22"), YearlySalary = 19000, Country = Countries[0] },
                    new Employee() { Id = 5, Name = "Émile", HiringDate = DateTimeOffset.Parse("2017-06-01"), YearlySalary = 84000, Country = Countries[2] },
                    new Employee() { Id = 6, Name = "Fanny", HiringDate = DateTimeOffset.Parse("2022-07-12"), YearlySalary = 30000, Country = Countries[1] },
                    new Employee() { Id = 7, Name = "Gaétan", HiringDate = DateTimeOffset.Parse("2020-12-07"), YearlySalary = 40000, Country = Countries[0] },
                    new Employee() { Id = 8, Name = "Hugo", HiringDate = DateTimeOffset.Parse("2003-08-23"), YearlySalary = 80000, Country = Countries[2] },
                    new Employee() { Id = 9, Name = "Ibrahem", HiringDate = DateTimeOffset.Parse("2018-04-09"), YearlySalary = 65000, Country = Countries[0] },
                    new Employee() { Id = 10, Name = "Jonathan", HiringDate = DateTimeOffset.Parse("2016-01-19"), YearlySalary = 70000 }

                ];
        }


        public static string EmployeesToString(IList<Employee> employees)
        {
            return string.Join("\r\n", employees.Select(e => e.ToString()));
        }
    }
}
