// EmployeeCompensationSummaryScenario.cs

namespace AdventureWorksLinqPractice.Scenarios
{
    public static class EmployeeCompensationSummaryScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("****** Employee Compensation and Department *********");
            Console.WriteLine("*****************************************************");
            using var context = new AdventureWorksContext();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // LINQ query to join Employee, EmployeePayHistory, EmployeeDepartmentHistory, and Person for employee names
            var query = from emp in context.Employees
                        join eph in context.EmployeePayHistories on emp.BusinessEntityId equals eph.BusinessEntityId
                        join edh in context.EmployeeDepartmentHistories on emp.BusinessEntityId equals edh.BusinessEntityId
                        join p in context.People on emp.BusinessEntityId equals p.BusinessEntityId // Join to get First and Last name
                        where emp.CurrentFlag == true // Only active employees
                        group new { emp, eph, edh, p } by new { emp.BusinessEntityId, p.FirstName, p.LastName, emp.JobTitle, emp.MaritalStatus, edh.DepartmentId } into g
                        select new
                        {
                            EmployeeName = g.Key.FirstName + " " + g.Key.LastName,
                            g.Key.JobTitle,
                            g.Key.MaritalStatus,
                            DepartmentId = g.Key.DepartmentId,
                            AverageRate = g.Average(e => e.eph.Rate),
                            TotalSickLeave = g.Max(e => e.emp.SickLeaveHours), // Assuming leave hours are cumulative over time
                            TotalVacationHours = g.Max(e => e.emp.VacationHours),
                            LastRateChangeDate = g.Max(e => e.eph.RateChangeDate)
                        };

            // Output the results
            foreach (var item in query)
            {
                Console.WriteLine($"Employee: {item.EmployeeName} ({item.JobTitle})");
                Console.WriteLine($"Marital Status: {item.MaritalStatus}");
                Console.WriteLine($"Department ID: {item.DepartmentId}");
                Console.WriteLine($"Average Rate: {item.AverageRate:C}");
                Console.WriteLine($"Total Sick Leave Hours: {item.TotalSickLeave}");
                Console.WriteLine($"Total Vacation Hours: {item.TotalVacationHours}");
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
