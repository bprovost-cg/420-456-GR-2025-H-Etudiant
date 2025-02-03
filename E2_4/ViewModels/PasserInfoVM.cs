using E2_4.Models;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace E2_4.ViewModels
{

    public class PasserInfoVM
    {        
        // Méthode Dict
        public IDictionary<string, int> EmployeesPerCountry_dict = new Dictionary<string, int>();

        // Méthode Type Custom
        public IList<CountryCount> EmployeesPerCountry = new List<CountryCount>();


        public class CountryCount
        {
            public string Name { get; set; }
            public int Count { get; set; }
        }
    }
}
