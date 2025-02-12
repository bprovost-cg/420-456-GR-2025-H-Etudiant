namespace E2_3.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset HiringDate { get; set; }
        public double YearsOfExperience => Math.Round((DateTimeOffset.Now - HiringDate).TotalDays / 365, 3);
        public double YearlySalary { get; set; }

        public override string ToString()
        {
            var hiringDate = HiringDate.DateTime.ToShortDateString();
            return string.Join(",", Id, Name, hiringDate, YearsOfExperience, YearlySalary);
        }
    }
}
