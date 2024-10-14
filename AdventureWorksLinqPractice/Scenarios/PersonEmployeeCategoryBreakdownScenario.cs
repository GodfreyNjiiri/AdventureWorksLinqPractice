// PersonEmployeeCategoryBreakdownScenario.cs

namespace AdventureWorksLinqPractice.Scenarios
{
    public static class PersonEmployeeCategoryBreakdownScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("****** Person and Employee Category Breakdown ********");
            Console.WriteLine("*****************************************************");

            using var context = new AdventureWorksContext();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Total number of people
            int totalPeople = context.People.Count();

            // Category: Male
            int maleCount = context.Employees.Where(e => e.Gender == "M").Count();
            Console.WriteLine($"Category: Male");
            Console.WriteLine($"Count: {maleCount}");
            Console.WriteLine($"Percentage: {(double)maleCount / totalPeople * 100}%");
            Console.WriteLine("------------------------------------------");

            // Category: Female
            int femaleCount = context.Employees.Where(e => e.Gender == "F").Count();
            Console.WriteLine($"Category: Female");
            Console.WriteLine($"Count: {femaleCount}");
            Console.WriteLine($"Percentage: {(double)femaleCount / totalPeople * 100}%");
            Console.WriteLine("------------------------------------------");

            // Category: Managers
            int managerCount = context.Employees.Where(e => e.JobTitle.Contains("Manager")).Count();
            Console.WriteLine($"Category: Managers");
            Console.WriteLine($"Count: {managerCount}");
            Console.WriteLine($"Percentage: {(double)managerCount / totalPeople * 100}%");
            Console.WriteLine("------------------------------------------");

            // Category: Female Managers (as a percentage of all managers)
            int femaleManagerCount = context.Employees.Where(e => e.Gender == "F" && e.JobTitle.Contains("Manager")).Count();
            Console.WriteLine($"Category: Female Managers");
            Console.WriteLine($"Count: {femaleManagerCount}");
            Console.WriteLine($"Percentage: {(double)femaleManagerCount / managerCount * 100}% of all managers");
            Console.WriteLine("------------------------------------------");

            // Category: Employees
            int employeeCount = context.Employees.Count();
            Console.WriteLine($"Category: Employees");
            Console.WriteLine($"Count: {employeeCount}");
            Console.WriteLine($"Percentage: {(double)employeeCount / totalPeople * 100}%");
            Console.WriteLine("------------------------------------------");

            // Category: Non-Employees
            int nonEmployeeCount = totalPeople - employeeCount;
            Console.WriteLine($"Category: Non-Employees");
            Console.WriteLine($"Count: {nonEmployeeCount}");
            Console.WriteLine($"Percentage: {(double)nonEmployeeCount / totalPeople * 100}%");
            Console.WriteLine("------------------------------------------");

            // Category: Employees earning above a certain rate (e.g., $30)
            decimal rateThreshold = 30.00m;
            int aboveRateCount = context.EmployeePayHistories.Where(eph => eph.Rate > rateThreshold).Select(eph => eph.BusinessEntityId).Distinct().Count();
            Console.WriteLine($"Category: Employees earning above {rateThreshold:C}");
            Console.WriteLine($"Count: {aboveRateCount}");
            Console.WriteLine($"Percentage: {(double)aboveRateCount / employeeCount * 100}%");
            Console.WriteLine("------------------------------------------");

            // Category: Female employees earning above the rate threshold
            int femaleAboveRateCount = (from emp in context.Employees
                                        join eph in context.EmployeePayHistories on emp.BusinessEntityId equals eph.BusinessEntityId
                                        where eph.Rate > rateThreshold && emp.Gender == "F"
                                        select emp.BusinessEntityId).Distinct().Count();
            Console.WriteLine($"Category: Female employees earning above {rateThreshold:C}");
            Console.WriteLine($"Count: {femaleAboveRateCount}");
            Console.WriteLine($"Percentage: {(double)femaleAboveRateCount / femaleCount * 100}% of all females");
            Console.WriteLine("------------------------------------------");

            // Category: Department-specific (e.g., Sales Department)
            var salesDeptCount = (from emp in context.Employees
                                  join edh in context.EmployeeDepartmentHistories on emp.BusinessEntityId equals edh.BusinessEntityId
                                  where edh.Department.Name == "Sales"
                                  select emp.BusinessEntityId).Distinct().Count();
            Console.WriteLine($"Category: Employees in Sales Department");
            Console.WriteLine($"Count: {salesDeptCount}");
            Console.WriteLine($"Percentage: {(double)salesDeptCount / employeeCount * 100}%");
            Console.WriteLine("------------------------------------------");

            // Category: Employees paid on a biweekly basis (PayFrequency == 2)
            int biweeklyCount = context.EmployeePayHistories.Where(eph => eph.PayFrequency == 2).Select(eph => eph.BusinessEntityId).Distinct().Count();
            Console.WriteLine($"Category: Employees paid biweekly");
            Console.WriteLine($"Count: {biweeklyCount}");
            Console.WriteLine($"Percentage: {(double)biweeklyCount / employeeCount * 100}%");
            Console.WriteLine("------------------------------------------");

            // Category: Employees with more than 5 years of tenure
            DateTime fiveYearsAgo = DateTime.Now.AddYears(-5);
            int fiveYearTenureCount = context.Employees.Where(e => e.HireDate <= fiveYearsAgo).Count();
            Console.WriteLine($"Category: Employees with more than 5 years of tenure");
            Console.WriteLine($"Count: {fiveYearTenureCount}");
            Console.WriteLine($"Percentage: {(double)fiveYearTenureCount / employeeCount * 100}%");
            Console.WriteLine("------------------------------------------");

            stopwatch.Stop();
            Console.WriteLine($"Execution Time: {stopwatch.Elapsed} ms");

            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
        }
    }
}
