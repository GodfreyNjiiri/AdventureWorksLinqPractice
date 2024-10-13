namespace AdventureWorksLinqPractice.Scenarios
{
    public static class SalesRevenueByTerritoryScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
            using var context = new AdventureWorksContext();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Query to group sales by TerritoryID and sum the TotalDue for each territory
            var query = from soh in context.SalesOrderHeaders
                        where soh.Status == 5 // Only include shipped orders
                        group soh by soh.TerritoryId into g
                        select new
                        {
                            TerritoryID = g.Key,
                            TotalRevenue = g.Sum(soh => soh.TotalDue)
                        };

            // Output the results
            foreach (var item in query)
            {
                Console.WriteLine($"Territory ID: {item.TerritoryID}, Total Revenue: {item.TotalRevenue:C}");
            }

            stopwatch.Stop();
            Console.WriteLine($"SalesRevenueByTerritoryScenario Execution Time: {stopwatch.Elapsed} ms");

            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
        }
    }
}
