namespace AdventureWorksLinqPractice.Scenarios
{
    public static class SalesTrendsOverTimeScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("************* Sales Trends Over Time ***************");
            Console.WriteLine("*****************************************************");

            using var context = new AdventureWorksContext();

            // LINQ query to calculate monthly sales totals grouped by year
            var query = from sales in context.SalesOrderHeaders
                        group sales by new
                        {
                            Year = sales.OrderDate.Year,
                            Month = sales.OrderDate.Month
                        } into g
                        orderby g.Key.Year, g.Key.Month
                        select new
                        {
                            g.Key.Year,
                            g.Key.Month,
                            TotalSales = g.Sum(s => s.TotalDue)
                        };

            // Group the results by year for a cleaner display
            var groupedByYear = query.GroupBy(q => q.Year);

            // Display the results
            foreach (var yearGroup in groupedByYear)
            {
                Console.WriteLine($"Year: {yearGroup.Key}");
                Console.WriteLine($"{"Month",-10} {"Total Sales",-15}");
                Console.WriteLine(new string('-', 30));

                foreach (var result in yearGroup)
                {
                    Console.WriteLine($"{new DateTime(1, result.Month, 1):MMMM,-10} {result.TotalSales,15:C}");
                }

                Console.WriteLine();
            }

            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
        }
    }
}
