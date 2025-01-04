namespace AdventureWorksLinqPractice.Scenarios
{
    public static class QuarterlySalesReportScenario
    {
        public static void Run()
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("************* Quarterly Sales Report ***************");
            Console.WriteLine("*****************************************************");

            using var context = new AdventureWorksContext();

            // LINQ query to calculate total sales per territory and quarter
            var query = from sales in context.SalesOrderHeaders
                        join territory in context.SalesTerritories on sales.TerritoryId equals territory.TerritoryId
                        group sales by new
                        {
                            TerritoryName = territory.Name,
                            Quarter = ((sales.OrderDate.Month - 1) / 3) + 1 // Calculate quarter from month
                        } into g
                        select new
                        {
                            g.Key.TerritoryName,
                            g.Key.Quarter,
                            TotalSales = g.Sum(s => s.TotalDue)
                        };

            // Pivot data into a dictionary
            var pivotData = query
                .GroupBy(q => q.TerritoryName)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToDictionary(q => q.Quarter, q => q.TotalSales)
                );

            // Display the pivoted data in a formatted table
            Console.WriteLine($"{"Territory",-20} {"Q1",-10} {"Q2",-10} {"Q3",-10} {"Q4",-10}");
            Console.WriteLine(new string('-', 60));

            foreach (var territory in pivotData)
            {
                Console.Write($"{territory.Key,-20}");

                for (int quarter = 1; quarter <= 4; quarter++)
                {
                    var sales = territory.Value.ContainsKey(quarter) ? territory.Value[quarter] : 0;
                    Console.Write($"{sales,10:C}");
                }

                Console.WriteLine();
            }

            Console.WriteLine("*****************************************************");
            Console.WriteLine("*****************************************************");
        }
    }
}
