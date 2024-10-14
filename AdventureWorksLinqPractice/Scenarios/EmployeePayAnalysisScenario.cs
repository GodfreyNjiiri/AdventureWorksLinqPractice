// EmployeePayAnalysisScenario.cs

namespace AdventureWorksLinqPractice.Scenarios
{
    public static class EmployeePayAnalysisScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("************** Employee Pay Analysis ****************");
            Console.WriteLine("*****************************************************");
            using var context = new AdventureWorksContext();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // LINQ query to analyze pay history with corrected join to Person for names
            var query = from eph in context.EmployeePayHistories
                        join emp in context.Employees on eph.BusinessEntityId equals emp.BusinessEntityId
                        join p in context.People on emp.BusinessEntityId equals p.BusinessEntityId // Join to get names
                        group eph by new { eph.BusinessEntityId, p.FirstName, p.LastName, emp.JobTitle, eph.PayFrequency } into g
                        select new
                        {
                            g.Key.BusinessEntityId,
                            EmployeeName = g.Key.FirstName + " " + g.Key.LastName,
                            g.Key.JobTitle,
                            PayFrequency = g.Key.PayFrequency == 1 ? "Monthly" : "Biweekly",
                            AverageRate = g.Average(eph => eph.Rate),
                            MaxRate = g.Max(eph => eph.Rate),
                            LastRateChangeDate = g.Max(eph => eph.RateChangeDate)
                        };

            // Output the results
            foreach (var item in query)
            {
                Console.WriteLine($"Employee: {item.EmployeeName} ({item.JobTitle})");
                Console.WriteLine($"Pay Frequency: {item.PayFrequency}");
                Console.WriteLine($"Average Rate: {item.AverageRate:C}");
                Console.WriteLine($"Max Rate: {item.MaxRate:C}");
                Console.WriteLine($"Last Rate Change Date: {item.LastRateChangeDate}");
                Console.WriteLine("------------------------------------------");
            }

            stopwatch.Stop();
            Console.WriteLine($"Execution Time: {stopwatch.Elapsed} ms");

            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
        }
    }
}
