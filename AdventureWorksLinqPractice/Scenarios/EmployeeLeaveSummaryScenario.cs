namespace AdventureWorksLinqPractice.Scenarios
{
    public static class EmployeeLeaveSummaryScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
            using var context = new AdventureWorksContext();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Query to group employees by JobTitle and sum their vacation and sick leave hours for active employees
            var query = from e in context.Employees
                        where e.CurrentFlag == true
                        group e by e.JobTitle into g
                        select new
                        {
                            JobTitle = g.Key,
                            TotalVacationHours = g.Sum(e => e.VacationHours),
                            TotalSickLeaveHours = g.Sum(e => e.SickLeaveHours)
                        };

            // Output the results
            foreach (var item in query)
            {
                Console.WriteLine($"Job Title: {item.JobTitle}, Total Vacation Hours: {item.TotalVacationHours}, Total Sick Leave Hours: {item.TotalSickLeaveHours}");
            }

            stopwatch.Stop();
            Console.WriteLine($" EmployeeLeaveSummaryScenario Execution Time: {stopwatch.Elapsed} ms");

            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
        }
    }
}
